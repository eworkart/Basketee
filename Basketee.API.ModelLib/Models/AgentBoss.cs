//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Basketee.API.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AgentBoss
    {
        public int AbosID { get; set; }
        public string OwnerName { get; set; }
        public int AgenID { get; set; }
        public string MobileNumber { get; set; }
        public string ProfileImage { get; set; }
        public string AppToken { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public bool StatusId { get; set; }
        public short CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public short UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public string AccToken { get; set; }
        public string AppID { get; set; }
        public string Email { get; set; }
    
        public virtual Agency Agency { get; set; }
        public virtual SuperAdmin SuperAdmin { get; set; }
    }
}