//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Skladistenje.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class grad
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public grad()
        {
            this.PoslovniPartners = new HashSet<PoslovniPartner>();
            this.Proizvodjacs = new HashSet<Proizvodjac>();
            this.Skladistes = new HashSet<Skladiste>();
            this.Zaposlenis = new HashSet<Zaposleni>();
        }
    
        public int id { get; set; }
        public string naziv { get; set; }
        public string drzava { get; set; }
        public string postanskibroj { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PoslovniPartner> PoslovniPartners { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Proizvodjac> Proizvodjacs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Skladiste> Skladistes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zaposleni> Zaposlenis { get; set; }
    }
}
