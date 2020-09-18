using System;
using System.Configuration;

namespace Basketee.API.Services
{
    public static class Common
    {
        /// <summary>
        /// Get value from Configuration file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetAppSetting<T>(string key, T defaultValue)
        {
            if (!string.IsNullOrEmpty(key))
            {
                string value = ConfigurationManager.AppSettings[key];
                try
                {
                    if (value != null)
                    {
                        var theType = typeof(T);
                        if (theType.IsEnum)
                            return (T)Enum.Parse(theType, value.ToString(), true);

                        return (T)Convert.ChangeType(value, theType);
                    }

                    return default(T);
                }
                catch
                {
                    // ignored
                }
            }

            return defaultValue;
        }

        public static string GetStandardMobileNumber(string mobileNumber)
        {
            if (string.IsNullOrEmpty(mobileNumber))
            {
                return null;
            }
                if (mobileNumber.StartsWith("+91"))
                    return mobileNumber;
                else
                {
                    if (mobileNumber.StartsWith("0"))
                    {
                        mobileNumber = mobileNumber.Remove(0, 1);
                    }
                    return mobileNumber.Insert(0, "+91");
                }
            
        }

        public static string ToDateFormat(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
    }
}
