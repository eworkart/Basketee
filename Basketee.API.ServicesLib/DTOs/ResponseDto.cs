using Basketee.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.Util;
using System.Net;

namespace Basketee.API.DTOs
{
    public class ResponseDto
    {
        public int code { get; set; }
        public string message { get; set; }
        public int has_resource { get; set; }
        public HttpStatusCode httpCode { get; set; }
        public List<string> errors { get; set; }

        public void MakeExceptionResponse(Exception exception = null, string methodName ="")
        {
            this.code = 1;
            this.has_resource = 0;
            this.httpCode = HttpStatusCode.InternalServerError;

            string msg = "";
            Exception ex = exception;
            while (ex != null)
            {
                msg += ex.Message + "\r\n";
                ex = ex.InnerException;
            }
            this.message = ("exception: " + msg + "\r\n" + exception.StackTrace);
            //Util.Logger.Log(LoggerLevel.ERROR, methodName, MethodFormat.ERROR, exception);
            //this.message = MessagesSource.GetMessage("exception: " + msg + "\r\n"+ exception.StackTrace);
        }
    }
}