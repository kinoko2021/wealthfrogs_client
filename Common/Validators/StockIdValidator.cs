using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using WealthFrogs.Common.Http;

namespace WealthFrogs.Common.Validators
{
    public class StockIdValidator : ValidationRule
    {
        private HttpRestClient client;

        public StockIdValidator()
        {
            this.client = new HttpRestClient();
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string str = "";
            try
            {
                if (value == null)
                {
                    return new ValidationResult(false, "证券代码不能为空");
                }
                if (((string)value).Length > 0)
                    str = ((String)value);

                BaseRequest request = new BaseRequest()
                {
                    Method = RestSharp.Method.Get,
                    Url = $"http://localhost:8000/api/choice/checkStockId?stockId={str}",
                };
                ApiResponse response = client.Execute(request);
                if (response.status == "ok" && response.message == "true")
                {
                    return ValidationResult.ValidResult;
                }
                else if (response.status == "ok" && response.message == "false")
                {
                    return new ValidationResult(false, "证券代码错误");
                }
                return new ValidationResult(false, "证券代码验证内部错误");
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "证券代码验证内部错误");
            }

            
        }
    }
}
