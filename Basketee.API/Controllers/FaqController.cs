using Basketee.API.DTOs.Gen;
using Basketee.API.Services;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;

namespace Basketee.API.Controllers
{
    public class FaqController : ApiController
    {
        [HttpPost]
        [ActionName("get_all")]
        public NegotiatedContentResult<GetAllResponse> GetAllFAQs(GetFAQRequest request)
        {
            GetAllResponse resp = FaqServices.GetAll(request);
            return Content(HttpStatusCode.OK, resp);
        }
    }
}
