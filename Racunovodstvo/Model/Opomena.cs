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
    
    public partial class Opomena
    {
        public System.DateTime datum { get; set; }
        public int id { get; set; }
        public int redovnafaktura_id { get; set; }
    
        public virtual Faktura Faktura { get; set; }
    }
}
