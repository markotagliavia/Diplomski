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
    
    public partial class StavkaFakture
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StavkaFakture()
        {
            this.StavkaKompenzacijes = new HashSet<StavkaKompenzacije>();
            this.StavkaProfaktures = new HashSet<StavkaProfakture>();
            this.StavkaSklDokumentas = new HashSet<StavkaSklDokumenta>();
        }
    
        public int rednibroj { get; set; }
        public double kolicina { get; set; }
        public Nullable<double> rabat { get; set; }
        public Nullable<double> cena { get; set; }
        public Nullable<bool> storno { get; set; }
        public int faktura_id { get; set; }
        public int zalihe_proizvod_id { get; set; }
        public int zalihe_skladiste_id { get; set; }
        public Nullable<int> stavkaprofakture_rednibroj { get; set; }
        public Nullable<int> stavkaprofakture_profaktura_id { get; set; }
        public Nullable<int> stavkaskldok_rednibroj { get; set; }
        public Nullable<int> stavkaskldok_skladistenidokument_id { get; set; }
        public Nullable<int> stavkaskldok_idproizvod { get; set; }
        public Nullable<int> stavkaskldok_idskladista { get; set; }
    
        public virtual Faktura Faktura { get; set; }
        public virtual StavkaProfakture StavkaProfakture { get; set; }
        public virtual StavkaSklDokumenta StavkaSklDokumenta { get; set; }
        public virtual Zalihe Zalihe { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StavkaKompenzacije> StavkaKompenzacijes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StavkaProfakture> StavkaProfaktures { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StavkaSklDokumenta> StavkaSklDokumentas { get; set; }
    }
}
