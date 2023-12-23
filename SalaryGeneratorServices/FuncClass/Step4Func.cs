using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalaryGeneratorServices.CustomModels;
using SalaryGeneratorServices.ModelsHQ;
using SalaryGeneratorServices.ModelsEstate;
using System.Data.Entity;

namespace SalaryGeneratorServices.FuncClass
{
    class Step4Func
    {
        private LogFunc LogFunc2 = new LogFunc();
        private DateTimeFunc DateTimeFunc = new DateTimeFunc();
        //public List<CustMod_WorkSCTrans> GetWorkActvtyPktFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, List<tbl_Pkjmast> PkjMastList)
        //{
        //    LogFunc LogFunc = new LogFunc();
        //    GenSalaryModelHQ db = new GenSalaryModelHQ();
        //    GetConnectFunc conn = new GetConnectFunc();
        //    List<CustMod_WorkSCTrans> WorkSCTransList = new List<CustMod_WorkSCTrans>();
        //    string host, catalog, user, pass = "";
        //    string keterangan = "";
        //    int ID = 1;
        //    conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
        //    GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

        //    try
        //    {
        //        var NoPkjList = PkjMastList.Select(s => s.fld_Nopkj).ToArray();
        //        var vw_KerjaInfoDetails = db2.vw_KerjaInfoDetails.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_KodAktvt != null); //&& NoPkjList.Contains(x.fld_Nopkj)
        //        var WorkDistincts = vw_KerjaInfoDetails.Select(s => new { s.fld_JnisAktvt, s.fld_KodAktvt, s.fld_JnsPkt, s.fld_KodPkt, s.fld_KodGL, s.fld_GLKod, s.fld_KodAktivitiSAP, s.fld_NNCC }).Distinct().ToList();
        //        var tbl_UpahAktiviti = db.tbl_UpahAktiviti.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID).ToList();

        //        foreach (var WorkDistinct in WorkDistincts)
        //        {
        //            LogFunc.DataScTransChecking("fld_JnisAktvt = " + WorkDistinct.fld_JnisAktvt + " - fld_JnsPkt = " + WorkDistinct.fld_JnsPkt + " - fld_Keterangan =  - fld_KodAktvt = " + WorkDistinct.fld_KodAktvt + " - fld_KodGL = " + WorkDistinct.fld_KodGL + " - fld_KodPkt = " + WorkDistinct.fld_KodPkt + " - fld_NNCC = " + WorkDistinct.fld_NNCC + " - fld_SapActCode =  - fld_SAPGL =  - fld_SAPIO = ");
        //            keterangan = tbl_UpahAktiviti.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktvt == WorkDistinct.fld_KodAktvt).Select(s => s.fld_Desc).FirstOrDefault();
        //            WorkSCTransList.Add(new CustMod_WorkSCTrans() { fld_KodGL = WorkDistinct.fld_KodGL, fld_JnisAktvt = WorkDistinct.fld_JnisAktvt, fld_KodAktvt = WorkDistinct.fld_KodAktvt, fld_JnsPkt = WorkDistinct.fld_JnsPkt, fld_KodPkt = WorkDistinct.fld_KodPkt, fld_Keterangan = keterangan, fld_SAPGL = WorkDistinct.fld_GLKod, fld_NNCC = WorkDistinct.fld_NNCC, fld_SapActCode = WorkDistinct.fld_KodAktivitiSAP == "0" ? "-" : WorkDistinct.fld_KodAktivitiSAP, fld_ID = ID });
        //            ID++;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogFunc.DataScTransChecking("Error = " + ex.Message + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.Data);
        //    }

        //    db.Dispose();
        //    db2.Dispose();

        //    return WorkSCTransList;
        //}

        //public void GetAmountWorkActivityFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, List<CustMod_WorkSCTrans> WorkSCTransList, out string Log, List<tbl_Pkjmast> PkjMastList)
        //{
        //    LogFunc LogFunc = new LogFunc();
        //    GetConnectFunc conn = new GetConnectFunc();
        //    string host, catalog, user, pass = "";
        //    conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
        //    GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
        //    Log = "";
        //    string message = "";
        //    int i = 1;
        //    var NoPkjList = PkjMastList.Select(s => s.fld_Nopkj).ToArray();
        //    string CodeActSAP = "";
        //    var vw_KerjaInfoDetails = db2.vw_KerjaDetailScTrans.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && NoPkjList.Contains(x.fld_Nopkj)).ToList();
        //    foreach (var WorkSCTrans in WorkSCTransList)
        //    {
        //        //var Amount = db2.vw_KerjaInfoDetails.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_JnsPkt == WorkSCTrans.fld_JnsPkt && x.fld_KodPkt == WorkSCTrans.fld_KodPkt && x.fld_JnisAktvt == WorkSCTrans.fld_JnisAktvt && x.fld_KodAktvt == WorkSCTrans.fld_KodAktvt && x.fld_KodGL == WorkSCTrans.fld_KodGL && x.fld_DivisionID == DivisionID).Sum(s => s.fld_OverallAmount);
        //        var Amount = vw_KerjaInfoDetails.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_JnsPkt == WorkSCTrans.fld_JnsPkt && x.fld_KodPkt == WorkSCTrans.fld_KodPkt && x.fld_JnisAktvt == WorkSCTrans.fld_JnisAktvt && x.fld_KodAktvt == WorkSCTrans.fld_KodAktvt && x.fld_GLKod == WorkSCTrans.fld_SAPGL && x.fld_NNCC == WorkSCTrans.fld_NNCC).Sum(s => s.fld_OverallAmount);
        //        if (Amount != 0)
        //        {
        //            if (WorkSCTrans.fld_SapActCode == null)
        //            {
        //                CodeActSAP = "-";
        //            }
        //            else
        //            {
        //                CodeActSAP = WorkSCTrans.fld_SapActCode.Trim();
        //            }
        //            LogFunc.DataScTransChecking("fld_JnisAktvt = " + WorkSCTrans.fld_JnisAktvt + " - fld_JnsPkt = " + WorkSCTrans.fld_JnsPkt + " - fld_Keterangan = " + WorkSCTrans.fld_Keterangan + " - fld_KodAktvt = " + WorkSCTrans.fld_KodAktvt + " - fld_KodGL = " + WorkSCTrans.fld_KodGL + " - fld_KodPkt = " + WorkSCTrans.fld_KodPkt + " - fld_NNCC = " + WorkSCTrans.fld_NNCC + " - fld_SapActCode = " + WorkSCTrans.fld_SapActCode + " - fld_SAPGL = " + WorkSCTrans.fld_SAPGL + " - fld_SAPIO = " + WorkSCTrans.fld_SAPIO);
        //            AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Amount, WorkSCTrans.fld_JnsPkt, WorkSCTrans.fld_KodPkt.Trim(), WorkSCTrans.fld_JnisAktvt.Trim(), WorkSCTrans.fld_KodAktvt.Trim(), WorkSCTrans.fld_KodGL.Trim(), WorkSCTrans.fld_Keterangan.Trim(), DTProcess, UserID, Month, Year, "D", 1, WorkSCTrans.fld_SAPGL.Trim(), "-", WorkSCTrans.fld_NNCC.Trim(), CodeActSAP);
        //            message = "Transaction Listing (Job). (Data - Code Pkt : " + WorkSCTrans.fld_KodPkt + ", Code Activity : " + WorkSCTrans.fld_KodAktvt + ", Amount : RM " + Amount + ")";
        //            Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
        //            i++;
        //        }
        //    }
        //    db2.Dispose();
        //}

