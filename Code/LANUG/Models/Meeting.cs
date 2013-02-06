//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LANUG.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Meeting
    {
        public Meeting()
        {
            this.Sponsors = new HashSet<Sponsor>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public string Info { get; set; }
        public string Summary { get; set; }
        public System.DateTime Created { get; set; }
        public int CreatedById { get; set; }
        public System.DateTime Modified { get; set; }
        public int ModifiedById { get; set; }
    
        public virtual ICollection<Sponsor> Sponsors { get; set; }
    }
}