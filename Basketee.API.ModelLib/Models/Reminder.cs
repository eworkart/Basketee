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
    
    public partial class Reminder
    {
        public int RmdrID { get; set; }
        public string ReminderImage { get; set; }
        public bool UserType { get; set; }
        public string Description { get; set; }
        public bool StatusId { get; set; }
        public short CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public short UpdatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public Nullable<int> ProdID { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual SuperAdmin SuperAdmin { get; set; }
        public virtual SuperAdmin SuperAdmin1 { get; set; }
    }
}
