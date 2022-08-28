using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WealthFrogs.Common.Http;
using WealthFrogs.Common.Models.DTOs;
using WealthFrogs.Extensions;

namespace WealthFrogs.ViewModels
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        // 请求工具
        private readonly HttpRestClient client;
        private readonly IEventAggregator aggregator;
        public event Action<IDialogResult> RequestClose;
        public LoginViewModel(IEventAggregator aggregator)
        {
            client = new HttpRestClient();
            this.aggregator = aggregator;

            User = new UserDto();

            LoginCommand = new DelegateCommand<UserDto>(login);
        }

        public string Title { get; set; } = "WealthFrogs";

        // ==================== Command =====================
        public DelegateCommand<UserDto> LoginCommand { get; private set; }
        private void login(UserDto user)
        {
            if (string.IsNullOrWhiteSpace(User.userId) ||
                string.IsNullOrWhiteSpace(User.password))
            {
                return;
            }

            BaseRequest request = new BaseRequest()
            {
                Method = RestSharp.Method.Post,
                Url = "http://localhost:8000/api/login/login",
                Parameter = User
            };
            ApiResponse response = client.Execute(request);

            if (response != null && response.status == ("ok"))
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            else
            {
                //登录失败提示...
                aggregator.SendMessage(response.message, "Login");
            }

        }

        // ==================== Model =====================
        private UserDto user;

        // ==================== Getter & Setter =================
        public UserDto User
        {
            get { return user; }
            set { user = value; RaisePropertyChanged(); }
        }

        // ==================== Override =====================
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
    }
}
