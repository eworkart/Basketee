using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basketee.API.DTOs.Reports;
using Basketee.API.Models;

namespace Basketee.API.Services.Helpers
{
    public class ReportsHelper
    {
        public static void CopyFromEntity(GetProductsDto dto, Product product)
        {
            dto.product_id = product.ProdID;
            dto.product_name = product.ProductName;
        }
        public static void CopyFromEntity(GetAgencyNameDto dto, Agency agency)
        {
            dto.agency_id = agency.AgenID;
            dto.agency_name = agency.AgencyName;
        }
    }
}
