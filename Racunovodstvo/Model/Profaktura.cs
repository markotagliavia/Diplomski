//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Racunovodstvo.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Profaktura
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Profaktura()
        {
            this.Fakturas = new HashSet<Faktura>();
            this.StavkaProfaktures = new HashSet<StavkaProfakture>();
        }
    
        public bool active { get; set; }
        public int id { get; set; }
        public int redovnafaktura_id { get; set; }
        public string oznaka { get; set; }
        public int zaposleni_id { get; set; }
        public int poslovnipartner_mbr { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Faktura> Fakturas { get; set; }
        public virtual Faktura Faktura { get; set; }
        public virtual PoslovniPartner PoslovniPartner { get; set; }
        public virtual Zaposleni Zaposleni { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StavkaProfakture> StavkaProfaktures { get; set; }
    }
}