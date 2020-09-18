using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Configuration;
using Basketee.API.DAOs;
using Basketee.API.Models;
using Microsoft.ApplicationInsights;

namespace Basketee.API.Services
{
    public class PushMessagingService
    {
        #region Telemetry
        private static TelemetryClient tm = new TelemetryClient();


        #endregion

        //TODO set the value here.
        const string PUSH_SENDER_ID = "";
        public const string APPSETTING_APPLICATION_ID_CONSUMER = "ApplicationIdConsumer";
        public const string APPSETTING_SENDER_ID_CONSUMER = "SenderIdConsumer";
        public const string APPSETTING_APPLICATION_ID_DRIVER = "ApplicationIdDriver";
        public const string APPSETTING_SENDER_ID_DRIVER = "SenderIdDriver";
        public const string APPSETTING_APPLICATION_ID_SUSER = "ApplicationIdSuperUser";
        public const string APPSETTING_SENDER_ID_SUSER = "SenderIdSuperUser";
        public const string APPSETTING_APPLICATION_ID_ABOSS = "ApplicationIdAgentBoss";
        public const string APPSETTING_SENDER_ID_ABOSS = "SenderIdAgentBoss";
        public const string APPSETTING_APPLICATION_ID_AADMIN = "ApplicationIdAgentAdmin";
        public const string APPSETTING_SENDER_ID_AADMIN = "SenderIdAgentAdmin";
        public const string APPSETTING_FCM_END_POINT = "FcmEndPoint";

        public enum PushType
        {
            TypeOne = 1,
            TypeTwo = 2,
            TypeThree = 3
        }

        public static string PushNotification(string appId, string appToken, string message)
        {
            //TODO
            return "";
        }

        public static void SendPushNotification(string deviceId, string body, string title, string applicationId, string senderId, int orderId, int driverId, int type)
        {
            NotifyMessage(deviceId, body, title, applicationId, senderId, orderId, driverId, type);
        }

        public static void NotifyMessage(string deviceId, string body, string title, string applicationId, string senderId, int orderId, int driverId, int type)
        {
            try
            {
                //Add this to conf gile
                string applicationID = Common.GetAppSetting<string>(applicationId, string.Empty); // ConfigurationManager.AppSettings["ApplicationId"].ToString();
                string senderID = Common.GetAppSetting<string>(senderId, string.Empty); //ConfigurationManager.AppSettings["SenderId"].ToString();
                string gcmEndPoint = Common.GetAppSetting<string>(APPSETTING_FCM_END_POINT, string.Empty);// ConfigurationManager.AppSettings["FcmEndPoint"].ToString();
                WebRequest tRequest = WebRequest.Create(gcmEndPoint);
                /*
                 WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                 */

                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                dynamic notifyData = string.Empty;

                if (orderId != 0 && driverId != 0)
                {
                     notifyData = new
                    {
                        to = deviceId,
                        priority = "high",
                        content_available = true,
                        notification = new
                        {
                            title = title,
                            body = body,
                            sound = "default"
                        },
                        data = new
                        {
                            type = type,
                            order_id = orderId,
                            driver_id = driverId
                        }
                    };
                }
                if (orderId != 0 && driverId == 0)
                {
                     notifyData = new
                    {
                        to = deviceId,
                        priority = "high",
                        content_available = true,
                        notification = new
                        {
                            title = title,
                            body = body,
                            sound = "default"
                        },
                        data = new
                        {
                            type = type,
                            order_id = orderId
                        }
                    };
                }
                if (driverId != 0 && orderId == 0)
                {
                     notifyData = new
                    {
                        to = deviceId,
                        priority = "high",
                        content_available = true,
                        notification = new
                        {
                            title = title,
                            body = body,
                            sound = "default"
                        },
                        data = new
                        {
                            type = type,
                            driver_id = driverId
                        }
                    };
                }
                if (driverId == 0 && orderId == 0)
                {
                    notifyData = new
                    {
                        to = deviceId,
                        priority = "high",
                        content_available = true,
                        notification = new
                        {
                            title = title,
                            body = body,
                            sound = "default"
                        },
                        data = new
                        {
                            type = type
                        }
                    };
                }

                //var notification  = new
                //{
                //    to = deviceId,
                //    data = new
                //    {
                //        body = body,
                //        title = title,
                //        sound = "default"
                //    }
                //};
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(notifyData);

                var properties = new Dictionary<string, string> {
                    { "firebase_payload", json },
                    { "firebase_api_key", applicationID },
                    { "device_id", deviceId }
                };
                tm.TrackTrace("Push Notification", properties);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderID));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                                LogMessage(deviceId, json.ToString(), str);

                                properties = new Dictionary<string, string> { { "firebase_payload", json } , { "firebase_response", str }, { "firebase_api_key", applicationID } };

                                tm.TrackEvent("DidSendPushNotification", properties);                             
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                string applicationID = Common.GetAppSetting<string>(applicationId, string.Empty);

                string senderID = Common.GetAppSetting<string>(senderId, string.Empty);
                string gcmEndPoint = Common.GetAppSetting<string>(APPSETTING_FCM_END_POINT, string.Empty);

                //string body, string title, string applicationId, string senderId, int orderId, int driverId, int type
                var properties = new Dictionary<string, string> {
                    { "device_id", deviceId },
                    { "title", title },
                    { "senderId", senderID },
                    { "firebase_api_key", applicationID },
                    };
                tm.TrackException(ex, properties);
            }
        }

        private static void LogMessage(string deviceId, string json, string resp)
        {
            using (NotificationLogDao dao = new NotificationLogDao())
            {
                NotificationLog log = new NotificationLog();
                log.ReceiverId = deviceId;
                log.JSONMessage = json;
                log.FCMResponse = resp;
                dao.Insert(log);
            }
        }
    }
}