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
    
    public partial class MFunctionality
    {
        public MFunctionality()
        {
            this.AccessControls = new HashSet<AccessControl>();
        }
    
        public int FuncID { get; set; }
        public short ModID { get; set; }
        public string FunctionalityName { get; set; }
        public bool StatusId { get; set; }
    
        public virtual ICollection<AccessControl> AccessControls { get; set; }
        public virtual MModule MModule { get; set; }
    }
}