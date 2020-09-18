using Basketee.API.DAOs;
using Basketee.API.DTOs;
using Basketee.API.DTOs.Gen;
using Basketee.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.Services
{
    public class ContactServices
    {
        public static GetContactResponse GetDefaultContact()
        {
            GetContactResponse response = new GetContactResponse();
            using (ContactInfoDao dao = new ContactInfoDao())
            {
                ContactInfo contact = dao.GetDefaultContact();
                if (contact == null)
                {
                    MakeNoConatctResponse(response);
                    return response;
                }
                response.contact_details = new ContactDto();
                response.contact_details.contact_id = contact.CinfoID;
                response.contact_details.description = string.IsNullOrEmpty(contact.Description) ? string.Empty : contact.Description;
                response.contact_details.image_name = string.IsNullOrEmpty(contact.ContactInfoImage) ? string.Empty : ImagePathService.contactInfoImagePath + contact.ContactInfoImage;
                response.code = 0;
                response.has_resource = 1;
                response.message = MessagesSource.GetMessage("conatct.found");
            }
            return response;
        }

        public static void MakeNoConatctResponse(ResponseDto response)
        {
            response.code = 1;
            response.has_resource = 0;
            response.message = MessagesSource.GetMessage("conatct.not.found");
        }
    }
}
