using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.DTOs.Gen;
using Basketee.API.Models;
using Basketee.API.DAOs;

namespace Basketee.API.Services
{
    public class DistrictServices
    {
        public static GetDistrictResponse GetDistricts()
        {
            GetDistrictResponse response = new GetDistrictResponse();
            //try {
            //    using (DistrictDao dao = new DistrictDao())
            //    {
            //        MDistrict[] districts = dao.GetDistricts();
            //        DistrictDetailsDto[] distDtos = new DistrictDetailsDto[districts.Length];
            //        for (int i = 0; i < districts.Length; i++)
            //        {
            //            MDistrict dist = districts[i];
            //            DistrictDetailsDto distDto = new DistrictDetailsDto();
            //            distDto.dist_id = dist.DistID;
            //            distDto.dist_name = dist.DistName;
            //            distDtos[i] = distDto;
            //        }
            //        response.district_details = distDtos;
            //        response.code = 0;
            //        response.has_resource = 1;
            //        response.message = MessagesSource.GetMessage("dist.list");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    response.MakeExceptionResponse(ex);
            //    return response;

            //}
            return response;
        }
    }
}