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
    
    public partial class Audit
    {
        public int id { get; set; }
        public string korisnickoime { get; set; }
        public string akcija { get; set; }
        public System.DateTime vreme { get; set; }
        public string tip { get; set; }
    }
}
