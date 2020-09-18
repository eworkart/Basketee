using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Basketee.API.DTOs;
using Basketee.API.DTOs.Reports;
using Basketee.API.Services;
namespace Basketee.API.Controllers
{
    public class ReportsController : ApiController
    {
        [HttpPost]
        [ActionName("get_product_name")]
        public NegotiatedContentResult<GetProductResponse> PostGetProducts([FromBody]GetProductRequest request)
        {
            GetProductResponse resp = ReportsServices.GetProductsByAgentBoss(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("get_driver_name")]
        public NegotiatedContentResult<GetDriverNameResponse> PostGetDriverNames([FromBody]GetDriverNameRequest request)
        {
            GetDriverNameResponse resp = ReportsServices.GetDriversByAgentBoss(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("seller_report_aboss")]
        public NegotiatedContentResult<ABossSellerRptResponse> PostGetSellerReport([FromBody]ABossSellerRptRequest request)
        {
            ABossSellerRptResponse resp = ReportsServices.GetSellerReportByAgentBoss(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("review_report_aboss")]
        public NegotiatedContentResult<ABossReviewReportResponse> PostGetReviewRatingReport([FromBody]ABossReviewReportRequest request)
        {
            ABossReviewReportResponse resp = ReportsServices.GetReviewReportByAgentBoss(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("review_reason_aboss")]
        public NegotiatedContentResult<ABossReviewReasonResponse> PostGetReviewReasonReport([FromBody]ABossReviewReasonRequest request)
        {
            ABossReviewReasonResponse resp = ReportsServices.GetReviewReasonByAgentBoss(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("get_agency_name")]
        public NegotiatedContentResult<GetAgencyNameResponse> PostAgencyNames([FromBody]GetAgencyNameRequest request)
        {
            GetAgencyNameResponse resp = ReportsServices.GetAgencyNames(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("review_report_suser")]
        public NegotiatedContentResult<SUserReviewReportResponse> PostGetSUserReviewReport([FromBody]SUserReviewReportRequest request)
        {
            SUserReviewReportResponse resp = ReportsServices.GetSUserReviewReport(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("review_reason_suser")]
        public NegotiatedContentResult<SUserReviewReasonResponse> PostGetSUserReasonReview([FromBody]SUserReviewReasonRequest request)
        {
            SUserReviewReasonResponse resp = ReportsServices.GetSUserReviewReasonReport(request);
            return Content(HttpStatusCode.OK, resp);
        }
        [HttpPost]
        [ActionName("seller_report_suser")]
        public NegotiatedContentResult<SUserSellerRptResponse> PostGetSellerReportBySuperUser([FromBody]SUserSellerRptRequest request)
        {
            SUserSellerRptResponse resp = ReportsServices.GetSellerReportBySuperUser(request);
            return Content(HttpStatusCode.OK, resp);
        }

        //AgentBoss Reports

        [HttpPost]
        [ActionName("seller_report_ontime_delivered_aboss")]
        [ActionInputValidationFilter()]
        public NegotiatedContentResult<ReportKeyValueListResponseFloatDto> PostGetSellerReportOnTimeBoss([FromBody]AgentBossReportSellerOnTimeRequest request)
        {
            ReportKeyValueListResponseFloatDto resp = ReportsServices.GetAgentBossReportSellerOnTimeRequest(request); //GetSellerReportOnTime(request, UserType.AgentBoss);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("seller_report_delivered_aboss")]
        [ActionInputValidationFilter()]
        public NegotiatedContentResult<ReportKeyValueListResponseFloatDto> PostGetSellerReportDeliveredBoss([FromBody]AgentBossReportSellerDeliveredRequest request)
        {
            ReportKeyValueListResponseFloatDto resp = ReportsServices.GetAgentBossSellerReportDelired(request);
            return Content(HttpStatusCode.OK, resp);
        }

        //Super User Reports
        [HttpPost]
        [ActionName("seller_report_ontime_delivered_suser")]
        [ActionInputValidationFilter()]
        public NegotiatedContentResult<ReportKeyValueListResponseFloatDto> PostGetSellerReportOnTimeSUser([FromBody]SuperUserReportSellerOnTimeRequest request)
        {
            ReportKeyValueListResponseFloatDto resp = ReportsServices.GetSuperUserReportSellerOnTime(request);
            return Content(HttpStatusCode.OK, resp);
        }

        [HttpPost]
        [ActionName("seller_report_delivered_suser")]
        [ActionInputValidationFilter()]
        public NegotiatedContentResult<ReportKeyValueListResponseFloatDto> PostGetSellerReportDeliveredSUser([FromBody]SuperUserReportSellerDeliveredRequest request)
        {
            ReportKeyValueListResponseFloatDto resp = ReportsServices.GetSuperUserSellerReportDelivered(request);
            return Content(HttpStatusCode.OK, resp);
        }

        //Send Reports to AgentBoss or SuperUser

        [HttpPost]
        [ActionName("request_report_to_email")]
        [ActionInputValidationFilter]
        public NegotiatedContentResult<ResponseDto> PostRequestReportToEmail([FromBody] GetReportToEmailRequest request) {

            ResponseDto resp = new ResponseDto();

            resp.code = 1;
            resp.has_resource = 1;
            resp.httpCode = HttpStatusCode.OK;
            resp.message = "Report was successfully sent";

            return Content(resp.httpCode, resp);

        }

    }
}
