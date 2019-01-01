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
        //识别火车
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


            return str;


        }
        public static string ComputerVision(string url)
        {
            var client = new RestClient("https://eastus2.api.cognitive.microsoft.com/vision/v1.0/describe?maxCandidates=1&language=zh");
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "f7993c74-eb20-5ef7-63c2-4c8ce895b7c8");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("ocp-apim-subscription-key", "0779bf9ee1904a799f46f5bbe95cbe28");
            request.AddParameter("application/json", "{\"url\":\"" + url + "\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            ComputerVisionJson coVision = JsonConvert.DeserializeObject<ComputerVisionJson>(response.Content);

            string str = "请求ID:" + coVision.requestId
                + "\r\n\r\n图片描述：" + coVision.description.captions[0].text
                + "\r\n\r\n可信度为：" + coVision.description.captions[0].confidence * 100
                + "%\r\n\r\n本次识别服务结束。";


            return str;
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

    #region ComputerVisionJson

    public class ComputerVisionJson
    {
        public Description description { get; set; }
        public string requestId { get; set; }
        public Metadata metadata { get; set; }
    }

    public class Description
    {
        public string[] tags { get; set; }
        public Caption[] captions { get; set; }
    }

    public class Caption
    {
        public string text { get; set; }
        public float confidence { get; set; }
    }

    public class Metadata
    {
        public int height { get; set; }
        public int width { get; set; }
        public string format { get; set; }
    }

    #endregion


}

