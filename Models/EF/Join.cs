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
    
    public partial class Join
    {
        public long ID { get; set; }
        public Nullable<long> Post_ID { get; set; }
        public Nullable<long> Candidate_ID { get; set; }
        public Nullable<System.DateTime> DateJoin { get; set; }
        public Nullable<bool> Status { get; set; }
    
        public virtual Candidate Candidate { get; set; }
        public virtual Post Post { get; set; }
    }
}
