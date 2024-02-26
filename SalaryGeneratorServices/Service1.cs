using SalaryGeneratorServices.CustomModels;
using SalaryGeneratorServices.FuncClass;
using SalaryGeneratorServices.ModelsEstate;
using SalaryGeneratorServices.ModelsHQ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace SalaryGeneratorServices
{
    public partial class Service1 : ServiceBase
    {
        private GenSalaryModelHQ db = new GenSalaryModelHQ();
        private GenSalaryModelEstate db2 = new GenSalaryModelEstate();
        private Step1Func Step1Func = new Step1Func();
        private Step2Func Step2Func = new Step2Func();
        private Step3Func Step3Func = new Step3Func();
        private Step4Func Step4Func = new Step4Func();
        private Step5Func Step5Func = new Step5Func();
        private DateTimeFunc DateTimeFunc = new DateTimeFunc();
        private RemoveDataFunc RemoveDataFunc = new RemoveDataFunc();
        private LogFunc LogFunc = new LogFunc();
        private GetConnectFunc conn = new GetConnectFunc();

        public Service1()
        {
            _ = DoProcess();
        }

        private async Task DoProcess()
        {
            long ServiceProcessID = 0;
            List<tbl_Pkjmast> Pkjmstlists = new List<tbl_Pkjmast>();
            List<tbl_Pkjmast> PkjmstlistsAll = new List<tbl_Pkjmast>();
            List<tbl_Insentif> PkjmstlistsSpecialInsentif = new List<tbl_Insentif>();//added by faeza 26.02.2023
            List<CustMod_DateList> DateLists = new List<CustMod_DateList>();
            List<tbl_CutiKategori> CutiKategoriList = new List<tbl_CutiKategori>();
            ModelsHQ.tbl_SevicesProcess SevicesProcess = new ModelsHQ.tbl_SevicesProcess();
            List<CustMod_WorkerPaidLeave> WorkerPaidLeaveLists = new List<CustMod_WorkerPaidLeave>();
            tbl_Kerjahdr WorkingAtt = new tbl_Kerjahdr();
            List<tbl_JenisAktiviti> JenisAktiviti = new List<tbl_JenisAktiviti>();
            tbl_KerjaBonus KerjaBonus = new tbl_KerjaBonus();
            List<tbl_KerjaBonus> KerjaBonusList = new List<tbl_KerjaBonus>();
            tbl_KerjaOT KerjaOT = new tbl_KerjaOT();
            List<tbl_KerjaOT> KerjaOTList = new List<tbl_KerjaOT>();
            tbl_SpecialInsentif SpecialInsentif = new tbl_SpecialInsentif();//added by faeza 26.02.2023
            List<tbl_SpecialInsentif> SpecialInsentifList = new List<tbl_SpecialInsentif>();//added by faeza 26.02.2023
            List<CustMod_WorkSCTrans> WorkSCTransList = new List<CustMod_WorkSCTrans>();
            Guid MonthSalaryID = new Guid();
            int? NegaraID = 0;
            int? SyarikatID = 0;
            int? WilayahID = 0;
            int? LadangID = 0;
            int? DivisionID = 0;
            int? Month = 0;
            int? Year = 0;
            int? UserID = 0;
            int KerjaBonusRemoveCount = 0;
            int KerjaOTRemoveCount = 0;
            int SpecialInsentifRemoveCount = 0;//added by faeza 26.02.2023
            int KerjahdrCutiRemoveCount = 0;
            int KerjahdrCutiBlmAmbilRemoveCount = 0;
            int GajiBulananRemoveCount = 0;
            int ScTranRemoveCount = 0;
            int ByrCrmnTmbhnRemoveCount = 0;
            DateTime? StartWorkDate = new DateTime();
            DateTime LastDateLoop = new DateTime();
            byte? PaidPeriod = 0;
            bool TakePaidLeave = false;
            decimal? WorkingPayment = 0;
            decimal? WorkingPaymentORP = 0;
            decimal? DiffAreaPayment = 0;
            decimal? DailyBonusPayment = 0;
            decimal? DailyBonusPaymentORP = 0;
            decimal? OTPayment = 0;
            decimal? OthrInsPayment = 0;
            decimal? OthrInsPaymentORP = 0;
            decimal? DeductInsPayment = 0;
            decimal? AveragePayment = 0;
            decimal? AIPSPayment = 0;
            decimal? LeavePayment = 0;
            decimal? KWSPMjkn = 0;
            decimal? KWSPPkj = 0;
            decimal? SocsoMjkn = 0;
            decimal? SocsoPkj = 0;
            decimal? TotalOthrContMjkCont = 0;
            decimal? TotalOthrContPkjCont = 0;
            decimal? OverallSalary = 0;
            decimal? Salary = 0;
            bool AttendStatus = false;
            string AttCode = "";
            string KumCode = "";
            string Log = "";
            string ServiceName = "";
            decimal Percentage = 0;
            decimal CountData = 0;
            decimal LoopCountData = 1;
            int TotalPkjCount = 0;
            int TotalDateCount = 0;
            int TotalDataCount = 0;
            int DataCount = 1;
            decimal? TotalDebtDeduction = 0;
            
            int TotalDataCount2 = 0;
            int DataCount2 = 1;

            NegaraID = int.Parse(ConfigReader("configs", "negaraid"));
            SyarikatID = int.Parse(ConfigReader("configs", "syarikatid"));
            WilayahID = int.Parse(ConfigReader("configs", "wilayahid"));
            LadangID = int.Parse(ConfigReader("configs", "ladangid"));
            DivisionID = int.Parse(ConfigReader("configs", "divisionid"));
            ServiceName = ConfigReader("configs", "servicename");
            var workerid = "";
            var workername = "";
            DateTime? datedata = DateTime.Now;
            var divid = DivisionID;
            try
            {
                var getservicesdetail = db.tbl_ServicesList.Where(x => x.fld_SevicesActivity == "LadangSalaryGen" && x.fldNegaraID == NegaraID && x.fldSyarikatID == SyarikatID && x.fldWilayahID == WilayahID && x.fldLadangID == LadangID && x.fldDivisionID == DivisionID).FirstOrDefault();
                //RemoveDataFunc.RemoveData_tbl_SevicesProcess(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID);
                //Step1Func.AddServicesProcessFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID);
                SevicesProcess = await Step1Func.GetServiceProcessDetail(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID);
                if (SevicesProcess != null)
                {
                    NegaraID = SevicesProcess.fld_NegaraID;
                    SyarikatID = SevicesProcess.fld_SyarikatID;
                    WilayahID = SevicesProcess.fld_WilayahID;
                    LadangID = SevicesProcess.fld_LadangID;
                    DivisionID = SevicesProcess.fld_DivisionID;
                    Month = SevicesProcess.fld_Month;
                    Year = SevicesProcess.fld_Year;
                    UserID = SevicesProcess.fld_UserID;
                    ServiceProcessID = SevicesProcess.fld_ID;

                    string host, catalog, user, pass = "";
                    conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
                    GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

                    WriteLog("Start Process.", true, ServiceName, ServiceProcessID);
                    WriteLog("Get Services Details. (Data - Services Name : " + getservicesdetail.fld_ServicesName + ", Month/Year : " + Month + "/" + Year + ")", false, ServiceName, ServiceProcessID);
                    var tbl_Pkjmast = db2.tbl_Pkjmast.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_DivisionID == DivisionID).ToList();
                    WriteLog("Added into Services Process. (Data - Services Process ID : " + ServiceProcessID + ")", false, ServiceName, ServiceProcessID);
                    Pkjmstlists = await Step1Func.GetActiveWorkerFunc(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, tbl_Pkjmast);
                    PkjmstlistsAll = Step1Func.GetAllWorkerFunc(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, tbl_Pkjmast);
                    WriteLog("Get Active Worker. (Data - Total Active Worker : " + Pkjmstlists.Count + ")", false, ServiceName, ServiceProcessID);
                    //added by faeza 26.02.2023
                    var tbl_JenisInsentifSpecial = await db.tbl_JenisInsentif.Where(x => x.fld_JenisInsentif == "P" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false && x.fld_InclSecondPayslip == true).ToListAsync();
                    var tbl_Insentif2 = await db2.tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && x.fld_Deleted == false).ToListAsync();
                    PkjmstlistsSpecialInsentif = Step1Func.GetWorkerSpecialInsentifFunc(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, tbl_JenisInsentifSpecial, tbl_Insentif2);
                    //end added
                    KerjaBonusRemoveCount = RemoveDataFunc.RemoveData_tbl_KerjaBonus(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, PkjmstlistsAll);
                    WriteLog("Removed Calculation Kerja Bonus Data. (Data - Total Data Removed : " + KerjaBonusRemoveCount + ")", false, ServiceName, ServiceProcessID);
                    KerjaOTRemoveCount = RemoveDataFunc.RemoveData_tbl_KerjaOT(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, PkjmstlistsAll);
                    WriteLog("Removed Calculation Kerja OT Data. (Data - Total Data Removed : " + KerjaOTRemoveCount + ")", false, ServiceName, ServiceProcessID);
                    KerjahdrCutiRemoveCount = RemoveDataFunc.RemoveData_tbl_KerjahdrCuti(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, PkjmstlistsAll);
                    WriteLog("Removed Calculation Hadir Cuti Data. (Data - Total Data Removed : " + KerjahdrCutiRemoveCount + ")", false, ServiceName, ServiceProcessID);
                    KerjahdrCutiBlmAmbilRemoveCount = RemoveDataFunc.RemoveData_tbl_KerjahdrCutiTahunan(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, PkjmstlistsAll);
                    WriteLog("Removed Calculation Cuti Tahunan Data. (Data - Total Data Removed : " + KerjahdrCutiBlmAmbilRemoveCount + ")", false, ServiceName, ServiceProcessID);
                    ByrCrmnTmbhnRemoveCount = RemoveDataFunc.RemoveData_tbl_CarumanTambahan(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, PkjmstlistsAll);
                    WriteLog("Removed Calculation Additional Contribution. (Data - Total Data Removed : " + ByrCrmnTmbhnRemoveCount + ")", false, ServiceName, ServiceProcessID);
                    GajiBulananRemoveCount = RemoveDataFunc.RemoveData_tbl_GajiBulanan(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, PkjmstlistsAll);
                    WriteLog("Removed Calculation Monthly Salary Data. (Data - Total Data Removed : " + GajiBulananRemoveCount + ")", false, ServiceName, ServiceProcessID);
                    //added by faeza 26.02.2023
                    //SpecialInsentifRemoveCount = RemoveDataFunc.RemoveData_tbl_SpecialInsentif(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, PkjmstlistsSpecialInsentif);
                    SpecialInsentifRemoveCount = RemoveDataFunc.RemoveData_tbl_SpecialInsentif(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, PkjmstlistsAll);
                    WriteLog("Removed Incentive from tbl_SpecialInsentif. (Data - Total Data Removed : " + KerjaOTRemoveCount + ")", false, ServiceName, ServiceProcessID);
                    CutiKategoriList = await  Step1Func.GetPaidLeaveFunc(NegaraID, SyarikatID, WilayahID, LadangID);
                    JenisAktiviti = await Step1Func.GetActvtyTypeBonusStatusFunc(NegaraID, SyarikatID, WilayahID, LadangID);
                    //80%
                    TotalPkjCount = Pkjmstlists.Count;
                    WriteLog("Total Pkj Count. (Data - Total Data : " + TotalPkjCount + ")", false, ServiceName, ServiceProcessID);
                    DateLists = Step1Func.GetDateListFunc(Month, Year);
                    TotalDateCount = DateLists.Count;
                    WriteLog("Total Date Count. (Data - Total Data : " + TotalDateCount + ")", false, ServiceName, ServiceProcessID);
                    TotalDataCount = TotalPkjCount * TotalDateCount;
                    TotalDataCount2 = TotalDataCount + 15;
                    WriteLog("Total Data Count. (Data - Total Data : " + TotalDataCount2 + ")", false, ServiceName, ServiceProcessID);

                    var tbl_Kerja = await db2.tbl_Kerja.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year).ToListAsync();
                    var tbl_KerjahdrYearly = await db2.tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Year == Year).ToListAsync();
                    var tbl_Kerjahdr = tbl_KerjahdrYearly.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year).ToList();
                    var tbl_PkjIncrmntSalary = await db2.tbl_PkjIncrmntSalary.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_AppStatus == true).ToListAsync();
                    //modified by faeza 26.02.2023 - add fld_InclSecondPayslip == false
                    var tbl_JenisInsentif = await db.tbl_JenisInsentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false && x.fld_InclSecondPayslip == false).ToListAsync();
                    var tbl_TaxRelief = await db.tbl_TaxRelief.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false).ToListAsync();
                    var tbl_Insentif = await db2.tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && x.fld_Deleted == false).ToListAsync();
                    var tbl_Produktiviti = await db2.tbl_Produktiviti.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year).ToListAsync();
                    var tblOptionConfigsWebs = await db.tblOptionConfigsWebs.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).ToListAsync();
                    var Attandance = tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "cuti" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).ToList();
                    var OTCulData = tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "kadarot" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).ToList();
                    var OTKadar = tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "kiraot" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).ToList();
                    var GetMinPayforLeave = tblOptionConfigsWebs.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldOptConfFlag1 == "leavepay" && x.fldDeleted == false).ToList();
                    var tbl_CutiPeruntukan = await db2.tbl_CutiPeruntukan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID).ToListAsync();
                    var tbl_Kwsp = await db.tbl_Kwsp.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID).ToListAsync();
                    var tbl_Socso = await db.tbl_Socso.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID).ToListAsync();
                    var tbl_PkjCarumanTambahan = await db2.tbl_PkjCarumanTambahan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Deleted == false).ToListAsync();
                    var tbl_CarumanTambahan = await db.tbl_CarumanTambahan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID).ToListAsync();
                    var tbl_SubCarumanTambahan = await db.tbl_SubCarumanTambahan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID).ToListAsync();
                    var tbl_JadualCarumanTambahan = await db.tbl_JadualCarumanTambahan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID).ToListAsync();
                    var tbl_HutangPekerjaJumlah = await db2.tbl_HutangPekerjaJumlah.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID).ToListAsync();
                    //Added by Shah 01_01_2024
                    var tbl_TaxWorkerInfo = await db2.tbl_TaxWorkerInfo.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Year == Year).ToListAsync();
                    Step3Func.GetPkjMastsData(Pkjmstlists);
                    var JenisInsentifExludePCB = tbl_JenisInsentif.Where(x => tbl_TaxRelief.Select(s => s.fld_TaxReliefCode).ToArray().Contains(x.fld_TaxReliefCode)).ToList();
                    var jenisInsentifExludePCBKod = JenisInsentifExludePCB.Select(s => s.fld_KodInsentif).ToArray();
                    var InsentifExcludePCBYearly = await db2.tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Year == Year && x.fld_Deleted == false && jenisInsentifExludePCBKod.Contains(x.fld_KodInsentif)).ToListAsync();

                    if (!Pkjmstlists.Any(x => x.fld_NopkjPermanent == null))
                    {
                        foreach (var Pkjmstlist in Pkjmstlists)
                        {
                            //DataCount2 = DataCount;
                            //Percentage = (DataCount / TotalDataCount) * 79.5m;
                            //WriteLog("Total Loop Data Count. (Data - Total Data : " + DataCount2 + ")", false, ServiceName, ServiceProcessID);
                            await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 1, db, TotalDataCount2, DataCount2);
                            KerjaBonusList = new List<tbl_KerjaBonus>();
                            KerjaOTList = new List<tbl_KerjaOT>();
                            WorkerPaidLeaveLists = new List<CustMod_WorkerPaidLeave>();
                            WriteLog("Get Worker Data. (Data : Worker No : " + Pkjmstlist.fld_Nopkj.Trim().Trim() + ", Worker Name : " + Pkjmstlist.fld_Nama.Trim() + ")", false, ServiceName, ServiceProcessID);
                            WriteLog("Get Date List. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Date From : " + string.Format("{0:dd/MM/yyyy}", DateLists.OrderBy(o => o.Date).Select(s => s.Date).Take(1).FirstOrDefault()) + ", Date Until : " + string.Format("{0:dd/MM/yyyy}", DateLists.OrderByDescending(o => o.Date).Select(s => s.Date).Take(1).FirstOrDefault()) + ")", false, ServiceName, ServiceProcessID);
                            LastDateLoop = DateLists.OrderByDescending(o => o.Date).Select(s => s.Date).Take(1).FirstOrDefault();
                            StartWorkDate = Step1Func.GetDateStarkWorkingFunc(NegaraID, SyarikatID, WilayahID, LadangID, Pkjmstlist.fld_Nopkj.Trim(), tbl_Kerjahdr);
                            if (StartWorkDate != null)
                            {
                                if (LoopCountData == 1)
                                {
                                    CountData = (Pkjmstlists.Count * DateLists.Count) + 12;
                                }
                                WriteLog("Get Worker Start Working Date. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Start Working Date : " + string.Format("{0:dd/MM/yyyy}", StartWorkDate) + ")", false, ServiceName, ServiceProcessID);
                                foreach (var DateList in DateLists)
                                {
                                    workerid = Pkjmstlist.fld_Nopkj;
                                    workername = Pkjmstlist.fld_Nama;
                                    datedata = DateList.Date;
                                    DataCount = DataCount + 1;
                                    DataCount2 = DataCount;
                                    Percentage = (DataCount / TotalDataCount) * 79.5m;
                                    WriteLog("Total Loop Data Count. (Data - Total Data : " + DataCount2 + ")", false, ServiceName, ServiceProcessID);
                                    await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 1, db, TotalDataCount2, DataCount2);
                                    WriteLog("Get Date. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Date : " + string.Format("{0:dd/MM/yyyy}", DateList.Date) + ")", false, ServiceName, ServiceProcessID);
                                    TakePaidLeave = Step2Func.GetWorkingPaidLeaveFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), DateList.Date, CutiKategoriList, out PaidPeriod, out WorkingAtt, out KumCode, tbl_Kerjahdr);
                                    WriteLog("Get Paid Leave. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Paid Leave : " + TakePaidLeave + ")", false, ServiceName, ServiceProcessID);
                                    if (TakePaidLeave)
                                    {
                                        WriteLog("Code Paid Leave. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Leave Code : " + WorkingAtt.fld_Kdhdct + ")", false, ServiceName, ServiceProcessID);
                                        WorkerPaidLeaveLists.Add(new CustMod_WorkerPaidLeave() { fld_Nopkj = Pkjmstlist.fld_Nopkj.Trim(), fld_Kdhdct = WorkingAtt.fld_Kdhdct, fld_Tarikh = DateList.Date, fld_PaidPeriod = PaidPeriod, fld_KerjahdrID = WorkingAtt.fld_UniqueID, fld_Kum = KumCode });
                                    }
                                    else
                                    {
                                        //modified by faeza 26.09.2022 - add out PaidPeriod, out WorkingAtt, out KumCode
                                        AttendStatus = Step2Func.GetAttendStatusFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), DateList.Date, out AttCode, tblOptionConfigsWebs, tbl_Kerjahdr, out PaidPeriod, out WorkingAtt, out KumCode);
                                        if (AttendStatus)
                                        {
                                            //added by faeza 26.09.2022
                                            if (AttCode == "H03")
                                            {
                                                WriteLog("Code Working Leave. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Leave Code : " + WorkingAtt.fld_Kdhdct + ")", false, ServiceName, ServiceProcessID);
                                                WorkerPaidLeaveLists.Add(new CustMod_WorkerPaidLeave() { fld_Nopkj = Pkjmstlist.fld_Nopkj.Trim(), fld_Kdhdct = WorkingAtt.fld_Kdhdct, fld_Tarikh = DateList.Date, fld_PaidPeriod = PaidPeriod, fld_KerjahdrID = WorkingAtt.fld_UniqueID, fld_Kum = KumCode });
                                            }
                                            //end added

                                            Step2Func.GetDailyBonusFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), DateList.Date, out KerjaBonus, JenisAktiviti, tbl_Kerja);
                                            if (KerjaBonus != null)
                                            {
                                                WriteLog("Get Daily Bonus. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Bonus Price : RM " + KerjaBonus.fld_Jumlah + ")", false, ServiceName, ServiceProcessID);
                                                KerjaBonusList.Add(KerjaBonus);
                                            }
                                            else
                                            {
                                                WriteLog("No Daily Bonus.", false, ServiceName, ServiceProcessID);
                                            }

                                            Step2Func.GetOTFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), DateList.Date, out KerjaOT, AttCode, OTCulData, OTKadar, tbl_PkjIncrmntSalary, tbl_Kerja);

                                            if (KerjaOT != null)
                                            {
                                                WriteLog("Get Daily OT. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", OT Price : RM " + KerjaOT.fld_Jumlah + ")", false, ServiceName, ServiceProcessID);
                                                KerjaOTList.Add(KerjaOT);
                                            }
                                            else
                                            {
                                                WriteLog("No Daily OT.", false, ServiceName, ServiceProcessID);
                                            }
                                        }
                                        else
                                        {
                                            WriteLog("On Leave. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Leave Code : " + AttCode + ")", false, ServiceName, ServiceProcessID);
                                        }
                                    }

                                    if (LastDateLoop == DateList.Date) // untuk paid leave
                                    {
                                        await Step2Func.AddTo_tbl_KerjaBonus(NegaraID, SyarikatID, WilayahID, LadangID, KerjaBonusList);
                                        WriteLog("Insert To tbl_KerjaBonus. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Data : " + KerjaBonusList.Count + ")", false, ServiceName, ServiceProcessID);
                                        await Step2Func.AddTo_tbl_KerjaOT(NegaraID, SyarikatID, WilayahID, LadangID, KerjaOTList);
                                        WriteLog("Insert To tbl_KerjaOT. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Data : " + KerjaOTList.Count + ")", false, ServiceName, ServiceProcessID);
                                        var CustMod_PaidWorking = await Step3Func.GetPaidWorkingFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), tbl_Kerja);
                                        MonthSalaryID = CustMod_PaidWorking.SalaryID;
                                        WorkingPayment = CustMod_PaidWorking.WorkingPayment;
                                        DiffAreaPayment = CustMod_PaidWorking.DiffAreaPayment;
                                        WriteLog("Get Daily Work Payment. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Payment : RM " + WorkingPayment + ")", false, ServiceName, ServiceProcessID);

                                        //added by faeza 09.06.2021
                                        await Step3Func.UpdatePaymentMode(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), Pkjmstlist.fld_PaymentMode, MonthSalaryID);
                                        WriteLog("Update Payment Mode. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Payment Mode : " + Pkjmstlist.fld_PaymentMode + ")", false, ServiceName, ServiceProcessID);

                                        //add by faeza 10.11.2020
                                        WorkingPaymentORP = await Step3Func.GetPaidWorkingORPFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, Attandance, tbl_Kerja, tbl_Kerjahdr);
                                        WriteLog("Get Daily Work Payment ORP. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Payment : RM " + WorkingPaymentORP + ")", false, ServiceName, ServiceProcessID);

                                        DailyBonusPayment = await Step3Func.GetPaidDailyBonusFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID);
                                        WriteLog("Get Daily Bonus Payment. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Payment : RM " + DailyBonusPayment + ")", false, ServiceName, ServiceProcessID);
                                        //add by faeza 10.11.2020
                                        DailyBonusPaymentORP = await Step3Func.GetPaidDailyBonusORPFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, Attandance, tbl_Kerjahdr);
                                        WriteLog("Get Daily Bonus Payment ORP. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Payment : RM " + DailyBonusPaymentORP + ")", false, ServiceName, ServiceProcessID);

                                        OTPayment = await Step3Func.GetPaidOTFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID);
                                        WriteLog("Get OT Payment. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Payment : RM " + OTPayment + ")", false, ServiceName, ServiceProcessID);
                                        OthrInsPayment = await Step3Func.GetPaidInsentifFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, tbl_JenisInsentif, tbl_Insentif, tbl_Kerjahdr);
                                        WriteLog("Get Other Insentif Payment. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Payment : RM " + OthrInsPayment + ")", false, ServiceName, ServiceProcessID);
                                        //add by faeza 10.11.2020
                                        OthrInsPaymentORP = await Step3Func.GetPaidInsentifORPFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, tbl_JenisInsentif, tbl_Insentif, tbl_Kerjahdr);
                                        WriteLog("Get Other Insentif Payment ORP. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Payment : RM " + OthrInsPaymentORP + ")", false, ServiceName, ServiceProcessID);

                                        DeductInsPayment = await Step3Func.GetDeductInsentifFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, tbl_JenisInsentif, tbl_Insentif);
                                        WriteLog("Get Insentif Deduction. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Deduction : RM " + DeductInsPayment + ")", false, ServiceName, ServiceProcessID);
                                        AIPSPayment = await Step3Func.GetAIPSFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, tbl_Produktiviti, tblOptionConfigsWebs, tbl_Kerjahdr, tbl_Kerja);
                                        WriteLog("Get AIPS Insentif. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Payment : RM " + AIPSPayment + ")", false, ServiceName, ServiceProcessID);

                                        AveragePayment = await Step3Func.GetAveragePaidFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, Attandance, tbl_Kerjahdr, tbl_KerjahdrYearly);
                                        WriteLog("Get Average Payment. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Payment : RM " + AveragePayment + ")", false, ServiceName, ServiceProcessID);

                                        //modify by Faeza on 25/2/2020
                                        //calc average salary rate - ORP
                                        AveragePayment = await Step3Func.GetAveragePaidORPFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, tblOptionConfigsWebs, tbl_Kerjahdr);
                                        WriteLog("Get Average ORP Payment. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Payment : RM " + AveragePayment + ")", false, ServiceName, ServiceProcessID);

                                        if (WorkerPaidLeaveLists.Count > 0)
                                        {
                                            //original code
                                            //modified by faeza on 02.08.2021
                                            //LeavePayment = await Step3Func.GetPaidLeaveFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, WorkerPaidLeaveLists, StartWorkDate, false, CutiKategoriList, Pkjmstlist, tblOptionConfigsWebs, tbl_Kerjahdr, tbl_CutiPeruntukan, tbl_PkjIncrmntSalary);
                                            //WriteLog("Get Leave Payment. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Payment : RM " + LeavePayment + ")", false, ServiceName, ServiceProcessID);

                                            //added by faeza 26.09.2022
                                            LeavePayment = await Step3Func.GetPaidLeaveORPFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, WorkerPaidLeaveLists, StartWorkDate, false, CutiKategoriList, Pkjmstlist, tblOptionConfigsWebs, tbl_Kerjahdr, tbl_CutiPeruntukan, tbl_PkjIncrmntSalary, tbl_KerjahdrYearly);
                                            WriteLog("Get Leave Payment. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Payment : RM " + LeavePayment + ")", false, ServiceName, ServiceProcessID);
                                        }
                                        else
                                        {
                                            //original code
                                            //modified by faeza on 02.08.2021
                                            //await Step3Func.GetPaidLeaveFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, WorkerPaidLeaveLists, StartWorkDate, true, CutiKategoriList, Pkjmstlist, tblOptionConfigsWebs, tbl_Kerjahdr, tbl_CutiPeruntukan, tbl_PkjIncrmntSalary);
                                            //WriteLog("No Leave Taken. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ")", false, ServiceName, ServiceProcessID);

                                            //added by faeza 26.09.2022
                                            await Step3Func.GetPaidLeaveORPFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, WorkerPaidLeaveLists, StartWorkDate, true, CutiKategoriList, Pkjmstlist, tblOptionConfigsWebs, tbl_Kerjahdr, tbl_CutiPeruntukan, tbl_PkjIncrmntSalary, tbl_KerjahdrYearly);
                                            WriteLog("No Leave Taken. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ")", false, ServiceName, ServiceProcessID);
                                        }

                                        if (Pkjmstlist.fld_StatusKwspSocso == "1")
                                        {
                                            var CustMod_KWSP = await Step3Func.GetKWSPFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, Pkjmstlist.fld_KodKWSP, false, tbl_JenisInsentif, tbl_Insentif, tbl_Kwsp);
                                            KWSPMjkn = CustMod_KWSP.KWSPMjk;
                                            KWSPPkj = CustMod_KWSP.KWSPPkj;
                                            WriteLog("Get KWSP. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Employer : RM " + KWSPMjkn + ", Employee : RM " + KWSPPkj + ")", false, ServiceName, ServiceProcessID);
                                        }
                                        else
                                        {
                                            await Step3Func.GetKWSPFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, Pkjmstlist.fld_KodKWSP, true, tbl_JenisInsentif, tbl_Insentif, tbl_Kwsp);
                                            WriteLog("No KWSP. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ")", false, ServiceName, ServiceProcessID);
                                        }

                                        if (Pkjmstlist.fld_StatusKwspSocso == "1")
                                        {
                                            var CustMod_Socso = await Step3Func.GetSocsoFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, Pkjmstlist.fld_KodSocso, false, tbl_JenisInsentif, tbl_Insentif, tbl_Socso);
                                            SocsoMjkn = CustMod_Socso.SocsoMjk;
                                            SocsoPkj = CustMod_Socso.SocsoPkj;
                                            WriteLog("Get Socso. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Employer : RM " + SocsoMjkn + ", Employee : RM " + SocsoPkj + ")", false, ServiceName, ServiceProcessID);
                                        }
                                        else
                                        {
                                            await Step3Func.GetSocsoFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, Pkjmstlist.fld_KodSocso, true, tbl_JenisInsentif, tbl_Insentif, tbl_Socso);
                                            WriteLog("No Socso. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ")", false, ServiceName, ServiceProcessID);
                                        }
                                        //Added by Shah 01_01_2024
                                        await Step3Func.GetOverallSalaryFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, tbl_JenisInsentif, tbl_Insentif, tblOptionConfigsWebs, tbl_HutangPekerjaJumlah, true);
                                        var taxWorkerInfo = tbl_TaxWorkerInfo.Where(x => x.fld_NopkjPermanent == Pkjmstlist.fld_NopkjPermanent).FirstOrDefault();
                                        //Added by Shah 01_01_2024
                                        var CustMod_OthrCon = await Step3Func.GetOtherContributionsFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, tbl_PkjCarumanTambahan, tbl_JenisInsentif, tbl_Insentif, tbl_CarumanTambahan, tbl_SubCarumanTambahan, tbl_JadualCarumanTambahan, tbl_TaxRelief, taxWorkerInfo, InsentifExcludePCBYearly);
                                        TotalOthrContMjkCont = CustMod_OthrCon.TotalMjkCont;
                                        TotalOthrContMjkCont = CustMod_OthrCon.TotalPkjCont;
                                        WriteLog("Get Other Contribution. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Employer : RM " + TotalOthrContMjkCont + ", Employee : RM " + TotalOthrContPkjCont + ")", false, ServiceName, ServiceProcessID);
                                        var CustMod_OverallSlry = await Step3Func.GetOverallSalaryFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), MonthSalaryID, tbl_JenisInsentif, tbl_Insentif, tblOptionConfigsWebs, tbl_HutangPekerjaJumlah, false);
                                        OverallSalary = CustMod_OverallSlry.OverallSalary;
                                        Salary = CustMod_OverallSlry.Salary;
                                        TotalDebtDeduction = CustMod_OverallSlry.TotalDebtDeduction;
                                        WriteLog("Get Debt Deduction. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Dept Deduction : RM " + TotalDebtDeduction + ")", false, ServiceName, ServiceProcessID);
                                        WriteLog("Get Overall Salary. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Payment : RM " + OverallSalary + ")", false, ServiceName, ServiceProcessID);
                                        WriteLog("Get Salary. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Payment : RM " + Salary + ")", false, ServiceName, ServiceProcessID);
                                    }
                                }
                            }
                            else
                            {
                                WriteLog("Get Worker Start Working Date. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Start Working Date : Date Not Found)", false, ServiceName, ServiceProcessID);
                            }
                            //DataCount = DataCount + 1;
                        }
                        WriteLog("Total Loop Data Count. (Data - Total Data : " + DataCount2 + ")", false, ServiceName, ServiceProcessID);
                        await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, 80, 1, db, TotalDataCount2, DataCount2);
                        //80%
                        //20%
                        if (Pkjmstlists != null)
                        {
                            DataCount = 1;
                            DataCount2 = DataCount2 + DataCount;
                            TotalDataCount = 15;
                            WriteLog("Start to create Transaction Listing", false, ServiceName, ServiceProcessID);

                            ScTranRemoveCount = RemoveDataFunc.RemoveData_tbl_Sctran(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID);
                            Percentage = ((DataCount / TotalDataCount) * 19.5m) + 80;
                            WriteLog("Total Loop Data Count. (Data - Total Data : " + DataCount2 + ")", false, ServiceName, ServiceProcessID);
                            await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 1, db, TotalDataCount2, DataCount2);
                            WriteLog("Removed Transaction Listing Data. (Data - Total Data Removed : " + ScTranRemoveCount + ")", false, ServiceName, ServiceProcessID);

                            WorkSCTransList = Step4Func.GetWorkActvtyPktFunc(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlists);
                            DataCount = DataCount + 1;
                            DataCount2 = DataCount2 + 1;
                            Percentage = ((DataCount / TotalDataCount) * 19.5m) + 80;
                            WriteLog("Total Loop Data Count. (Data - Total Data : " + DataCount2 + ")", false, ServiceName, ServiceProcessID);
                            await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 1, db, TotalDataCount2, DataCount2);
                            WriteLog("Get Work Data By Activity & Peringkat. (Data - Total Data : " + WorkSCTransList.Count + ")", false, ServiceName, ServiceProcessID);

                            Step4Func.GetAmountWorkActivityFunc(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, WorkSCTransList, out Log, Pkjmstlists);
                            DataCount = DataCount + 1;
                            DataCount2 = DataCount2 + 1;
                            Percentage = ((DataCount / TotalDataCount) * 19.5m) + 80;
                            WriteLog("Total Loop Data Count. (Data - Total Data : " + DataCount2 + ")", false, ServiceName, ServiceProcessID);
                            await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 1, db, TotalDataCount2, DataCount2);
                            WriteLog2(Log, ServiceName, ServiceProcessID);

                            Step4Func.GetAmountDailyBonusFunc(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, WorkSCTransList, out Log, Pkjmstlists);
                            DataCount = DataCount + 1;
                            DataCount2 = DataCount2 + 1;
                            Percentage = ((DataCount / TotalDataCount) * 19.5m) + 80;
                            WriteLog("Total Loop Data Count. (Data - Total Data : " + DataCount2 + ")", false, ServiceName, ServiceProcessID);
                            await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 1, db, TotalDataCount2, DataCount2);
                            WriteLog2(Log, ServiceName, ServiceProcessID);

                            Step4Func.GetAmountOTFunc(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, WorkSCTransList, out Log, Pkjmstlists);
                            DataCount = DataCount + 1;
                            DataCount2 = DataCount2 + 1;
                            Percentage = ((DataCount / TotalDataCount) * 19.5m) + 80;
                            WriteLog("Total Loop Data Count. (Data - Total Data : " + DataCount2 + ")", false, ServiceName, ServiceProcessID);
                            await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 1, db, TotalDataCount2, DataCount2);
                            WriteLog2(Log, ServiceName, ServiceProcessID);

                            Step4Func.GetAmountLeaveFunc(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, out Log, Pkjmstlists);
                            DataCount = DataCount + 1;
                            DataCount2 = DataCount2 + 1;
                            Percentage = ((DataCount / TotalDataCount) * 19.5m) + 80;
                            WriteLog("Total Loop Data Count. (Data - Total Data : " + DataCount2 + ")", false, ServiceName, ServiceProcessID);
                            await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 1, db, TotalDataCount2, DataCount2);
                            WriteLog2(Log, ServiceName, ServiceProcessID);

                            Step4Func.GetAmountAddedInsentifFunc(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, out Log, Pkjmstlists);
                            DataCount = DataCount + 1;
                            DataCount2 = DataCount2 + 1;
                            Percentage = ((DataCount / TotalDataCount) * 19.5m) + 80;
                            WriteLog("Total Loop Data Count. (Data - Total Data : " + DataCount2 + ")", false, ServiceName, ServiceProcessID);
                            await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 1, db, TotalDataCount2, DataCount2);
                            WriteLog2(Log, ServiceName, ServiceProcessID);

                            Step4Func.GetAmountDeductedInsentifFunc(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, out Log, Pkjmstlists);
                            DataCount = DataCount + 1;
                            DataCount2 = DataCount2 + 1;
                            Percentage = ((DataCount / TotalDataCount) * 19.5m) + 80;
                            WriteLog("Total Loop Data Count. (Data - Total Data : " + DataCount2 + ")", false, ServiceName, ServiceProcessID);
                            await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 1, db, TotalDataCount2, DataCount2);
                            WriteLog2(Log, ServiceName, ServiceProcessID);

                            Step4Func.GetAmountKWSPFunc(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, out Log, Pkjmstlists);
                            DataCount = DataCount + 1;
                            DataCount2 = DataCount2 + 1;
                            Percentage = ((DataCount / TotalDataCount) * 19.5m) + 80;
                            WriteLog("Total Loop Data Count. (Data - Total Data : " + DataCount2 + ")", false, ServiceName, ServiceProcessID);
                            await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 1, db, TotalDataCount2, DataCount2);
                            WriteLog2(Log, ServiceName, ServiceProcessID);

                            Step4Func.GetAmountSocsoFunc(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, out Log, Pkjmstlists);
                            DataCount = DataCount + 1;
                            DataCount2 = DataCount2 + 1;
                            Percentage = ((DataCount / TotalDataCount) * 19.5m) + 80;
                            WriteLog("Total Loop Data Count. (Data - Total Data : " + DataCount2 + ")", false, ServiceName, ServiceProcessID);
                            await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 1, db, TotalDataCount2, DataCount2);
                            WriteLog2(Log, ServiceName, ServiceProcessID);

                            Step4Func.GetAmountOtherContributionsFunc(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, out Log, Pkjmstlists);
                            DataCount = DataCount + 1;
                            DataCount2 = DataCount2 + 1;
                            Percentage = ((DataCount / TotalDataCount) * 19.5m) + 80;
                            WriteLog("Total Loop Data Count. (Data - Total Data : " + DataCount2 + ")", false, ServiceName, ServiceProcessID);
                            await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 1, db, TotalDataCount2, DataCount2);
                            WriteLog2(Log, ServiceName, ServiceProcessID);

                            Step4Func.GetAmountAIPSFunc(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, out Log, Pkjmstlists);
                            DataCount = DataCount + 1;
                            DataCount2 = DataCount2 + 1;
                            Percentage = ((DataCount / TotalDataCount) * 19.5m) + 80;
                            WriteLog("Total Loop Data Count. (Data - Total Data : " + DataCount2 + ")", false, ServiceName, ServiceProcessID);
                            await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 1, db, TotalDataCount2, DataCount2);
                            WriteLog2(Log, ServiceName, ServiceProcessID);

                            Step4Func.GetAmountWorkerSalaryFunc(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, out Log, Pkjmstlists);
                            DataCount = DataCount + 1;
                            DataCount2 = DataCount2 + 1;
                            Percentage = ((DataCount / TotalDataCount) * 19.5m) + 80;
                            WriteLog("Total Loop Data Count. (Data - Total Data : " + DataCount2 + ")", false, ServiceName, ServiceProcessID);
                            await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 1, db, TotalDataCount2, DataCount2);
                            WriteLog2(Log, ServiceName, ServiceProcessID);

                            Step4Func.GetDebitCreditBalanceFunc(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, out Log);
                            DataCount = DataCount + 1;
                            DataCount2 = DataCount2 + 1;
                            Percentage = ((DataCount / TotalDataCount) * 19.5m) + 80;
                            WriteLog("Total Loop Data Count. (Data - Total Data : " + DataCount2 + ")", false, ServiceName, ServiceProcessID);
                            await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 1, db, TotalDataCount2, DataCount2);
                            WriteLog2(Log, ServiceName, ServiceProcessID);

                            Step5Func.AddTo_tbl_SAPPostRef(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, out Log, 1, "SAP Data Creat.", Pkjmstlists);
                            DataCount = DataCount + 1;
                            DataCount2 = DataCount2 + 1 - 1;
                            Percentage = ((DataCount / TotalDataCount) * 19.5m) + 80;
                            WriteLog("Total Loop Data Count. (Data - Total Data : " + DataCount2 + ")", false, ServiceName, ServiceProcessID);
                            await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 1, db, TotalDataCount2, DataCount2);
                            WriteLog2(Log, ServiceName, ServiceProcessID);

                            Step4Func.Add_tbl_AuditTrail(NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Year);
                        }
                        else
                        {
                            WriteLog("No worker found)", false, ServiceName, ServiceProcessID);
                        }
                        Percentage = 100;
                        //20%

                        //************************Others insentif for second payslip*********************
                        //added faeza 26.02.2023
                        if (PkjmstlistsSpecialInsentif != null)
                        {
                            foreach (var Pkjmstlist in PkjmstlistsSpecialInsentif)
                            {
                                SpecialInsentifList = new List<tbl_SpecialInsentif>();
                                Step2Func.GetSpecialInsentifFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DateTimeFunc.GetDateTime(), Month, Year, getservicesdetail.fld_SevicesActivity, getservicesdetail.fld_ServicesName, getservicesdetail.fld_ClientID, Pkjmstlist.fld_Nopkj.Trim(), out SpecialInsentif, tbl_JenisInsentifSpecial, tbl_Insentif);

                                if (SpecialInsentif != null)
                                {
                                    WriteLog("Get SpecialInsentif Payment. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Payment : RM " + SpecialInsentif.fld_NilaiInsentif + ")", false, ServiceName, ServiceProcessID);
                                    SpecialInsentifList.Add(SpecialInsentif);
                                }
                                else
                                {
                                    WriteLog("No SpecialInsentif.", false, ServiceName, ServiceProcessID);
                                }

                                await Step2Func.AddTo_tbl_SpecialInsentif(NegaraID, SyarikatID, WilayahID, LadangID, SpecialInsentifList);
                                WriteLog("Insert To tbl_SpecialInsentif. (Data - No Pkj : " + Pkjmstlist.fld_Nopkj.Trim() + ", Total Data : " + SpecialInsentifList.Count + ")", false, ServiceName, ServiceProcessID);
                            }
                        }
                    }
                    else
                    {
                        var workerNoPermIds = Pkjmstlists.Where(x => x.fld_NopkjPermanent == null).ToList();
                        var listworkerNoPermIds = "";

                        foreach (var item in workerNoPermIds)
                        {
                            listworkerNoPermIds += item.fld_Nopkj + " - " + item.fld_Nama + ", ";
                        }

                        string hdrmsg = "Generate Not Completed!";
                        string msg = "Some worker no assign permanent worker id:<b/>";
                        msg += "Worker list: " + listworkerNoPermIds + "<b/>";
                        string status = "warning";
                        SendStatusToWeb(divid.Value, hdrmsg, msg, status);
                        await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 0, db, TotalDataCount2, DataCount2);
                        SevicesProcess = new ModelsHQ.tbl_SevicesProcess();
                    }

                }
            }
            catch(Exception ex)
            {
                LogFunc.WriteErrorLog(ex.Message, ex.StackTrace, ex.Source, ex.TargetSite.ToString(), ServiceName, ServiceProcessID);
                //Shah - notification after finish generate
                string hdrmsg = "Generate Not Completed!";
                string msg = "Error on generate:<b/>";
                msg += "Worker ID: " + workerid + "<b/>";
                msg += "Worker Name: " + workername + "<b/>";
                msg += "Date Process: " + datedata.Value.ToString("dd/MM/yyyy") + "<b/>";
                msg += "Error Trace: " + ex.StackTrace;
                string status = "warning";
                SendStatusToWeb(divid.Value, hdrmsg, msg, status);
                await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 0, db, TotalDataCount2, DataCount2);
                SevicesProcess = new ModelsHQ.tbl_SevicesProcess();
            }
            finally
            {
                if (SevicesProcess != null)
                {
                    await Step1Func.UpdateServicesProcessPercFunc(SevicesProcess, Percentage, 0, db, TotalDataCount2, DataCount2);
                    //Shah - notification after finish generate
                    string hdrmsg = "Generate Completed!";
                    string msg = "Overall data process: " + DataCount2 + "/" + TotalDataCount2;
                    string status = "success";
                    SendStatusToWeb(divid.Value, hdrmsg, msg, status);
                }
                WriteLog("End Process.", false, ServiceName, ServiceProcessID);
                OnStop();
            }
        }

        public void SendStatusToWeb(int divid, string hdrmsg, string msg, string status)
        {
            var url = ConfigReader2("commonconfig", "appurl");
            var connection = new HubConnection(url);
            var myHub = connection.CreateHubProxy("GenerateSalaryHub");
            connection.Start().Wait();
            myHub.Invoke("GetGenEnd", divid, hdrmsg, msg, status).Wait();
            connection.Dispose();
        }

        private string ConfigReader2(string Name, string Data)
        {
            string getresult = "";
            INIReaderFunc parser = new INIReaderFunc(AppDomain.CurrentDomain.BaseDirectory + "CommonConfigs.ini");

            getresult = parser.GetSetting(Name, Data);

            return getresult;
        }

        public void WriteLog(string message, bool startprocess, string ServicesName, long ServiceProcessID)
        {
            string msg = "";
            if (startprocess)
            {
                msg += DateTimeFunc.GetDateTime() + " - " + message;
            }
            else
            {
                msg += DateTimeFunc.GetDateTime() + " - " + message;
            }
            LogFunc.WriteProcessLog(msg, ServicesName, ServiceProcessID);
        }

        public void WriteLog2(string message, string ServicesName, long ServiceProcessID)
        {
            string msg = "";
            if (message != "")
            {
                msg += message;
                LogFunc.WriteProcessLog(msg, ServicesName, ServiceProcessID);
            }
        }

        private string ConfigReader(string Name, string Data)
        {
            string getresult = "";
            INIReaderFunc parser = new INIReaderFunc(AppDomain.CurrentDomain.BaseDirectory + "Configs.ini");

            getresult = parser.GetSetting(Name, Data);

            return getresult;
        }

        public async Task OnDebugAsync()
        {
            await OnStartAsync(null);
        }

        //uncomment for debug
        //public void OnDebugAsync()
        //{
        //    OnStartAsync(null);
        //}

        protected async Task OnStartAsync(string[] args)
        {
            await DoProcess();
        }

        //uncomment for debug
        //protected async void OnStartAsync(string[] args)
        //{
        //    await DoProcess();
        //}

        protected override void OnStop()
        {
            Stop();
        }
    }
}
