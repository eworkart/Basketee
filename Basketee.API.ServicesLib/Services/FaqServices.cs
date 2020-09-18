using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basketee.API.DTOs.Gen;
using Basketee.API.DAOs;
using Basketee.API.Models;

namespace Basketee.API.Services
{
    public class FaqServices
    {
        public static GetAllResponse GetAll(GetFAQRequest request)
        {
            GetAllResponse response = new GetAllResponse();
            try
            {
                using (FAQDao dao = new FAQDao())
                {
                    MFaq[] faqs = dao.GetFaqs(request.page_number,request.row_per_page);
                    if (faqs.Length > 0)
                    {
                        FAQDto[] faqDtos = new FAQDto[faqs.Length];
                        for (int i = 0; i < faqs.Length; i++)
                        {
                            FAQDto dto = new FAQDto();
                            dto.faq_id = faqs[i].FaqID;
                            dto.question = faqs[i].Question;
                            dto.answer = faqs[i].Answer;
                            faqDtos[i] = dto;
                        }
                        response.faq_list = faqDtos;
                        response.code = 0;
                        response.has_resource = 1;
                        response.message = MessagesSource.GetMessage("faq.list");
                        return response;
                    }
                    else
                    {
                        response.code = 0;
                        response.has_resource = 0;
                        response.message = MessagesSource.GetMessage("faq.list.not.found");
                    }
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