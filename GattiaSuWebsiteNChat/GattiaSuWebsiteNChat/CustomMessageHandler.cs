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


        /// <summary>
        /// 默认回复
        /// 关注、未知消息
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            //在这里可以返回各种各样的消息给客户。详情参见博客

            var responseMessage = base.CreateResponseMessage<ResponseMessageText>(); //ResponseMessageText也可以是News等其他类型
            responseMessage.Content = "欢迎来到佳蒂亚树！\r\n" + "访问博客点击：http://wicrosoft.ml" + "\r\n\r\n" + "Gattia Su是游戏《彩虹岛》海龙王的侍女，其BGM佳蒂亚树也相当好听。" + "\r\n" + "欢迎点击链接欣赏\r\n" + "http://music.163.com/m/song?id=28445647&userid=37542553" + "本公众号当前支持服务：1.发送其他消息默认接入智能QnA Bot！\r\n 这条消息来自DefaultResponseMessage。";

            return responseMessage;
        }

        /// <summary>
        /// 当接收到文本消息
        /// 判断命令：1.[是CV就回复1] 2.[是计算机视觉就回复2] 3.[是Flow（暂时预定）就回复3]
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {

            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();

            if (requestMessage.Content == "1")
            {
                responseMessage.Content = "下一步发送图片，进行计算机视觉识别";
            }
            else if (requestMessage.Content == "2")
            {
                responseMessage.Content = "下一步发送图片，进行普通机车与动车组图像识别";
            }
            else if (requestMessage.Content == "3")
            {
                responseMessage.Content = "Microsoft Flow功能正在开发中，敬请期待！";

            }
            else
            {
                responseMessage.Content = "未知命令哦！Gattia Bot为您服务!(下一步将进行语音开发)";
            }



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
            if (CurrentMessageContext.RequestMessages.Count > 0)
            {
                if (CurrentMessageContext.RequestMessages[CurrentMessageContext.RequestMessages.Count - 2] is RequestMessageText)
                {
                    CVServiceSwitch = (CurrentMessageContext.RequestMessages[CurrentMessageContext.RequestMessages.Count - 2] as RequestMessageText).Content;
                }

                //计算机视觉
                switch (CVServiceSwitch)
                {
                    case "1":
                        //处理图片-计算机视觉
                        //requestMessage.PicUrl
                        result = CognitiveServiceTools.ComputerVision(requestMessage.PicUrl);

                        break;
                    case "2":
                        //处理图片-火车识别
                        result = CognitiveServiceTools.TrainCog(requestMessage.PicUrl);
                        break;
                    case "3":
                        result = "Current function have not been released!";

                        break;

                    default:
                        result = "识别命令已失效，请重新选择识别功能！\r\n" + "发送[1]选择计算机视觉\r\n" + "发送[2]选择自定义影像\r\n" + "发送[3]选择微软Flow服务\r\n" + "If you need more help，contact me:NarisDrum@outlook.com";
                        break;

                }

                var responseMessage = base.CreateResponseMessage<ResponseMessageText>();


                responseMessage.Content = result;
                //这时候才赋值 返回回复消息
                return responseMessage;
            }//if
            else
            {
                var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
                responseMessage.Content= "识别命令已失效，请重新选择识别功能！\r\n\r\n" + "发送[1]选择计算机视觉\r\n\r\n" + "发送[2]选择自定义影像\r\n\r\n" + "发送[3]选择微软Flow服务\r\n\r\n" + "If you need more help，contact me:NarisDrum@outlook.com";
                return responseMessage;
            }
        }
    }

}
