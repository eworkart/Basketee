using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.DTOs.SuperUser
{
    public class GetSuperUserDetailsResponse:ResponseDto
    {
        public SuperUserDetailsDto super_user_details { get; set; }
    }
}
