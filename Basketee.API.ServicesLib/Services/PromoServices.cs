using System.Collections.Generic;
using System.Linq;
using Basketee.API.DTOs.Promo;
using Basketee.API.Models;
using Basketee.API.DAOs;
using Basketee.API.Services.Helpers;
using System;

namespace Basketee.API.Services
{
    public enum UserType
    {
        SuperUser = 1,
        AgentBoss = 2,
        AgentAdmin = 3,
        Driver = 4,
        Consumer = 5
    }

    public class PromoServices
    {
        private UserServices _userServices = new UserServices();

        public GetBannerResponse GetBanner(GetBannerRequest request)
        {
            GetBannerResponse response = new GetBannerResponse();
            try
            {
                switch (request.user_type)
                {
                    case (int)UserType.SuperUser:
                        if (!SuperUserServices.CheckSuperUser(request.user_id, request.auth_token, response))
                        {
                            response.message = MessagesSource.GetMessage("invalid.super.user");
                            return response;
                        }
                        break;
                    case (int)UserType.AgentBoss:
                        if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                        {
                            return response;
                        }
                        break;
                    case (int)UserType.AgentAdmin:
                        if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                        {
                            return response;
                        }
                        break;
                    case (int)UserType.Driver:
                        if (!DriverServices.CheckAuthDriver(request.user_id, request.auth_token))
                        {
                            _userServices.MakeNouserResponse(response);
                            return response;
                        }
                        break;
                    case (int)UserType.Consumer:
                        if (!_userServices.CheckAuthUser(request.user_id, request.auth_token))
                        {
                            _userServices.MakeNouserResponse(response);
                            return response;
                        }
                        break;
                    default:
                        {
                            response.has_resource = 0;
                            response.code = 1;
                            response.message = MessagesSource.GetMessage("invalid.user.type");
                            return response;
                        }
                }

                using (PromoDao dao = new PromoDao())
                {
                    GetBannerResponse dto = new GetBannerResponse();
                    PromoBanner promo = dao.FindByCategoty(request.category);
                    if (promo == null)
                    {
                        response.has_resource = 1;
                        response.code = 0;
                        response.message = MessagesSource.GetMessage("promo.banner.not.found");
                        return response;
                    }
                    if (promo != null)
                    {
                        PromoHelper.CopyFromEntity(dto, promo);
                    }
                    response = dto;
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("promo.banner.found");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        public GetBannerListResponse GetBannerList(GetBannerListRequest request)
        {
            GetBannerListResponse response = new GetBannerListResponse();
            try
            {
                switch (request.user_type)
                {
                    case (int)UserType.SuperUser:
                            if (!SuperUserServices.CheckSuperUser(request.user_id, request.auth_token, response))
                            {
                                response.message = MessagesSource.GetMessage("invalid.super.user");
                                return response;
                            }
                            break;                        
                    case (int)UserType.AgentBoss:
                        if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                        {
                            return response;
                        }
                        break;
                    case (int)UserType.AgentAdmin:
                        if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                        {
                            return response;
                        }
                        break;
                    case (int)UserType.Driver:
                        if (!DriverServices.CheckAuthDriver(request.user_id, request.auth_token))
                        {
                            _userServices.MakeNouserResponse(response);
                            return response;
                        }
                        break;
                    case (int)UserType.Consumer:
                        if (!_userServices.CheckAuthUser(request.user_id, request.auth_token))
                        {
                            _userServices.MakeNouserResponse(response);
                            return response;
                        }
                        break;
                    default:
                        {
                            response.has_resource = 0;
                            response.code = 1;
                            response.message = MessagesSource.GetMessage("invalid.user.type");
                            return response;
                        }
                }
                
                using (PromoDao dao = new PromoDao())
                {
                    List<PromoBanner> pList = dao.GetBannerList(request.page_number, request.records_per_page);
                    if (pList.Count <= 0)
                    {
                        response.has_resource = 1;
                        response.code = 0;
                        response.message = MessagesSource.GetMessage("promo.banner.not.found");
                        return response;
                    }
                    BannerDto[] promoDtos = new BannerDto[pList.Count()];
                    for (int i = 0; i < pList.Count; i++)
                    {
                        BannerDto dto = new BannerDto();
                        PromoHelper.CopyFromEntity(dto, pList[i]);
                        promoDtos[i] = dto;
                    }
                    response.banners = promoDtos;
                    response.has_resource = 1;
                    response.code = 0;
                    response.message = MessagesSource.GetMessage("promo.banner.found");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        public GetInfoBannerListResponse GetInfoBannerList(GetInfoBannerListRequest request)
        {
            GetInfoBannerListResponse response = new GetInfoBannerListResponse();
            try
            {
                switch (request.user_type)
                {
                    case (int)UserType.SuperUser:
                        if (!SuperUserServices.CheckSuperUser(request.user_id, request.auth_token, response))
                        {
                            response.message = MessagesSource.GetMessage("invalid.super.user");
                            return response;
                        }
                        break;
                    case (int)UserType.AgentBoss:
                        if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                        {
                            return response;
                        }
                        break;
                    case (int)UserType.AgentAdmin:
                        if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                        {
                            return response;
                        }
                        break;
                    case (int)UserType.Driver:
                        if (!DriverServices.CheckAuthDriver(request.user_id, request.auth_token))
                        {
                            _userServices.MakeNouserResponse(response);
                            return response;
                        }
                        break;
                    case (int)UserType.Consumer:
                        if (!_userServices.CheckAuthUser(request.user_id, request.auth_token))
                        {
                            _userServices.MakeNouserResponse(response);
                            return response;
                        }
                        break;
                    default:
                        {
                            response.has_resource = 0;
                            response.code = 1;
                            response.message = MessagesSource.GetMessage("invalid.user.type");
                            return response;
                        }
                }

                using (PromoDao dao = new PromoDao())
                {
                    List<PromoInfo> pList = dao.GetInfoBannerList(request.page_number, request.records_per_page);
                    if(pList.Count <= 0)
                    {
                        response.has_resource = 1;
                        response.code = 0;
                        response.message = MessagesSource.GetMessage("promo.info.not.found");
                        return response;
                    }
                    InfoBannerListDto[] promoDtos = new InfoBannerListDto[pList.Count()];
                    for (int i = 0; i < pList.Count; i++)
                    {
                        InfoBannerListDto dto = new InfoBannerListDto();
                        PromoHelper.CopyFromEntity(dto, pList[i]);
                        promoDtos[i] = dto;
                    }
                    response.info_banners = promoDtos;
                    response.has_resource = 1;
                    response.code = 0;
                    response.message = MessagesSource.GetMessage("promo.info.found");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }

        public InfoBannerResponse GetInfoBanner(GetInfoBannerRequest request)
        {
            InfoBannerResponse response = new InfoBannerResponse();
            try
            {
                switch (request.user_type)
                {
                    case (int)UserType.SuperUser:
                        if (!SuperUserServices.CheckSuperUser(request.user_id, request.auth_token, response))
                        {
                            response.message = MessagesSource.GetMessage("invalid.super.user");
                            return response;
                        }
                        break;
                    case (int)UserType.AgentBoss:
                        if (!AgentBossServices.CheckAgentBoss(request.user_id, request.auth_token, response))
                        {
                            return response;
                        }
                        break;
                    case (int)UserType.AgentAdmin:
                        if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                        {
                            return response;
                        }
                        break;
                    case (int)UserType.Driver:
                        if (!DriverServices.CheckAuthDriver(request.user_id, request.auth_token))
                        {
                            _userServices.MakeNouserResponse(response);
                            return response;
                        }
                        break;
                    case (int)UserType.Consumer:
                        if (!_userServices.CheckAuthUser(request.user_id, request.auth_token))
                        {
                            _userServices.MakeNouserResponse(response);
                            return response;
                        }
                        break;
                    default:
                        {
                            response.has_resource = 0;
                            response.code = 1;
                            response.message = MessagesSource.GetMessage("invalid.user.type");
                            return response;
                        }
                }

                using (PromoDao dao = new PromoDao())
                {
                    List<PromoInfo> bList = dao.GetInfoBanners();
                    if (bList.Count <= 0)
                    {
                        response.has_resource = 1;
                        response.code = 0;
                        response.message = MessagesSource.GetMessage("promo.info.not.found");
                        return response;
                    }
                    InfoBannerDto[] promoDtos = new InfoBannerDto[bList.Count()];
                    for (int i = 0; i < bList.Count; i++)
                    {
                        InfoBannerDto dto = new InfoBannerDto();
                        PromoHelper.CopyFromEntity(dto, bList[i]);
                        promoDtos[i] = dto;
                    }
                    response.info_banners = promoDtos;
                    response.has_resource = 1;
                    response.code = 0;
                    response.message = MessagesSource.GetMessage("promo.info.found");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }

            return response;
        }
    }
}