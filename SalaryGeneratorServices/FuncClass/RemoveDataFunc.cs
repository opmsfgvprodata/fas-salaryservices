using SalaryGeneratorServices.ModelsEstate;
using SalaryGeneratorServices.ModelsHQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryGeneratorServices.FuncClass
{
    class RemoveDataFunc
    {
        private GetConnectFunc conn = new GetConnectFunc();
        public int RemoveData_tbl_KerjaBonus(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, List<tbl_Pkjmast> Pkjmstlists)
        {
            int totalcount = 0;
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            var GetPkjNo = Pkjmstlists.Select(s => s.fld_Nopkj).ToList();
            var KerjaBonus = db2.tbl_KerjaBonus.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && GetPkjNo.Contains(x.fld_Nopkj)).ToList();
            totalcount = KerjaBonus.Count;
            if (totalcount != 0)
            {
                db2.tbl_KerjaBonus.RemoveRange(KerjaBonus);
                db2.SaveChanges();
            }
            db2.Dispose();
            return totalcount;
        }

        public int RemoveData_tbl_KerjaOT(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, List<tbl_Pkjmast> Pkjmstlists)
        {
            int totalcount = 0;
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            var GetPkjNo = Pkjmstlists.Select(s => s.fld_Nopkj).ToList();
            var KerjaOT = db2.tbl_KerjaOT.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && GetPkjNo.Contains(x.fld_Nopkj)).ToList();
            totalcount = KerjaOT.Count;
            if (totalcount != 0)
            {
                db2.tbl_KerjaOT.RemoveRange(KerjaOT);
                db2.SaveChanges();
            }
            db2.Dispose();
            return totalcount;
        }

        //addedd by faeza 26.02.2023
        public int RemoveData_tbl_SpecialInsentif(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, List<tbl_Pkjmast> Pkjmstlists, List<tbl_JenisInsentif> tbl_JenisInsentifSpecial)
        {
            int totalcount = 0;
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            var specialIncentiveCode = tbl_JenisInsentifSpecial.Select(s => s.fld_KodInsentif).ToArray();
            var GetPkjNo = Pkjmstlists.Select(s => s.fld_Nopkj).ToList();
            var SpecialInsentif = db2.tbl_SpecialInsentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Year == Year && x.fld_Month == Month && GetPkjNo.Contains(x.fld_Nopkj) && specialIncentiveCode.Contains(x.fld_KodInsentif)).ToList();
            var SpecialInsentif1 = db2.tbl_SpecialInsentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Year == Year && x.fld_Month == Month && specialIncentiveCode.Contains(x.fld_KodInsentif)).ToList();

            totalcount = SpecialInsentif.Count;
            if (totalcount != 0)
            {
                db2.tbl_SpecialInsentif.RemoveRange(SpecialInsentif);
                db2.SaveChanges();
            }
            db2.Dispose();
            return totalcount;
        }

        public int RemoveData_tbl_KerjahdrCuti(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, List<tbl_Pkjmast> Pkjmstlists)
        {
            int totalcount = 0;
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            var GetPkjNo = Pkjmstlists.Select(s => s.fld_Nopkj).ToList();
            var KerjahdrCuti = db2.tbl_KerjahdrCuti.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && GetPkjNo.Contains(x.fld_Nopkj)).ToList();
            totalcount = KerjahdrCuti.Count;
            if (totalcount != 0)
            {
                db2.tbl_KerjahdrCuti.RemoveRange(KerjahdrCuti);
                db2.SaveChanges();
            }
            db2.Dispose();
            return totalcount;
        }

        public int RemoveData_tbl_KerjahdrCutiTahunan(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, List<tbl_Pkjmast> Pkjmstlists)
        {
            int totalcount = 0;
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            var GetPkjNo = Pkjmstlists.Select(s => s.fld_Nopkj).ToList();
            var KerjahdrCutiTahunan = db2.tbl_KerjahdrCutiTahunan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && GetPkjNo.Contains(x.fld_Nopkj)).ToList();
            totalcount = KerjahdrCutiTahunan.Count;
            if (totalcount != 0)
            {
                db2.tbl_KerjahdrCutiTahunan.RemoveRange(KerjahdrCutiTahunan);
                db2.SaveChanges();
            }
            db2.Dispose();
            return totalcount;
        }

        public int RemoveData_tbl_GajiBulanan(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, List<tbl_Pkjmast> Pkjmstlists)
        {
            int totalcount = 0;
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            var GetPkjNo = Pkjmstlists.Select(s => s.fld_Nopkj).ToList();
            var GajiBulanan = db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && GetPkjNo.Contains(x.fld_Nopkj)).ToList();
            totalcount = GajiBulanan.Count;
            if (totalcount != 0)
            {
                db2.tbl_GajiBulanan.RemoveRange(GajiBulanan);
                db2.SaveChanges();
            }
            db2.Dispose();
            return totalcount;
        }

        public int RemoveData_tbl_Sctran(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID)
        {
            int totalcount = 0;
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            var Sctran = db2.tbl_Sctran.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && x.fld_DivisionID == DivisionID).ToList();
            totalcount = Sctran.Count;
            if (totalcount != 0)
            {
                db2.tbl_Sctran.RemoveRange(Sctran);
                db2.SaveChanges();
            }
            db2.Dispose();
            return totalcount;
        }

        public void RemoveData_tbl_SevicesProcess(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();

            var ProcessService = db.tbl_SevicesProcess.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID).ToList();

            if (ProcessService.Count != 0)
            {
                db.tbl_SevicesProcess.RemoveRange(ProcessService);
                db.SaveChanges();
            }
        }

        public int RemoveData_tbl_CarumanTambahan(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, List<tbl_Pkjmast> Pkjmstlists)
        {
            int totalcount = 0;
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            var GetPkjNo = Pkjmstlists.Select(s => s.fld_Nopkj).ToList();
            var GajiBulananID = db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && GetPkjNo.Contains(x.fld_Nopkj)).Select(s => s.fld_ID).ToList();
            var CarumanTambahan = db2.tbl_ByrCarumanTambahan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && GajiBulananID.Contains(x.fld_GajiID.Value)).ToList();
            totalcount = CarumanTambahan.Count;
            if (totalcount != 0)
            {
                db2.tbl_ByrCarumanTambahan.RemoveRange(CarumanTambahan);
                db2.SaveChanges();
            }
            db2.Dispose();
            return totalcount;
        }

        public void RemoveData_tbl_SAPPostingDetail(GenSalaryModelEstate db, Guid SAPPostID)
        {
            var SAPPostDataDetails = db.tbl_SAPPostDataDetails.Where(x => x.fld_SAPPostRefID == SAPPostID).ToList();
            if (SAPPostDataDetails.Count > 0)
            {
                db.tbl_SAPPostDataDetails.RemoveRange(SAPPostDataDetails);
                db.SaveChanges();
            }
        }
    }
}
