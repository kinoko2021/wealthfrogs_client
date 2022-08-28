using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WealthFrogs.Common.DialogConfig;
using WealthFrogs.Common.Http;
using WealthFrogs.Extensions;
using WealthFrogs.Models.DTOs;

namespace WealthFrogs.ViewModels
{
    public class OrderViewModel : NavigationViewModel
    {

        private readonly IDialogHostService dialog;

        public OrderViewModel(IContainerProvider provider, IContainerProvider containerProvider) : base(containerProvider)
        {
            client = new HttpRestClient();
            this.dialog = provider.Resolve<IDialogHostService>();
            OrderItems = new ObservableCollection<StockOrderDto>();
            SelectedOrder = new StockOrderDto();

            CancelCommand = new DelegateCommand<string>(CancelOrderAsync);
        }


        // 买卖档位
        private ObservableCollection<StockOrderDto> orderItems;
        private StockOrderDto selectedOrder;

        // 请求工具
        private readonly HttpRestClient client;


        // ==================== Command =====================
        public DelegateCommand<string> CancelCommand { get; private set; }


        // ================= getter & setter =================
        public ObservableCollection<StockOrderDto> OrderItems
        {
            get { return orderItems; }
            set { orderItems = value; RaisePropertyChanged(); }
        }

        public StockOrderDto SelectedOrder
        {
            get { return selectedOrder; }
            set { selectedOrder = value; RaisePropertyChanged(); }
        }



        // ====================== Private ====================
        private void showOrders()
        {
            new Task(() =>
            {
                BaseRequest request = new BaseRequest()
                {
                    Method = RestSharp.Method.Get,
                    Url = "http://localhost:8000/api/order/queryAll"
                };
                ApiResponse<List<StockOrderDto>> response = client.Execute<List<StockOrderDto>>(request);

                if (response.status.Equals("ok"))
                {
                    // 启用非 UI 线程拉取数据，UI线程更改数据源 
                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        OrderItems.Clear();
                        response.result.ForEach((order) => OrderItems.Add(order));
                    }));
                }
            }).Start();
        }

        private async void CancelOrderAsync(string orderId)
        {
            var dialogResult = await dialog.Question("温馨提示", "您确定要撤单?");
            if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;

            SetLoading(true);
            BaseRequest request = new BaseRequest()
            {
                Method = RestSharp.Method.Get,
                Url = $"http://localhost:8000/api/order/cancel?orderId={orderId}"
            };
            ApiResponse response = client.Execute(request);
            Thread.Sleep(500);
            SetLoading(false);
            if (response.status == "ok")
            {
                ShowMessage("撤单成功");
            } else
            {
                ShowMessage(response.message);
            }
            showOrders();
        }


        // ===================== Override ======================
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            showOrders();
        }
    }
}
