using log4net;
using System;
using System.Configuration;
using System.Web;
using System.Web.Script.Serialization;

namespace Basketee.API.Util
{
    public static class Logger
    {
       
        private static ILog log = LogManager.GetLogger("");
        private static bool IsMasked = Convert.ToBoolean(ConfigurationManager.AppSettings["IsMaskedLog"]);
        public static bool EnableLog = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableLog"]); 
        public static bool LogIsLive = Convert.ToBoolean(ConfigurationManager.AppSettings["LogIsLive"]);

        #region Log - Methods

        public static void LogTest()
        {
            LogConfig.ProductionConfig();
            LogicalThreadContext.Properties["date1"] = DateTime.Now.ToString("MM/dd/yyyy:hh:mm.ssffffff");
            LogicalThreadContext.Properties["RequestIp"] = HttpContext.Current.Request.UserHostAddress;
            LogicalThreadContext.Properties["customMessage"] = "Hello";
            log.Info("Hello");
        }

        public static void Log(LoggerLevel logType, string logMethod, MethodFormat modelFormat, object model)
        {
           
            if (!LogIsLive)
            {
                LogConfig.DebugConfig();
                if (modelFormat == MethodFormat.REQUEST)
                {
                    LogRequest(logType, logMethod, model);
                }
                else if (modelFormat == MethodFormat.RESPONSE)
                {
                    LogResponse(logType, logMethod, model);
                }
                else
                {
                    LogERROR(logType, logMethod, model);
                }
            }
            else
            {
                LogConfig.ProductionConfig();
                if (modelFormat == MethodFormat.REQUEST)
                {
                    LogRequest(logType, logMethod, model);
                }
                else if (modelFormat == MethodFormat.RESPONSE)
                {
                    LogResponse(logType, logMethod, model);
                }
                else
                {
                    LogERROR(logType, logMethod, model);
                }

            }
        }

        private static void LogRequest(LoggerLevel logType, string logMethod, object model)
        {
           
            string jsongString = new JavaScriptSerializer().Serialize(model);
            if (logType == LoggerLevel.INFO)
            {
                LogicalThreadContext.Properties["method_name"] = logMethod;
                LogicalThreadContext.Properties["type"] = MethodFormat.REQUEST;
                LogicalThreadContext.Properties["model"] = jsongString;
            }
        }

        private static void LogResponse(LoggerLevel logType, string logMethod, object model)
        {
          
            string jsongString = new JavaScriptSerializer().Serialize(model);
            if (logType == LoggerLevel.INFO)
            {
                LogicalThreadContext.Properties["method_name"] = logMethod;
                LogicalThreadContext.Properties["type"] = MethodFormat.RESPONSE;
                LogicalThreadContext.Properties["model"] = jsongString;
            }
        }
        private static void LogERROR(LoggerLevel logType, string logMethod, object model)
        {
            string jsongString = new JavaScriptSerializer().Serialize(model);
            if (logType == LoggerLevel.ERROR)
            {
                LogicalThreadContext.Properties["method_name"] = logMethod;
                LogicalThreadContext.Properties["type"] = MethodFormat.ERROR;
                LogicalThreadContext.Properties["model"] = jsongString;
            }
        }

        #endregion
    }

    public enum LoggerLevel
    {
        ERROR,
        INFO,
        DEBUG
    }

    public enum MethodFormat
    {
        REQUEST,
        RESPONSE,
        ERROR
    }
}
