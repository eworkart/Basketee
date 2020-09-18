using Basketee.API.DTOs.Gen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Basketee.API.Services
{
    public class SMSService
    {
        public static void SendSMS(string mobileNumber, string textMessage)
        {
            //TODO 
        }

        public static string SendOTP(string mobileNumber, string NoSPBU = "")
        {
            DataSendOTP data = new DataSendOTP();
            data.NoSPBU = NoSPBU;
            data.NoTelp = mobileNumber;
            //data.NoSPBU = "";
            //data.NoTelp = "+628122725643";
            string encKey = GenerateEncryptedKeySendOTP(data);
            JavaScriptSerializer _jsserializer = new JavaScriptSerializer();
            string strJSON = _jsserializer.Serialize(data);
            PertaminaServices.SVC_MS2Mobile service = new PertaminaServices.SVC_MS2Mobile();
            string responseMessageJSON = service.SendOTP(encKey, strJSON);
            //string responseMessageJSON = SVC_MS2Mobile.SendOTP(encKey, strJSON);
            SentOTPResponseFromService otpSentResponse = _jsserializer.Deserialize<SentOTPResponseFromService>(responseMessageJSON);
            return otpSentResponse.OTP;
        }

        private static string GenerateEncryptedKeySendOTP(DataSendOTP data)
        {
            string saltKey = "123@ptm";
            string key = data.NoSPBU + data.NoTelp + saltKey;

            string encKey = EncryptToSHA256(key);
            return encKey;
        }

        public static string EncryptToSHA256(string value)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));
                foreach (Byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }
    }

    public class DataSendOTP
    {
        public string NoSPBU { get; set; }
        public string NoTelp { get; set; }
    }
}