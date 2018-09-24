using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

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
            request.AddParameter("undefined", "{\"Url\": \"" + url + "\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);


            TrainJson trainJson = JsonConvert.DeserializeObject<TrainJson>(response.Content);
            string str = "ID:" + trainJson.id
                + "\r\n\r\n为CRH的概率为：" + trainJson.predictions[0].probability * 100
                + "%\r\n\r\n为普通机车的概率为" + trainJson.predictions[2].probability * 100
                + "%\r\n\r\nSession创建时间：" + trainJson.created.ToString() + "\r\n\r\n本次识别服务结束。";


            return str;//这个应该要序列化 拿回来再说吧


        }
        public static string ComputerVision(string url)
        {



            return "";
        }


    }


    #region TrainJson Object


    public class TrainJson
    {
        public string id { get; set; }
        public string project { get; set; }
        public string iteration { get; set; }
        public DateTime created { get; set; }
        public Prediction[] predictions { get; set; }
    }

    public class Prediction
    {
        public float probability { get; set; }
        public string tagId { get; set; }
        public string tagName { get; set; }
    }


    #endregion


    class ComputerVisionJson
    {
        //可能用不上了

    }
}