        public List<CustMod_WorkSCTrans> GetWorkActvtyPktFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, List<tbl_Pkjmast> PkjMastList)
        {
            //GenSalaryModelHQ db = new GenSalaryModelHQ();
            //GetConnectFunc conn = new GetConnectFunc();
            //List<CustMod_WorkSCTrans> WorkSCTransList = new List<CustMod_WorkSCTrans>();
            //string host, catalog, user, pass = "";
            //string keterangan = "";
            //conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            //GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            //var NoPkjList = PkjMastList.Select(s => s.fld_Nopkj).ToArray();
            ////var WorkDistincts = db2.vw_KerjaInfoDetails.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_KodAktvt != null && x.fld_DivisionID == DivisionID).Select(s => new { s.fld_JnisAktvt, s.fld_KodAktvt, s.fld_JnsPkt, s.fld_KodPkt, s.fld_KodGL, s.fld_GLKod, s.fld_KodAktivitiSAP, s.fld_NNCC }).Distinct().ToList();
            //var WorkDistincts = db2.vw_KerjaInfoDetails.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_KodAktvt != null && NoPkjList.Contains(x.fld_Nopkj)).Select(s => new { s.fld_JnisAktvt, s.fld_KodAktvt, s.fld_JnsPkt, s.fld_KodPkt, s.fld_KodGL, s.fld_GLKod, s.fld_KodAktivitiSAP, s.fld_NNCC }).Distinct().ToList();

            //foreach (var WorkDistinct in WorkDistincts)
            //{
            //    keterangan = db.tbl_UpahAktiviti.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_KodAktvt == WorkDistinct.fld_KodAktvt).Select(s => s.fld_Desc).FirstOrDefault();
            //    WorkSCTransList.Add(new CustMod_WorkSCTrans() { fld_KodGL = WorkDistinct.fld_KodGL, fld_JnisAktvt = WorkDistinct.fld_JnisAktvt, fld_KodAktvt = WorkDistinct.fld_KodAktvt, fld_JnsPkt = WorkDistinct.fld_JnsPkt, fld_KodPkt = WorkDistinct.fld_KodPkt, fld_Keterangan = keterangan, fld_SAPGL = WorkDistinct.fld_GLKod, fld_NNCC = WorkDistinct.fld_NNCC, fld_SapActCode = WorkDistinct.fld_KodAktivitiSAP == "0" ? "-" : WorkDistinct.fld_KodAktivitiSAP });
            //}

            //db.Dispose();
            //db2.Dispose();

            //return WorkSCTransList;

            LogFunc LogFunc = new LogFunc();
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            List<CustMod_WorkSCTrans> WorkSCTransList = new List<CustMod_WorkSCTrans>();
            string host, catalog, user, pass = "";
            string keterangan = "";
            int ID = 1;
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            try
            {
                var NoPkjList = PkjMastList.Select(s => s.fld_Nopkj).ToArray();
                //modified by Faeza on 21/2/2020
                //back to original code - remove .ToList()
                var vw_KerjaInfoDetails = db2.vw_KerjaInfoDetails.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_KodAktvt != null); //&& NoPkjList.Contains(x.fld_Nopkj)
                var WorkDistincts = vw_KerjaInfoDetails.Select(s => new { s.fld_JnisAktvt, s.fld_KodAktvt, s.fld_JnsPkt, s.fld_KodPkt, s.fld_KodGL, s.fld_GLKod, s.fld_KodAktivitiSAP, s.fld_NNCC }).Distinct().ToList();
                var tbl_UpahAktiviti = db.tbl_UpahAktiviti.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID).ToList();

