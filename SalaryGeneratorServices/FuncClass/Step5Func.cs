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
    class Step5Func
    {
        private DateTimeFunc DateTimeFunc = new DateTimeFunc();
        public void AddTo_tbl_SAPPostRef(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, out string Log, short Purpose, string PurMsg, List<tbl_Pkjmast> Pkjmstlists)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            RemoveDataFunc RemoveDataFunc = new RemoveDataFunc();
            DateTime DateNow = DateTimeFunc.GetDateTime();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            Log = "";
            string message = "";
            int i = 1;
            bool Contribution = false;
            bool DeductionStatus = false;

            decimal? GajiKawalan = 0;
            string GLGajiKawalan = "";
            string GLKeteranganGajiKawalan = "";
            decimal? BalAmountDeduct = 0;
            DateTime LastDayMonthPosting = GetLastDayOfMonth(Month, Year);

            Guid SAPPostID1 = new Guid();
            Guid SAPPostID2 = new Guid();
            Guid SAPPostID3 = new Guid();
            List<tbl_SAPPostDataDetails> tbl_SAPPostDataDetails = new List<tbl_SAPPostDataDetails>();

            tbl_SAPPostRef tbl_SAPPostRef1 = new tbl_SAPPostRef();
            tbl_SAPPostRef tbl_SAPPostRef2 = new tbl_SAPPostRef();
            tbl_SAPPostRef tbl_SAPPostRef3 = new tbl_SAPPostRef();

            var GetSctran = db2.tbl_Sctran.Where(x => x.fld_Month == Month && x.fld_Year == Year && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_DivisionID == DivisionID).Count();
            if (GetSctran > 0)
            {
                var NSWL = db.vw_NSWL_2.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_DivisionID == DivisionID).FirstOrDefault();
                var CheckStatusProceed = db2.tbl_SAPPostRef.Where(x => x.fld_Month == Month && x.fld_Year == Year && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_DivisionID == DivisionID).ToList();
                var SapDocType = db.tblOptionConfigsWebs.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldOptConfFlag1 == "purposeCode" && x.fldDeleted == false).ToList();
                var CheckStatusProceed1 = CheckStatusProceed.Where(x => x.fld_Purpose == 1).FirstOrDefault();
                if (CheckStatusProceed1 != null)
                {
                    SAPPostID1 = CheckStatusProceed1.fld_ID;
                    if (CheckStatusProceed1.fld_StatusProceed == false)
                    {
                        RemoveDataFunc.RemoveData_tbl_SAPPostingDetail(db2, CheckStatusProceed1.fld_ID);

                        CheckStatusProceed1.fld_DocDate = LastDayMonthPosting;
                        CheckStatusProceed1.fld_PostingDate = LastDayMonthPosting;
                        CheckStatusProceed1.fld_ModifiedDT = DateNow;
                        CheckStatusProceed1.fld_ModifiedBy = UserID;
                        db2.Entry(CheckStatusProceed1).State = EntityState.Modified;
                        db2.SaveChanges();
                        Add_To_tbl_SAPPostRef1(db, db2, Month, Year, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, SAPPostID1, out GajiKawalan, out GLGajiKawalan, out GLKeteranganGajiKawalan, out Contribution, Pkjmstlists);
                    }
                    else
                    {
                        Add_To_tbl_SAPPostRef1Check(db, db2, Month, Year, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, SAPPostID1, out GajiKawalan, out GLGajiKawalan, out GLKeteranganGajiKawalan, out Contribution);
                    }
                }
                else
                {
                    var SAPDocType1 = SapDocType.Where(x => x.fldOptConfValue == "1").FirstOrDefault();
                    tbl_SAPPostRef1.fld_Month = Month;
                    tbl_SAPPostRef1.fld_Year = Year;
                    tbl_SAPPostRef1.fld_CompCode = NSWL.fld_DivisionSAPCode;
                    tbl_SAPPostRef1.fld_DocDate = LastDayMonthPosting;
                    tbl_SAPPostRef1.fld_PostingDate = LastDayMonthPosting;
                    tbl_SAPPostRef1.fld_DocType = SAPDocType1.fldOptConfFlag2;
                    tbl_SAPPostRef1.fld_Purpose = short.Parse(SAPDocType1.fldOptConfValue);
                    tbl_SAPPostRef1.fld_CreatedDT = DateNow;
                    tbl_SAPPostRef1.fld_CreatedBy = UserID;
                    tbl_SAPPostRef1.fld_NegaraID = NegaraID;
                    tbl_SAPPostRef1.fld_SyarikatID = SyarikatID;
                    tbl_SAPPostRef1.fld_WilayahID = WilayahID;
                    tbl_SAPPostRef1.fld_LadangID = LadangID;
                    tbl_SAPPostRef1.fld_DivisionID = DivisionID;
                    tbl_SAPPostRef1.fld_StatusProceed = false;
                    db2.tbl_SAPPostRef.Add(tbl_SAPPostRef1);
                    db2.SaveChanges();

                    SAPPostID1 = tbl_SAPPostRef1.fld_ID;

                    Add_To_tbl_SAPPostRef1(db, db2, Month, Year, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, SAPPostID1, out GajiKawalan, out GLGajiKawalan, out GLKeteranganGajiKawalan, out Contribution, Pkjmstlists);
                }

                message = PurMsg + " (Created SAP Data Step 1)";
                Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;

                if (Contribution)
                {
                    var CheckStatusProceed2 = CheckStatusProceed.Where(x => x.fld_Purpose == 2).FirstOrDefault();

                    if (CheckStatusProceed2 != null)
                    {
                        SAPPostID2 = CheckStatusProceed2.fld_ID;
                        if (CheckStatusProceed2.fld_StatusProceed == false)
                        {
                            RemoveDataFunc.RemoveData_tbl_SAPPostingDetail(db2, CheckStatusProceed2.fld_ID);

                            CheckStatusProceed2.fld_ModifiedDT = DateNow;
                            CheckStatusProceed2.fld_ModifiedBy = UserID;
                            CheckStatusProceed2.fld_DocDate = LastDayMonthPosting;
                            CheckStatusProceed2.fld_PostingDate = LastDayMonthPosting;
                            db2.Entry(CheckStatusProceed2).State = EntityState.Modified;
                            db2.SaveChanges();

                            Add_To_tbl_SAPPostRef2(db, db2, Month, Year, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, SAPPostID2, GajiKawalan, GLGajiKawalan, GLKeteranganGajiKawalan, out DeductionStatus, out BalAmountDeduct);
                        }
                        else
                        {
                            Add_To_tbl_SAPPostRef2Check(db, db2, Month, Year, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, SAPPostID2, GajiKawalan, GLGajiKawalan, GLKeteranganGajiKawalan, out DeductionStatus, out BalAmountDeduct);
                        }
                    }
                    else
                    {
                        var SAPDocType2 = SapDocType.Where(x => x.fldOptConfValue == "2").FirstOrDefault();
                        tbl_SAPPostRef2.fld_Month = Month;
                        tbl_SAPPostRef2.fld_Year = Year;
                        tbl_SAPPostRef2.fld_CompCode = NSWL.fld_DivisionSAPCode;
                        tbl_SAPPostRef2.fld_DocDate = LastDayMonthPosting;
                        tbl_SAPPostRef2.fld_PostingDate = LastDayMonthPosting;
                        tbl_SAPPostRef2.fld_DocType = SAPDocType2.fldOptConfFlag2;
                        tbl_SAPPostRef2.fld_Purpose = short.Parse(SAPDocType2.fldOptConfValue);
                        tbl_SAPPostRef2.fld_CreatedDT = DateNow;
                        tbl_SAPPostRef2.fld_CreatedBy = UserID;
                        tbl_SAPPostRef2.fld_NegaraID = NegaraID;
                        tbl_SAPPostRef2.fld_SyarikatID = SyarikatID;
                        tbl_SAPPostRef2.fld_WilayahID = WilayahID;
                        tbl_SAPPostRef2.fld_LadangID = LadangID;
                        tbl_SAPPostRef2.fld_DivisionID = DivisionID;
                        tbl_SAPPostRef2.fld_StatusProceed = false;
                        db2.tbl_SAPPostRef.Add(tbl_SAPPostRef2);
                        db2.SaveChanges();

                        SAPPostID2 = tbl_SAPPostRef2.fld_ID;

                        Add_To_tbl_SAPPostRef2(db, db2, Month, Year, NegaraID, SyarikatID, WilayahID, LadangID, DivisionID, SAPPostID2, GajiKawalan, GLGajiKawalan, GLKeteranganGajiKawalan, out DeductionStatus, out BalAmountDeduct);
                    }
                    message = PurMsg + " (Created SAP Data Step 2)";
                    Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                }

                //                var CheckStatusProceed3 = CheckStatusProceed.Where(x => x.fld_Purpose == 3).FirstOrDefault();
                //                if (CheckStatusProceed3 != null)
                //                {
                //                    SAPPostID3 = CheckStatusProceed3.fld_ID;
                //                    if (CheckStatusProceed3.fld_StatusProceed == false)
                //                    {
                //                        RemoveDataFunc.RemoveData_tbl_SAPPostingDetail(db2, CheckStatusProceed3.fld_ID);

                //                        CheckStatusProceed3.fld_ModifiedDT = DateNow;
                //                        CheckStatusProceed3.fld_ModifiedBy = UserID;
                //                        CheckStatusProceed3.fld_DocDate = LastDayMonthPosting;
                //                        CheckStatusProceed3.fld_PostingDate = LastDayMonthPosting;
                //                        db2.Entry(CheckStatusProceed3).State = EntityState.Modified;
                //                        db2.SaveChanges();
                //;
                //                        Add_To_tbl_SAPPostRef3(db, db2, Month, Year, NegaraID, SyarikatID, WilayahID, LadangID, SAPPostID3, Pkjmstlists, BalAmountDeduct, GLGajiKawalan, GLKeteranganGajiKawalan, DeductionStatus);
                //                    }
                //                }
                //                else
                //                {
                //                    var SAPDocType3 = SapDocType.Where(x => x.fldOptConfValue == "3").FirstOrDefault();
                //                    tbl_SAPPostRef3.fld_Month = Month;
                //                    tbl_SAPPostRef3.fld_Year = Year;
                //                    tbl_SAPPostRef3.fld_CompCode = NSWL.fld_LdgCode;
                //                    tbl_SAPPostRef3.fld_DocDate = LastDayMonthPosting;
                //                    tbl_SAPPostRef3.fld_PostingDate = LastDayMonthPosting;
                //                    tbl_SAPPostRef3.fld_DocType = SAPDocType3.fldOptConfFlag2;
                //                    tbl_SAPPostRef3.fld_Purpose = short.Parse(SAPDocType3.fldOptConfValue);
                //                    tbl_SAPPostRef3.fld_CreatedDT = DateNow;
                //                    tbl_SAPPostRef3.fld_CreatedBy = UserID;
                //                    tbl_SAPPostRef3.fld_NegaraID = NegaraID;
                //                    tbl_SAPPostRef3.fld_SyarikatID = SyarikatID;
                //                    tbl_SAPPostRef3.fld_WilayahID = WilayahID;
                //                    tbl_SAPPostRef3.fld_LadangID = LadangID;
                //                    tbl_SAPPostRef3.fld_DivisionID = DivisionID;
                //                    tbl_SAPPostRef3.fld_StatusProceed = false;
                //                    db2.tbl_SAPPostRef.Add(tbl_SAPPostRef3);
                //                    db2.SaveChanges();

                //                    SAPPostID3 = tbl_SAPPostRef3.fld_ID;

                //                    Add_To_tbl_SAPPostRef3(db, db2, Month, Year, NegaraID, SyarikatID, WilayahID, LadangID, SAPPostID3, Pkjmstlists, BalAmountDeduct, GLGajiKawalan, GLKeteranganGajiKawalan, DeductionStatus);

                //                    message = PurMsg + " (Created SAP Data Step 3)";
                //                    Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
                //                }
            }
            else
            {
                message = PurMsg + " (No Data SAP Created)";
                Log += i == 1 ? DateTimeFunc.GetDateTime() + " - " + message : "\r\n" + DateTimeFunc.GetDateTime() + " - " + message;
            }

        }

        public void Add_To_tbl_SAPPostRef1(GenSalaryModelHQ db, GenSalaryModelEstate db2, int? Month, int? Year, int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, Guid SAPPostID1, out decimal? GajiKawalan, out string GLGajiKawalan, out string GLKeteranganGajiKawalan, out bool Contribution, List<tbl_Pkjmast> Pkjmstlists)
        {
            List<tbl_SAPPostDataDetails> tbl_SAPPostDataDetails = new List<tbl_SAPPostDataDetails>();
            int i = 1;
            decimal? Amount = 0;
            string DescActvt = "";
            GajiKawalan = 0;
            GLGajiKawalan = "";
            GLKeteranganGajiKawalan = "";
            Contribution = false;

            var ScTrans = db2.tbl_Sctran.Where(x => x.fld_Month == Month && x.fld_Year == Year && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_DivisionID == DivisionID).Select(s => new { s.fld_NNCC, s.fld_GL, s.fld_SapActCode, s.fld_Amt, s.fld_KodAktvt, s.fld_Keterangan }).ToList();

            var GetWorkActvt = ScTrans.Where(x => x.fld_KodAktvt.Length == 4).Select(s => new { s.fld_NNCC, s.fld_GL, s.fld_SapActCode, s.fld_Amt, s.fld_KodAktvt }).ToList();

            var GetWorkActvtDistincts = GetWorkActvt.Select(s => new { s.fld_NNCC, s.fld_GL, s.fld_SapActCode }).Distinct().ToList();

            var GetDescriptionText = db.tbl_SAPGLPUP.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID).Select(s => new { s.fld_GLCode, s.fld_GLDesc }).ToList();

            var GetGLData = db.tbl_MapGL.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false && x.fld_KodGL != "").ToList();

            var GetEstateCOde = db.tbl_Ladang.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WlyhID == WilayahID && x.fld_ID == LadangID).Select(s => s.fld_LdgCode).FirstOrDefault();


            var GetPkjKrytn = Pkjmstlists.Select(s => s.fld_Kdrkyt).Distinct().ToList();

            if (GetPkjKrytn.Count == 1 && GetPkjKrytn.Contains("MA"))
            {
                //Working Data TKT
                var GetTKTGL = GetGLData.Where(x => x.fld_Paysheet == "PT").Select(s => s.fld_KodGL).Distinct().ToList();
                var GetWorkActvtDistinctsTKTs = GetWorkActvtDistincts.Where(x => GetTKTGL.Contains(x.fld_GL)).ToList();
                foreach (var GetWorkActvtDistinctsTKT in GetWorkActvtDistinctsTKTs)
                {
                    DescActvt = "RUMUSAN GAJI - TKT" + " (" + GetEstateCOde + ") " + Month + "/" + Year;
                    Amount = GetWorkActvt.Where(x => x.fld_NNCC == GetWorkActvtDistinctsTKT.fld_NNCC && x.fld_SapActCode == GetWorkActvtDistinctsTKT.fld_SapActCode && x.fld_GL == GetWorkActvtDistinctsTKT.fld_GL).Sum(s => s.fld_Amt);
                    if (Amount != 0)
                    {
                        tbl_SAPPostDataDetails.Add(new tbl_SAPPostDataDetails() { fld_Amount = Amount, fld_Currency = "RM", fld_Desc = DescActvt, fld_GL = GetWorkActvtDistinctsTKT.fld_GL, fld_Item = GetWorkActvtDistinctsTKT.fld_NNCC == "-" ? null : GetWorkActvtDistinctsTKT.fld_NNCC, fld_ItemNo = i, fld_Purpose = "1", fld_SAPActivityCode = GetWorkActvtDistinctsTKT.fld_SapActCode == "-" ? null : GetWorkActvtDistinctsTKT.fld_SapActCode, fld_SAPPostRefID = SAPPostID1 });
                        i++;
                    }
                }
            }
            else if (GetPkjKrytn.Count > 1 && GetPkjKrytn.Contains("MA"))
            {
                //Working Data TKT
                var GetTKTGL = GetGLData.Where(x => x.fld_Paysheet == "PT").Select(s => s.fld_KodGL).Distinct().ToList();
                var GetWorkActvtDistinctsTKTs = GetWorkActvtDistincts.Where(x => GetTKTGL.Contains(x.fld_GL)).ToList();
                foreach (var GetWorkActvtDistinctsTKT in GetWorkActvtDistinctsTKTs)
                {
                    DescActvt = "RUMUSAN GAJI - TKT" + " (" + GetEstateCOde + ") " + Month + "/" + Year;
                    Amount = GetWorkActvt.Where(x => x.fld_NNCC == GetWorkActvtDistinctsTKT.fld_NNCC && x.fld_SapActCode == GetWorkActvtDistinctsTKT.fld_SapActCode && x.fld_GL == GetWorkActvtDistinctsTKT.fld_GL).Sum(s => s.fld_Amt);
                    if (Amount != 0)
                    {
                        tbl_SAPPostDataDetails.Add(new tbl_SAPPostDataDetails() { fld_Amount = Amount, fld_Currency = "RM", fld_Desc = DescActvt, fld_GL = GetWorkActvtDistinctsTKT.fld_GL, fld_Item = GetWorkActvtDistinctsTKT.fld_NNCC == "-" ? null : GetWorkActvtDistinctsTKT.fld_NNCC, fld_ItemNo = i, fld_Purpose = "1", fld_SAPActivityCode = GetWorkActvtDistinctsTKT.fld_SapActCode == "-" ? null : GetWorkActvtDistinctsTKT.fld_SapActCode, fld_SAPPostRefID = SAPPostID1 });
                        i++;
                    }
                }

                //Working Data TKA
                var GetDataAlreadyInPost = tbl_SAPPostDataDetails.Select(s => new { s.fld_GL, s.fld_Item, s.fld_SAPActivityCode }).Distinct().ToList();
                var GetTKAGL = GetGLData.Where(x => x.fld_Paysheet == "PA").Select(s => s.fld_KodGL).Distinct().ToList();
                var GetWorkActvtDistinctsTKAs = GetWorkActvtDistincts.Where(x => GetTKAGL.Contains(x.fld_GL)).ToList();
                foreach (var GetWorkActvtDistinctsTKA in GetWorkActvtDistinctsTKAs)
                {
                    var fld_NNCC = GetWorkActvtDistinctsTKA.fld_NNCC == "-" ? null : GetWorkActvtDistinctsTKA.fld_NNCC;
                    var fld_SapActCode = GetWorkActvtDistinctsTKA.fld_SapActCode == "-" ? null : GetWorkActvtDistinctsTKA.fld_SapActCode;
                    if (GetDataAlreadyInPost.Where(x => x.fld_GL == GetWorkActvtDistinctsTKA.fld_GL && x.fld_Item == fld_NNCC && x.fld_SAPActivityCode == fld_SapActCode).Count() == 0)
                    {
                        DescActvt = "RUMUSAN GAJI - TKA" + " (" + GetEstateCOde + ") " + Month + "/" + Year;
                        Amount = GetWorkActvt.Where(x => x.fld_NNCC == GetWorkActvtDistinctsTKA.fld_NNCC && x.fld_SapActCode == GetWorkActvtDistinctsTKA.fld_SapActCode && x.fld_GL == GetWorkActvtDistinctsTKA.fld_GL).Sum(s => s.fld_Amt);
                        if (Amount != 0)
                        {
                            tbl_SAPPostDataDetails.Add(new tbl_SAPPostDataDetails() { fld_Amount = Amount, fld_Currency = "RM", fld_Desc = DescActvt, fld_GL = GetWorkActvtDistinctsTKA.fld_GL, fld_Item = GetWorkActvtDistinctsTKA.fld_NNCC == "-" ? null : GetWorkActvtDistinctsTKA.fld_NNCC, fld_ItemNo = i, fld_Purpose = "1", fld_SAPActivityCode = GetWorkActvtDistinctsTKA.fld_SapActCode == "-" ? null : GetWorkActvtDistinctsTKA.fld_SapActCode, fld_SAPPostRefID = SAPPostID1 });
                            i++;
                        }
                    }
                }
            }
            else
            {
                //Working Data TKA
                var GetTKAGL = GetGLData.Where(x => x.fld_Paysheet == "PA").Select(s => s.fld_KodGL).Distinct().ToList();
                var GetWorkActvtDistinctsTKAs = GetWorkActvtDistincts.Where(x => GetTKAGL.Contains(x.fld_GL)).ToList();
                foreach (var GetWorkActvtDistinctsTKA in GetWorkActvtDistinctsTKAs)
                {
                    DescActvt = "RUMUSAN GAJI - TKA" + " (" + GetEstateCOde + ") " + Month + "/" + Year;
                    Amount = GetWorkActvt.Where(x => x.fld_NNCC == GetWorkActvtDistinctsTKA.fld_NNCC && x.fld_SapActCode == GetWorkActvtDistinctsTKA.fld_SapActCode && x.fld_GL == GetWorkActvtDistinctsTKA.fld_GL).Sum(s => s.fld_Amt);
                    if (Amount != 0)
                    {
                        tbl_SAPPostDataDetails.Add(new tbl_SAPPostDataDetails() { fld_Amount = Amount, fld_Currency = "RM", fld_Desc = DescActvt, fld_GL = GetWorkActvtDistinctsTKA.fld_GL, fld_Item = GetWorkActvtDistinctsTKA.fld_NNCC == "-" ? null : GetWorkActvtDistinctsTKA.fld_NNCC, fld_ItemNo = i, fld_Purpose = "1", fld_SAPActivityCode = GetWorkActvtDistinctsTKA.fld_SapActCode == "-" ? null : GetWorkActvtDistinctsTKA.fld_SapActCode, fld_SAPPostRefID = SAPPostID1 });
                        i++;
                    }
                }
            }

            var GetNotWorkActs = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && (x.fld_TypeCode == "GLTKT" || x.fld_TypeCode == "GLTKA" || x.fld_TypeCode == "GL")).ToList();

            //Kira Caruman Majikan flag = 1
            var GetNotWorkAct2s = GetNotWorkActs.Where(x => x.fld_Flag == "1" && x.fld_TypeCode == "GL").Select(s => s.fld_KodAktiviti).ToList();
            var CheckNotWorkAct2 = ScTrans.Where(x => GetNotWorkAct2s.Contains(x.fld_KodAktvt)).Select(s => new { s.fld_KodAktvt, s.fld_NNCC, s.fld_GL }).ToList();
            if (CheckNotWorkAct2.Count > 0)
            {
                foreach (var CheckNotWorkAct2Data in CheckNotWorkAct2)
                {
                    DescActvt = ScTrans.Where(x => x.fld_GL == CheckNotWorkAct2Data.fld_GL && x.fld_NNCC == CheckNotWorkAct2Data.fld_NNCC && x.fld_KodAktvt == CheckNotWorkAct2Data.fld_KodAktvt).Select(s => s.fld_Keterangan).FirstOrDefault() + " (" + GetEstateCOde + ") " + Month + "/" + Year;
                    Amount = ScTrans.Where(x => x.fld_GL == CheckNotWorkAct2Data.fld_GL && x.fld_NNCC == CheckNotWorkAct2Data.fld_NNCC && x.fld_KodAktvt == CheckNotWorkAct2Data.fld_KodAktvt).Sum(s => s.fld_Amt);
                    if (Amount != 0)
                    {
                        tbl_SAPPostDataDetails.Add(new tbl_SAPPostDataDetails() { fld_Amount = Amount, fld_Currency = "RM", fld_Desc = DescActvt.ToUpper(), fld_GL = CheckNotWorkAct2Data.fld_GL, fld_Item = CheckNotWorkAct2Data.fld_NNCC, fld_ItemNo = i, fld_Purpose = "1", fld_SAPActivityCode = null, fld_SAPPostRefID = SAPPostID1 });
                        i++;
                        Contribution = true;
                    }
                }

                //var CCNo = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WIlayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Flag == "1" && x.fld_TypeCode == "CCGLFINANCE").Select(s => s.fld_SAPCode).FirstOrDefault();
                //foreach (var GetNotWorkAct2 in GetNotWorkAct2s)
                //{
                //    DescActvt = ScTrans.Where(x => x.fld_KodAktvt == GetNotWorkAct2).Select(s => s.fld_Keterangan).FirstOrDefault();
                //    Amount = ScTrans.Where(x => x.fld_KodAktvt == GetNotWorkAct2).Sum(s => s.fld_Amt);
                //    if (Amount != 0)
                //    {
                //        var GLCCNo = ScTrans.Where(x => x.fld_KodAktvt == GetNotWorkAct2).Select(s => new { s.fld_GL, s.fld_NNCC }).FirstOrDefault();
                //        //var CCNo = CCNoList.Where(x => x.fld_KodAktiviti == GetNotWorkAct2).Select(s => s.fld_SAPCode).FirstOrDefault();
                //        tbl_SAPPostDataDetails.Add(new tbl_SAPPostDataDetails() { fld_Amount = Amount, fld_Currency = "RM", fld_Desc = DescActvt, fld_GL = GLCCNo.fld_GL, fld_Item = GLCCNo.fld_NNCC, fld_ItemNo = i, fld_Purpose = "1", fld_SAPActivityCode = null, fld_SAPPostRefID = SAPPostID1 });
                //        i++;
                //        Contribution = true;
                //    }
                //}
            }

            //Kira Elaun, Byar Cuti flag = 2
            var GetNotWorkAct3s = GetNotWorkActs.Where(x => x.fld_Flag == "2" && (x.fld_TypeCode == "GLTKT" || x.fld_TypeCode == "GLTKA")).Select(s => s.fld_KodAktiviti).Distinct().ToList();
            var CheckNotWorkAct3 = ScTrans.Where(x => GetNotWorkAct3s.Contains(x.fld_KodAktvt)).Select(s => new { s.fld_KodAktvt, s.fld_NNCC, s.fld_GL }).Distinct().ToList();
            if (CheckNotWorkAct3.Count > 0)
            {
                foreach (var CheckNotWorkAct3Data in CheckNotWorkAct3)
                {
                    DescActvt = ScTrans.Where(x => x.fld_GL == CheckNotWorkAct3Data.fld_GL && x.fld_NNCC == CheckNotWorkAct3Data.fld_NNCC && x.fld_KodAktvt == CheckNotWorkAct3Data.fld_KodAktvt).Select(s => s.fld_Keterangan).FirstOrDefault() + " (" + GetEstateCOde + ") " + Month + "/" + Year;
                    Amount = ScTrans.Where(x => x.fld_GL == CheckNotWorkAct3Data.fld_GL && x.fld_NNCC == CheckNotWorkAct3Data.fld_NNCC && x.fld_KodAktvt == CheckNotWorkAct3Data.fld_KodAktvt).Sum(s => s.fld_Amt);
                    if (Amount != 0)
                    {
                        tbl_SAPPostDataDetails.Add(new tbl_SAPPostDataDetails() { fld_Amount = Amount, fld_Currency = "RM", fld_Desc = DescActvt.ToUpper(), fld_GL = CheckNotWorkAct3Data.fld_GL, fld_Item = CheckNotWorkAct3Data.fld_NNCC, fld_ItemNo = i, fld_Purpose = "1", fld_SAPActivityCode = null, fld_SAPPostRefID = SAPPostID1 });
                        i++;
                        Contribution = true;
                    }
                }
                //var CCNo = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WIlayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Flag == "1" && x.fld_TypeCode == "CCGLFINANCE").Select(s => s.fld_SAPCode).FirstOrDefault();
                //foreach (var GetNotWorkAct3 in CheckNotWorkAct3)
                //{
                //    DescActvt = ScTrans.Where(x => x.fld_KodAktvt == GetNotWorkAct3.fld_KodAktvt && x.fld_GL == GetNotWorkAct3.fld_GL).Select(s => s.fld_Keterangan).FirstOrDefault();
                //    Amount = ScTrans.Where(x => x.fld_KodAktvt == GetNotWorkAct3.fld_KodAktvt && x.fld_GL == GetNotWorkAct3.fld_GL).Sum(s => s.fld_Amt);
                //    if (Amount != 0)
                //    {
                //        var GLCCNo = ScTrans.Where(x => x.fld_KodAktvt == GetNotWorkAct3.fld_KodAktvt && x.fld_GL == GetNotWorkAct3.fld_GL).Select(s => new { s.fld_GL, s.fld_NNCC }).FirstOrDefault();
                //        tbl_SAPPostDataDetails.Add(new tbl_SAPPostDataDetails() { fld_Amount = Amount, fld_Currency = "RM", fld_Desc = DescActvt, fld_GL = GLCCNo.fld_GL, fld_Item = GLCCNo.fld_NNCC, fld_ItemNo = i, fld_Purpose = "1", fld_SAPActivityCode = null, fld_SAPPostRefID = SAPPostID1 });
                //        i++;
                //    }
                //}
            }

            //Kira Tolakkan
            var GetNotWorkAct6s = GetNotWorkActs.Where(x => x.fld_Flag == "5" && (x.fld_TypeCode == "GLTKT" || x.fld_TypeCode == "GLTKA")).Select(s => s.fld_KodAktiviti).ToList();
            var CheckNotWorkAct6 = ScTrans.Where(x => GetNotWorkAct6s.Contains(x.fld_KodAktvt)).Select(s => new { s.fld_KodAktvt, s.fld_NNCC, s.fld_GL }).Distinct().ToList();
            string CCSap = "";
            if (CheckNotWorkAct6.Count > 0)
            {
                foreach (var CheckNotWorkAct6Data in CheckNotWorkAct6)
                {
                    CCSap = "";
                    DescActvt = ScTrans.Where(x => x.fld_GL == CheckNotWorkAct6Data.fld_GL && x.fld_NNCC == CheckNotWorkAct6Data.fld_NNCC && x.fld_KodAktvt == CheckNotWorkAct6Data.fld_KodAktvt).Select(s => s.fld_Keterangan).FirstOrDefault() + " (" + GetEstateCOde + ") " + Month + "/" + Year;
                    Amount = ScTrans.Where(x => x.fld_GL == CheckNotWorkAct6Data.fld_GL && x.fld_NNCC == CheckNotWorkAct6Data.fld_NNCC && x.fld_KodAktvt == CheckNotWorkAct6Data.fld_KodAktvt).Sum(s => s.fld_Amt);
                    if (Amount != 0)
                    {
                        if (CheckNotWorkAct6Data.fld_GL.Substring(0, 3) == "001")
                        {
                            CCSap = null;
                        }
                        else
                        {
                            CCSap = CheckNotWorkAct6Data.fld_NNCC;
                        }
                        tbl_SAPPostDataDetails.Add(new tbl_SAPPostDataDetails() { fld_Amount = -Amount, fld_Currency = "RM", fld_Desc = DescActvt.ToUpper(), fld_GL = CheckNotWorkAct6Data.fld_GL, fld_Item = CCSap, fld_ItemNo = i, fld_Purpose = "1", fld_SAPActivityCode = null, fld_SAPPostRefID = SAPPostID1 });
                        i++;
                        Contribution = true;
                    }
                }
                ////var CCNo = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WIlayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Flag == "1" && x.fld_TypeCode == "CCGLFINANCE").Select(s => s.fld_SAPCode).FirstOrDefault();
                //var CCNo = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WIlayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Flag == "1" && x.fld_TypeCode == "CCGLFINANCE").Select(s => s.fld_SAPCode).FirstOrDefault();
                ////DescActvt = "Clearing - Salary Deduction";
                //foreach (var GetNotWorkAct6 in CheckNotWorkAct6)
                //{
                //    DescActvt = ScTrans.Where(x => x.fld_KodAktvt == GetNotWorkAct6.fld_KodAktvt && x.fld_GL == GetNotWorkAct6.fld_GL).Select(s => s.fld_Keterangan).FirstOrDefault();
                //    Amount = ScTrans.Where(x => x.fld_KodAktvt == GetNotWorkAct6.fld_KodAktvt && x.fld_GL == GetNotWorkAct6.fld_GL).Sum(s => s.fld_Amt);
                //    if (Amount != 0)
                //    {
                //        var GLNo = ScTrans.Where(x => GetNotWorkAct6s.Contains(x.fld_KodAktvt) && x.fld_GL != "-").Select(s => s.fld_GL).FirstOrDefault();
                //        tbl_SAPPostDataDetails.Add(new tbl_SAPPostDataDetails() { fld_Amount = -Amount, fld_Currency = "RM", fld_Desc = DescActvt, fld_GL = GLNo, fld_Item = CCNo, fld_ItemNo = i, fld_Purpose = "1", fld_SAPActivityCode = null, fld_SAPPostRefID = SAPPostID1 });
                //        i++;
                //    }
                //}
            }

            //Kira Caruman Majikan Pekerja (Kawalan Gaji)
            var GetNotWorkAct4s = GetNotWorkActs.Where(x => x.fld_Flag == "3" && x.fld_TypeCode == "GL").Select(s => s.fld_KodAktiviti).Distinct().ToList();
            var CheckNotWorkAct4 = ScTrans.Where(x => GetNotWorkAct4s.Contains(x.fld_KodAktvt)).ToList();
            if (CheckNotWorkAct4.Count > 0)
            {
                //var CCNo = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WIlayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Flag == "1" && x.fld_TypeCode == "CCGLFINANCE").Select(s => s.fld_SAPCode).FirstOrDefault();
                DescActvt = "Clearing" + " (" + GetEstateCOde + ") " + Month + "/" + Year;
                Amount = ScTrans.Where(x => GetNotWorkAct4s.Contains(x.fld_KodAktvt)).Sum(s => s.fld_Amt);
                if (Amount != 0)
                {
                    var GLNo = ScTrans.Where(x => GetNotWorkAct4s.Contains(x.fld_KodAktvt) && x.fld_GL != "-").Select(s => s.fld_GL).FirstOrDefault();
                    tbl_SAPPostDataDetails.Add(new tbl_SAPPostDataDetails() { fld_Amount = -Amount, fld_Currency = "RM", fld_Desc = DescActvt.ToUpper(), fld_GL = GLNo, fld_Item = null, fld_ItemNo = i, fld_Purpose = "1", fld_SAPActivityCode = null, fld_SAPPostRefID = SAPPostID1 });
                    i++;

                    GajiKawalan = Amount;
                    GLGajiKawalan = GLNo;
                    GLKeteranganGajiKawalan = DescActvt;
                }
            }

            GetNotWorkAct4s = GetNotWorkActs.Where(x => x.fld_Flag == "3" && x.fld_TypeCode == "GLTKT").Select(s => s.fld_KodAktiviti).Distinct().ToList();
            CheckNotWorkAct4 = ScTrans.Where(x => GetNotWorkAct4s.Contains(x.fld_KodAktvt)).ToList();
            if (CheckNotWorkAct4.Count > 0)
            {
                //var CCNo = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WIlayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Flag == "1" && x.fld_TypeCode == "CCGLFINANCE").Select(s => s.fld_SAPCode).FirstOrDefault();
                DescActvt = "Clearing TKT" + " (" + GetEstateCOde + ") " + Month + "/" + Year;
                Amount = ScTrans.Where(x => GetNotWorkAct4s.Contains(x.fld_KodAktvt)).Sum(s => s.fld_Amt);
                if (Amount != 0)
                {
                    var GLNo = ScTrans.Where(x => GetNotWorkAct4s.Contains(x.fld_KodAktvt) && x.fld_GL != "-").Select(s => s.fld_GL).FirstOrDefault();
                    tbl_SAPPostDataDetails.Add(new tbl_SAPPostDataDetails() { fld_Amount = -Amount, fld_Currency = "RM", fld_Desc = DescActvt.ToUpper(), fld_GL = GLNo, fld_Item = null, fld_ItemNo = i, fld_Purpose = "1", fld_SAPActivityCode = null, fld_SAPPostRefID = SAPPostID1 });
                    i++;

                    GajiKawalan = Amount;
                    GLGajiKawalan = GLNo;
                    GLKeteranganGajiKawalan = DescActvt;
                }
            }

            GetNotWorkAct4s = GetNotWorkActs.Where(x => x.fld_Flag == "3" && x.fld_TypeCode == "GLTKA").Select(s => s.fld_KodAktiviti).Distinct().ToList();
            CheckNotWorkAct4 = ScTrans.Where(x => GetNotWorkAct4s.Contains(x.fld_KodAktvt)).ToList();
            if (CheckNotWorkAct4.Count > 0)
            {
                //var CCNo = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WIlayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Flag == "1" && x.fld_TypeCode == "CCGLFINANCE").Select(s => s.fld_SAPCode).FirstOrDefault();
                DescActvt = "Clearing TKA" + " (" + GetEstateCOde + ") " + Month + "/" + Year;
                Amount = ScTrans.Where(x => GetNotWorkAct4s.Contains(x.fld_KodAktvt)).Sum(s => s.fld_Amt);
                if (Amount != 0)
                {
                    var GLNo = ScTrans.Where(x => GetNotWorkAct4s.Contains(x.fld_KodAktvt) && x.fld_GL != "-").Select(s => s.fld_GL).FirstOrDefault();
                    tbl_SAPPostDataDetails.Add(new tbl_SAPPostDataDetails() { fld_Amount = -Amount, fld_Currency = "RM", fld_Desc = DescActvt.ToUpper(), fld_GL = GLNo, fld_Item = null, fld_ItemNo = i, fld_Purpose = "1", fld_SAPActivityCode = null, fld_SAPPostRefID = SAPPostID1 });
                    i++;

                    GajiKawalan = Amount;
                    GLGajiKawalan = GLNo;
                    GLKeteranganGajiKawalan = DescActvt;
                }
            }

            var GetNotWorkAct5s = GetNotWorkActs.Where(x => x.fld_Flag == "4" && (x.fld_TypeCode == "GLTKT" || x.fld_TypeCode == "GLTKA")).Select(s => s.fld_KodAktiviti).ToList();
            var CheckNotWorkAct5 = ScTrans.Where(x => GetNotWorkAct5s.Contains(x.fld_KodAktvt)).ToList();
            if (CheckNotWorkAct5.Count > 0)
            {
                foreach (var GetNotWorkAct5 in CheckNotWorkAct5)
                {
                    DescActvt = ScTrans.Where(x => x.fld_KodAktvt == GetNotWorkAct5.fld_KodAktvt && x.fld_GL == GetNotWorkAct5.fld_GL).Select(s => s.fld_Keterangan).FirstOrDefault() + " (" + GetEstateCOde + ") " + Month + "/" + Year;
                    Amount = ScTrans.Where(x => x.fld_KodAktvt == GetNotWorkAct5.fld_KodAktvt && x.fld_GL == GetNotWorkAct5.fld_GL).Sum(s => s.fld_Amt);
                    if (Amount != 0)
                    {
                        var GLNo = ScTrans.Where(x => x.fld_KodAktvt == GetNotWorkAct5.fld_KodAktvt && x.fld_GL == GetNotWorkAct5.fld_GL).Select(s => s.fld_GL).FirstOrDefault();
                        tbl_SAPPostDataDetails.Add(new tbl_SAPPostDataDetails() { fld_Amount = -Amount, fld_Currency = "RM", fld_Desc = DescActvt.ToUpper(), fld_GL = GLNo, fld_Item = null, fld_ItemNo = i, fld_Purpose = "1", fld_SAPActivityCode = null, fld_SAPPostRefID = SAPPostID1 });
                        i++;
                    }
                }
            }

            if (tbl_SAPPostDataDetails.Count > 0)
            {
                db2.tbl_SAPPostDataDetails.AddRange(tbl_SAPPostDataDetails);
                db2.SaveChanges();
            }
        }

        public void Add_To_tbl_SAPPostRef1Check(GenSalaryModelHQ db, GenSalaryModelEstate db2, int? Month, int? Year, int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, Guid SAPPostID1, out decimal? GajiKawalan, out string GLGajiKawalan, out string GLKeteranganGajiKawalan, out bool Contribution)
        {
            int i = 1;
            decimal? Amount = 0;
            string DescActvt = "";
            GajiKawalan = 0;
            GLGajiKawalan = "";
            GLKeteranganGajiKawalan = "";
            Contribution = false;

            var ScTrans = db2.tbl_Sctran.Where(x => x.fld_Month == Month && x.fld_Year == Year && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_DivisionID == DivisionID).Select(s => new { s.fld_NNCC, s.fld_GL, s.fld_SapActCode, s.fld_Amt, s.fld_KodAktvt, s.fld_Keterangan }).ToList();

            var GetWorkActvt = ScTrans.Where(x => x.fld_KodAktvt.Length == 4).Select(s => new { s.fld_NNCC, s.fld_GL, s.fld_SapActCode, s.fld_Amt, s.fld_KodAktvt }).ToList();

            var GetWorkActvtDistincts = GetWorkActvt.Select(s => new { s.fld_NNCC, s.fld_GL, s.fld_SapActCode, s.fld_KodAktvt }).Distinct().ToList();

            var GetDescriptionText = db.tbl_SAPGLPUP.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID).Select(s => new { s.fld_GLCode, s.fld_GLDesc }).ToList();

            var GetNotWorkActs = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && (x.fld_TypeCode == "GLTKT" || x.fld_TypeCode == "GLTKA" || x.fld_TypeCode == "GL")).ToList();

            //Kira Caruman Majikan flag = 1
            var GetNotWorkAct2s = GetNotWorkActs.Where(x => x.fld_Flag == "1" && x.fld_TypeCode == "GL").Select(s => s.fld_KodAktiviti).ToList();
            var CheckNotWorkAct2 = ScTrans.Where(x => GetNotWorkAct2s.Contains(x.fld_KodAktvt)).Count();
            if (CheckNotWorkAct2 > 0)
            {
                var CCNoList = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WIlayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Flag == "1" && x.fld_TypeCode == "CCGL").ToList();
                foreach (var GetNotWorkAct2 in GetNotWorkAct2s)
                {
                    Contribution = true;
                }
            }

            //Kira Caruman Majikan Pekerja (Kawalan Gaji), penolakkan gaji
            var GetNotWorkAct4s = GetNotWorkActs.Where(x => x.fld_Flag == "3" && x.fld_TypeCode == "GL").Select(s => s.fld_KodAktiviti).ToList();
            var CheckNotWorkAct4 = ScTrans.Where(x => GetNotWorkAct4s.Contains(x.fld_KodAktvt)).Count();
            if (CheckNotWorkAct4 > 0)
            {
                DescActvt = "Clearing";
                Amount = ScTrans.Where(x => GetNotWorkAct4s.Contains(x.fld_KodAktvt)).Sum(s => s.fld_Amt);
                if (Amount != 0)
                {
                    var GLNo1 = ScTrans.Where(x => GetNotWorkAct4s.Contains(x.fld_KodAktvt) && x.fld_GL != "-").Select(s => s.fld_GL).FirstOrDefault();

                    GajiKawalan = Amount;
                    GLGajiKawalan = GLNo1;
                    GLKeteranganGajiKawalan = DescActvt;
                }
            }

        }

        public void Add_To_tbl_SAPPostRef2(GenSalaryModelHQ db, GenSalaryModelEstate db2, int? Month, int? Year, int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, Guid SAPPostID1, decimal? GajiKawalan, string GLGajiKawalan, string GLKeteranganGajiKawalan, out bool DeductionStatus, out decimal? BalAmountDeduct)
        {
            List<tbl_SAPPostDataDetails> tbl_SAPPostDataDetails = new List<tbl_SAPPostDataDetails>();
            int i = 1;
            decimal? Amount = 0;
            BalAmountDeduct = 0;
            string DescActvt = "";
            DeductionStatus = false;
            string TypeCode = "";

            var GetFASRND = db.tbl_Ladang.Where(x => x.fld_ID == LadangID).Select(s => s.fld_CostCentre).FirstOrDefault();

            if (GetFASRND == "RNDSB")
            {
                TypeCode = "VDRND";
            }
            else
            {
                TypeCode = "VDFAS";
            }

            var GetNotWorkActs = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_TypeCode == TypeCode).ToList();

            var GetNotWorkCodeAct1s = GetNotWorkActs.Where(x => x.fld_Flag == "1").Select(s => s.fld_KodAktiviti).ToArray();

            var ScTrans = db2.tbl_Sctran.Where(x => x.fld_Month == Month && x.fld_Year == Year && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_DivisionID == DivisionID).Select(s => new { s.fld_NNCC, s.fld_GL, s.fld_SapActCode, s.fld_Amt, s.fld_KodAktvt, s.fld_Keterangan }).ToList();

            var GetWorkActvt = ScTrans.Where(x => x.fld_KodAktvt.Length == 5).Select(s => new { s.fld_NNCC, s.fld_GL, s.fld_SapActCode, s.fld_Amt, s.fld_KodAktvt }).ToList();

            var GLClearings = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Flag == "3" && (x.fld_TypeCode == "GLTKT" || x.fld_TypeCode == "GLTKA" || x.fld_TypeCode == "GL")).Select(s => new { s.fld_SAPCode, s.fld_TypeCode }).Distinct().ToList();

            var GetEstateCOde = db.tbl_Ladang.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WlyhID == WilayahID && x.fld_ID == LadangID).Select(s => s.fld_LdgCode).FirstOrDefault();

            foreach (var Clearing in GLClearings)
            {
                var GLClearing = Clearing.fld_SAPCode;
                var typeGL = Clearing.fld_TypeCode == "GLTKA" ? "TKA" : "TKT";
                DescActvt = GLKeteranganGajiKawalan + " - " + typeGL + " (" + GetEstateCOde + ") " + Month + "/" + Year;
                Amount = ScTrans.Where(x => GetNotWorkCodeAct1s.Contains(x.fld_KodAktvt) && x.fld_GL == GLClearing).Sum(s => s.fld_Amt);
                if (Amount != 0)
                {
                    var GLNo = GLClearing;
                    tbl_SAPPostDataDetails.Add(new tbl_SAPPostDataDetails() { fld_Amount = Amount, fld_Currency = "RM", fld_Desc = DescActvt.ToUpper(), fld_GL = GLNo, fld_Item = null, fld_ItemNo = i, fld_Purpose = "2", fld_SAPActivityCode = null, fld_SAPPostRefID = SAPPostID1 });
                    i++;
                }
            }

            if (GajiKawalan - Amount > 0)
            {
                BalAmountDeduct = GajiKawalan - Amount;
                DeductionStatus = true;
            }
            //Bekaitan Caruman

            foreach (var GetNotWorkCodeAct1 in GetNotWorkCodeAct1s)
            {
                DescActvt = ScTrans.Where(x => x.fld_KodAktvt == GetNotWorkCodeAct1).Select(s => s.fld_Keterangan).FirstOrDefault() + " (" + GetEstateCOde + ") " + Month + "/" + Year;
                Amount = ScTrans.Where(x => x.fld_KodAktvt == GetNotWorkCodeAct1).Sum(s => s.fld_Amt);
                if (Amount != 0)
                {
                    var GLNo1 = GetNotWorkActs.Where(x => x.fld_KodAktiviti == GetNotWorkCodeAct1).Select(s => s.fld_SAPCode).FirstOrDefault();
                    tbl_SAPPostDataDetails.Add(new tbl_SAPPostDataDetails() { fld_Amount = -Amount, fld_Currency = "RM", fld_Desc = DescActvt.ToUpper(), fld_GL = null, fld_Item = GLNo1, fld_ItemNo = i, fld_Purpose = "2", fld_SAPActivityCode = null, fld_SAPPostRefID = SAPPostID1 });
                    i++;
                }
            }
            db2.tbl_SAPPostDataDetails.AddRange(tbl_SAPPostDataDetails);
            db2.SaveChanges();
        }

        public void Add_To_tbl_SAPPostRef2Check(GenSalaryModelHQ db, GenSalaryModelEstate db2, int? Month, int? Year, int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? DivisionID, Guid SAPPostID1, decimal? GajiKawalan, string GLGajiKawalan, string GLKeteranganGajiKawalan, out bool DeductionStatus, out decimal? BalAmountDeduct)
        {
            int i = 1;
            decimal? Amount = 0;
            BalAmountDeduct = 0;
            string DescActvt = "";
            DeductionStatus = false;

            var GetNotWorkActs = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_TypeCode == "VD").ToList();

            var GetNotWorkCodeAct1s = GetNotWorkActs.Where(x => x.fld_Flag == "1").Select(s => s.fld_KodAktiviti).ToArray();

            var ScTrans = db2.tbl_Sctran.Where(x => x.fld_Month == Month && x.fld_Year == Year && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_DivisionID == DivisionID).Select(s => new { s.fld_NNCC, s.fld_GL, s.fld_SapActCode, s.fld_Amt, s.fld_KodAktvt, s.fld_Keterangan }).ToList();

            var GetWorkActvt = ScTrans.Where(x => x.fld_KodAktvt.Length == 5).Select(s => new { s.fld_NNCC, s.fld_GL, s.fld_SapActCode, s.fld_Amt, s.fld_KodAktvt }).ToList();

            var GLClearing = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Flag == "3" && x.fld_TypeCode == "GL").Select(s => s.fld_SAPCode).FirstOrDefault();

            DescActvt = GLKeteranganGajiKawalan;
            if (Amount != 0)
            {
                Amount = ScTrans.Where(x => GetNotWorkCodeAct1s.Contains(x.fld_KodAktvt)).Sum(s => s.fld_Amt);
                var GLNo = GLClearing;

                if (GajiKawalan - Amount > 0)
                {
                    BalAmountDeduct = GajiKawalan - Amount;
                    DeductionStatus = true;
                }
            }
        }

        public void Add_To_tbl_SAPPostRef3(GenSalaryModelHQ db, GenSalaryModelEstate db2, int? Month, int? Year, int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, Guid SAPPostID1, List<tbl_Pkjmast> Pkjmstlists, decimal? GajiKawalan, string GLGajiKawalan, string GLKeteranganGajiKawalan, bool DeductionStatus)
        {
            List<tbl_SAPPostDataDetails> tbl_SAPPostDataDetails = new List<tbl_SAPPostDataDetails>();
            int i = 1;
            decimal? Amount = 0;
            string DescActvt = "";

            Amount = GajiKawalan;
            if (DeductionStatus)
            {
                var GLNo = GLGajiKawalan;
                DescActvt = GLKeteranganGajiKawalan;
                tbl_SAPPostDataDetails.Add(new tbl_SAPPostDataDetails() { fld_Amount = Amount, fld_Currency = "RM", fld_Desc = DescActvt, fld_GL = GLNo, fld_Item = null, fld_ItemNo = i, fld_Purpose = "3", fld_SAPActivityCode = null, fld_SAPPostRefID = SAPPostID1 });
                i++;

                var GetNotWorkActs = db.tbl_CustomerVendorGLMap.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_TypeCode == "CS").Select(s => s.fld_KodAktiviti).ToList();
                foreach (var Pkjmstlist in Pkjmstlists)
                {
                    var GetInsentifData = db2.tbl_Insentif.Where(x => x.fld_Month == Month && x.fld_Year == Year && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == Pkjmstlist.fld_Nopkj && x.fld_Deleted == false).ToList();
                    if (GetInsentifData.Count() > 0)
                    {
                        var GetInsemtifKod = GetInsentifData.Select(s => s.fld_KodInsentif).ToArray();
                        var GetKodActInsentif = db.tbl_JenisInsentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && GetInsemtifKod.Contains(x.fld_KodInsentif) && GetNotWorkActs.Contains(x.fld_KodAktvt)).Select(s => s.fld_KodInsentif).ToList();
                        Amount = GetInsentifData.Where(x => GetKodActInsentif.Contains(x.fld_KodInsentif)).Sum(s => s.fld_NilaiInsentif);
                        if (Amount != 0)
                        {
                            var GetPkjDetail = db2.tbl_Pkjmast.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == Pkjmstlist.fld_Nopkj).FirstOrDefault();
                            DescActvt = GetPkjDetail.fld_Nama;
                            var GLNo2 = GetPkjDetail.fld_KodSAPPekerja;
                            tbl_SAPPostDataDetails.Add(new tbl_SAPPostDataDetails() { fld_Amount = -Amount, fld_Currency = "RM", fld_Desc = DescActvt, fld_GL = null, fld_Item = GLNo2, fld_ItemNo = i, fld_Purpose = "3", fld_SAPActivityCode = null, fld_SAPPostRefID = SAPPostID1 });
                            i++;
                        }
                    }
                }

                db2.tbl_SAPPostDataDetails.AddRange(tbl_SAPPostDataDetails);
                db2.SaveChanges();
            }
        }

        public static DateTime GetLastDayOfMonth(int? Month, int? Year)
        {
            return new DateTime(Year.Value, Month.Value, DateTime.DaysInMonth(Year.Value, Month.Value));
        }
    }
}
