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
    
    public partial class Skill
    {
        public long ID { get; set; }
        public string MainSkill { get; set; }
        public Nullable<int> Level { get; set; }
        public Nullable<long> Candidate_ID { get; set; }
    
        public virtual Candidate Candidate { get; set; }
    }
}
