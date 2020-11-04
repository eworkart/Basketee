using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Basketee.API.Services
{
    public class UploadServices
    {
        private UserServices _userServices = new UserServices();

        public Dictionary<string, object> UploadProfilePicture(HttpRequest httpRequest, int userType)
        {
            string message = string.Empty;
            Dictionary<string, object> dict = new Dictionary<string, object>();
            string _imgname = "";
            try
            {
                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {

                            int MaxContentLength = 1024 * 1024 * 5; //5 MB  

                            IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                            var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                            var extension = ext.ToLower();
                            if (!AllowedFileExtensions.Contains(extension))
                            {

                                message = string.Format("Please Upload image of type .jpg,.gif,.png.");
                                dict.Add("error", "0");
                                dict.Add("message", message);
                            }
                            else if (postedFile.ContentLength > MaxContentLength)
                            {

                                message = string.Format("Please Upload a file upto 1 mb.");
                                dict.Add("error", "0");
                                dict.Add("message", message);
                            }
                            else
                            {
                                string path = HttpContext.Current.Server.MapPath("~/extfiles/profile/");

                                int userId = 0;
                                if (httpRequest.Form["user_id"] != null)
                                    userId = httpRequest.Form["user_id"].ToInt();

                                string auth_token = string.Empty;
                                if (httpRequest.Form["auth_token"] != null)
                                    auth_token = httpRequest.Form["auth_token"].ToString();

                                if (userId > 0 && userType > 0)
                                {
                                    bool userExist = false;
                                    switch ((UserType)userType)
                                    {
                                        case UserType.SuperUser:
                                            path = HttpContext.Current.Server.MapPath("~/extfiles/profile/superuser/");
                                            userExist = SuperUserServices.CheckSuperUser(userId, auth_token, null);
                                            break;
                                        case UserType.AgentBoss:
                                            path = HttpContext.Current.Server.MapPath("~/extfiles/profile/agentboss/");
                                            userExist = AgentBossServices.CheckAgentBoss(userId, auth_token, null);
                                            break;
                                        case UserType.AgentAdmin:
                                            path = HttpContext.Current.Server.MapPath("~/extfiles/profile/agentadmin/");
                                            userExist = AgentAdminServices.CheckAdmin(userId, auth_token, null);
                                            break;
                                        case UserType.Driver:
                                            path = HttpContext.Current.Server.MapPath("~/extfiles/profile/driver/");
                                            userExist = DriverServices.CheckAuthDriver(userId, auth_token);
                                            break;
                                        case UserType.Consumer:
                                            path = HttpContext.Current.Server.MapPath("~/extfiles/profile/customer/");
                                            userExist = _userServices.CheckAuthUser(userId, auth_token);
                                            break;
                                    }

                                    if (!userExist)
                                    {
                                        message = string.Format("Invalid User");
                                        dict.Add("error", "0");
                                        dict.Add("message", message);
                                    }

                                    if (!Directory.Exists(path))
                                    {
                                        Directory.CreateDirectory(path);
                                    }
                                    _imgname = string.Format("ProfileImg_{0}{1}", Guid.NewGuid().ToString(), extension);
                                    var _comPath = string.Format("{0}{1}", path, _imgname);

                                    postedFile.SaveAs(_comPath);
                                }
                                else
                                {
                                    message = string.Format("userId or userType cannot be empty");
                                    dict.Add("error", "0");
                                    dict.Add("message", message);
                                }

                                dict.Add("user_id", userId);
                                dict.Add("auth_token", auth_token);
                            }
                        }

                        dict.Add("success", "1");
                        dict.Add("message", _imgname);
                    }
                }
                else
                {
                    message = string.Format("Please Upload a image.");
                    dict.Add("error", "0");
                    dict.Add("message", message);
                }
                return dict;
            }
            catch (Exception ex)
            {
                dict.Add("error", "ex");
                dict.Add("message", ex.Message);
                return dict;
            }
        }
    }
}
