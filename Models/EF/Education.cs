//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FindJob.Models.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Education
    {
        public long ID { get; set; }
        public string Education1 { get; set; }
        public string School { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Descriptions { get; set; }
        public Nullable<long> Candidate_ID { get; set; }
    
        public virtual Candidate Candidate { get; set; }
    }
}
