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
    
    public partial class Popi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Popi()
        {
            this.StavkaPopisas = new HashSet<StavkaPopisa>();
            this.Zaposlenis = new HashSet<Zaposleni>();
        }
    
        public int id { get; set; }
        public System.DateTime datum { get; set; }
        public int skladiste_id { get; set; }
        public string oznaka { get; set; }
    
        public virtual Skladiste Skladiste { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StavkaPopisa> StavkaPopisas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zaposleni> Zaposlenis { get; set; }
    }
}
