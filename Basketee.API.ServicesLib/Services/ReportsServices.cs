using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basketee.API.DTOs;
using Basketee.API.DAOs;
using Basketee.API.DTOs.Reports;
using Basketee.API.Models;
using Basketee.API.Services.Helpers;
using System.Globalization;

namespace Basketee.API.Services
{
    public class ReportsServices
    {
        public const string APPSETTING_REPORTPERIOD_RANGE = "ReportPeriodRange";

        public static GetProductResponse GetProductsByAgentBoss(GetProductRequest request)
        {
            GetProductResponse response = new GetProductResponse();
            try
            {
                if (request.is_boss)
                {
                    if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                    {
                        return response;
                    }
                }
                else
                {
                    if (!SuperUserServices.CheckSuperUser(request.user_id, request.auth_token, response))
                    {
                        return response;
                    }
                }
                response.product_names = new List<GetProductsDto>();
                using (OrderDao dao = new OrderDao())
                {
                    var productList = dao.GetProductsByAgentBoss(request.is_boss ? request.user_id : 0);
                    if (productList != null && productList.Count > 0)
                    {
                        response.product_names = productList.Select(r => new GetProductsDto
                        {
                            product_id = r.ProdID,
                            product_name = r.ProductName
                        }).ToList();
                    }
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("boss.prdt.listed");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }

        public static GetDriverNameResponse GetDriversByAgentBoss(GetDriverNameRequest request)
        {
            GetDriverNameResponse response = new GetDriverNameResponse();
            try
            {
                if (request.is_boss)
                {
                    if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                    {
                        return response;
                    }
                }
                else
                {
                    if (!SuperUserServices.CheckSuperUser(request.user_id, request.auth_token, response))
                    {
                        return response;
                    }
                }
                response.driver_names = new List<GetDriverNameDto>();
                using (OrderDao dao = new OrderDao())
                {
                    var driverList = dao.GetDriversByAgentBoss(request.is_boss ? request.user_id : 0);
                    if (driverList != null && driverList.Count > 0)
                    {
                        response.driver_names = driverList.Select(r => new GetDriverNameDto
                        {
                            driver_id = r.DrvrID,
                            driver_name = r.DriverName
                        }).ToList();
                    }
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("boss.drv.listed");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }

        public static ABossSellerRptResponse GetSellerReportByAgentBoss(ABossSellerRptRequest request)
        {
            var productIdList = request.products.Select(x => x.product_id).ToList();
            ABossSellerRptResponse response = new ABossSellerRptResponse();
            try
            {
                if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                response.sales_details = new List<ABossSellerRptDto>();
                using (OrderDao dao = new OrderDao())
                {
                    int periodRange = Common.GetAppSetting<int>(APPSETTING_REPORTPERIOD_RANGE, 6);

                    if (productIdList != null && productIdList.Count > 0)
                    {
                        string productIds = string.Join(",", productIdList.Select(n => n.ToString()).ToArray());

                        var sellerRpt = dao.GetSellerReportByAgentBoss(request.user_id, request.total_type, request.periodical_data, periodRange, productIds);
                        if (sellerRpt != null && sellerRpt.Count > 0)
                        {
                            response.sales_details = sellerRpt.Select(r => new ABossSellerRptDto
                            {
                                key = r.Period,
                                value = r.Value.ToDecimal()
                            }).ToList();
                        }
                    }
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("boss.sales.report");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }

        

        public static ABossReviewReportResponse GetReviewReportByAgentBoss(ABossReviewReportRequest request)
        {
            ABossReviewReportResponse response = new ABossReviewReportResponse();
            try
            {
                if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                response.service_rating = new List<ABossReviewReportDto>();
                using (OrderDao dao = new OrderDao())
                {
                    int periodRange = Common.GetAppSetting<int>(APPSETTING_REPORTPERIOD_RANGE, 6);
                    var reportDetails = dao.GetReviewReportByAgentBoss(request.user_id, request.driver_id, request.periodical_data, periodRange);
                    if (reportDetails != null && reportDetails.Count > 0)
                    {
                        response.service_rating = reportDetails.Select(r => new ABossReviewReportDto
                        {
                            key = r.Period,
                            value = r.Value.ToDecimal()
                        }).ToList();
                    }
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("boss.sales.report");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }

        }

        public static ABossReviewReasonResponse GetReviewReasonByAgentBoss(ABossReviewReasonRequest request)
        {
            ABossReviewReasonResponse response = new ABossReviewReasonResponse();
            try
            {
                if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                {
                    return response;
                }
                response.service_reason_rating = new List<ABossReviewReasonDto>();
                using (OrderDao dao = new OrderDao())
                {
                    int periodRange = Common.GetAppSetting<int>(APPSETTING_REPORTPERIOD_RANGE, 6);
                    var reportDetails = dao.GetReviewReasonByAgentBoss(request.user_id, request.driver_id, request.periodical_data, periodRange);
                    if (reportDetails != null && reportDetails.Count > 0)
                    {
                        response.service_reason_rating = reportDetails.Select(r => new ABossReviewReasonDto
                        {
                            key = r.ReasonText,
                            value = r.Value.ToDecimal()
                        }).ToList();
                    }
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("boss.rating.report");
                    return response;

                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }

        }

        public static GetAgencyNameResponse GetAgencyNames(GetAgencyNameRequest request)
        {

            GetAgencyNameResponse response = new GetAgencyNameResponse();
            try
            {
                if (!SuperUserServices.CheckSuperUser(request.user_id, request.auth_token, response))
                {
                    response.message = MessagesSource.GetMessage("invalid.super.user");
                    return response;
                }
                response.agency_names = new List<GetAgencyNameDto>();
                using (AgencyDao dao = new AgencyDao())
                {
                    var agencyList = dao.GetAgencies();
                    if (agencyList != null && agencyList.Count > 0)
                    {
                        response.agency_names = agencyList.Select(r => new GetAgencyNameDto
                        {
                            agency_id = r.AgenID,
                            agency_name = r.AgencyName
                        }).ToList();
                    }
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("agencies.listed");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }

        public static SUserReviewReportResponse GetSUserReviewReport(SUserReviewReportRequest request)
        {
            SUserReviewReportResponse response = new SUserReviewReportResponse();
            try
            {
                if (!SuperUserServices.CheckSuperUser(request.user_id, request.auth_token, response))
                {
                    response.message = MessagesSource.GetMessage("invalid.super.user");
                    return response;
                }
                response.service_rating = new List<SUserReviewReportDto>();
                using (OrderDao dao = new OrderDao())
                {
                    int periodRange = Common.GetAppSetting<int>(APPSETTING_REPORTPERIOD_RANGE, 6);
                    var reportDetails = dao.GetReviewReportBySUser(request.user_id, request.agency_id, request.periodical_data, periodRange);
                    if (reportDetails != null && reportDetails.Count > 0)
                    {
                        response.service_rating = reportDetails.Select(r => new SUserReviewReportDto
                        {
                            key = r.Period,
                            value = r.Value.ToDecimal()
                        }).ToList();
                    }
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("suser.sales.report");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }

        }
        public static SUserReviewReasonResponse GetSUserReviewReasonReport(SUserReviewReasonRequest request)
        {
            SUserReviewReasonResponse response = new SUserReviewReasonResponse();
            try
            {
                if (!SuperUserServices.CheckSuperUser(request.user_id, request.auth_token, response))
                {
                    response.message = MessagesSource.GetMessage("invalid.super.user");
                    return response;
                }
                response.service_reason_rating = new List<SUserReviewReasonDto>();
                using (OrderDao dao = new OrderDao())
                {
                    int periodRange = Common.GetAppSetting<int>(APPSETTING_REPORTPERIOD_RANGE, 6);
                    var reportDetails = dao.GetReviewReasonBySUser(request.user_id, request.agency_id, request.periodical_data, periodRange);
                    if (reportDetails != null && reportDetails.Count > 0)
                    {
                        response.service_reason_rating = reportDetails.Select(r => new SUserReviewReasonDto
                        {
                            key = r.ReasonText,
                            value = r.Value.ToDecimal()
                        }).ToList();
                    }
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("suser.rating.report");
                    return response;

                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }

        }
        public static SUserSellerRptResponse GetSellerReportBySuperUser(SUserSellerRptRequest request)
        {
            SUserSellerRptResponse response = new SUserSellerRptResponse();
            try
            {
                var productIdList = request.products.Select(x => x.product_id).ToList();
                if (!SuperUserServices.CheckSuperUser(request.user_id, request.auth_token, response))
                {
                    response.message = MessagesSource.GetMessage("invalid.super.user");
                    return response;
                }
                response.sales_details = new List<SUserSellerRptDto>();
                using (OrderDao dao = new OrderDao())
                {
                    int periodRange = Common.GetAppSetting<int>(APPSETTING_REPORTPERIOD_RANGE, 6);

                    if (productIdList != null && productIdList.Count > 0)
                    {
                        string productIds = string.Join(",", productIdList.Select(n => n.ToString()).ToArray());

                        var sellerRpt = dao.GetSellerReportBySUser(request.user_id, request.total_type, request.periodical_data, periodRange, request.number_of_products, productIds, request.agency_id);
                        if (sellerRpt != null && sellerRpt.Count > 0)
                        {
                            response.sales_details = sellerRpt.Select(r => new SUserSellerRptDto
                            {
                                key = r.Period,
                                value = r.Value.ToDecimal()
                            }).ToList();
                        }
                    }
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("suser.sales.report");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
                return response;
            }
        }




        //AgentBoss
        public static ReportKeyValueListResponseFloatDto GetAgentBossReportSellerOnTimeRequest(AgentBossReportSellerOnTimeRequest request)
        {

            ReportKeyValueListResponseFloatDto response = new ReportKeyValueListResponseFloatDto();

            try {
                if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                {
                    response.message = MessagesSource.GetMessage("invalid.agentboss");
                    return response;
                }
                
                SellerReportOnTimeRequest req = new SellerReportOnTimeRequest {
                    entity_id = request.driver_id,
                    periodical_data = request.periodical_data,
                    for_role = UserType.AgentBoss
                };

                return ReportsServices.GetSellerReportOnTime(req);
            } catch (Exception e)
            {

                response.MakeExceptionResponse(e);
                return response;
            
            }
        }


        public static ReportKeyValueListResponseFloatDto GetAgentBossSellerReportDelired(AgentBossReportSellerDeliveredRequest request)
        {

            ReportKeyValueListResponseFloatDto response = new ReportKeyValueListResponseFloatDto();

            try
            {
                if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                {
                    response.message = MessagesSource.GetMessage("invalid.agentboss");
                    return response;
                }

                SellerReportDeliveredRequest req = new SellerReportDeliveredRequest
                {
                    entity_id = request.driver_id,
                    periodical_data = request.periodical_data,
                    for_role = UserType.AgentBoss
                };

                return ReportsServices.GetSellerReportSellerReportDelivered(req);
            }
            catch (Exception e)
            {

                response.MakeExceptionResponse(e);
                return response;

            }

        }



        //Super User
        public static ReportKeyValueListResponseFloatDto GetSuperUserReportSellerOnTime(SuperUserReportSellerOnTimeRequest request)
        {
            ReportKeyValueListResponseFloatDto response = new ReportKeyValueListResponseFloatDto();

            try
            {
                if (!SuperUserServices.CheckSuperUser(request.user_id, request.auth_token, response))
                {
                    response.message = MessagesSource.GetMessage("invalid.super.user");
                    return response;
                }

                SellerReportOnTimeRequest req = new SellerReportOnTimeRequest
                {
                    entity_id = request.agency_id,
                    periodical_data = request.periodical_data,
                    for_role = UserType.SuperUser
                };

                return ReportsServices.GetSellerReportOnTime(req);
            }
            catch (Exception e)
            {

                response.MakeExceptionResponse(e);
                return response;

            }
        }


        public static ReportKeyValueListResponseFloatDto GetSuperUserSellerReportDelivered(SuperUserReportSellerDeliveredRequest request) {

            ReportKeyValueListResponseFloatDto response = new ReportKeyValueListResponseFloatDto();

            try
            {
                if (!SuperUserServices.CheckSuperUser(request.user_id, request.auth_token, response))
                {
                    response.message = MessagesSource.GetMessage("invalid.super.user");
                    return response;
                }

                SellerReportDeliveredRequest req = new SellerReportDeliveredRequest
                {
                    entity_id = request.agency_id,
                    periodical_data = request.periodical_data,
                    for_role = UserType.SuperUser
                };

                return ReportsServices.GetSellerReportSellerReportDelivered(req);
            }
            catch (Exception e)
            {

                response.MakeExceptionResponse(e);
                return response;

            }

        }


        //Common methods
        public static ReportKeyValueListResponseFloatDto GetSellerReportSellerReportDelivered(SellerReportDeliveredRequest request)
        {

            ReportKeyValueListResponseFloatDto resp = new ReportKeyValueListResponseFloatDto
            {

                //TODO Implement

                report = new List<ReportKeyValuePairDoubleDto>()
            };

            switch (request.for_role)   
            {
                case UserType.SuperUser:

                    resp.code = 1;
                    resp.has_resource = 1;
                    resp.httpCode = System.Net.HttpStatusCode.OK;

                    switch (request.periodical_data)
                    {
                        case 1: //Month
                            resp.report.AddRange(GetSuperUserReportDataDeliveredMonthly(request));
                            break;
                        case 2: //Week
                            resp.report.AddRange(GetSuperUserReportDataDeliveredWeekly(request));
                            break;

                        default:

                            resp.message = "Wrong period. Only accept 1 for Month and 2 for Week" + request.periodical_data;
                            resp.code = 0;
                            resp.has_resource = 0;
                            resp.httpCode = System.Net.HttpStatusCode.BadRequest;
                            break;
                    }

                    break;
                case UserType.AgentBoss:

                    resp.code = 1;
                    resp.has_resource = 1;
                    resp.httpCode = System.Net.HttpStatusCode.OK;

                    switch (request.periodical_data)
                    {
                        case 1: //Month
                            resp.report.AddRange(GetAgentBossReportDataDeliveredMonthly(request));
                            break;
                        case 2: //Week
                            resp.report.AddRange(GetAgentBossReportDataDeliveredWeekly(request));
                            break;

                        default:

                            resp.message = "Wrong period. Only accept 1 for Month and 2 for Week" + request.periodical_data;
                            resp.code = 0;
                            resp.has_resource = 0;
                            resp.httpCode = System.Net.HttpStatusCode.BadRequest;
                            break;
                    }

                    break;
                case UserType.AgentAdmin:
                    resp.message = "Wrong role " + request.for_role.ToCleanString();
                    resp.code = 0;
                    resp.has_resource = 0;
                    resp.httpCode = System.Net.HttpStatusCode.BadRequest;
                    break;
                case UserType.Driver:
                    resp.message = "Wrong role " + request.for_role.ToCleanString();
                    resp.code = 0;
                    resp.has_resource = 0;
                    resp.httpCode = System.Net.HttpStatusCode.BadRequest;
                    break;
                case UserType.Consumer:
                    resp.message = "Wrong role " + request.for_role.ToCleanString();
                    resp.code = 0;
                    resp.has_resource = 0;
                    resp.httpCode = System.Net.HttpStatusCode.BadRequest;
                    break;
                default:
                    resp.message = "Wrong role " + request.for_role.ToCleanString();
                    resp.code = 0;
                    resp.has_resource = 0;
                    resp.httpCode = System.Net.HttpStatusCode.BadRequest;
                    break;
            }



            return resp;
        }

        

        public static ReportKeyValueListResponseFloatDto GetSellerReportOnTime(SellerReportOnTimeRequest request)
        {

            ReportKeyValueListResponseFloatDto resp = new ReportKeyValueListResponseFloatDto
            {
                report = new List<ReportKeyValuePairDoubleDto>()
            };

            switch (request.for_role)
            {
                case UserType.SuperUser:

                    resp.code = 1;
                    resp.has_resource = 1;
                    resp.httpCode = System.Net.HttpStatusCode.OK;

                    switch (request.periodical_data)
                    {
                        case 1: //Month
                            resp.report.AddRange(GetSuperUserReportDataOnTimeMonthly(request));
                            break;
                        case 2: //Week
                            resp.report.AddRange(GetSuperUserReportDataOnTimeWeekly(request));
                            break;

                        default:

                            resp.message = "Wrong period. Only accept 1 for Month and 2 for Week" + request.periodical_data;
                            resp.code = 0;
                            resp.has_resource = 0;
                            resp.httpCode = System.Net.HttpStatusCode.BadRequest;
                            break;
                    }

                    break;
                case UserType.AgentBoss:

                    resp.code = 1;
                    resp.has_resource = 1;
                    resp.httpCode = System.Net.HttpStatusCode.OK;

                    switch (request.periodical_data)
                    {
                        case 1: //Month
                            resp.report.AddRange(GetAgentBossReportDataOnTimeMonthly(request));
                            break;
                        case 2: //Week
                            resp.report.AddRange(GetAgentBossReportDataOnTimeWeekly(request));
                            break;

                        default:

                            resp.message = "Wrong period. Only accept 1 for Month and 2 for Week" + request.periodical_data;
                            resp.code = 0;
                            resp.has_resource = 0;
                            resp.httpCode = System.Net.HttpStatusCode.BadRequest;

                            break;
                    }

                    break;
                case UserType.AgentAdmin:
                    resp.message = "Wrong role " + request.for_role.ToCleanString();
                    resp.code = 0;
                    resp.has_resource = 0;
                    resp.httpCode = System.Net.HttpStatusCode.BadRequest;
                    break;
                case UserType.Driver:
                    resp.message = "Wrong role " + request.for_role.ToCleanString();
                    resp.code = 0;
                    resp.has_resource = 0;
                    resp.httpCode = System.Net.HttpStatusCode.BadRequest;
                    break;
                case UserType.Consumer:
                    resp.message = "Wrong role " + request.for_role.ToCleanString();
                    resp.code = 0;
                    resp.has_resource = 0;
                    resp.httpCode = System.Net.HttpStatusCode.BadRequest;
                    break;
                default:
                    resp.message = "Wrong role " + request.for_role.ToCleanString();
                    resp.code = 0;
                    resp.has_resource = 0;
                    resp.httpCode = System.Net.HttpStatusCode.BadRequest;
                    break;
            }

            return resp;

        }


        //Reports On Time for SuperUser
        private static List<ReportKeyValuePairDoubleDto> GetSuperUserReportDataOnTimeMonthly(SellerReportOnTimeRequest request)
        {
            List<ReportKeyValuePairDoubleDto> resp = new List<ReportKeyValuePairDoubleDto>();

            List<String> lastFourMonth = GetLastMonths(4);

            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {

                resp.Add(new ReportKeyValuePairDoubleDto()
                {
                    key = lastFourMonth[i],
                    value = random.NextDouble() * 100.0
                });
            }

            return resp;
        }
        
        private static List<ReportKeyValuePairDoubleDto> GetSuperUserReportDataOnTimeWeekly(SellerReportOnTimeRequest request)
        {
            List<ReportKeyValuePairDoubleDto> resp = new List<ReportKeyValuePairDoubleDto>();

            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                
                resp.Add(new ReportKeyValuePairDoubleDto()
                {
                    key = "Week " + (i + 1),
                    value = random.NextDouble() * 100.0
                });
            }

            return resp;
        }

        //Reports On Time for AgentBoss
        private static List<ReportKeyValuePairDoubleDto> GetAgentBossReportDataOnTimeMonthly(SellerReportOnTimeRequest request)
        {
            List<ReportKeyValuePairDoubleDto> resp = new List<ReportKeyValuePairDoubleDto>();

            List<String> lastFourMonth = GetLastMonths(4);

            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {

                resp.Add(new ReportKeyValuePairDoubleDto()
                {
                    key = lastFourMonth[i],
                    value = random.NextDouble() * 100.0
                });
            }

            return resp;
        }

        //Reports OnTime for AgentBoss
        private static List<ReportKeyValuePairDoubleDto> GetAgentBossReportDataOnTimeWeekly(SellerReportOnTimeRequest request)
        {
            List<ReportKeyValuePairDoubleDto> resp = new List<ReportKeyValuePairDoubleDto>();

            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                
                resp.Add(new ReportKeyValuePairDoubleDto()
                {
                    key = "Week " + (i + 1),
                    value = random.NextDouble() * 100.0
                });
            }

            return resp;
        }


        //-------------
        //Reports Delivered for SuperUser
        private static List<ReportKeyValuePairDoubleDto> GetSuperUserReportDataDeliveredMonthly(SellerReportDeliveredRequest request)
        {
            List<ReportKeyValuePairDoubleDto> resp = new List<ReportKeyValuePairDoubleDto>();

            List<String> lastFourMonth = GetLastMonths(4);

            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {

                resp.Add(new ReportKeyValuePairDoubleDto()
                {
                    key = lastFourMonth[i],
                    value = random.NextDouble() * 100.0
                });
            }

            return resp;
        }


        
        private static List<ReportKeyValuePairDoubleDto> GetSuperUserReportDataDeliveredWeekly(SellerReportDeliveredRequest request)
        {
            List<ReportKeyValuePairDoubleDto> resp = new List<ReportKeyValuePairDoubleDto>();

            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                
                resp.Add(new ReportKeyValuePairDoubleDto()
                {
                    key = "Week " + (i + 1),
                    value = random.NextDouble() * 100.0
                });
            }

            return resp;
        }

        private static List<ReportKeyValuePairDoubleDto> GetAgentBossReportDataDeliveredMonthly(SellerReportDeliveredRequest request)
        {
            List<ReportKeyValuePairDoubleDto> resp = new List<ReportKeyValuePairDoubleDto>();

            List<String> lastFourMonth = GetLastMonths(4);
 
            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                
                resp.Add(new ReportKeyValuePairDoubleDto()
                {
                    key = lastFourMonth[i],
                    value = random.NextDouble() * 100.0
                });
            }

            return resp;
        }

        

        //Reports Delivered for AgentBoss
        private static List<ReportKeyValuePairDoubleDto> GetAgentBossReportDataDeliveredWeekly(SellerReportDeliveredRequest request)
        {
            List<ReportKeyValuePairDoubleDto> resp = new List<ReportKeyValuePairDoubleDto>();


            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                
                resp.Add(new ReportKeyValuePairDoubleDto()
                {
                    key = "Week " + (i + 1),
                    value = random.NextDouble() * 100.0
                });
            }

            return resp;
        }

        /// <summary>
        /// Helper method for fake data
        /// </summary>
        /// <param name="months">Number of month from the current month and back</param>
        /// <returns>List of strings in format fx 'Nov 2017'</returns>
        private static List<string> GetLastMonths(int months)
        {
            List<string> resp = new List<string>();
            var today = DateTime.Now;
            var year = today.Year;

            for (int i = 0; i < months; i++)
            {
                var monthNr = today.Month - i;

                if (monthNr < 1) {
                    year = year - 1;
                    monthNr = 12 + monthNr;
                }
                
                var month = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(monthNr) + " " + year;
                resp.Add(month);
            }
            

            return resp;
        }

    }
}
