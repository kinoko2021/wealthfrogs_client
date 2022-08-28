using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WealthFrogs.Common;
using WealthFrogs.Extensions;
using WealthFrogs.Models;

namespace WealthFrogs.ViewModels
{
    public class MainViewModel : BindableBase, IConfigureService
    {
        public MainViewModel(IRegionManager regionManager)
        {
            MenuBars = new ObservableCollection<MenuBar>();
            UserName = " --- ";
            this.regionManager = regionManager;

            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            GoBackCommand = new DelegateCommand(() =>
            {
                if (journal!= null && journal.CanGoBack)
                    journal.GoBack();
            });
            GoForwardCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoForward)
                    journal.GoForward();
            });

        }

        private readonly IRegionManager regionManager;
        private IRegionNavigationJournal journal;

        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });
        }

        // ======================== Command =====================
        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }

        // ======================== Models =======================
        private ObservableCollection<MenuBar> menuBars;
        private String userName;


        // ===================== Getter & Setter ===================
        public ObservableCollection<MenuBar> MenuBars 
        { 
            get { return menuBars; } 
            set { menuBars = value; RaisePropertyChanged(); } 
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }

        // ==================== Private =====================
        public void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Icon = "Home", Title = "行情", NameSpace = "IndexView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookOutline", Title = "订单", NameSpace = "OrderView" });
        }

        // ===================== Override ======================

        // 配置首页初始化参数
        public void Configure()
        {
            UserName = AppSession.UserName;
            CreateMenuBar();
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView");
        }
    }
}
