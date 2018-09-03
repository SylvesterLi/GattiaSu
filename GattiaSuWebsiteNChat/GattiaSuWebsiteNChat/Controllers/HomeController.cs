using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GattiaSuWebsiteNChat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public void playCRIS()
        {

            var client = new RestClient("https://westus.api.cognitive.microsoft.com/sts/v1.0/issueToken");
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "26c47b9e-82ee-f6cb-4bb0-9d3d6020dad1");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-length", "0");
            request.AddHeader("ocp-apim-subscription-key", "376f7e9f102e453186e79fcac3f878fa");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            IRestResponse response = client.Execute(request);


            
            //var request1 = new RestRequest(Method.POST);
            //request.AddHeader("postman-token", "af76cb09-c273-fa5d-1be6-9fb79db7c15c");
            //request.AddHeader("cache-control", "no-cache");
            //request.AddHeader("content-type", "application/ssml+xml");
            //request.AddHeader("X-Microsoft-OutputFormat", "riff-16khz-16bit-mono-pcm");
            //request.AddHeader("authorization", "Bearer "+ );
            //request.AddHeader("content-length", "199");
            //request.AddParameter("application/ssml+xml", "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"en-US\">\r\n<voice  name=\"Gattia_VoiceModel\">\r\n    hello world\r\n</voice> </speak>", ParameterType.RequestBody);
            //IRestResponse response1 = client1.Execute(request1);

            string url = "https://westus.voice.speech.microsoft.com/cognitiveservices/v1?deploymentId=399100a0-f981-4051-aefe-ac3f7299ad30";
            string tokenString= "Bearer " + response.Content.ToString();

            WebRequest webRequest = WebRequest.Create(url);
            webRequest.ContentType = "application/ssml+xml";
            webRequest.Headers.Add("X-MICROSOFT-OutputFormat", "riff-16khz-16bit-mono-pcm");
            webRequest.Headers["Authorization"] = tokenString;
            webRequest.Timeout = 6000000;
            webRequest.Method = "POST";


            //StreamReader sr = new StreamReader(@"D:\testcode\ssml_customer.txt");
            string ssml = "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"en-US\"><voice  name=\"Gattia_VoiceModel\">" + "hellow" + "</voice> </speak>"; //sr.ReadToEnd();

            byte[] btBodys = Encoding.UTF8.GetBytes(ssml);
            webRequest.ContentLength = btBodys.Length;
            webRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            WebResponse httpWebResponse = null;
            try
            {
                httpWebResponse = webRequest.GetResponse();
                Debug.WriteLine(httpWebResponse.GetType().ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            //Debug.WriteLine();
            //return View();
        }


        
    }
}