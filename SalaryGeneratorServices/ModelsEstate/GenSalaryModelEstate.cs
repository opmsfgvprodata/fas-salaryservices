namespace SalaryGeneratorServices.ModelsEstate
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GenSalaryModelEstate : DbContext
    {
        public static string host1 = "";
        public static string catalog1 = "";
        public static string user1 = "";
        public static string pass1 = "";
        public GenSalaryModelEstate()
            : base(nameOrConnectionString: "BYOWN")
        {
            base.Database.Connection.ConnectionString = "data source=" + host1 + ";initial catalog=" + catalog1 + ";user id=" + user1 + ";password=" + pass1 + ";MultipleActiveResultSets=True;App=EntityFramework";
        }

        public static GenSalaryModelEstate ConnectToSqlServer(string host, string catalog, string user, string pass)
        {
            host1 = host;
            catalog1 = catalog;
            user1 = user;
            pass1 = pass;

            return new GenSalaryModelEstate();

        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<tbl_AktvtKerja> tbl_AktvtKerja { get; set; }
        public virtual DbSet<tbl_Blok> tbl_Blok { get; set; }
        public virtual DbSet<tbl_CutiDiambil> tbl_CutiDiambil { get; set; }
        public virtual DbSet<tbl_CutiPeruntukan> tbl_CutiPeruntukan { get; set; }
        public virtual DbSet<tbl_HasilSawitBlok> tbl_HasilSawitBlok { get; set; }
        public virtual DbSet<tbl_HasilSawitPkt> tbl_HasilSawitPkt { get; set; }
        public virtual DbSet<tbl_HasilSawitSubPkt> tbl_HasilSawitSubPkt { get; set; }
        public virtual DbSet<tbl_Kerja> tbl_Kerja { get; set; }
        public virtual DbSet<tbl_KerjaBonus> tbl_KerjaBonus { get; set; }
        public virtual DbSet<tbl_Kerjahdr> tbl_Kerjahdr { get; set; }
        public virtual DbSet<tbl_KerjahdrCuti> tbl_KerjahdrCuti { get; set; }
        public virtual DbSet<tbl_KerjaOT> tbl_KerjaOT { get; set; }
        public virtual DbSet<tbl_KumpulanKerja> tbl_KumpulanKerja { get; set; }
        public virtual DbSet<tbl_PktUtama> tbl_PktUtama { get; set; }
        public virtual DbSet<tbl_Produktiviti> tbl_Produktiviti { get; set; }
        public virtual DbSet<tbl_Sctran> tbl_Sctran { get; set; }
        public virtual DbSet<tbl_SubPkt> tbl_SubPkt { get; set; }
        public virtual DbSet<tblStatusPkj> tblStatusPkjs { get; set; }
        public virtual DbSet<tbl_GajiBulanan> tbl_GajiBulanan { get; set; }
        public virtual DbSet<tbl_Insentif> tbl_Insentif { get; set; }
        public virtual DbSet<tbl_LogDetail> tbl_LogDetail { get; set; }
        public virtual DbSet<tbl_Photo> tbl_Photo { get; set; }
        public virtual DbSet<tbl_Pkjmast> tbl_Pkjmast { get; set; }
        public virtual DbSet<tbl_ServicesList> tbl_ServicesList { get; set; }
        public virtual DbSet<tbl_SevicesProcess> tbl_SevicesProcess { get; set; }
        public virtual DbSet<tbl_SevicesProcessHistory> tbl_SevicesProcessHistory { get; set; }
        public virtual DbSet<tbl_Skb> tbl_Skb { get; set; }
        public virtual DbSet<tblHtmlReport> tblHtmlReports { get; set; }
        public virtual DbSet<vw_GajiBulananPekerja> vw_GajiBulananPekerja { get; set; }
        public virtual DbSet<vw_HasilSawitBlok> vw_HasilSawitBlok { get; set; }
        public virtual DbSet<vw_HasilSawitPkt> vw_HasilSawitPkt { get; set; }
        public virtual DbSet<vw_HasilSawitSubPkt> vw_HasilSawitSubPkt { get; set; }
        public virtual DbSet<vw_InsentifPekerja> vw_InsentifPekerja { get; set; }
        public virtual DbSet<vw_Kerja_Bonus> vw_Kerja_Bonus { get; set; }
        public virtual DbSet<vw_Kerja_Hdr_Cuti> vw_Kerja_Hdr_Cuti { get; set; }
        public virtual DbSet<vw_Kerja_OT> vw_Kerja_OT { get; set; }
        public virtual DbSet<vw_Kerjahdr> vw_Kerjahdr { get; set; }
        public virtual DbSet<vw_KerjaPekerja> vw_KerjaPekerja { get; set; }
        public virtual DbSet<vw_KumpulanKerja> vw_KumpulanKerja { get; set; }
        public virtual DbSet<vw_KumpulanPekerja> vw_KumpulanPekerja { get; set; }
        public virtual DbSet<vw_MaklumatCuti> vw_MaklumatCuti { get; set; }
        public virtual DbSet<vw_MaklumatInsentif> vw_MaklumatInsentif { get; set; }
        public virtual DbSet<vw_MaklumatProduktiviti> vw_MaklumatProduktiviti { get; set; }
        public virtual DbSet<vw_RptSctran> vw_RptSctran { get; set; }
        public virtual DbSet<tbl_TutupUrusNiaga> tbl_TutupUrusNiaga { get; set; }
        public virtual DbSet<tbl_KerjahdrCutiTahunan> tbl_KerjahdrCutiTahunan { get; set; }
        public virtual DbSet<tbl_ByrCarumanTambahan> tbl_ByrCarumanTambahan { get; set; }
        public virtual DbSet<tbl_PkjCarumanTambahan> tbl_PkjCarumanTambahan { get; set; }
        public virtual DbSet<vw_KerjaInfoDetails> vw_KerjaInfoDetails { get; set; }
        public virtual DbSet<tbl_SAPPostDataDetails> tbl_SAPPostDataDetails { get; set; }
        public virtual DbSet<tbl_SAPPostRef> tbl_SAPPostRef { get; set; }
        public virtual DbSet<tbl_PkjIncrmntSalary> tbl_PkjIncrmntSalary { get; set; }
        public virtual DbSet<tbl_HutangPekerjaJumlah> tbl_HutangPekerjaJumlah { get; set; }
        public virtual DbSet<vw_KerjaDetailScTrans> vw_KerjaDetailScTrans { get; set; }

        //added by faeza 26.02.2023
        public virtual DbSet<tbl_SpecialInsentif> tbl_SpecialInsentif { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
        }
    }
}
