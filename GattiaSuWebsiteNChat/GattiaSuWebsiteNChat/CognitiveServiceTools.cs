using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GattiaSuWebsiteNChat
{
    public static class CognitiveServiceTools
    {
        public static string TrainCog(string url)
        {
            var client = new RestClient("https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/f7bb3f9a-e1a5-44a9-b05c-2808e3ad7c43/url");
            var request = new RestRequest(Method.POST);
            
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Prediction-Key", "15e04ba80f464cb3aa6afdd4b117a1d7");
            request.AddParameter("undefined", "{\"Url\": \""+url+"\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response.Content;//这个应该要序列化 拿回来再说吧
        }
        public static  string ComputerVision(string url)
        {
            return "";
        }
    }
}