                foreach (var WorkDistinct in WorkDistincts)
                {
                    LogFunc.DataScTransChecking("fld_JnisAktvt = " + WorkDistinct.fld_JnisAktvt + " - fld_JnsPkt = " + WorkDistinct.fld_JnsPkt + " - fld_Keterangan =  - fld_KodAktvt = " + WorkDistinct.fld_KodAktvt + " - fld_KodGL = " + WorkDistinct.fld_KodGL + " - fld_KodPkt = " + WorkDistinct.fld_KodPkt + " - fld_NNCC = " + WorkDistinct.fld_NNCC + " - fld_SapActCode =  - fld_SAPGL =  - fld_SAPIO = ");
                    keterangan = tbl_UpahAktiviti.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktvt == WorkDistinct.fld_KodAktvt).Select(s => s.fld_Desc).FirstOrDefault();
                    WorkSCTransList.Add(new CustMod_WorkSCTrans() { fld_KodGL = WorkDistinct.fld_KodGL, fld_JnisAktvt = WorkDistinct.fld_JnisAktvt, fld_KodAktvt = WorkDistinct.fld_KodAktvt, fld_JnsPkt = WorkDistinct.fld_JnsPkt, fld_KodPkt = WorkDistinct.fld_KodPkt, fld_Keterangan = keterangan, fld_SAPGL = WorkDistinct.fld_GLKod, fld_NNCC = WorkDistinct.fld_NNCC, fld_SapActCode = WorkDistinct.fld_KodAktivitiSAP == "0" ? "-" : WorkDistinct.fld_KodAktivitiSAP, fld_ID = ID });
                    ID++;
                }
            }
            catch (Exception ex)
            {
                LogFunc.DataScTransChecking("Error = " + ex.Message + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.Data);
            }

            db.Dispose();
            db2.Dispose();

            return WorkSCTransList;
        }

        public void GetAmountWorkActivityFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, List<CustMod_WorkSCTrans> WorkSCTransList, out string Log, List<tbl_Pkjmast> PkjMastList)
        {
            LogFunc LogFunc = new LogFunc();
            GetConnectFunc conn = new GetConnectFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            Log = "";
            string message = "";
            int i = 1;
            var NoPkjList = PkjMastList.Select(s => s.fld_Nopkj).ToArray();
            string CodeActSAP = "";
            var vw_KerjaInfoDetails = db2.vw_KerjaDetailScTrans.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && NoPkjList.Contains(x.fld_Nopkj)).ToList();
            try
            {
                foreach (var WorkSCTrans in WorkSCTransList)
                {
                    //var Amount = db2.vw_KerjaInfoDetails.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_JnsPkt == WorkSCTrans.fld_JnsPkt && x.fld_KodPkt == WorkSCTrans.fld_KodPkt && x.fld_JnisAktvt == WorkSCTrans.fld_JnisAktvt && x.fld_KodAktvt == WorkSCTrans.fld_KodAktvt && x.fld_KodGL == WorkSCTrans.fld_KodGL && x.fld_DivisionID == DivisionID).Sum(s => s.fld_OverallAmount);
                    var Amount = vw_KerjaInfoDetails.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_JnsPkt == WorkSCTrans.fld_JnsPkt && x.fld_KodPkt == WorkSCTrans.fld_KodPkt && x.fld_JnisAktvt == WorkSCTrans.fld_JnisAktvt && x.fld_KodAktvt == WorkSCTrans.fld_KodAktvt && x.fld_GLKod == WorkSCTrans.fld_SAPGL && NoPkjList.Contains(x.fld_Nopkj) && x.fld_NNCC == WorkSCTrans.fld_NNCC).Sum(s => s.fld_OverallAmount);
                    if (Amount != 0)
                    {
                        if (WorkSCTrans.fld_SapActCode == null)
                        {
                            CodeActSAP = "-";
                        }
                        else
                        {
                            CodeActSAP = WorkSCTrans.fld_SapActCode.Trim();
                        }
                        LogFunc2.DataScTransChecking2("fld_JnisAktvt = " + WorkSCTrans.fld_JnisAktvt + " - fld_JnsPkt = " + WorkSCTrans.fld_JnsPkt + " - fld_Keterangan = " + WorkSCTrans.fld_Keterangan + " - fld_KodAktvt = " + WorkSCTrans.fld_KodAktvt + " - fld_KodGL = " + WorkSCTrans.fld_KodGL + " - fld_KodPkt = " + WorkSCTrans.fld_KodPkt + " - fld_NNCC = " + WorkSCTrans.fld_NNCC + " - fld_SapActCode = " + WorkSCTrans.fld_SapActCode + " - fld_SAPGL = " + WorkSCTrans.fld_SAPGL + " - fld_SAPIO = " + WorkSCTrans.fld_SAPIO, "GetAmountWorkActivityFunc");
                        AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Amount, WorkSCTrans.fld_JnsPkt, WorkSCTrans.fld_KodPkt.Trim(), WorkSCTrans.fld_JnisAktvt.Trim(), WorkSCTrans.fld_KodAktvt.Trim(), WorkSCTrans.fld_KodGL.Trim(), WorkSCTrans.fld_Keterangan.Trim(), DTProcess, UserID, Month, Year, "D", 1, WorkSCTrans.fld_SAPGL.Trim(), "-", WorkSCTrans.fld_NNCC.Trim(), CodeActSAP);
                        message = "Transaction Listing (Job). (Data - Code Pkt : " + WorkSCTrans.fld_KodPkt + ", Code Activity : " + WorkSCTrans.fld_KodAktvt + ", Amount : RM " + Amount + ")";
                        Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogFunc2.DataScTransChecking2("Error = " + ex.Message + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.Data, "Error - GetAmountWorkActivityFunc");
            }
            db2.Dispose();
        }

        public void GetAmountDailyBonusFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, List<CustMod_WorkSCTrans> WorkSCTransList, out string Log, List<tbl_Pkjmast> PkjMastList)
        {
            GetConnectFunc conn = new GetConnectFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            Log = "";
            string message = "";
            int i = 1;
            var NoPkjList = PkjMastList.Select(s => s.fld_Nopkj).ToArray();
            var vw_Kerja_Bonus = db2.vw_Kerja_Bonus.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year).ToList();
            foreach (var WorkSCTrans in WorkSCTransList)
            {
                //var Amount = db2.vw_Kerja_Bonus.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_JnsPkt == WorkSCTrans.fld_JnsPkt && x.fld_KodPkt == WorkSCTrans.fld_KodPkt && x.fld_JnisAktvt == WorkSCTrans.fld_JnisAktvt && x.fld_KodAktvt == WorkSCTrans.fld_KodAktvt && x.fld_KodGL == WorkSCTrans.fld_KodGL && x.fld_DivisionID == DivisionID).Sum(s => s.fld_Jumlah_B);
                var Amount = vw_Kerja_Bonus.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_JnsPkt == WorkSCTrans.fld_JnsPkt && x.fld_KodPkt == WorkSCTrans.fld_KodPkt && x.fld_JnisAktvt == WorkSCTrans.fld_JnisAktvt && x.fld_KodAktvt == WorkSCTrans.fld_KodAktvt && x.fld_KodGL == WorkSCTrans.fld_KodGL && x.fld_NNCC == WorkSCTrans.fld_NNCC && x.fld_KodAktivitiSAP == WorkSCTrans.fld_SapActCode && x.fld_GLKod == WorkSCTrans.fld_SAPGL && NoPkjList.Contains(x.fld_Nopkj)).Sum(s => s.fld_Jumlah_B);
                Amount = Amount == null ? 0 : Amount;
                if (Amount != 0)
                {
                    //original code
                    //AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Amount, WorkSCTrans.fld_JnsPkt, WorkSCTrans.fld_KodPkt, WorkSCTrans.fld_JnisAktvt, WorkSCTrans.fld_KodAktvt, WorkSCTrans.fld_KodGL, WorkSCTrans.fld_Keterangan + " (Bonus)", DTProcess, UserID, Month, Year, "D", 2, WorkSCTrans.fld_SAPGL, WorkSCTrans.fld_SAPIO, WorkSCTrans.fld_NNCC, WorkSCTrans.fld_SapActCode);
                    //added by faeza 23.03.2022
                    AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Amount, WorkSCTrans.fld_JnsPkt, WorkSCTrans.fld_KodPkt, WorkSCTrans.fld_JnisAktvt, WorkSCTrans.fld_KodAktvt, WorkSCTrans.fld_KodGL, WorkSCTrans.fld_Keterangan + " (Daily Wage)", DTProcess, UserID, Month, Year, "D", 2, WorkSCTrans.fld_SAPGL, WorkSCTrans.fld_SAPIO, WorkSCTrans.fld_NNCC, WorkSCTrans.fld_SapActCode);
                    message = "Transaction Listing (Daily Bonus). (Data - Code Pkt : " + WorkSCTrans.fld_KodPkt + ", Code Activity : " + WorkSCTrans.fld_KodAktvt + ", Amount : RM " + Amount + ")";
                    Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                    i++;
                }
            }
            db2.Dispose();
        }

        public void GetAmountDailyBonusV2Func(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, List<CustMod_WorkSCTrans> WorkSCTransList, out string Log, List<tbl_Pkjmast> PkjMastList)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            Log = "";
            string message = "";
            int i = 1;
            var NoPkjList = PkjMastList.Select(s => s.fld_Nopkj).ToArray();
            //var GetTotaWorkSCTransListCount = WorkSCTransList.Count();
            //foreach (var WorkSCTrans in WorkSCTransList)
            //{
            //var Amount = db2.vw_Kerja_Bonus.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_JnsPkt == WorkSCTrans.fld_JnsPkt && x.fld_KodPkt == WorkSCTrans.fld_KodPkt && x.fld_JnisAktvt == WorkSCTrans.fld_JnisAktvt && x.fld_KodAktvt == WorkSCTrans.fld_KodAktvt && x.fld_KodGL == WorkSCTrans.fld_KodGL && x.fld_DivisionID == DivisionID).Sum(s => s.fld_Jumlah_B);
            var Amount = db2.vw_Kerja_Bonus.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && NoPkjList.Contains(x.fld_Nopkj)).Sum(s => s.fld_Jumlah_B);
            Amount = Amount == null ? 0 : Amount;
            if (Amount != 0)
            {
                var GetCodeActDI = db.tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "CodeActDI" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).FirstOrDefault();
                var GetAddInsGL = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktiviti == GetCodeActDI.fldOptConfValue && x.fld_Flag == "2" && x.fld_TypeCode == "GL" && x.fld_Deleted == false).Select(s => s.fld_SAPCode).FirstOrDefault();
                AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Amount, 0, "-", GetCodeActDI.fldOptConfValue.Substring(0, 2), GetCodeActDI.fldOptConfValue, "602", GetCodeActDI.fldOptConfDesc, DTProcess, UserID, Month, Year, "D", 5, GetAddInsGL, "-", "-", "-");
                message = "Transaction Listing (Daily Bonus). (Data - Code Activity : " + GetCodeActDI.fldOptConfValue + ", Amount : RM " + Amount + ")";
                Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                i++;
            }
            //}
            db.Dispose();
            db2.Dispose();
        }

        public void GetAmountOTFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, List<CustMod_WorkSCTrans> WorkSCTransList, out string Log, List<tbl_Pkjmast> PkjMastList)
        {
            GetConnectFunc conn = new GetConnectFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            Log = "";
            string message = "";
            int i = 1;
            var NoPkjList = PkjMastList.Select(s => s.fld_Nopkj).ToArray();
            var vw_Kerja_OT = db2.vw_Kerja_OT.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year).ToList();
            foreach (var WorkSCTrans in WorkSCTransList)
            {
                //var Amount = db2.vw_Kerja_OT.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_JnsPkt == WorkSCTrans.fld_JnsPkt && x.fld_KodPkt == WorkSCTrans.fld_KodPkt && x.fld_JnisAktvt == WorkSCTrans.fld_JnisAktvt && x.fld_KodAktvt == WorkSCTrans.fld_KodAktvt && x.fld_KodGL == WorkSCTrans.fld_KodGL && x.fld_DivisionID == DivisionID).Sum(s => s.fld_Jumlah);
                var Amount = vw_Kerja_OT.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_JnsPkt == WorkSCTrans.fld_JnsPkt && x.fld_KodPkt == WorkSCTrans.fld_KodPkt && x.fld_JnisAktvt == WorkSCTrans.fld_JnisAktvt && x.fld_KodAktvt == WorkSCTrans.fld_KodAktvt && x.fld_KodGL == WorkSCTrans.fld_KodGL && x.fld_NNCC == WorkSCTrans.fld_NNCC && x.fld_KodAktivitiSAP == WorkSCTrans.fld_SapActCode && x.fld_GLKod == WorkSCTrans.fld_SAPGL && NoPkjList.Contains(x.fld_Nopkj)).Sum(s => s.fld_Jumlah);
                Amount = Amount == null ? 0 : Amount;
                if (Amount != 0)
                {
                    AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Amount, WorkSCTrans.fld_JnsPkt, WorkSCTrans.fld_KodPkt, WorkSCTrans.fld_JnisAktvt, WorkSCTrans.fld_KodAktvt, WorkSCTrans.fld_KodGL, WorkSCTrans.fld_Keterangan + " (OT)", DTProcess, UserID, Month, Year, "D", 3, WorkSCTrans.fld_SAPGL, "-", WorkSCTrans.fld_NNCC, WorkSCTrans.fld_SapActCode);
                    message = "Transaction Listing (OT). (Data - Code Pkt : " + WorkSCTrans.fld_KodPkt + ", Code Activity : " + WorkSCTrans.fld_KodAktvt + ", Amount : RM " + Amount + ")";
                    Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                    i++;
                }
            }
            db2.Dispose();
        }

        public void GetAmountOTV2Func(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, List<CustMod_WorkSCTrans> WorkSCTransList, out string Log, List<tbl_Pkjmast> PkjMastList)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            Log = "";
            string message = "";
            int i = 1;
            var NoPkjList = PkjMastList.Select(s => s.fld_Nopkj).ToArray();
            //foreach (var WorkSCTrans in WorkSCTransList)
            //{
            //var Amount = db2.vw_Kerja_OT.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_JnsPkt == WorkSCTrans.fld_JnsPkt && x.fld_KodPkt == WorkSCTrans.fld_KodPkt && x.fld_JnisAktvt == WorkSCTrans.fld_JnisAktvt && x.fld_KodAktvt == WorkSCTrans.fld_KodAktvt && x.fld_KodGL == WorkSCTrans.fld_KodGL && x.fld_DivisionID == DivisionID).Sum(s => s.fld_Jumlah);
            var Amount = db2.vw_Kerja_OT.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && NoPkjList.Contains(x.fld_Nopkj)).Sum(s => s.fld_Jumlah);
            Amount = Amount == null ? 0 : Amount;
            if (Amount != 0)
            {
                var GetCodeActOT = db.tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "CodeActOT" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).FirstOrDefault();
                var GetOTGL = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktiviti == GetCodeActOT.fldOptConfValue && x.fld_Flag == "2" && x.fld_TypeCode == "GL" && x.fld_Deleted == false).Select(s => s.fld_SAPCode).FirstOrDefault();
                AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Amount, 0, "-", GetCodeActOT.fldOptConfValue.Substring(0, 2), GetCodeActOT.fldOptConfValue, "602", GetCodeActOT.fldOptConfDesc, DTProcess, UserID, Month, Year, "D", 3, GetOTGL, "-", "-", "-");
                message = "Transaction Listing (OT). (Data - Code Activity : " + GetCodeActOT.fldOptConfValue + ", Amount : RM " + Amount + ")";
                Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                i++;
            }
            //}
            db.Dispose();
            db2.Dispose();
        }

        public void GetAmountLeaveFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, out string Log, List<tbl_Pkjmast> PkjMastList)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            Log = "";
            string message = "";
            int i = 1;
            decimal? Amount = 0;
            decimal? AmountW = 0; //added by faeza 26.09.2022

            var GetLeave = db.tbl_CutiKategori.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false).ToList();
            //added by faeza 26.09.2022
            var GetWorkedLeave = db.tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "mappingCutiKerja" && x.fldDeleted == false && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID).ToList();

            // TEMPATAN
            var NoPkjListTKTDetails = PkjMastList.Where(x => x.fld_Kdrkyt == "MA").ToArray();
            var vw_Kerja_Hdr_Cuti = db2.vw_Kerja_Hdr_Cuti.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year).ToList();
            var tbl_CustomerVendorGLMap = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false).ToList();
            if (NoPkjListTKTDetails.Count() > 0)
            {
                var GetCostCenterList = NoPkjListTKTDetails.Select(s => s.fld_KodSAPPekerja).Distinct().ToList();
                foreach (var GetCostCenter in GetCostCenterList)
                {
                    var NoPkjListTKT = NoPkjListTKTDetails.Where(x => x.fld_Kdrkyt == "MA" && x.fld_KodSAPPekerja == GetCostCenter).Select(s => s.fld_Nopkj).ToArray();
                    foreach (var Leave in GetLeave)
                    {
                        if (Leave.fld_KodCuti != "C99")
                        {
                            Amount = Leave.fld_WaktuBayaranCuti == 1 ? vw_Kerja_Hdr_Cuti.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_Kdhdct == Leave.fld_KodCuti && NoPkjListTKT.Contains(x.fld_Nopkj)).Sum(s => s.fld_Jumlah)
                        :
                        vw_Kerja_Hdr_Cuti.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Year == Year && x.fld_Kdhdct == Leave.fld_KodCuti && NoPkjListTKT.Contains(x.fld_Nopkj)).Sum(s => s.fld_Jumlah);
                        }
                        else
                        {
                            Amount = db2.tbl_KerjahdrCutiTahunan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Year == Year && x.fld_KodCuti == Leave.fld_KodCuti && NoPkjListTKT.Contains(x.fld_Nopkj)).Sum(s => s.fld_JumlahAmt);
                        }

                        Amount = Amount == null ? 0 : Amount;

                        if (Amount != 0)
                        {
                            var GetLeaveGL = tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktiviti == Leave.fld_KodAktvt && x.fld_Flag == "2" && x.fld_TypeCode == "GLTKT" && x.fld_Deleted == false).Select(s => s.fld_SAPCode).FirstOrDefault();
                            AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Amount, 0, "-", Leave.fld_KodAktvt.Substring(0, 2), Leave.fld_KodAktvt, Leave.fld_KodGL, FirstCharToUpper(Leave.fld_KeteranganCuti.ToLower()) + " - TKT", DTProcess, UserID, Month, Year, "D", 4, GetLeaveGL, "-", GetCostCenter, "-");
                            message = "Transaction Listing (Leave - TKT). (Data - Code Leave : " + Leave.fld_KodCuti + ", Code Activity : " + Leave.fld_KodAktvt + ", Amount : RM " + Amount + ")";
                            Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                            i++;
                        }
                    }

                    //added by faeza 26.09.2022 - H03
                    foreach (var WorkedLeave in GetWorkedLeave)
                    {
                        AmountW = vw_Kerja_Hdr_Cuti.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_Kdhdct == WorkedLeave.fldOptConfValue && NoPkjListTKT.Contains(x.fld_Nopkj)).Sum(s => s.fld_Jumlah);

                        AmountW = AmountW == null ? 0 : AmountW;

                        if (AmountW != 0)
                        {
                            var GetLeaveGL = tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktiviti == WorkedLeave.fldOptConfFlag2 && x.fld_Flag == "2" && x.fld_TypeCode == "GLTKT" && x.fld_Deleted == false).Select(s => s.fld_SAPCode).FirstOrDefault();
                            AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, AmountW, 0, "-", WorkedLeave.fldOptConfFlag2.Substring(0, 2), WorkedLeave.fldOptConfFlag2, WorkedLeave.fldOptConfFlag3, FirstCharToUpper(WorkedLeave.fldOptConfDesc.ToLower()) + " - TKT", DTProcess, UserID, Month, Year, "D", 4, GetLeaveGL, "-", GetCostCenter, "-");
                            message = "Transaction Listing (Worked Leave - TKT). (Data - Code Leave : " + WorkedLeave.fldOptConfValue + ", Code Activity : " + WorkedLeave.fldOptConfFlag2 + ", Amount : RM " + AmountW + ")";
                            Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                            i++;
                        }
                    }
                }

            }

            // ASING
            var NoPkjListTKADetails = PkjMastList.Where(x => x.fld_Kdrkyt != "MA").ToArray();
            Amount = 0;
            if (NoPkjListTKADetails.Count() > 0)
            {
                var GetCostCenterList = NoPkjListTKADetails.Select(s => s.fld_KodSAPPekerja).Distinct().ToList();
                foreach (var GetCostCenter in GetCostCenterList)
                {
                    var NoPkjListTKA = NoPkjListTKADetails.Where(x => x.fld_Kdrkyt != "MA" && x.fld_KodSAPPekerja == GetCostCenter).Select(s => s.fld_Nopkj).ToArray();
                    foreach (var Leave in GetLeave)
                    {
                        if (Leave.fld_KodCuti != "C99")
                        {
                            Amount = Leave.fld_WaktuBayaranCuti == 1 ? vw_Kerja_Hdr_Cuti.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_Kdhdct == Leave.fld_KodCuti && NoPkjListTKA.Contains(x.fld_Nopkj)).Sum(s => s.fld_Jumlah)
                        :
                        vw_Kerja_Hdr_Cuti.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Year == Year && x.fld_Kdhdct == Leave.fld_KodCuti && NoPkjListTKA.Contains(x.fld_Nopkj)).Sum(s => s.fld_Jumlah);
                        }
                        else
                        {
                            Amount = db2.tbl_KerjahdrCutiTahunan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Year == Year && x.fld_KodCuti == Leave.fld_KodCuti && NoPkjListTKA.Contains(x.fld_Nopkj)).Sum(s => s.fld_JumlahAmt);
                        }

                        Amount = Amount == null ? 0 : Amount;

                        if (Amount != 0)
                        {
                            var GetLeaveGL = tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktiviti == Leave.fld_KodAktvt && x.fld_Flag == "2" && x.fld_TypeCode == "GLTKA" && x.fld_Deleted == false).Select(s => s.fld_SAPCode).FirstOrDefault();
                            AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Amount, 0, "-", Leave.fld_KodAktvt.Substring(0, 2), Leave.fld_KodAktvt, Leave.fld_KodGL, FirstCharToUpper(Leave.fld_KeteranganCuti.ToLower()) + " - TKA", DTProcess, UserID, Month, Year, "D", 4, GetLeaveGL, "-", GetCostCenter, "-");
                            message = "Transaction Listing (Leave - TKA). (Data - Code Leave : " + Leave.fld_KodCuti + ", Code Activity : " + Leave.fld_KodAktvt + ", Amount : RM " + Amount + ")";
                            Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                            i++;
                        }
                    }

                    //added by faeza 26.09.2022 - H03
                    foreach (var WorkedLeave in GetWorkedLeave)
                    {
                        AmountW = vw_Kerja_Hdr_Cuti.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_Kdhdct == WorkedLeave.fldOptConfValue && NoPkjListTKA.Contains(x.fld_Nopkj)).Sum(s => s.fld_Jumlah);

                        AmountW = AmountW == null ? 0 : AmountW;

                        if (AmountW != 0)
                        {
                            var GetLeaveGL = tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktiviti == WorkedLeave.fldOptConfFlag2 && x.fld_Flag == "2" && x.fld_TypeCode == "GLTKA" && x.fld_Deleted == false).Select(s => s.fld_SAPCode).FirstOrDefault();
                            AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, AmountW, 0, "-", WorkedLeave.fldOptConfFlag2.Substring(0, 2), WorkedLeave.fldOptConfFlag2, WorkedLeave.fldOptConfFlag3, FirstCharToUpper(WorkedLeave.fldOptConfDesc.ToLower()) + " - TKA", DTProcess, UserID, Month, Year, "D", 4, GetLeaveGL, "-", GetCostCenter, "-");
                            message = "Transaction Listing (Worked Leave - TKA). (Data - Code Leave : " + WorkedLeave.fldOptConfValue + ", Code Activity : " + WorkedLeave.fldOptConfFlag2 + ", Amount : RM " + AmountW + ")";
                            Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                            i++;
                        }
                    }
                }
            }
            db.Dispose();
            db2.Dispose();
        }

        public void GetAmountAddedInsentifFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, out string Log, List<tbl_Pkjmast> PkjMastList)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            Log = "";
            string message = "";
            int i = 1;
            decimal? Amount = 0;

            try
            {
                //modified by faeza 26.02.2023
                var GetInsentif = db.tbl_JenisInsentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false && x.fld_JenisInsentif == "P" && x.fld_InclSecondPayslip == false).ToList();
                // TEMPATAN
                var NoPkjListTKTDetails = PkjMastList.Where(x => x.fld_Kdrkyt == "MA").ToArray();
                var tbl_Insentif = db2.tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year).ToList();
                var tbl_CustomerVendorGLMap = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false).ToList();
                if (NoPkjListTKTDetails.Count() > 0)
                {
                    var GetCostCenterList = NoPkjListTKTDetails.Select(s => s.fld_KodSAPPekerja).Distinct().ToList();
                    foreach (var GetCostCenter in GetCostCenterList)
                    {
                        var NoPkjListTKT = NoPkjListTKTDetails.Where(x => x.fld_Kdrkyt == "MA" && x.fld_KodSAPPekerja == GetCostCenter).Select(s => s.fld_Nopkj).ToArray();
                        foreach (var Insentif in GetInsentif)
                        {
                            Amount = tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && x.fld_KodInsentif == Insentif.fld_KodInsentif && x.fld_Deleted == false && NoPkjListTKT.Contains(x.fld_Nopkj)).Sum(s => s.fld_NilaiInsentif);

                            Amount = Amount == null ? 0 : Amount;

                            if (Amount != 0)
                            {
                                var GetAddInsGL = tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktiviti == Insentif.fld_KodAktvt && x.fld_Flag == "2" && x.fld_TypeCode == "GLTKT" && x.fld_Deleted == false).Select(s => s.fld_SAPCode).FirstOrDefault();
                                LogFunc2.DataScTransChecking2("Insentif = " + Insentif.fld_KodAktvt + " GetAddInsGL = " + Amount, "Insentif");
                                AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Amount, 0, "-", Insentif.fld_KodAktvt.Substring(0, 2), Insentif.fld_KodAktvt, Insentif.fld_KodGL, FirstCharToUpper(Insentif.fld_Keterangan.ToLower()) + " - TKT", DTProcess, UserID, Month, Year, "D", 5, GetAddInsGL, "-", GetCostCenter, "-");
                                message = "Transaction Listing (Insentif Payment - TKT). (Data - Code Insentif : " + Insentif.fld_KodInsentif + ", Code Activity : " + Insentif.fld_KodAktvt + ", Amount : RM " + Amount + ")";
                                Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                                i++;
                            }
                        }
                    }
                }

                // ASING
                var NoPkjListTKADetails = PkjMastList.Where(x => x.fld_Kdrkyt != "MA").ToArray();

                Amount = 0;
                if (NoPkjListTKADetails.Count() > 0)
                {
                    var GetCostCenterList = NoPkjListTKADetails.Select(s => s.fld_KodSAPPekerja).Distinct().ToList();
                    foreach (var GetCostCenter in GetCostCenterList)
                    {
                        var NoPkjListTKA = NoPkjListTKADetails.Where(x => x.fld_Kdrkyt != "MA" && x.fld_KodSAPPekerja == GetCostCenter).Select(s => s.fld_Nopkj).ToArray();
                        foreach (var Insentif in GetInsentif)
                        {
                            Amount = tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && x.fld_KodInsentif == Insentif.fld_KodInsentif && x.fld_Deleted == false && NoPkjListTKA.Contains(x.fld_Nopkj)).Sum(s => s.fld_NilaiInsentif);

                            Amount = Amount == null ? 0 : Amount;

                            if (Amount != 0)
                            {
                                var GetAddInsGL = tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktiviti == Insentif.fld_KodAktvt && x.fld_Flag == "2" && x.fld_TypeCode == "GLTKA" && x.fld_Deleted == false).Select(s => s.fld_SAPCode).FirstOrDefault();
                                AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Amount, 0, "-", Insentif.fld_KodAktvt.Substring(0, 2), Insentif.fld_KodAktvt, Insentif.fld_KodGL, FirstCharToUpper(Insentif.fld_Keterangan.ToLower()) + " - TKA", DTProcess, UserID, Month, Year, "D", 5, GetAddInsGL, "-", GetCostCenter, "-");
                                message = "Transaction Listing (Insentif Payment - TKA). (Data - Code Insentif : " + Insentif.fld_KodInsentif + ", Code Activity : " + Insentif.fld_KodAktvt + ", Amount : RM " + Amount + ")";
                                Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                                i++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogFunc2.DataScTransChecking2("Error = " + ex.Message + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.Data, "Error");
            }
            db.Dispose();
            db2.Dispose();
        }

        public void GetAmountDeductedInsentifFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, out string Log, List<tbl_Pkjmast> PkjMastList)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            Log = "";
            string message = "";
            int i = 1;
            decimal? Amount = 0;

            var GetInsentif = db.tbl_JenisInsentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false && x.fld_JenisInsentif == "T").ToList();
            // TEMPATAN
            var NoPkjListTKTDetails = PkjMastList.Where(x => x.fld_Kdrkyt == "MA").ToArray();
            var tbl_Insentif = db2.tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year).ToList();
            var tbl_CustomerVendorGLMap = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false).ToList();
            if (NoPkjListTKTDetails.Count() > 0)
            {
                var GetCostCenterList = NoPkjListTKTDetails.Select(s => s.fld_KodSAPPekerja).Distinct().ToList();
                foreach (var GetCostCenter in GetCostCenterList)
                {
                    var NoPkjListTKT = NoPkjListTKTDetails.Where(x => x.fld_Kdrkyt == "MA" && x.fld_KodSAPPekerja == GetCostCenter).Select(s => s.fld_Nopkj).ToArray();
                    foreach (var Insentif in GetInsentif)
                    {
                        Amount = tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && x.fld_KodInsentif == Insentif.fld_KodInsentif && x.fld_Deleted == false && NoPkjListTKT.Contains(x.fld_Nopkj)).Sum(s => s.fld_NilaiInsentif);

                        Amount = Amount == null ? 0 : Amount;

                        if (Amount != 0)
                        {
                            var GetDedcInsGL = tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktiviti == Insentif.fld_KodAktvt && x.fld_Flag == "5" && x.fld_TypeCode == "GLTKT" && x.fld_Deleted == false).Select(s => s.fld_SAPCode).FirstOrDefault();
                            AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Amount, 0, "-", Insentif.fld_KodAktvt.Substring(0, 2), Insentif.fld_KodAktvt, Insentif.fld_KodGL, FirstCharToUpper(Insentif.fld_Keterangan.ToLower()) + " - TKT", DTProcess, UserID, Month, Year, "C", 6, GetDedcInsGL, "-", GetCostCenter, "-");
                            message = "Transaction Listing (Insentif Deduction - TKT). (Data - Code Insentif : " + Insentif.fld_KodInsentif + ", Code Activity : " + Insentif.fld_KodAktvt + ", Amount : RM " + Amount + ")";
                            Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                            i++;
                        }
                    }
                }
            }

            // ASING
            var NoPkjListTKADetails = PkjMastList.Where(x => x.fld_Kdrkyt != "MA").ToArray();
            Amount = 0;
            if (NoPkjListTKADetails.Count() > 0)
            {
                var GetCostCenterList = NoPkjListTKADetails.Select(s => s.fld_KodSAPPekerja).Distinct().ToList();
                foreach (var GetCostCenter in GetCostCenterList)
                {
                    var NoPkjListTKA = NoPkjListTKADetails.Where(x => x.fld_Kdrkyt != "MA" && x.fld_KodSAPPekerja == GetCostCenter).Select(s => s.fld_Nopkj).ToArray();
                    foreach (var Insentif in GetInsentif)
                    {
                        Amount = tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && x.fld_KodInsentif == Insentif.fld_KodInsentif && x.fld_Deleted == false && NoPkjListTKA.Contains(x.fld_Nopkj)).Sum(s => s.fld_NilaiInsentif);

                        Amount = Amount == null ? 0 : Amount;

                        if (Amount != 0)
                        {
                            var GetDedcInsGL = tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktiviti == Insentif.fld_KodAktvt && x.fld_Flag == "5" && x.fld_TypeCode == "GLTKA" && x.fld_Deleted == false).Select(s => s.fld_SAPCode).FirstOrDefault();
                            AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Amount, 0, "-", Insentif.fld_KodAktvt.Substring(0, 2), Insentif.fld_KodAktvt, Insentif.fld_KodGL, FirstCharToUpper(Insentif.fld_Keterangan.ToLower()) + " - TKA", DTProcess, UserID, Month, Year, "C", 6, GetDedcInsGL, "-", GetCostCenter, "-");
                            message = "Transaction Listing (Insentif Deduction - TKA). (Data - Code Insentif : " + Insentif.fld_KodInsentif + ", Code Activity : " + Insentif.fld_KodAktvt + ", Amount : RM " + Amount + ")";
                            Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                            i++;
                        }
                    }
                }
            }
            db.Dispose();
            db2.Dispose();
        }

        public void GetAmountKWSPFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, out string Log, List<tbl_Pkjmast> PkjMastList)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            Log = "";
            string message = "";
            int i = 1;
            decimal? Amount = 0;
            decimal? AmountMix = 0;

            var GetKWSP = db.tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "kwsp" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).ToList();
            var NoPkjListTKTDetails = PkjMastList.ToArray();
            var tbl_GajiBulanan = db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year).ToList();
            var tbl_CustomerVendorGLMap = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false).ToList();
            var KWSPMix = db.tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "kwspmix" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).FirstOrDefault();
            var GetCostCenterList = NoPkjListTKTDetails.Select(s => s.fld_KodSAPPekerja).Distinct().ToList();
            foreach (var GetCostCenter in GetCostCenterList)
            {
                AmountMix = 0;
                var NoPkjList = NoPkjListTKTDetails.Where(x => x.fld_KodSAPPekerja == GetCostCenter).Select(s => s.fld_Nopkj).ToArray();
                foreach (var KWSP in GetKWSP)
                {
                    Amount = KWSP.fldOptConfFlag3 == "Employee" ?
                    tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && NoPkjList.Contains(x.fld_Nopkj)).Sum(s => s.fld_KWSPPkj)
                    :
                    tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && NoPkjList.Contains(x.fld_Nopkj)).Sum(s => s.fld_KWSPMjk);

                    Amount = Amount == null ? 0 : Amount;

                    if (Amount != 0)
                    {
                        var GetKWSPGL = tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktiviti == KWSP.fldOptConfValue && x.fld_Flag == "1" && x.fld_TypeCode == "GL" && x.fld_Deleted == false).Select(s => s.fld_SAPCode).FirstOrDefault();
                        GetKWSPGL = GetKWSPGL != null ? GetKWSPGL : "-";
                        AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Amount, 0, "-", KWSP.fldOptConfValue.Substring(0, 2), KWSP.fldOptConfValue, KWSP.fldOptConfFlag2, KWSP.fldOptConfDesc, DTProcess, UserID, Month, Year, "D", 7, GetKWSPGL, "-", GetCostCenter, "-");
                        message = "Transaction Listing (KWSP " + KWSP.fldOptConfFlag3 + "). (Data - Code Activity : " + KWSP.fldOptConfValue + ", Amount : RM " + Amount + ")";
                        Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                        i++;
                    }
                    AmountMix = AmountMix + Amount;
                }

                //var KWSPMix = db.tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "kwspmix" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).FirstOrDefault();

                if (AmountMix != 0)
                {
                    var GetKWSPGL = tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktiviti == KWSPMix.fldOptConfValue && x.fld_Flag == "3" && x.fld_TypeCode == "GL" && x.fld_Deleted == false).Select(s => s.fld_SAPCode).FirstOrDefault();
                    AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, AmountMix, 0, "-", KWSPMix.fldOptConfValue.Substring(0, 2), KWSPMix.fldOptConfValue, KWSPMix.fldOptConfFlag2, KWSPMix.fldOptConfDesc, DTProcess, UserID, Month, Year, "C", 7, GetKWSPGL, "-", "-", "-");
                    message = "Transaction Listing (KWSP " + KWSPMix.fldOptConfFlag3 + "). (Data - Code Activity : " + KWSPMix.fldOptConfValue + ", Amount : RM " + AmountMix + ")";
                    Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                    i++;
                }
            }


            db.Dispose();
            db2.Dispose();
        }

        public void GetAmountSocsoFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, out string Log, List<tbl_Pkjmast> PkjMastList)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            Log = "";
            string message = "";
            int i = 1;
            decimal? Amount = 0;
            decimal? AmountMix = 0;

            var GetSocso = db.tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "socso" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).ToList();
            var NoPkjListTKTDetails = PkjMastList.ToArray();
            var tbl_GajiBulanan = db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year).ToList();
            var SocsoMix = db.tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "socsomix" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).FirstOrDefault();
            var GetCostCenterList = NoPkjListTKTDetails.Select(s => s.fld_KodSAPPekerja).Distinct().ToList();
            var tbl_CustomerVendorGLMap = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false).ToList();
            foreach (var GetCostCenter in GetCostCenterList)
            {
                AmountMix = 0;
                var NoPkjList = NoPkjListTKTDetails.Where(x => x.fld_KodSAPPekerja == GetCostCenter).Select(s => s.fld_Nopkj).ToArray();
                foreach (var Socso in GetSocso)
                {
                    Amount = Socso.fldOptConfFlag3 == "Employee" ?
                    tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && NoPkjList.Contains(x.fld_Nopkj)).Sum(s => s.fld_SocsoPkj)
                    :
                    tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && NoPkjList.Contains(x.fld_Nopkj)).Sum(s => s.fld_SocsoMjk);

                    Amount = Amount == null ? 0 : Amount;

                    if (Amount != 0)
                    {
                        var GetSocsoGL = tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktiviti == Socso.fldOptConfValue && x.fld_Flag == "1" && x.fld_TypeCode == "GL" && x.fld_Deleted == false).Select(s => s.fld_SAPCode).FirstOrDefault();
                        GetSocsoGL = GetSocsoGL != null ? GetSocsoGL : "-";
                        AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Amount, 0, "-", Socso.fldOptConfValue.Substring(0, 2), Socso.fldOptConfValue, Socso.fldOptConfFlag2, Socso.fldOptConfDesc, DTProcess, UserID, Month, Year, "D", 8, GetSocsoGL, "-", GetCostCenter, "-");
                        message = "Transaction Listing (Socso " + Socso.fldOptConfFlag3 + "). (Data - Code Activity : " + Socso.fldOptConfValue + ", Amount : RM " + Amount + ")";
                        Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                        i++;
                    }
                    AmountMix = AmountMix + Amount;
                }

                //var SocsoMix = db.tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "socsomix" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).FirstOrDefault();

                if (AmountMix != 0)
                {
                    var GetSocsoGL = tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktiviti == SocsoMix.fldOptConfValue && x.fld_Flag == "3" && x.fld_TypeCode == "GL" && x.fld_Deleted == false).Select(s => s.fld_SAPCode).FirstOrDefault();
                    AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, AmountMix, 0, "-", SocsoMix.fldOptConfValue.Substring(0, 2), SocsoMix.fldOptConfValue, SocsoMix.fldOptConfFlag2, SocsoMix.fldOptConfDesc, DTProcess, UserID, Month, Year, "C", 8, GetSocsoGL, "-", "-", "-");
                    message = "Transaction Listing (Socso " + SocsoMix.fldOptConfFlag3 + "). (Data - Code Activity : " + SocsoMix.fldOptConfValue + ", Amount : RM " + AmountMix + ")";
                    Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                    i++;
                }
            }

            db.Dispose();
            db2.Dispose();
        }

        public void GetAmountOtherContributionsFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, out string Log, List<tbl_Pkjmast> PkjMastList)
        {
            Log = "";
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            var GetOtherContributions = db.tbl_CarumanTambahan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false).ToList();
            string KodCaruman = "";
            decimal? AmountP = 0;
            decimal? AmountM = 0;
            decimal? AmountMix = 0;
            string message = "";

            var NoPkjListTKDetails = PkjMastList.ToArray();

            var GetCostCenterList = NoPkjListTKDetails.Select(s => s.fld_KodSAPPekerja).Distinct().ToList();
            foreach (var GetCostCenter in GetCostCenterList)
            {
                var NoPkjList = NoPkjListTKDetails.Where(x => x.fld_KodSAPPekerja == GetCostCenter).Select(s => s.fld_Nopkj).ToArray();
                var GajiBulananID = db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && NoPkjList.Contains(x.fld_Nopkj)).Select(s => s.fld_ID).ToList();

                foreach (var GetOtherContribution in GetOtherContributions)
                {
                    AmountP = 0;
                    AmountM = 0;
                    AmountMix = 0;
                    KodCaruman = GetOtherContribution.fld_KodCaruman;

                    var GetOtherContribute = db.tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == KodCaruman.ToLower() && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).ToList();
                    switch (GetOtherContribution.fld_CarumanOleh)
                    {
                        case 1:
                            AmountP = db2.tbl_ByrCarumanTambahan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && GajiBulananID.Contains(x.fld_GajiID.Value) && x.fld_KodCaruman == KodCaruman).Sum(s => s.fld_CarumanPekerja);
                            AmountM = 0;
                            AmountMix = AmountP + AmountM;
                            break;
                        case 2:
                            AmountP = 0;
                            AmountM = db2.tbl_ByrCarumanTambahan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && GajiBulananID.Contains(x.fld_GajiID.Value) && x.fld_KodCaruman == KodCaruman).Sum(s => s.fld_CarumanMajikan);
                            AmountMix = AmountP + AmountM;
                            break;
                        case 3:
                            AmountP = db2.tbl_ByrCarumanTambahan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && GajiBulananID.Contains(x.fld_GajiID.Value) && x.fld_KodCaruman == KodCaruman).Sum(s => s.fld_CarumanPekerja);
                            AmountM = db2.tbl_ByrCarumanTambahan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && GajiBulananID.Contains(x.fld_GajiID.Value) && x.fld_KodCaruman == KodCaruman).Sum(s => s.fld_CarumanMajikan);
                            AmountMix = AmountP + AmountM;
                            break;
                    }
                    AmountP = AmountP == null ? 0 : AmountP;
                    AmountM = AmountM == null ? 0 : AmountM;
                    AmountMix = AmountMix == null ? 0 : AmountMix;
                    if (AmountP != 0)
                    {
                        var PkjAktvtDetail = GetOtherContribute.Where(x => x.fldOptConfFlag3 == "Employee").FirstOrDefault();
                        AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, AmountP, 0, "-", PkjAktvtDetail.fldOptConfValue.Substring(0, 2), PkjAktvtDetail.fldOptConfValue, PkjAktvtDetail.fldOptConfFlag2, PkjAktvtDetail.fldOptConfDesc, DTProcess, UserID, Month, Year, "D", 10, "-", "-", GetCostCenter, "-");
                        message = "Transaction Listing (" + GetOtherContribution.fld_NamaCaruman + " " + PkjAktvtDetail.fldOptConfFlag3 + "). (Data - Code Activity : " + PkjAktvtDetail.fldOptConfValue + ", Amount : RM " + AmountP + ")";
                        Log += DateTimeFunc.GetDateTime() + " - " + message;
                    }

                    if (AmountM != 0)
                    {
                        var MjkAktvtDetail = GetOtherContribute.Where(x => x.fldOptConfFlag3 == "Employer").FirstOrDefault();
                        var GetOtherContriMjkGL = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktiviti == MjkAktvtDetail.fldOptConfValue && x.fld_Flag == "1" && x.fld_TypeCode == "GL" && x.fld_Deleted == false).Select(s => s.fld_SAPCode).FirstOrDefault();
                        AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, AmountM, 0, "-", MjkAktvtDetail.fldOptConfValue.Substring(0, 2), MjkAktvtDetail.fldOptConfValue, MjkAktvtDetail.fldOptConfFlag2, MjkAktvtDetail.fldOptConfDesc, DTProcess, UserID, Month, Year, "D", 10, GetOtherContriMjkGL, "-", GetCostCenter, "-");
                        message = "Transaction Listing (" + GetOtherContribution.fld_NamaCaruman + " " + MjkAktvtDetail.fldOptConfFlag3 + "). (Data - Code Activity : " + MjkAktvtDetail.fldOptConfValue + ", Amount : RM " + AmountM + ")";
                        Log += "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                    }

                    if (AmountMix != 0)
                    {
                        var PjkMjkAktvtDetail = GetOtherContribute.Where(x => x.fldOptConfFlag3 == "Employee + Employer").FirstOrDefault();
                        var GetOtherContriPjkMjkGL = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktiviti == PjkMjkAktvtDetail.fldOptConfValue && x.fld_Flag == "3" && x.fld_TypeCode == "GL" && x.fld_Deleted == false).Select(s => s.fld_SAPCode).FirstOrDefault();
                        AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, AmountMix, 0, "-", PjkMjkAktvtDetail.fldOptConfValue.Substring(0, 2), PjkMjkAktvtDetail.fldOptConfValue, PjkMjkAktvtDetail.fldOptConfFlag2, PjkMjkAktvtDetail.fldOptConfDesc, DTProcess, UserID, Month, Year, "C", 10, GetOtherContriPjkMjkGL, "-", "-", "-");
                        message = "Transaction Listing (" + GetOtherContribution.fld_NamaCaruman + " " + PjkMjkAktvtDetail.fldOptConfFlag3 + "). (Data - Code Activity : " + PjkMjkAktvtDetail.fldOptConfValue + ", Amount : RM " + AmountMix + ")";
                        Log += "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                    }
                }
            }

            db.Dispose();
            db2.Dispose();
        }

        public void GetAmountAIPSFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, out string Log, List<tbl_Pkjmast> PkjMastList)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            Log = "";
            string message = "";
            int i = 1;
            decimal? Amount = 0;

            var GetAIPS = db.tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "aips" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).ToList();
            var NoPkjList = PkjMastList.Select(s => s.fld_Nopkj).ToArray();
            var tbl_GajiBulanan = db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year).ToList();
            var GajiBulananID = tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && NoPkjList.Contains(x.fld_Nopkj)).Select(s => s.fld_ID).ToList();
            foreach (var AIPS in GetAIPS)
            {
                Amount = tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && GajiBulananID.Contains(x.fld_ID)).Sum(s => s.fld_AIPS);

                Amount = Amount == null ? 0 : Amount;

                if (Amount != 0)
                {
                    AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Amount, 0, "-", AIPS.fldOptConfValue.Substring(0, 2), AIPS.fldOptConfValue, AIPS.fldOptConfFlag2, AIPS.fldOptConfDesc, DTProcess, UserID, Month, Year, "D", 9, "-", "-", "-", "-");
                    message = "Transaction Listing (AIPS). (Data - Code Activity : " + AIPS.fldOptConfValue + ", Amount : RM " + Amount + ")";
                    Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                    i++;
                }
            }
            db.Dispose();
            db2.Dispose();
        }

        public void GetAmountWorkerSalaryFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, out string Log, List<tbl_Pkjmast> PkjMastList)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            Log = "";
            string message = "";
            int i = 1;
            decimal? Amount = 0;

            var GetWorkerSalary = db.tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "gajipkj" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).FirstOrDefault();
            var GetEstateCOde = db.tbl_Ladang.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WlyhID == WilayahID && x.fld_ID == LadangID).Select(s => s.fld_LdgCode).FirstOrDefault();
            // TEMPATAN
            var NoPkjListTKT = PkjMastList.Where(x => x.fld_Kdrkyt == "MA").Select(s => s.fld_Nopkj).ToArray();
            if (NoPkjListTKT.Count() > 0)
            {
                Amount = db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && NoPkjListTKT.Contains(x.fld_Nopkj)).Sum(s => s.fld_GajiBersih);

                Amount = Amount == null ? 0 : Amount;

                if (Amount != 0)
                {
                    var GetSalaryGL = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktiviti == GetWorkerSalary.fldOptConfValue && x.fld_Flag == "4" && x.fld_TypeCode == "GLTKT" && x.fld_Deleted == false).Select(s => s.fld_SAPCode).FirstOrDefault();
                    AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Amount, 0, "-", GetWorkerSalary.fldOptConfValue.Substring(0, 2), GetWorkerSalary.fldOptConfValue, GetWorkerSalary.fldOptConfFlag2, GetWorkerSalary.fldOptConfDesc + " - TKT", DTProcess, UserID, Month, Year, "C", 10, GetSalaryGL, "-", "-", "-");
                    message = "Transaction Listing (Worker Salary - TKT). (Data - Code Activity : " + GetWorkerSalary.fldOptConfValue + ", Amount : RM " + Amount + ")";
                    Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                }
            }

            // ASING
            var NoPkjListTKA = PkjMastList.Where(x => x.fld_Kdrkyt != "MA").Select(s => s.fld_Nopkj).ToArray();
            Amount = 0;
            if (NoPkjListTKA.Count() > 0)
            {
                Amount = db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && NoPkjListTKA.Contains(x.fld_Nopkj)).Sum(s => s.fld_GajiBersih);

                Amount = Amount == null ? 0 : Amount;

                if (Amount != 0)
                {
                    var GetSalaryGL = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodAktiviti == GetWorkerSalary.fldOptConfValue && x.fld_Flag == "4" && x.fld_TypeCode == "GLTKA" && x.fld_Deleted == false).Select(s => s.fld_SAPCode).FirstOrDefault();
                    AddTo_tbl_Sctran(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Amount, 0, "-", GetWorkerSalary.fldOptConfValue.Substring(0, 2), GetWorkerSalary.fldOptConfValue, GetWorkerSalary.fldOptConfFlag2, GetWorkerSalary.fldOptConfDesc + " - TKA", DTProcess, UserID, Month, Year, "C", 10, GetSalaryGL, "-", "-", "-");
                    message = "Transaction Listing (Worker Salary - TKA). (Data - Code Activity : " + GetWorkerSalary.fldOptConfValue + ", Amount : RM " + Amount + ")";
                    Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                }
            }

            db.Dispose();
            db2.Dispose();
        }

        public void GetDebitCreditBalanceFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, out string Log)
        {
            GetConnectFunc conn = new GetConnectFunc();
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            Log = "";
            string message = "";
            decimal? Debit = 0;
            decimal? Credit = 0;
            int i = 1;

            var GetCotribution = db.tblOptionConfigsWebs.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldOptConfFlag3 == "Employee" && x.fldDeleted == false).Select(s => s.fldOptConfValue).ToList();

            var CheckCloseBizTable = db2.tbl_TutupUrusNiaga.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_DivisionID == DivisionID && x.fld_Month == Month && x.fld_Year == Year).FirstOrDefault();

            var TransactionListingList = db2.tbl_Sctran
                    .Where(x => !GetCotribution.Contains(x.fld_KodAktvt) && x.fld_Month == Month &&
                                x.fld_Year == Year && x.fld_NegaraID == NegaraID &&
                                x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID &&
                                x.fld_LadangID == LadangID && x.fld_DivisionID == DivisionID).ToList();

            Debit = TransactionListingList.Where(x => x.fld_KdCaj == "D").Sum(s => s.fld_Amt);
            Credit = TransactionListingList.Where(x => x.fld_KdCaj == "C").Sum(s => s.fld_Amt);

            AddTo_tbl_Skb(db2, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, Credit, Month, Year);

            if (CheckCloseBizTable != null)
            {
                CheckCloseBizTable.fld_StsTtpUrsNiaga = false;
                CheckCloseBizTable.fld_Debit = Debit;
                CheckCloseBizTable.fld_Credit = Credit;
                CheckCloseBizTable.fld_CreatedDT = DTProcess;
                CheckCloseBizTable.fld_CreatedBy = UserID;
                db2.Entry(CheckCloseBizTable).State = EntityState.Modified;
                db2.SaveChanges();
            }
            else
            {
                tbl_TutupUrusNiaga tbl_TutupUrusNiaga = new tbl_TutupUrusNiaga();

                tbl_TutupUrusNiaga.fld_NegaraID = NegaraID;
                tbl_TutupUrusNiaga.fld_SyarikatID = SyarikatID;
                tbl_TutupUrusNiaga.fld_WilayahID = WilayahID;
                tbl_TutupUrusNiaga.fld_LadangID = LadangID;
                tbl_TutupUrusNiaga.fld_DivisionID = DivisionID;
                tbl_TutupUrusNiaga.fld_StsTtpUrsNiaga = false;
                tbl_TutupUrusNiaga.fld_Year = Year;
                tbl_TutupUrusNiaga.fld_Month = Month;
                tbl_TutupUrusNiaga.fld_CreatedDT = DTProcess;
                tbl_TutupUrusNiaga.fld_CreatedBy = UserID;
                tbl_TutupUrusNiaga.fld_Debit = Debit;
                tbl_TutupUrusNiaga.fld_Credit = Credit;
                db2.tbl_TutupUrusNiaga.Add(tbl_TutupUrusNiaga);
                db2.SaveChanges();
            }

            message = "Debit Credit Balance. (Data - Debit Amount : RM " + Debit + ", Credit Amount : RM " + Credit + ")";
            Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;

            db2.Dispose();
        }

        public string FirstCharToUpper(string s)
        {
            char[] array = s.ToCharArray();

            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }

            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }

            return new string(array);
        }

        public void Add_tbl_AuditTrail(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? Year)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            tbl_AuditTrail tbl_AuditTrail = new tbl_AuditTrail();

            var checkAuditTrail = db.tbl_AuditTrail.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_DivisionID == DivisionID && x.fld_Thn == Year).FirstOrDefault();

            if (checkAuditTrail == null)
            {
                tbl_AuditTrail.fld_Bln1 = 0;
                tbl_AuditTrail.fld_Bln2 = 0;
                tbl_AuditTrail.fld_Bln3 = 0;
                tbl_AuditTrail.fld_Bln4 = 0;
                tbl_AuditTrail.fld_Bln5 = 0;
                tbl_AuditTrail.fld_Bln6 = 0;
                tbl_AuditTrail.fld_Bln7 = 0;
                tbl_AuditTrail.fld_Bln8 = 0;
                tbl_AuditTrail.fld_Bln9 = 0;
                tbl_AuditTrail.fld_Bln10 = 0;
                tbl_AuditTrail.fld_Bln11 = 0;
                tbl_AuditTrail.fld_Bln12 = 0;
                tbl_AuditTrail.fld_NegaraID = NegaraID;
                tbl_AuditTrail.fld_SyarikatID = SyarikatID;
                tbl_AuditTrail.fld_WilayahID = WilayahID;
                tbl_AuditTrail.fld_LadangID = LadangID;
                tbl_AuditTrail.fld_DivisionID = DivisionID;
                tbl_AuditTrail.fld_Thn = Year;
                tbl_AuditTrail.fld_Deleted = false;

                db.tbl_AuditTrail.Add(tbl_AuditTrail);
                db.SaveChanges();
            }
        }

        public void AddTo_tbl_Skb(GenSalaryModelEstate db2, int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, decimal? Amount, int? Month, int? Year)
        {
            string monthstring = Month.ToString();
            tbl_Skb tbl_Skb = new tbl_Skb();
            if (monthstring.Length == 1)
            {
                monthstring = "0" + monthstring;
            }
            var CheckSkb = db2.tbl_Skb.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tahun == Year && x.fld_Bulan == monthstring && x.fld_DivisionID == DivisionID).FirstOrDefault();
            if (CheckSkb != null)
            {
                CheckSkb.fld_GajiBersih = Amount;
                db2.Entry(CheckSkb).State = EntityState.Modified;
                db2.SaveChanges();
            }
            else
            {
                tbl_Skb.fld_GajiBersih = Amount;
                tbl_Skb.fld_NegaraID = NegaraID;
                tbl_Skb.fld_SyarikatID = SyarikatID;
                tbl_Skb.fld_WilayahID = WilayahID;
                tbl_Skb.fld_LadangID = LadangID;
                tbl_Skb.fld_DivisionID = DivisionID;
                tbl_Skb.fld_Tahun = Year;
                tbl_Skb.fld_Bulan = monthstring;
                tbl_Skb.fld_Deleted = false;
                db2.tbl_Skb.Add(tbl_Skb);
                db2.SaveChanges();
            }
        }

        public void AddTo_tbl_Sctran(GenSalaryModelEstate db2, int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, decimal? Amount, byte? JnsPkt, string KodPkt, string JnisAktvt, string KodAktvt, string KodGL, string Keterangan, DateTime DTProcess, int? UserID, int? Month, int? Year, string KdCaj, byte? Kategori, string GLCode, string IOCode, string NNCC, string SAPActCode)
        {
            try
            {
                LogFunc2.DataScTransChecking2("JnisAktvt = " + JnisAktvt + "\n KodAktvt = " + KodAktvt + "\n JnsPkt = " + JnsPkt + "\n KodPkt = "
                    + KodPkt + "\n GLCode = " + GLCode + "\n Keterangan = " + Keterangan + "\n KdCaj = " + KdCaj + "\n SAPActCode = " + SAPActCode, "Error - add Trace");
                tbl_Sctran Sctran = new tbl_Sctran();

                Sctran.fld_NegaraID = NegaraID;
                Sctran.fld_SyarikatID = SyarikatID;
                Sctran.fld_WilayahID = WilayahID;
                Sctran.fld_LadangID = LadangID;
                Sctran.fld_DivisionID = DivisionID;
                Sctran.fld_Month = Month;
                Sctran.fld_Year = Year;
                Sctran.fld_JnisAktvt = JnisAktvt;
                Sctran.fld_KodAktvt = KodAktvt;
                Sctran.fld_JnsPkt = JnsPkt;
                Sctran.fld_KodPkt = KodPkt;
                Sctran.fld_KodGL = KodGL;
                Sctran.fld_Keterangan = Keterangan;
                Sctran.fld_KdCaj = KdCaj;
                Sctran.fld_CreatedDT = DTProcess;
                Sctran.fld_CreatedBy = UserID;
                Sctran.fld_Amt = Amount;
                Sctran.fld_Kategori = Kategori;
                Sctran.fld_GL = GLCode;
                Sctran.fld_IO = IOCode;
                Sctran.fld_NNCC = NNCC;
                Sctran.fld_SapActCode = SAPActCode;

                db2.tbl_Sctran.Add(Sctran);
                db2.SaveChanges();
            }
            catch (Exception ex)
            {
                LogFunc2.DataScTransChecking2("Error = " + ex.Message + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.Data, "Error -add");
            }
        }
    }
}
