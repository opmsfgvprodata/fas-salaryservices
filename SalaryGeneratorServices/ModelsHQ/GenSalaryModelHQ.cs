namespace SalaryGeneratorServices.ModelsHQ
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GenSalaryModelHQ : DbContext
    {
        public GenSalaryModelHQ()
            : base("name=GenSalaryModelHQ")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<tbl_ASCRawDataDetail> tbl_ASCRawDataDetail { get; set; }
        public virtual DbSet<tbl_AuditTrail> tbl_AuditTrail { get; set; }
        public virtual DbSet<tbl_Bank> tbl_Bank { get; set; }
        public virtual DbSet<tbl_CutiKategori> tbl_CutiKategori { get; set; }
        public virtual DbSet<tbl_CutiMaintenance> tbl_CutiMaintenance { get; set; }
        public virtual DbSet<tbl_CutiUmum> tbl_CutiUmum { get; set; }
        public virtual DbSet<tbl_EstateSelection> tbl_EstateSelection { get; set; }
        public virtual DbSet<tbl_HargaSawitRange> tbl_HargaSawitRange { get; set; }
        public virtual DbSet<tbl_HargaSawitSemasa> tbl_HargaSawitSemasa { get; set; }
        public virtual DbSet<tbl_JenisAktiviti> tbl_JenisAktiviti { get; set; }
        public virtual DbSet<tbl_JenisCaruman> tbl_JenisCaruman { get; set; }
        public virtual DbSet<tbl_JenisInsentif> tbl_JenisInsentif { get; set; }
        public virtual DbSet<tbl_KumpulanSyarikat> tbl_KumpulanSyarikat { get; set; }
        public virtual DbSet<tbl_Kwsp> tbl_Kwsp { get; set; }
        public virtual DbSet<tbl_Ladang> tbl_Ladang { get; set; }
        public virtual DbSet<tbl_Lejar> tbl_Lejar { get; set; }
        public virtual DbSet<tbl_ListASCFile> tbl_ListASCFile { get; set; }
        public virtual DbSet<tbl_LogDetail> tbl_LogDetail { get; set; }
        public virtual DbSet<tbl_MingguNegeri> tbl_MingguNegeri { get; set; }
        public virtual DbSet<tbl_Negara> tbl_Negara { get; set; }
        public virtual DbSet<tbl_OptionConfig> tbl_OptionConfig { get; set; }
        public virtual DbSet<tbl_Pembekal> tbl_Pembekal { get; set; }
        public virtual DbSet<tbl_PerluLadang> tbl_PerluLadang { get; set; }
        public virtual DbSet<tbl_PerluLadangHistory> tbl_PerluLadangHistory { get; set; }
        public virtual DbSet<tbl_Poskod> tbl_Poskod { get; set; }
        public virtual DbSet<tbl_QuotaPerluLadang> tbl_QuotaPerluLadang { get; set; }
        public virtual DbSet<tbl_QuotaPerluLadangHistory> tbl_QuotaPerluLadangHistory { get; set; }
        public virtual DbSet<tbl_ServicesList> tbl_ServicesList { get; set; }
        public virtual DbSet<tbl_SevicesProcess> tbl_SevicesProcess { get; set; }
        public virtual DbSet<tbl_SevicesProcessHistory> tbl_SevicesProcessHistory { get; set; }
        public virtual DbSet<tbl_Socso> tbl_Socso { get; set; }
        public virtual DbSet<tbl_SokPermhnWang> tbl_SokPermhnWang { get; set; }
        public virtual DbSet<tbl_SuperAdminSelection> tbl_SuperAdminSelection { get; set; }
        public virtual DbSet<tbl_Syarikat> tbl_Syarikat { get; set; }
        public virtual DbSet<tbl_Upah> tbl_Upah { get; set; }
        public virtual DbSet<tbl_UpahAktiviti> tbl_UpahAktiviti { get; set; }
        public virtual DbSet<tbl_UpahMenuai> tbl_UpahMenuai { get; set; }
        public virtual DbSet<tbl_UploadedCountDetail> tbl_UploadedCountDetail { get; set; }
        public virtual DbSet<tbl_Wilayah> tbl_Wilayah { get; set; }
        public virtual DbSet<tblAktiviti> tblAktivitis { get; set; }
        public virtual DbSet<tblASCApprovalFileDetail> tblASCApprovalFileDetails { get; set; }
        public virtual DbSet<tblASCApprovalRawData> tblASCApprovalRawDatas { get; set; }
        public virtual DbSet<tblClient> tblClients { get; set; }
        public virtual DbSet<tblConnection> tblConnections { get; set; }
        public virtual DbSet<tblDataEntryList> tblDataEntryLists { get; set; }
        public virtual DbSet<tblEmailList> tblEmailLists { get; set; }
        public virtual DbSet<tblEmailNotiStatu> tblEmailNotiStatus { get; set; }
        public virtual DbSet<tblHtmlReport> tblHtmlReports { get; set; }
        public virtual DbSet<tblMaintenanceList> tblMaintenanceLists { get; set; }
        public virtual DbSet<tblMenuList> tblMenuLists { get; set; }
        public virtual DbSet<tblNgrSmbrSyrkt> tblNgrSmbrSyrkts { get; set; }
        public virtual DbSet<tblOptionConfigsWeb> tblOptionConfigsWebs { get; set; }
        public virtual DbSet<tblPkjmastApp> tblPkjmastApps { get; set; }
        public virtual DbSet<tblReportExport> tblReportExports { get; set; }
        public virtual DbSet<tblReportList> tblReportLists { get; set; }
        public virtual DbSet<tblRoleReport> tblRoleReports { get; set; }
        public virtual DbSet<tblRole> tblRoles { get; set; }
        public virtual DbSet<tblSokPermhnWangHisAction> tblSokPermhnWangHisActions { get; set; }
        public virtual DbSet<tblSubMenuList> tblSubMenuLists { get; set; }
        public virtual DbSet<tblSubReportList> tblSubReportLists { get; set; }
        public virtual DbSet<tblSystemConfig> tblSystemConfigs { get; set; }
        public virtual DbSet<tblTaskRemainder> tblTaskRemainders { get; set; }
        public virtual DbSet<tblTKABatch> tblTKABatches { get; set; }
        public virtual DbSet<tblTKADetail> tblTKADetails { get; set; }
        public virtual DbSet<tblUserIDApp> tblUserIDApps { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }
        public virtual DbSet<tbl_ASCRawData> tbl_ASCRawData { get; set; }
        public virtual DbSet<tbl_HariBekerja> tbl_HariBekerja { get; set; }
        public virtual DbSet<tbl_MasterPkj> tbl_MasterPkj { get; set; }
        public virtual DbSet<vw_CutiUmumNegeri> vw_CutiUmumNegeri { get; set; }
        public virtual DbSet<vw_Socso> vw_Socso { get; set; }
        public virtual DbSet<tbl_JadualCarumanTambahan> tbl_JadualCarumanTambahan { get; set; }
        public virtual DbSet<tbl_CarumanTambahan> tbl_CarumanTambahan { get; set; }
        public virtual DbSet<tbl_SubCarumanTambahan> tbl_SubCarumanTambahan { get; set; }
        public virtual DbSet<vw_NSWL> vw_NSWL { get; set; }
        public virtual DbSet<vw_GLDetails> vw_GLDetails { get; set; }
        public virtual DbSet<tbl_CustomerVendorGLMap> tbl_CustomerVendorGLMap { get; set; }
        public virtual DbSet<tbl_SAPCCPUP> tbl_SAPCCPUP { get; set; }
        public virtual DbSet<tbl_SAPCustomerPUP> tbl_SAPCustomerPUP { get; set; }
        public virtual DbSet<tbl_SAPCUSTOMPUP> tbl_SAPCUSTOMPUP { get; set; }
        public virtual DbSet<tbl_SAPGLPUP> tbl_SAPGLPUP { get; set; }
        public virtual DbSet<tbl_SAPPDPUP> tbl_SAPPDPUP { get; set; }
        public virtual DbSet<tbl_SAPVendorPUP> tbl_SAPVendorPUP { get; set; }
        public virtual DbSet<tbl_SAPLogPUP> tbl_SAPLogPUP { get; set; }
        public virtual DbSet<vw_NSWL_2> vw_NSWL_2 { get; set; }
        public virtual DbSet<tbl_MapGL> tbl_MapGL { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_JenisCaruman>()
                .Property(e => e.fld_PeratusCarumanPekerja)
                .HasPrecision(18, 3);

            modelBuilder.Entity<tbl_JenisCaruman>()
                .Property(e => e.fld_PeratusCarumanMajikanBawah5000)
                .HasPrecision(18, 3);

            modelBuilder.Entity<tbl_JenisCaruman>()
                .Property(e => e.fld_PeratusCarumanMajikanAtas5000)
                .HasPrecision(18, 3);

            modelBuilder.Entity<tbl_Lejar>()
                .Property(e => e.fld_KodCaj)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Poskod>()
                .Property(e => e.fld_Postcode)
                .IsFixedLength();

            modelBuilder.Entity<tbl_Poskod>()
                .Property(e => e.fld_DistrictArea)
                .IsFixedLength();

            modelBuilder.Entity<tbl_Poskod>()
                .Property(e => e.fld_State)
                .IsFixedLength();

            modelBuilder.Entity<tbl_Poskod>()
                .Property(e => e.fld_Region)
                .IsFixedLength();

            modelBuilder.Entity<tbl_SokPermhnWang>()
                .Property(e => e.fld_JumlahPermohonan)
                .HasPrecision(13, 2);

            modelBuilder.Entity<tbl_SokPermhnWang>()
                .Property(e => e.fld_JumlahPDP)
                .HasPrecision(13, 2);

            modelBuilder.Entity<tbl_SokPermhnWang>()
                .Property(e => e.fld_JumlahTT)
                .HasPrecision(13, 2);

            modelBuilder.Entity<tbl_SokPermhnWang>()
                .Property(e => e.fld_JumlahCIT)
                .HasPrecision(13, 2);

            modelBuilder.Entity<tbl_SokPermhnWang>()
                .Property(e => e.fld_JumlahManual)
                .HasPrecision(13, 2);

            modelBuilder.Entity<tbl_Upah>()
                .Property(e => e.fld_Harga)
                .HasPrecision(18, 3);

            modelBuilder.Entity<tbl_UpahAktiviti>()
                .Property(e => e.fld_KdhByr)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPkjmastApp>()
                .Property(e => e.fldNoPkj)
                .IsFixedLength();

            modelBuilder.Entity<tblPkjmastApp>()
                .Property(e => e.fldNama1)
                .IsFixedLength();

            modelBuilder.Entity<tblPkjmastApp>()
                .Property(e => e.fldNoKP)
                .IsFixedLength();

            modelBuilder.Entity<tblPkjmastApp>()
                .Property(e => e.fldKdJnsPkj)
                .IsFixedLength();

            modelBuilder.Entity<tblPkjmastApp>()
                .Property(e => e.fldKdRkyt)
                .IsFixedLength();

            modelBuilder.Entity<tblPkjmastApp>()
                .Property(e => e.fldKdLdg)
                .IsFixedLength();

            modelBuilder.Entity<tblPkjmastApp>()
                .Property(e => e.fldJumPjm)
                .HasPrecision(7, 2);

            modelBuilder.Entity<tblPkjmastApp>()
                .Property(e => e.fldSbbMsk)
                .IsFixedLength();

            modelBuilder.Entity<tblPkjmastApp>()
                .Property(e => e.fldAlsnMsk)
                .IsFixedLength();

            modelBuilder.Entity<tblTaskRemainder>()
                .Property(e => e.fldPurpose)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblUserIDApp>()
                .Property(e => e.fldUserid)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblUserIDApp>()
                .Property(e => e.fldNama)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblUserIDApp>()
                .Property(e => e.fldNoKP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblUserIDApp>()
                .Property(e => e.fldKdLdg)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserIDApp>()
                .Property(e => e.fldNamaLdg)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblUserIDApp>()
                .Property(e => e.fldJawatan)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblUserIDApp>()
                .Property(e => e.fldPassword)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblUserIDApp>()
                .Property(e => e.fldStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tbl_MasterPkj>()
                .Property(e => e.fld_ID)
                .IsFixedLength();

            modelBuilder.Entity<tbl_MasterPkj>()
                .Property(e => e.fld_Nopkj)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tbl_MasterPkj>()
                .Property(e => e.fld_Nama)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tbl_MasterPkj>()
                .Property(e => e.fld_Nokp)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tbl_MasterPkj>()
                .Property(e => e.fld_IDpkj)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tbl_MasterPkj>()
                .Property(e => e.fld_Kdaktf)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<vw_CutiUmumNegeri>()
                .Property(e => e.fld_TarikhCuti)
                .IsUnicode(false);
        }
    }
}
