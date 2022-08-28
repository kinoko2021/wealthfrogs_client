using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WealthFrogs.Common.Http
{
    public class HttpRestClient
    {
        protected readonly RestClient client;

        public HttpRestClient()
        {
            client = new RestClient();
        }

        public ApiResponse Execute(BaseRequest baseRequest)
        {
            RestRequest request = new RestRequest(baseRequest.Url, baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);

            if (baseRequest.Parameter != null)
            {
                request.AddJsonBody(baseRequest.Parameter);
            }
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
            else return new ApiResponse()
            {
                status = "error",
                message = "client network error",
            };
        }

        public ApiResponse<T> Execute<T>(BaseRequest baseRequest)
        {
            RestRequest request = new RestRequest(baseRequest.Url, baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);

            if (baseRequest.Parameter != null)
                request.AddJsonBody(baseRequest.Parameter);
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
            else return new ApiResponse<T>()
            {
                status = "error",
                message = "client network error",
            };
        }
    }
}
