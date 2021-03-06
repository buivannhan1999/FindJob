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
    
    public partial class Post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Post()
        {
            this.Joins = new HashSet<Join>();
            this.Saves = new HashSet<Save>();
        }
    
        public long ID { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public Nullable<long> Career_ID { get; set; }
        public string Form { get; set; }
        public Nullable<System.DateTime> Dealine { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public string Descriptions { get; set; }
        public string Experience { get; set; }
        public Nullable<long> Employer_ID { get; set; }
        public Nullable<bool> Status { get; set; }
    
        public virtual Career Career { get; set; }
        public virtual Employer Employer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Join> Joins { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Save> Saves { get; set; }
    }
}
