using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WealthFrogs.Common.Http;
using WealthFrogs.Models;
using LiveCharts;
using LiveCharts.Wpf;
using WealthFrogs.Common.DialogConfig;
using Prism.Events;
using WealthFrogs.Extensions;

namespace WealthFrogs.ViewModels
{
    public class IndexViewModel : NavigationViewModel
    {
        private readonly IDialogHostService dialog;
        private readonly IRegionManager regionManager;
        private SynchronizationContext mainThreadSynContext;

        public IndexViewModel(IContainerProvider provider, IDialogHostService dialog) : base(provider)
        {
            // 初始化工具以及依赖注入
            client = new HttpRestClient();
            this.regionManager = provider.Resolve<IRegionManager>();
            this.dialog = provider.Resolve<IDialogHostService>();
            mainThreadSynContext = SynchronizationContext.Current;

            // 初始化 Model
            ChoiceList = new ObservableCollection<ChoiceStockDto>();
            LevelItems = new ObservableCollection<LevelItem>();
            StockOrder = new StockOrderRequestParam();
            MiniteValues = new ChartValues<double>();
            AxisYFormatter = yFormatter;

            // 初始化 Command
            AddChoiceCommand = new DelegateCommand<string>(AddChoiceComandAction);
            DeleteChoiceCommand = new DelegateCommand(DeleteChoiceCommandAction);
            ShowDetailCommand = new DelegateCommand<ChoiceStockDto>(ShowDetailCommandAction);
            MouseRightCommand = new DelegateCommand<ChoiceStockDto>(OnMouseRightClick);
            SubmmitOrderCommand = new DelegateCommand<string>(OnOrderSubmmitAsync);

            // 定时任务
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += new EventHandler((sender, e) => { UpdateChoiceList(); });
            timer.Tick += new EventHandler((sender, e) => { ShowDetail(this.leftSelectedStock, true); });
            timer.Start();

        }



        // 自选股列表
        private ObservableCollection<ChoiceStockDto> choiceList;
        // 买卖档位
        private ObservableCollection<LevelItem> levelItems;
        // 股票详情
        private StockDetailDto stockDetail;
        // 分时图
        public ChartValues<double> miniteValues;
        // 下单表单
        public StockOrderRequestParam stockOrder;

        // 请求工具
        private readonly HttpRestClient client;
        // 定时拉取数据定时器
        private DispatcherTimer timer;
        // 缓存右键选中的自选股，用于在 ContextMenu 中删除
        private ChoiceStockDto rightSelectedStock;
        // 缓存左键选中的自选股，用于刷新 Detail
        private ChoiceStockDto leftSelectedStock;
        private Func<double, string> axisYFormatter;


        // =======================  getter & setter =======================

        public ObservableCollection<ChoiceStockDto> ChoiceList
        {
            get { return choiceList; }
            set { choiceList = value; RaisePropertyChanged(); }
        }

        public StockDetailDto StockDetail
        {
            get { return stockDetail; }
            set { stockDetail = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<LevelItem> LevelItems
        {
            get { return levelItems; }
            set { levelItems = value; RaisePropertyChanged(); }
        }

        public ChartValues<double> MiniteValues
        {
            get { return miniteValues; }
            set { miniteValues = value; RaisePropertyChanged(); }
        }

        public StockOrderRequestParam StockOrder
        {
            get { return stockOrder; }
            set { stockOrder = value; RaisePropertyChanged(); }
        }


        public Func<double, string> AxisYFormatter
        {
            get { return axisYFormatter; }
            set { axisYFormatter = value; RaisePropertyChanged(); }
        }

        // =========================== Command ==========================
        public DelegateCommand<string> AddChoiceCommand { get; private set; }
        public DelegateCommand DeleteChoiceCommand { get; private set; }
        public DelegateCommand<ChoiceStockDto> ShowDetailCommand { get; private set; }
        public DelegateCommand<ChoiceStockDto> MouseRightCommand { get; private set; }
        public DelegateCommand<string> SubmmitOrderCommand { get; private set; }


        private void ShowDetailCommandAction(ChoiceStockDto stock)
        {
            ShowDetail(stock, false);
        }

        private void AddChoiceComandAction(string id)
        {
            BaseRequest request = new BaseRequest()
            {
                Method = RestSharp.Method.Get,
                Url = $"http://localhost:8000/api/choice/add?stockId={id}"
            };
            new Task(() =>
            {

                SetLoading(true);
                ApiResponse response = client.Execute(request);
                Thread.Sleep(500);
                SetLoading(false);

                if (response.status == "error")
                {
                    ShowMessage(response.message);
                }
                else
                {
                    ShowMessage("添加成功");
                }
            }).Start();
            UpdateChoiceList();
        }

        private void DeleteChoiceCommandAction()
        {
            new Task(() =>
            {
                BaseRequest request = new BaseRequest()
                {
                    Method = RestSharp.Method.Get,
                    Url = $"http://localhost:8000/api/choice/delete?stockId={this.rightSelectedStock.stock_id}"
                };
                SetLoading(true);
                ApiResponse response = client.Execute(request);
                Thread.Sleep(500);
                SetLoading(false);
                if (response.status.Equals("ok"))
                {
                    this.rightSelectedStock = null;
                    UpdateChoiceList();
                    ShowMessage("删除成功");
                }
                else
                {
                    ShowMessage(response.message);
                }
            }).Start();
        }

        private void OnMouseRightClick(ChoiceStockDto stock)
        {
            this.rightSelectedStock = stock;
        }

        private async void OnOrderSubmmitAsync(string direction)
        {
            string dir = direction == "0" ? "买入" : "卖出";
            var dialogResult = await dialog.Question("正在下单", $"证券代码: {StockOrder.stockId}\n方向: {dir}\n数量: {StockOrder.payload}");
            if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;

            SetLoading(true);
            StockOrder.direction = int.Parse(direction);
            BaseRequest request = new BaseRequest()
            {
                Method = RestSharp.Method.Post,
                Url = $"http://localhost:8000/api/order/trade",
                Parameter = StockOrder
            };
            ApiResponse response = client.Execute(request);
            Thread.Sleep(500);
            SetLoading(false);
            if (response.status == "ok")
            {
                ShowMessage("下单成功");
            }
            else
            {
                ShowMessage(response.message);
            }
        }


        // =========================== Private =========================
        public void UpdateChoiceList(bool firstCalled = false)
        {
            new Task(() =>
            {
                BaseRequest request = new BaseRequest()
                {
                    Method = RestSharp.Method.Get,
                    Url = "http://localhost:8000/api/choice/queryAll"
                };
                ApiResponse<List<ChoiceStockDto>> response = client.Execute<List<ChoiceStockDto>>(request);

                if (response.status.Equals("ok"))
                {
                    // 启用非 UI 线程拉取数据，UI线程更改数据源 
                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        ChoiceList.Clear();
                        response.result.ForEach((choice) => ChoiceList.Add(choice));
                        if (firstCalled && ChoiceList.Count > 0)
                        {
                            // 默认展示第一条数据
                            ShowDetail(ChoiceList[0], false);
                        }
                    }));
                }
            }).Start();
        }

