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
    
    public partial class Korisnik
    {
        public string korisnickoime { get; set; }
        public string lozinka { get; set; }
        public int id { get; set; }
        public int zaposleni_id { get; set; }
        public bool active { get; set; }
        public bool ulogovan { get; set; }
    
        public virtual Zaposleni Zaposleni { get; set; }
    }
}
