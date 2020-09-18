using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketee.API.Services
{
    public class ImagePathService
    {
        public static string  agentAdminImagePath= Common.GetAppSetting<string>("AgentAdminImagePath", string.Empty);
        public static string agentBossImagePath = Common.GetAppSetting<string>("AgentBossImagePath", string.Empty);
        public static string consumerImagePath = Common.GetAppSetting<string>("ConsumerImagePath", string.Empty);
        public static string driverImagePath = Common.GetAppSetting<string>("DriverImagePath", string.Empty);
        public static string superUserImagePath = Common.GetAppSetting<string>("SuperAdminImagePath", string.Empty);
        public static string productImagePath = Common.GetAppSetting<string>("ProductImagePath", string.Empty);
        public static string reminderImagePath = Common.GetAppSetting<string>("ReminderImagePath", string.Empty);
        public static string bannersImagePath = Common.GetAppSetting<string>("BannersImagePath", string.Empty);
        public static string promoInfoImagePath = Common.GetAppSetting<string>("PromoInfoImagePath", string.Empty);
        public static string contactInfoImagePath = Common.GetAppSetting<string>("ContactInfoImagePath", string.Empty);
    }
}