        private void ShowDetail(ChoiceStockDto stock, bool triggerByTimer = false)
        {
            if (stock == null)
                return;
            this.leftSelectedStock = stock;
            new Task(() =>
            {
                BaseRequest request = new BaseRequest()
                {
                    Method = RestSharp.Method.Get,
                    Url = $"http://localhost:8000/api/choice/detail?stockId={stock.stock_id}"
                };
                ApiResponse<StockDetailDto> response = client.Execute<StockDetailDto>(request);
                if (response.status.Equals("ok"))
                {
                    StockDetail = response.result;
                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        LevelItems.Clear();
                        GetLevelItems(StockDetail).ForEach((item) =>
                        {
                            LevelItems.Add(item);
                        });
                    }));
                    if (!triggerByTimer)
                    {
                        UpdateOrderForm();
                    }
                } 
                else if (!triggerByTimer)
                {
                    ShowMessage("股票信息拉取失败");
                }
            }).Start();
            ShowMinutes(stock, triggerByTimer);
        }

        private void ShowMinutes(ChoiceStockDto stock, bool triggerByTimer = false)
        {
            if (StockDetail != null && stock.stock_id == StockDetail.stock_id)
                return;
            new Task(() =>
            {
                BaseRequest request = new BaseRequest()
                {
                    Method = RestSharp.Method.Get,
                    Url = $"http://localhost:8000/api/choice/minute?stockId={stock.stock_id}"
                };
                ApiResponse<List<List<string>>> response = client.Execute<List<List<string>>>(request);
                if (response.status.Equals("ok"))
                {
                    List<List<string>> minutes = response.result;
                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        List<double> records = minutes.Select((record) =>
                        {
                            return double.Parse(record[1]);
                        }).ToList();

                        MiniteValues.Clear();
                        MiniteValues.AddRange(records);

                    }));
                } 
                else if (!triggerByTimer)
                {
                    ShowMessage("分时图信息拉取失败");
                }
            }).Start();
        }



        public void UpdateOrderForm()
        {
            StockOrder = new StockOrderRequestParam() { stockId = StockDetail.stock_id , price = StockDetail.price, payload = 100};
        }

        private List<LevelItem> GetLevelItems(StockDetailDto stock)
        {
            List<LevelItem> items = new List<LevelItem>();
            List<List<double>> buyLevel = stock.buy_level;
            List<List<double>> sellLevel = stock.sell_level;
            for (int i = 0; i < buyLevel.Count; i++)
            {
                LevelItem item = new LevelItem() { 
                    buyLevel = $"买{i + 1}",
                    buyPrice = buyLevel[i][0],
                    buyVolume = (int)buyLevel[i][1],
                    sellLevel = $"卖{i + 1}",
                    sellPrice = sellLevel[i][0],
                    sellVolume = (int)sellLevel[i][1],
                };
                items.Add(item);
            }

            return items;
        }

        private string yFormatter(double value)
        {
            return value.ToString("f2");
        }


        // ======================= Override =========================

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            UpdateChoiceList(true);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);
            if (timer != null)
            {
                timer.Stop();
            }
        }
    }
}
