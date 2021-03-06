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
    
    public partial class PoslovniPartner
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PoslovniPartner()
        {
            this.Fakturas = new HashSet<Faktura>();
            this.Profakturas = new HashSet<Profaktura>();
            this.SkladisteniDokuments = new HashSet<SkladisteniDokument>();
        }
    
        public int mbr { get; set; }
        public string pib { get; set; }
        public string naziv { get; set; }
        public string adresa { get; set; }
        public double dugovanja { get; set; }
        public string email { get; set; }
        public string brojtelefona { get; set; }
        public string tekuciracun { get; set; }
        public int grad_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Faktura> Fakturas { get; set; }
        public virtual grad grad { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Profaktura> Profakturas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SkladisteniDokument> SkladisteniDokuments { get; set; }
    }
}
