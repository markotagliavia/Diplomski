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
    
    public partial class Kompenzacija
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kompenzacija()
        {
            this.StavkaKompenzacijes = new HashSet<StavkaKompenzacije>();
        }
    
        public System.DateTime datum { get; set; }
        public int id { get; set; }
        public bool active { get; set; }
        public int zaposleni_id { get; set; }
    
        public virtual Zaposleni Zaposleni { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StavkaKompenzacije> StavkaKompenzacijes { get; set; }
    }
}