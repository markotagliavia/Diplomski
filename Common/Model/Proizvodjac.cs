//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Common.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Proizvodjac
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Proizvodjac()
        {
            this.Proizvods = new HashSet<Proizvod>();
        }
    
        public int id { get; set; }
        public string naziv { get; set; }
        public Nullable<int> grad_id { get; set; }
    
        public virtual grad grad { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Proizvod> Proizvods { get; set; }
    }
}
