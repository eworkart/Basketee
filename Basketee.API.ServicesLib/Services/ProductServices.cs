using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.DTOs.Product;
using Basketee.API.DAOs;
using Basketee.API.Models;
using Basketee.API.Services.Helpers;

namespace Basketee.API.Services
{
    public class ProductServices
    {
        private UserServices _userServices = new UserServices();

        public GetProductListResponse GetProductList(GetProductListRequest request)
        {
            GetProductListResponse response = new GetProductListResponse();
            try
            {
                if (request.is_admin)
                {
                    if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token,response))
                    {
                        //_userServices.MakeNouserResponse(response);
                        return response;
                    }
                }
                else
                {
                    if (!_userServices.CheckAuthUser(request.user_id, request.auth_token))
                    {
                        _userServices.MakeNouserResponse(response);
                        return response;
                    }
                }
               
                using (ProductDao dao = new ProductDao())
                {
                    List<Product> pList = dao.GetProducts(request.page_number, request.row_per_page);
                    int totalCount = dao.GetTotalCount();
                    response.product_details = new ProductPagenationDetailsDto();
                    response.product_details.total_num_products = totalCount;
                    ProductDto[] prodDtos = new ProductDto[pList.Count()];
                    for (int i = 0; i < pList.Count; i++)
                    {
                        ProductDto dto = new ProductDto();
                        ProductHelper.CopyFromEntity(dto, pList[i]);
                        prodDtos[i] = dto;

                        response.products = prodDtos;
                        //response.has_exchange = (pList[i].ProductExchanges.Count > 0 ? 1 : 0);
                        //if (response.has_exchange == 1)
                        //{
                        //    if (response.exchange == null)
                        //        response.exchange = new List<ExchangeDto>();
                        //    foreach (var item in pList[i].ProductExchanges.ToList())
                        //    {
                        //        ExchangeDto exDto = new ExchangeDto();
                        //        ProductHelper.CopyFromEntity(exDto, item);
                        //        response.exchange.Add(exDto);
                        //    }
                        //}
                    }
                    
                    var reminder = dao.GetRemindersForProducts();
                    response.has_reminder = (reminder == null ? 0 : 1);                    
                    ProductHelper.CopyFromEntity(response, reminder);

                    response.has_resource = 1;
                    response.code = 0;
                    response.message = MessagesSource.GetMessage("has.products");
                }
            }
            catch (Exception ex)
            {
                response.MakeExceptionResponse(ex);
            }
            return response;
        }

        public GetProductDetailsResponse GetProductDetails(GetProductDetailsRequest request)
        {
            GetProductDetailsResponse response = new GetProductDetailsResponse();
            try
            {
                if (request.is_admin)
                {
                    if (!AgentAdminServices.CheckAdmin(request.user_id, request.auth_token, response))
                    {
                        //_userServices.MakeNouserResponse(response);
                        return response;
                    }
                }
                else
                {
                    if (!_userServices.CheckAuthUser(request.user_id, request.auth_token))
                    {
                        _userServices.MakeNouserResponse(response);
                        return response;
                    }
                }

                using (ProductDao dao = new ProductDao())
                {
                    ProductDetailsDto dto = new ProductDetailsDto();
                    Product prd = dao.FindProductById(request.product_id);
                    ProductHelper.CopyFromEntity(dto, prd);
                    response.product_details = dto;
                    response.code = 0;
                    response.has_resource = 1;
                    response.message = MessagesSource.GetMessage("has.prod.details");
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