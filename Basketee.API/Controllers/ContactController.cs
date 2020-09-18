using Basketee.API.DTOs.Gen;
using Basketee.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Basketee.API.Controllers
{
    public class ContactController : ApiController
    {
        [HttpGet]
        [ActionName("get_contact")]
        public NegotiatedContentResult<GetContactResponse> GetContact()
        {
            GetContactResponse resp = ContactServices.GetDefaultContact();
            return Content(HttpStatusCode.OK, resp);
        }
    }
}
