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
    
    public partial class Evaluate
    {
        public long ID { get; set; }
        public Nullable<long> Candidate_ID { get; set; }
        public Nullable<long> Employer_ID { get; set; }
        public Nullable<int> Rate { get; set; }
        public string Content { get; set; }
        public string Descriptions { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<bool> Status { get; set; }
    
        public virtual Candidate Candidate { get; set; }
        public virtual Employer Employer { get; set; }
    }
}
