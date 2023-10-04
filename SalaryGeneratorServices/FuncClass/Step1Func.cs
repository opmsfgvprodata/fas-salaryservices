using SalaryGeneratorServices.CustomModels;
using SalaryGeneratorServices.ModelsEstate;
using SalaryGeneratorServices.ModelsHQ;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryGeneratorServices.FuncClass
{
    class Step1Func
    {
        public async Task AddServicesProcessFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            var checkservicesprocess = await db.tbl_SevicesProcess.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_ServicesName == servicesname && x.fld_ProcessName == processname && x.fld_Flag == 1).CountAsync();
            ModelsHQ.tbl_SevicesProcess tbl_SevicesProcess = new ModelsHQ.tbl_SevicesProcess();

            if (checkservicesprocess == 0)
            {
                tbl_SevicesProcess.fld_ClientID = ClientID;
                tbl_SevicesProcess.fld_DTProcess = DTProcess;
                tbl_SevicesProcess.fld_Flag = 1;
                tbl_SevicesProcess.fld_Month = Month;
                tbl_SevicesProcess.fld_NegaraID = NegaraID;
                tbl_SevicesProcess.fld_ProcessName = processname;
                tbl_SevicesProcess.fld_ProcessPercentage = 0;
                tbl_SevicesProcess.fld_SelCatVal = LadangID;
                tbl_SevicesProcess.fld_ServicesName = servicesname;
                tbl_SevicesProcess.fld_SyarikatID = SyarikatID;
                tbl_SevicesProcess.fld_UplSelCat = 4;
                tbl_SevicesProcess.fld_UserID = UserID;
                tbl_SevicesProcess.fld_Year = Year;
                tbl_SevicesProcess.fld_WilayahID = WilayahID;
                tbl_SevicesProcess.fld_LadangID = LadangID;

                db.tbl_SevicesProcess.Add(tbl_SevicesProcess);
                await db.SaveChangesAsync();
            }
            else
            {
            }

            db.Dispose();
        }

        public async Task UpdateServicesProcessPercFunc(ModelsHQ.tbl_SevicesProcess tbl_SevicesProcess, decimal Percentage, int Flag, GenSalaryModelHQ db, int TotalData, int RunningData)
        {
            string DataToProcess = RunningData + "/" + TotalData;
            tbl_SevicesProcess.fld_ProcessPercentage = Percentage;
            tbl_SevicesProcess.fld_DataToProcess = DataToProcess;
            tbl_SevicesProcess.fld_Flag = Flag;
            db.Entry(tbl_SevicesProcess).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task<ModelsHQ.tbl_SevicesProcess> GetServiceProcessDetail(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            ModelsHQ.tbl_SevicesProcess SevicesProcess = new ModelsHQ.tbl_SevicesProcess();

            SevicesProcess = await db.tbl_SevicesProcess.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_ServicesName == servicesname && x.fld_ProcessName == processname && x.fld_Flag == 1 && x.fld_DivisionID == DivisionID).FirstOrDefaultAsync();

            return SevicesProcess;
        }

        public async Task<List<tbl_Pkjmast>> GetActiveWorkerFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, List<tbl_Pkjmast> tbl_Pkjmast)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            LogFunc LogFunc = new LogFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            List<tbl_Pkjmast> Pkjmstlist = new List<tbl_Pkjmast>();

            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            var GetStatusXActv = await db.tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "sbbTakAktif" && x.fldOptConfFlag2 == "1" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).ToArrayAsync();

            var PkjKerja = await db2.tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year).Select(s => s.fld_Nopkj).Distinct().ToListAsync();

            Pkjmstlist = tbl_Pkjmast.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_DivisionID == DivisionID && x.fld_StatusApproved == 1 && (x.fld_Kdaktf == "1" || (x.fld_Kdaktf == "0" && GetStatusXActv.Contains(x.fld_Sbtakf))) && PkjKerja.Contains(x.fld_Nopkj)).ToList();

            db.Dispose();
            db2.Dispose();

            return Pkjmstlist;
        }

        public List<tbl_Pkjmast> GetAllWorkerFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, List<tbl_Pkjmast> tbl_Pkjmast)
        {
            //GenSalaryModelHQ db = new GenSalaryModelHQ();
            //GetConnectFunc conn = new GetConnectFunc();
            //LogFunc LogFunc = new LogFunc();
            //string host, catalog, user, pass = "";
            //conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            List<tbl_Pkjmast> Pkjmstlist = new List<tbl_Pkjmast>();

            //GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            Pkjmstlist = tbl_Pkjmast.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_DivisionID == DivisionID).ToList();

            //db.Dispose();
            //db2.Dispose();

            return Pkjmstlist;
        }

        //added by faeza 26.02.2023
        public List<tbl_Insentif> GetWorkerSpecialInsentifFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, List<tbl_JenisInsentif> tbl_JenisInsentifSpecial, List<tbl_Insentif> tbl_Insentif)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            LogFunc LogFunc = new LogFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            List<tbl_Insentif> Pkjmstlist = new List<tbl_Insentif>();

            // GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            var KodInsentifSpecial = tbl_JenisInsentifSpecial.Select(s => s.fld_KodInsentif).ToList();
            Pkjmstlist = tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && x.fld_Deleted == false && KodInsentifSpecial.Contains(x.fld_KodInsentif)).ToList();

            //db.Dispose();
            //db2.Dispose();

            return Pkjmstlist;
        }

        public List<CustMod_DateList> GetDateListFunc(int? month, int? year)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            List<CustMod_DateList> DateList = new List<CustMod_DateList>();
            var frstdaythsmnth = new DateTime(int.Parse(year.ToString()), int.Parse(month.ToString()), 1);
            var fstdaynxtmnth = frstdaythsmnth.AddMonths(1);

            for (DateTime date = frstdaythsmnth; date < fstdaynxtmnth; date = date.AddDays(1))
            {
                DateList.Add(new CustMod_DateList() { Date = date });
            }

            return DateList;
        }

        public DateTime? GetDateStarkWorkingFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, string PkjNo, List<tbl_Kerjahdr> tbl_Kerjahdr)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            string host, catalog, user, pass = "";
            DateTime? startworkdate = new DateTime();

            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            startworkdate = tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == PkjNo).OrderBy(o => o.fld_Tarikh).Select(s => s.fld_Tarikh).Take(1).SingleOrDefault();

            db.Dispose();
            db2.Dispose();
            return startworkdate;
        }

        public async Task<List<tbl_CutiKategori>> GetPaidLeaveFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            List<tbl_CutiKategori> CutiKategoriList = new List<tbl_CutiKategori>();

            CutiKategoriList = await db.tbl_CutiKategori.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false).ToListAsync();

            db.Dispose();
            return CutiKategoriList;
        }

        public async Task<List<tbl_JenisAktiviti>> GetActvtyTypeBonusStatusFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            List<tbl_JenisAktiviti> JenisAktiviti = new List<tbl_JenisAktiviti>();

            JenisAktiviti = await db.tbl_JenisAktiviti.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_DisabledFlag != 3 && x.fld_Deleted == false).ToListAsync();

            db.Dispose();
            return JenisAktiviti;
        }
    }
}
