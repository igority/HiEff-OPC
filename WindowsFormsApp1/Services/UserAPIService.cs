using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Services
{
    class UserAPIService
    {
        private string apiUrlBase;
        private string loginUrl;
        private string currentFIFOUrl;
        private string changeRobotStatusUrl;
        private string webSocketUrl;

        readonly string _accountSid;
        readonly string _secretKey;

        public UserAPIService()
        {
            apiUrlBase = "http://hiefficiencybar.com/api/";
            loginUrl = "user/me/";
            currentFIFOUrl = "user/order/?format=json&amp;limit=100";
            changeRobotStatusUrl = "robot/change/";
            webSocketUrl = "ws://hiefficiencybar.com:80/";

            _accountSid = "robot@hiefficiencybar.com";
            _secretKey = "password1231";

            //_accountSid = ConfigurationManager.AppSettings["Username"];
            //_secretKey = ConfigurationManager.AppSettings["Password"];

        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = new System.Uri(apiUrlBase);
            client.Authenticator = new HttpBasicAuthenticator(_accountSid, _secretKey);
            request.AddParameter("AccountSid", _accountSid, ParameterType.UrlSegment); // used on every request
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var myException = new ApplicationException(message, response.ErrorException);
                throw myException;
            }
            return response.Data;
        }

        public Boolean UpdateOrder(string json)
        {
            var request = new RestRequest(changeRobotStatusUrl, Method.POST);

            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;

            var client = new RestClient();
            client.BaseUrl = new System.Uri(apiUrlBase);
            client.Authenticator = new HttpBasicAuthenticator(_accountSid, _secretKey);
            request.AddParameter("AccountSid", _accountSid, ParameterType.UrlSegment); // used on every request
            IRestResponse response;
            try
            {
                response = client.Execute(request);
            }
            catch (Exception ex)
            {
                return false;
            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // OK
                return true;
            }
            else
            {
                // NOK
                return false;
            }

           

            //var request = new RestRequest();
            //request.Resource = currentFIFOUrl;
            //request.RootElement = "Order";

            //var client = new RestClient();
            //client.BaseUrl = new System.Uri(apiUrlBase);
            //client.Authenticator = new HttpBasicAuthenticator(_accountSid, _secretKey);
            //request.AddParameter("AccountSid", _accountSid, ParameterType.UrlSegment); // used on every request
            //var response = client.Execute<OrdersObj>(request);

            //if (response.ErrorException != null)
            //{
            //    const string message = "Error retrieving response.  Check inner details for more info.";
            //    var myException = new ApplicationException(message, response.ErrorException);
            //    throw myException;
            //}
            //return response.Content;
        }
    }
}

