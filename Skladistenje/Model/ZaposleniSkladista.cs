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
    
    public partial class ZaposleniSkladista
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ZaposleniSkladista()
        {
            this.SkladisteniDokuments = new HashSet<SkladisteniDokument>();
        }
    
        public int zaposleni_id { get; set; }
        public int skladiste_id { get; set; }
    
        public virtual Skladiste Skladiste { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SkladisteniDokument> SkladisteniDokuments { get; set; }
        public virtual Zaposleni Zaposleni { get; set; }
    }
}
