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
    
    public partial class MSuperUserType
    {
        public MSuperUserType()
        {
            this.AccessControls = new HashSet<AccessControl>();
        }
    
        public short UserTypeID { get; set; }
        public string TypeName { get; set; }
        public bool StatusId { get; set; }
    
        public virtual ICollection<AccessControl> AccessControls { get; set; }
    }
}