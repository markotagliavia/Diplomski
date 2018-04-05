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
    
    public partial class SkladisteniDokument
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SkladisteniDokument()
        {
            this.SkladisteniDokument1 = new HashSet<SkladisteniDokument>();
            this.StavkaSklDokumentas = new HashSet<StavkaSklDokumenta>();
            this.Fakturas = new HashSet<Faktura>();
        }
    
        public int id { get; set; }
        public System.DateTime datum { get; set; }
        public bool upripremi { get; set; }
        public int zaposleniskladista_zaposleni_id { get; set; }
        public int zaposleniskladista_skladiste_id { get; set; }
        public string sifra { get; set; }
        public bool active { get; set; }
        public bool redovni { get; set; }
        public string tipredovnog { get; set; }
        public int redovniskldok_id { get; set; }
        public bool storniranceo { get; set; }
        public string vozac { get; set; }
        public string regbr { get; set; }
        public string nacinotpreme { get; set; }
        public string izdao { get; set; }
        public string primio { get; set; }
        public int poslovnipartner_mbr { get; set; }
        public int skladiste_id { get; set; }
    
        public virtual PoslovniPartner PoslovniPartner { get; set; }
        public virtual Skladiste Skladiste { get; set; }
        public virtual ZaposleniSkladista ZaposleniSkladista { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SkladisteniDokument> SkladisteniDokument1 { get; set; }
        public virtual SkladisteniDokument SkladisteniDokument2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StavkaSklDokumenta> StavkaSklDokumentas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Faktura> Fakturas { get; set; }
    }
}
