﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DeltaEximEntities : DbContext
    {
        public DeltaEximEntities()
            : base("name=DeltaEximEntities")
        {
            this.Database.Connection.ConnectionString = ConnectionString.secureConnectionString();
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Audit> Audits { get; set; }
        public virtual DbSet<Faktura> Fakturas { get; set; }
        public virtual DbSet<grad> grads { get; set; }
        public virtual DbSet<jedinicamere> jedinicameres { get; set; }
        public virtual DbSet<Karakteristika> Karakteristikas { get; set; }
        public virtual DbSet<Kompenzacija> Kompenzacijas { get; set; }
        public virtual DbSet<Korisnik> Korisniks { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Opomena> Opomenas { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Popi> Popis { get; set; }
        public virtual DbSet<PoslovniPartner> PoslovniPartners { get; set; }
        public virtual DbSet<Profaktura> Profakturas { get; set; }
        public virtual DbSet<Proizvod> Proizvods { get; set; }
        public virtual DbSet<Proizvodjac> Proizvodjacs { get; set; }
        public virtual DbSet<Skladiste> Skladistes { get; set; }
        public virtual DbSet<SkladisteniDokument> SkladisteniDokuments { get; set; }
        public virtual DbSet<StavkaFakture> StavkaFaktures { get; set; }
        public virtual DbSet<StavkaKompenzacije> StavkaKompenzacijes { get; set; }
        public virtual DbSet<StavkaPopisa> StavkaPopisas { get; set; }
        public virtual DbSet<StavkaProfakture> StavkaProfaktures { get; set; }
        public virtual DbSet<StavkaSklDokumenta> StavkaSklDokumentas { get; set; }
        public virtual DbSet<Uloga> Ulogas { get; set; }
        public virtual DbSet<Zalihe> Zalihes { get; set; }
        public virtual DbSet<Zaposleni> Zaposlenis { get; set; }
        public virtual DbSet<ZaposleniSkladista> ZaposleniSkladistas { get; set; }
    }
}
