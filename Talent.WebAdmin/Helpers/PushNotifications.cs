using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CorePush.Google;
using Newtonsoft.Json;

namespace Talent.WebAdmin.Helpers
{
    public class PushNotificationHelpers
    {
        public static async Task SendPushNotification(string deviceToken, GoogleNotification notification)
        {
            using (var fcm = new FcmSender("AAAAehHDqnY:APA91bFt2TXJu1iJK6OyG53y1P8dMMbyQrOm78ZloGdkyaMWv6BV_ih2D8QBZsjr-BPL11MwMQ4g5iRoByaCebSUaMZFFkTb3JkC1ktHsou7m7uKTzuo7_5xqsb3FvcQ3eTts9vAv8yb", "524284045942"))
            {
                await fcm.SendAsync(deviceToken, notification);
            }
        }

        public static GoogleNotification CreateNotification(string content)
        {
            return new GoogleNotification()
            {
                Data = new GoogleNotification.DataPayload()
                {
                    Message = content,
                }
            };
        }


        public static async Task<string> SendNotificationJSON(string deviceId, string title, string body, string click_action, string category, Guid id, Guid extraId)
        {
            string SERVER_KEY_TOKEN = "AAAAehHDqnY:APA91bFt2TXJu1iJK6OyG53y1P8dMMbyQrOm78ZloGdkyaMWv6BV_ih2D8QBZsjr-BPL11MwMQ4g5iRoByaCebSUaMZFFkTb3JkC1ktHsou7m7uKTzuo7_5xqsb3FvcQ3eTts9vAv8yb";
            var SENDER_ID = "524284045942";

            WebRequest tRequest;
            tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = " application/json";

            tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_KEY_TOKEN));
            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            var a = new
            {
                notification = new
                {
                    title,
                    body,
                    icon = "https://domain/path/to/logo.png",
                    click_action,
                    sound = "mySound"
                },
                data = new
                {
                    category,
                    id,
                    extraId
                },
                to = deviceId
            };

            byte[] byteArray = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(a));
            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();
            dataStream = tResponse.GetResponseStream();

            var tReader = new StreamReader(dataStream);
            string sResponseFromServer = tReader.ReadToEnd();

            tReader.Close();
            dataStream.Close();
            tResponse.Close();

            return sResponseFromServer;
        }
        public static async Task<string> SendNotificationJSONRemed(string deviceId, string title, string body, string click_action, string category, int? id, int? extraId)
        {
            string SERVER_KEY_TOKEN = "AAAAehHDqnY:APA91bFt2TXJu1iJK6OyG53y1P8dMMbyQrOm78ZloGdkyaMWv6BV_ih2D8QBZsjr-BPL11MwMQ4g5iRoByaCebSUaMZFFkTb3JkC1ktHsou7m7uKTzuo7_5xqsb3FvcQ3eTts9vAv8yb";
            var SENDER_ID = "524284045942";

            WebRequest tRequest;
            tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = " application/json";

            tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_KEY_TOKEN));
            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            var a = new
            {
                notification = new
                {
                    title,
                    body,
                    icon = "https://domain/path/to/logo.png",
                    click_action,
                    sound = "mySound"
                },
                data = new
                {
                    category,
                    id,
                    extraId
                },
                to = deviceId
            };

            byte[] byteArray = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(a));
            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();
            dataStream = tResponse.GetResponseStream();

            var tReader = new StreamReader(dataStream);
            string sResponseFromServer = tReader.ReadToEnd();

            tReader.Close();
            dataStream.Close();
            tResponse.Close();

            return sResponseFromServer;
        }
    }

    public class GoogleNotification
    {
        public class DataPayload
        {
            // Add your custom properties as needed
            [JsonProperty("message")]
            public string Message { get; set; }
        }

        [JsonProperty("priority")]
        public string Priority { get; set; } = "high";

        [JsonProperty("data")]
        public DataPayload Data { get; set; }
    }

}
