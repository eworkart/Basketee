//TODO Move it somewhere appropriate
using Basketee.API.DTOs;
using Basketee.API.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

//namespace System.Web.Http
//{

//    using Basketee.API.DTOs;
//    using Basketee.API.Services;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Net;
//    using System.Web.Http.ModelBinding;
//    using System.Web.Http.Results;

//    public static class ApiControllerExtensions
//    {

//        public static ResponseDto GetValidationResponse(this ApiController controller)
//        {
//            ModelStateDictionary modelState = controller.ModelState;

//            var errors = new List<string>();
//            foreach (var modelStateVal in modelState.Values.Select(d => d.Errors))
//            {
//                errors.AddRange(modelStateVal.Select(error => error.ErrorMessage));
//            }

//            ResponseDto r = new ResponseDto();
//            r.code = 0;
//            r.httpCode = HttpStatusCode.BadRequest;
//            r.errors = errors;
//            r.message = errors.Join("\n");

//            return r;
//        }

//        public static NegotiatedContentResult<T> Content<T>(this ApiController controller, HttpStatusCode statusCode, T value)
//        {
//            return controller.Content(statusCode, value);
//        }

//    }

//}

namespace Basketee.API.Controllers
{

    public enum AuthRoles {
        Superuser,
        Boss,
        Admin,
        Driver,
        Consumer
    }

    public class ActionInputValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            if (!actionContext.ModelState.IsValid)
            {
                var controller = ((ApiController)actionContext.ControllerContext.Controller);
                ModelStateDictionary modelState = controller.ModelState;

                var errors = new List<string>();
                foreach (var modelStateVal in modelState.Values.Select(d => d.Errors))
                {
                    errors.AddRange(modelStateVal.Select(error => error.ErrorMessage));
                }

                ResponseDto r = new ResponseDto();
                r.code = 0;
                r.httpCode = HttpStatusCode.BadRequest;
                r.errors = errors;
                r.message = errors.Join("\n");

                actionContext.Response = actionContext.Request.CreateResponse(r.httpCode, r, JsonMediaTypeFormatter.DefaultMediaType);
                return;
            }
            else
            {
                base.OnActionExecuting(actionContext);
            }
        }



        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
    }

    public class AuthenticationValidationFilter : ActionFilterAttribute {

        private AuthRoles _role;

        public AuthenticationValidationFilter(AuthRoles role) {
            this._role = role;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            var controller = ((ApiController)actionContext.ControllerContext.Controller);

            switch (this._role)
            {
                case AuthRoles.Superuser:

                    break;
                case AuthRoles.Boss:

                    break;
                case AuthRoles.Admin:
                    break;
                case AuthRoles.Driver:
                    break;
                case AuthRoles.Consumer:

                    //Consumer consumer = null;
                    //using (UserDao dao = new UserDao())
                    //{
                    //    consumer = GetAuthUser(dao, userId, authCode, withDetails);
                    //    if (consumer == null)
                    //        return false;
                    //}
                    //if (consumer.ConsBlocked || (!consumer.ConsActivated))
                    //{
                    //    return false;
                    //}
                    //return consumer != null;


                    break;
                default:
                    break;
            }


            //if (!actionContext.ModelState.IsValid)
            //{
            //    var controller = ((ApiController)actionContext.ControllerContext.Controller);
            //    ModelStateDictionary modelState = controller.ModelState;

            //    var errors = new List<string>();
            //    foreach (var modelStateVal in modelState.Values.Select(d => d.Errors))
            //    {
            //        errors.AddRange(modelStateVal.Select(error => error.ErrorMessage));
            //    }

            //    ResponseDto r = new ResponseDto();
            //    r.code = 0;
            //    r.httpCode = HttpStatusCode.Unauthorized;
            //    //r.errors = errors;
            //    r.message = "Wrong credentials";

            //    actionContext.Response = actionContext.Request.CreateResponse(r.httpCode, r, JsonMediaTypeFormatter.DefaultMediaType);
            //    return;
            //}
            //else
            //{
            //    base.OnActionExecuting(actionContext);
            //}
        }



        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }

    }
}
