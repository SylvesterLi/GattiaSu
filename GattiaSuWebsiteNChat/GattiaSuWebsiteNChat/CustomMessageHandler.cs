using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Text;
using Senparc.Weixin;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.MessageHandlers;

namespace GattiaSuWebsiteNChat
{
    public class CustomMessageHandler : MessageHandler<MessageContext<IRequestMessageBase, IResponseMessageBase>>
    {


        public CustomMessageHandler(Stream inputStream, PostModel postModel)
            : base(inputStream, postModel)
        {

        }


        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            //在这里可以返回各种各样的消息给客户。详情参见博客

            var responseMessage = base.CreateResponseMessage<ResponseMessageText>(); //ResponseMessageText也可以是News等其他类型
            responseMessage.Content = "欢迎来到佳蒂亚树！\r\n" + "访问博客点击：http://wicrosoft.ml" + "\r\n\r\n" + "Gattia Su是游戏《彩虹岛》海龙王的侍女，其BGM佳蒂亚树也相当好听。" + "\r\n" + "欢迎点击链接欣赏\r\n" + "http://music.163.com/m/song?id=28445647&userid=37542553" + "本公众号当前支持服务：1.发送其他消息默认接入智能QnA Bot！\r\n 这条消息来自DefaultResponseMessage。";

            return responseMessage;
        }


        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var result = new StringBuilder();
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            //responseMessage.Content

            //功能预定：
            //在微信公众平台得到语音


            /*
            
            if (requestMessage.Content == "家")
            {

                responseMessage.Content = "当前家中情况如下：\r\n" + "温度：18℃\r\n" + "湿度：适合";//这里的requestMessage.FromUserName也可以直接写成base.WeixinOpenId
                return responseMessage;

            }
            else if (requestMessage.Content == "车")
            {

                responseMessage.Content = "正在查找您的车...";//这里的requestMessage.FromUserName也可以直接写成base.WeixinOpenId
            }
            else if (requestMessage.Content == "换")
            {

                responseMessage.Content = "好的，您可以开启摄像头，我们能够根据您的心情给您更适合的放松";//这里的requestMessage.FromUserName也可以直接写成base.WeixinOpenId
            }
            else
            {
                //bot进行服务
                responseMessage.Content = "接收到消息：\r\n" + requestMessage.Content;// + requestMessage.Content + "\r\n Gattia回复你：" + CognitiveService.BotService(requestMessage.Content);//这里的requestMessage.FromUserName也可以直接写成base.WeixinOpenId          
            }
            
             */

            //\r\n用于换行，requestMessage.Content即用户发过来的文字内容

            return responseMessage;
        }
        public override IResponseMessageBase OnUnknownTypeRequest(RequestMessageUnknownType requestMessage)
        {
            /*
             * 此方法用于应急处理SDK没有提供的消息类型，
             * 原始XML可以通过requestMessage.RequestDocument（或this.RequestDocument）获取到。
             * 如果不重写此方法，遇到未知的请求类型将会抛出异常（v14.8.3 之前的版本就是这么做的）
             */
            var msgType = MsgTypeHelper.GetRequestMsgTypeString(requestMessage.RequestDocument);
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "未知消息类型：" + msgType;

            WeixinTrace.SendCustomLog("未知请求消息类型", requestMessage.RequestDocument.ToString());//记录到日志中

            return responseMessage;
        }

        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            string result = "";
            string CVServiceSwitch = "";//CVServiceSwitch是根据上一次发送的内容 赋值
            if (CurrentMessageContext.RequestMessages[CurrentMessageContext.RequestMessages.Count - 2] is RequestMessageText)
            {
                CVServiceSwitch = (CurrentMessageContext.RequestMessages[CurrentMessageContext.RequestMessages.Count - 2] as RequestMessageText).Content;
            }
            //计算机视觉
            if (CVServiceSwitch == "换")
            {
                //暂时不调了

                //float f = CognitiveService.ComputerVisionService(requestMessage.PicUrl);
                //if (f==1)
                //{
                //    result = "今天心情不错嘛！\r\n"+"我给你的生活加了一点小浪漫哦~";
                //}
                //else
                //{
                result = "今天需要放松一下心情吗？\r\n你看起来很累！";
                //}

            }

            else
            {
                //此时CVServiceSwitch已经失效
                result = "识别命令已失效，请重新选择识别功能！\r\n" + "If you need more help，contact me:NarisDrum@outlook.com";
            }
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();

            responseMessage.Content = result;
            return responseMessage;
        }

    }
}