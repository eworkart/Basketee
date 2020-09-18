//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace Basketee.API.Services
//{
//    /// <summary>
//    /// Gets / returns a textual message for a given key string.
//    /// </summary>
//    public class MessagesSource
//    {
//        // TODO : This mapping may be moved to a database table and GetMessage method modified accordingly.
//        private static Dictionary<string, string> _Messages = new Dictionary<string, string>
//        {
//            {"cons.reg.ok", "User Registered Successfully"},
//            {"cons.reg.fail", "Registration failed"},
//            {"cons.reg.dupl","This phone number is already registered" },
//            {"no.user", "This user is not registered or active" },
//            {"user.activated", "User Activated Successfully" },
//            {"user.active", "User is already Activated" },
//            {"user.found","User Exists" },
//            {"otp.resent", "OTP Resent Successfully" },
//            {"passwd.reset", "Password Reset Successfully" },
//            {"no.reset", "Resetting password failed" },
//            {"login.ok", "Successfully logged in to the system" },
//            {"user.blocked", "Blocked User"},
//            {"user.not.active", "User not activated"},
//            {"user.details", "User Details Found" },
//            {"password.changed", "Password Updated Successfully" },
//            {"addr.added", "New Address Added Successfully" },
//            {"no.addr", "Address not found" },
//            {"addr.updated", "Address Updated Successfully" },
//            {"addr.deleted", "Address Deleted Successfully" },
//            {"otp.valid", "Valid OTP" },
//            {"otp.not.valid", "Invalid OTP" },
//            {"addr.details", "User Address Details" },
//            {"profile.updated", "Profile Details Updated Successfully" },
//            {"dist.list", "District List Obtained" },
//            {"exception", "An error occurred" },
//            {"get.products", "Get all products" },
//            {"get.drivers", "Get all drivers" },
//            {"has.products", "Products listed" },
//            {"active.orders", "Active Orders exist" },
//            {"no.active.orders", "No Active Order exists" },
//            {"review.posted", "Review Posted" },
//            {"ordr.listed", "Orders Listed" },
//            {"ordr.details", "Order Details" },
//            {"ordr.cancelled", "Order Cancelled" },
//            {"with.timeslots","Timeslots Listed" },
//            {"order.placed", "Order placed" },
//            {"no.admin", "Agent Admin record not found" },
//            {"no.agentboss", "Agent Boss record not found" },
//            {"login.fail", "Login Failed" },
//            {"invalid.admin", "Admin credentials invalid" },
//            {"invalid.agentboss", "Agent Boss credentials invalid" },
//            {"order.count", "Orders counted" },
//            {"admin.details", "Admin details" },
//            {"admin.boss.details", "Get full details of a user" },
//            {"driver.details", "Driver Details" },
//            {"no.details", "Details not found" },
//            {"invoice.details", "Invoice Details" },
//            {"place.pickup.order", "Pickup order placed" },
//            {"cnfrm.pickup.order", "Pickup order confirmed"},
//            {"add.tele.order", "Tele-order placed" },
//            {"cnfrm.tele.order" , "Tele-order confirmed"},
//            {"no.dist", "No distribution near you" },
//            {"pass.not.chg", "Could not change password" },
//            {"got.agent.driver", "Driver details obtained" },
//            {"assg.order.count", "Assigned order count" },
//            {"drv.ordr.listed", "Orders listed" },
//            {"ordr.closed", "Order has been delivered" },
//            {"profile.changed", "Profile is updated" },
//            {"email.sent", "Email has been sent" },
//            {"out.for.delivery", "Order details" },
//            {"ereceipt.details", "E-Receipt found" },
//            {"no.order", "No order found" },
//            {"no.agentboss", "Agentboss record not found" },
//            {"invalid.agentboss", "Agentboss credentials invalid" },
//            {"superuser.details", "Superuser details" },
//            {"invalid.superuser", "Superuser credentials invalid" },
//            {"has.prod.details", "Product details found" },
//            {"driver_list_for_order_found", "Drivers listed" },
//            {"driver_list_for_order_not_found", "Drivers not found" },
//            { "", ""}
//        };
//        /// <summary>
//        /// Get the message to display for the given key.
//        /// </summary>
//        /// <param name="key">The key to the message.</param>
//        /// <returns>The display text for the key. If the key is not found, returns "!".</returns>
//        public static string GetMessage(string key)
//        {
//            if (_Messages.ContainsKey(key))
//            {
//                return _Messages[key];
//            }
//            return "!";
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Basketee.API.Services
{
    /// <summary>
    /// Gets / returns a textual message for a given key string.
    /// </summary>
    public class MessagesSource
    {
        // TODO : This mapping may be moved to a database table and GetMessage method modified accordingly.
        private static Dictionary<string, string> _Messages = new Dictionary<string, string>
        {
            {"cons.reg.ok", "User Registered Successfully"},
            {"cons.reg.fail", "Registration failed"},
            {"cons.reg.dupl","This phone number is already registered" },
            {"no.user", "This user is not registered or active" },
            {"user.activated", "User Activated Successfully" },
            {"user.active", "User is already Activated" },
            {"user.found","User Exists" },
            {"otp.resent", "OTP Resent Successfully" },
            {"passwd.reset", "Password Reset Successfully" },
            {"no.reset", "Resetting password failed" },
            {"login.ok", "Successfully logged in to the system" },
            {"user.blocked", "Blocked User"},
            {"user.not.active", "User not activated"},
            {"user.details", "User Details Found" },
            {"password.changed", "Password Updated Successfully" },
            {"addr.added", "New Address Added Successfully" },
            {"no.addr", "Address not found" },
            {"addr.updated", "Address Updated Successfully" },
            {"addr.deleted", "Address Deleted Successfully" },
            {"otp.valid", "Valid OTP" },
            {"otp.not.valid", "Invalid OTP" },
            {"addr.details", "User Address Details" },
            {"profile.updated", "Profile Details Updated Successfully" },
            {"dist.list", "District List Obtained" },
            {"exception", "An error occurred" },
            {"has.products", "Products listed" },
            {"active.orders", "Active Orders exist" },
            {"no.active.orders", "No Active Order exists" },
            {"review.posted", "Review Posted" },
            {"ordr.listed", "Orders Listed" },
            {"ordr.details", "Order Details" },
            {"ordr.cancelled", "Order Cancelled" },
            {"with.timeslots","Timeslots Listed" },
            {"order.placed", "Order placed" },
            {"no.admin", "Agent Admin record not found" },
            {"no.agentboss", "Agent Boss record not found" },
            {"login.fail", "Login Failed" },
            {"invalid.admin", "Admin credentials invalid" },
            {"invalid.agentboss", "Agent Boss credentials invalid" },
            {"order.count", "Orders counted" },
            {"admin.details", "Admin details" },
            {"admin.boss.details", "Get full details of a user" },
            {"driver.details", "Driver Details" },
            {"no.details", "Details not found" },
            {"invoice.details", "Invoice Details" },
            {"place.pickup.order", "Pickup order placed" },
            {"cnfrm.pickup.order", "Pickup order confirmed"},
            {"add.tele.order", "Tele-order placed" },
            {"cnfrm.tele.order" , "Tele-order confirmed"},
            {"no.dist", "No distribution near you" },
            {"pass.not.chg", "Could not change password" },
            {"got.agent.driver", "Driver details obtained" },
            {"assg.order.count", "Assigned order count" },
            {"drv.ordr.listed", "Orders listed" },
            {"ordr.closed", "Order has been delivered" },
            {"profile.changed", "Profile is updated" },
            {"email.sent", "Email has been sent" },
            {"out.for.delivery", "Order details" },
            {"ereceipt.details", "E-Receipt found" },
            {"no.order", "No order found" },
            {"has.prod.details", "Product details found" },
            {"driver_list_for_order_found", "Drivers listed" },
            {"driver_list_for_order_not_found", "Drivers not found" },
            {"no.address.found", "Address not found" },
            {"address.details.found", "Address details found" },
            {"boss.ordr.listed", "Orders listed" },
            {"boss.rating.report", "Rating Reason Report" },
            {"boss.prdt.listed", "Products listed" },
            {"boss.drv.listed", "Drivers listed" },
            {"boss.sales.report", "Monthly Sales Report" },
            {"invoice.mail.sent", "e-Reciept sent successfully" },
            {"mail.id.not.found", "Email id not found for this user" },
            {"invalid.order.type", "Invalid order type. Please enter order type as OrderApp or OrderTelp." },
            {"no.super.user", "Superuser not found" },
            {"suser.sales.report", "Sales Report" },
            {"superuser.details", "Superuser details" },
            {"no.driver", "Invalid driver credentials" },
            {"invalid.super.user", "Invalid Super User" },
            {"issue.details", "Issue Details Listed" },
            {"drv.ordr.got", "Order details found." },
            {"agencies.listed", "Agencies Listed." },
            {"invalid.order", "Order is invalid" },
            {"faq.list", "FAQs listed" },
            {"faq.list.not.found", "FAQs listed" },
            {"invalid.exchange.input", "Exchange input not valid" },
            {"invalid.tele.order", "Tele order detalis not found" },
            {"suser.rating.report", "Rating Reason Report" },
            {"otp.sent", "OTP sent successfully" },
            {"otp.not.sent", "OTP not sent" },
            {"invalid.superuser", "Superuser credentials invalid" },
            {"ordr.cant.cancel", "Order can't cancel" },
            {"invalid.user.type", "Invalid user type. Please enter 1- Super User, 2 - Agent Boss,3 - Agent Admin,4 - Driver,5 - Consumer" },
            {"no.tele.order", "No tele order found" },
            {"addr.defualt", "This is your default address. Please try again after updating new default address." },
            {"conatct.found", "Contact found" },
            {"conatct.not.found", "No contact found" },
            {"promo.info.not.found", "Promo info not found" },
            {"promo.info.found", "Promo info found" },
            {"promo.banner.not.found", "Promo banner not  found" },
            {"promo.banner.found", "Promo banner found" },
            {"invalid.delivery.date.format", "Invalid delivery date. Date should be in yyyy-mm-dd format." },
            {"invalid.type.input", "Invalid is_agent_admin input. Please provide 1 for agent admin, 0 for agent boss." },
            { "", ""}
        };
        /// <summary>
        /// Get the message to display for the given key.
        /// </summary>
        /// <param name="key">The key to the message.</param>
        /// <returns>The display text for the key. If the key is not found, returns "!".</returns>
        public static string GetMessage(string key)
        {
            if (_Messages.ContainsKey(key))
            {
                return _Messages[key];
            }
            return "!";
        }

    }
}
