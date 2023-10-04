using SalaryGeneratorServices.ModelsEstate;
using SalaryGeneratorServices.ModelsHQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryGeneratorServices.FuncClass
{
    class Step2Func
    {
        private LogFunc LogFunc2 = new LogFunc();
        public bool GetWorkingPaidLeaveFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, DateTime tarikh, List<tbl_CutiKategori> CutiKategoriList, out byte? PaidPeriod, out tbl_Kerjahdr WorkingAtt, out string KumCode, List<tbl_Kerjahdr> tbl_Kerjahdr)
        {
            //GenSalaryModelHQ db = new GenSalaryModelHQ();
            //GetConnectFunc conn = new GetConnectFunc();
            //string host, catalog, user, pass = "";
            bool PaidLeave = false;
            //conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            //GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            var PaidLeaveCode = CutiKategoriList.Select(s => s.fld_KodCuti).ToList();
            var WorkingData = tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Tarikh == tarikh && PaidLeaveCode.Contains(x.fld_Kdhdct)).FirstOrDefault();
            WorkingAtt = WorkingData;

            if (WorkingData != null)
            {
                KumCode = WorkingData.fld_Kum;
                PaidLeave = true;
                PaidPeriod = CutiKategoriList.Where(x => x.fld_KodCuti == WorkingData.fld_Kdhdct).Select(s => s.fld_WaktuBayaranCuti).FirstOrDefault();
            }
            else
            {
                KumCode = "";
                PaidLeave = false;
                PaidPeriod = 0;
            }

            //db.Dispose();
            //db2.Dispose();
            return PaidLeave;
        }

        public void GetDailyBonusFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, DateTime tarikh, out tbl_KerjaBonus KerjaBonus, List<tbl_JenisAktiviti> JenisAktiviti, List<tbl_Kerja> tbl_Kerja)
        {
            //GenSalaryModelHQ db = new GenSalaryModelHQ();
            //GetConnectFunc conn = new GetConnectFunc();

            tbl_KerjaBonus KerjaBonusData = new tbl_KerjaBonus();

            //string host, catalog, user, pass = "";
            //decimal? BonusRate = 1;
            decimal? BonusPay = 0;
            //conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            //GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            //BonusRate = db.tbl_HargaSawitSemasa.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Bulan == Month && x.fld_Tahun == Year).Select(s => s.fld_Insentif).FirstOrDefault();

            var JenisAktvkod = JenisAktiviti.Select(s => s.fld_KodJnsAktvt).ToList();

            var KerjaData = tbl_Kerja.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh == tarikh && x.fld_Nopkj == NoPkj).ToList();

            if (KerjaData.Count != 0)
            {
                var KerjaData2 = KerjaData.OrderByDescending(o => o.fld_DailyIncentive).FirstOrDefault();
                if (KerjaData2 != null)
                {
                    if (KerjaData2.fld_DailyIncentive > 0)
                    {
                        BonusPay = KerjaData2.fld_DailyIncentive;

                        KerjaBonusData.fld_KerjaID = KerjaData2.fld_ID;
                        KerjaBonusData.fld_Bonus = 100;
                        KerjaBonusData.fld_Kadar = BonusPay;
                        KerjaBonusData.fld_Jumlah = BonusPay;
                        KerjaBonusData.fld_Kum = KerjaData2.fld_Kum;
                        KerjaBonusData.fld_LadangID = LadangID;
                        KerjaBonusData.fld_WilayahID = WilayahID;
                        KerjaBonusData.fld_SyarikatID = SyarikatID;
                        KerjaBonusData.fld_NegaraID = NegaraID;
                        KerjaBonusData.fld_Nopkj = NoPkj;
                        KerjaBonusData.fld_Tarikh = tarikh;
                        KerjaBonusData.fld_CreatedBy = UserID;
                        KerjaBonusData.fld_CreatedDT = DTProcess;
                        KerjaBonus = KerjaBonusData;
                    }
                    else
                    {
                        KerjaBonus = null;
                    }
                }
                else
                {
                    KerjaBonus = null;
                }
                
            }
            else
            {
                KerjaBonus = null;
            }
            //db.Dispose();
            //db2.Dispose();
        }

        public async Task AddTo_tbl_KerjaBonus(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, List<tbl_KerjaBonus> KerjaBonus)
        {
            GetConnectFunc conn = new GetConnectFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            db2.tbl_KerjaBonus.AddRange(KerjaBonus);
            await db2.SaveChangesAsync();
            db2.Dispose();
        }

        //commented by faeza 13.02.2023 - original code
        //public void GetOTFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, DateTime tarikh, out tbl_KerjaOT KerjaOT, string AttCode, List<tblOptionConfigsWeb> OTCulData, List<tblOptionConfigsWeb> OTKadar, List<tbl_PkjIncrmntSalary> tbl_PkjIncrmntSalary, List<tbl_Kerja> tbl_Kerja)
        //{
        //    //GenSalaryModelHQ db = new GenSalaryModelHQ();
        //    //GetConnectFunc conn = new GetConnectFunc();

        //    tbl_KerjaOT KerjaOTData = new tbl_KerjaOT();

        //    string host, catalog, user, pass = "";
        //    decimal? firstNo = 0;
        //    decimal? secondNo = 0;
        //    decimal? thirdNo = 0;
        //    decimal? OTRate = 0;
        //    decimal? OTPay = 0;
        //    decimal? AfterRounded = 0;

        //    //conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
        //    //GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

        //    //var OTCulData = db.tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "kadarot" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).ToList();
        //    //var OTKadar = db.tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "kiraot" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).ToList();

        //    firstNo = decimal.Parse(OTCulData.Where(x => x.fldOptConfFlag2 == "1").Select(s => s.fldOptConfValue).FirstOrDefault());
        //    secondNo = decimal.Parse(OTCulData.Where(x => x.fldOptConfFlag2 == "2").Select(s => s.fldOptConfValue).FirstOrDefault());
        //    thirdNo = decimal.Parse(OTKadar.Where(x => x.fldOptConfFlag2 == AttCode).Select(s => s.fldOptConfValue).FirstOrDefault());

        //    decimal? SalaryIncrement = tbl_PkjIncrmntSalary.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_AppStatus == true).Select(s => s.fld_IncrmntSalary).FirstOrDefault();

        //    if (SalaryIncrement != null)
        //    {
        //        firstNo = firstNo + SalaryIncrement;
        //    }

        //    AfterRounded = (firstNo / secondNo) * thirdNo;
        //    OTRate = Math.Round(decimal.Parse(AfterRounded.ToString()), 2);

        //    var KerjaData = tbl_Kerja.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh == tarikh && x.fld_Nopkj == NoPkj).ToList();

        //    if (KerjaData.Count != 0)
        //    {
        //        var KerjaData2 = KerjaData.OrderByDescending(o => o.fld_JamOT).First();
        //        if (KerjaData2.fld_JamOT != 0)
        //        {
        //            OTPay = KerjaData2.fld_JamOT * Math.Round(decimal.Parse(OTRate.ToString()), 2);

        //            KerjaOTData.fld_KerjaID = KerjaData2.fld_ID;
        //            KerjaOTData.fld_JamOT = KerjaData2.fld_JamOT;
        //            KerjaOTData.fld_Kadar = Math.Round(decimal.Parse(OTRate.ToString()), 2);
        //            KerjaOTData.fld_Jumlah = Math.Round(decimal.Parse(OTPay.ToString()), 2);
        //            KerjaOTData.fld_Kum = KerjaData2.fld_Kum;
        //            KerjaOTData.fld_LadangID = LadangID;
        //            KerjaOTData.fld_WilayahID = WilayahID;
        //            KerjaOTData.fld_SyarikatID = SyarikatID;
        //            KerjaOTData.fld_NegaraID = NegaraID;
        //            KerjaOTData.fld_Nopkj = NoPkj;
        //            KerjaOTData.fld_Tarikh = tarikh;
        //            KerjaOTData.fld_CreatedBy = UserID;
        //            KerjaOTData.fld_CreatedDT = DTProcess;
        //            KerjaOT = KerjaOTData;
        //        }
        //        else
        //        {
        //            KerjaOT = null;
        //        }
        //    }
        //    else
        //    {
        //        KerjaOT = null;
        //    }
        //    //db.Dispose();
        //    //db2.Dispose();
        //}

        public void GetOTFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, DateTime tarikh, out tbl_KerjaOT KerjaOT, string AttCode, List<tblOptionConfigsWeb> OTCulData, List<tblOptionConfigsWeb> OTKadar, List<tbl_PkjIncrmntSalary> tbl_PkjIncrmntSalary, List<tbl_Kerja> tbl_Kerja)
        {
            tbl_KerjaOT KerjaOTData = new tbl_KerjaOT();

            string host, catalog, user, pass = "";
            GetConnectFunc conn = new GetConnectFunc();
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            //decimal? firstNo = 0;
            decimal? secondNo = 0;
            decimal? thirdNo = 0;
            decimal? OTRate = 0;
            decimal? OTPay = 0;
            decimal? AfterRounded = 0;
            decimal? GetMinPay = 0;

            //added by faeza 13.02.2023
            int YearC = int.Parse(Year.ToString());
            int MonthC = int.Parse(Month.ToString());
            DateTime SelectDateOri = new DateTime(YearC, MonthC, 15);
            DateTime LastSelectMonthDate = SelectDateOri.AddMonths(-1);

            //var tbl_GajiBulanan = db2.tbl_GajiBulanan.Where(x => x.fld_Nopkj == NoPkj && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == LastSelectMonthDate.Month && x.fld_Year == LastSelectMonthDate.Year && x.fld_Nopkj == NoPkj).FirstOrDefault();

            //firstNo = decimal.Parse(OTCulData.Where(x => x.fldOptConfFlag2 == "1").Select(s => s.fldOptConfValue).FirstOrDefault());//commented by faeza 13.02.2023
            GetMinPay = decimal.Parse(OTCulData.Where(x => x.fldOptConfFlag2 == "1").Select(s => s.fldOptConfValue).FirstOrDefault());//added by faeza 13.02.2023
            var GetLastMonthAverageSalary = db2.tbl_GajiBulanan.Where(x => x.fld_Nopkj == NoPkj && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == LastSelectMonthDate.Month && x.fld_Year == LastSelectMonthDate.Year && x.fld_Nopkj == NoPkj).Select(s => s.fld_PurataGaji).FirstOrDefault(); //added by faeza 13.02.2023
            secondNo = decimal.Parse(OTCulData.Where(x => x.fldOptConfFlag2 == "2").Select(s => s.fldOptConfValue).FirstOrDefault());
            thirdNo = decimal.Parse(OTKadar.Where(x => x.fldOptConfFlag2 == AttCode).Select(s => s.fldOptConfValue).FirstOrDefault());

            decimal? SalaryIncrement = tbl_PkjIncrmntSalary.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_AppStatus == true).Select(s => s.fld_IncrmntSalary).FirstOrDefault();

            if (GetLastMonthAverageSalary != null || GetLastMonthAverageSalary == 0)
            {
                if (GetLastMonthAverageSalary < GetMinPay)
                {
                    GetLastMonthAverageSalary = GetMinPay;
                    if (SalaryIncrement != null)
                    {
                        GetLastMonthAverageSalary = GetLastMonthAverageSalary + SalaryIncrement;
                    }
                }
                else
                {
                    if (SalaryIncrement != null)
                    {
                        GetLastMonthAverageSalary = GetLastMonthAverageSalary + SalaryIncrement;
                    }
                }
            }
            else
            {
                GetLastMonthAverageSalary = GetMinPay;
                if (SalaryIncrement != null)
                {
                    GetLastMonthAverageSalary = GetLastMonthAverageSalary + SalaryIncrement;
                }
            }

            AfterRounded = (GetLastMonthAverageSalary / secondNo) * thirdNo;
            OTRate = Math.Round(decimal.Parse(AfterRounded.ToString()), 2);

            var KerjaData = tbl_Kerja.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh == tarikh && x.fld_Nopkj == NoPkj).ToList();

            if (KerjaData.Count != 0)
            {
                var KerjaData2 = KerjaData.OrderByDescending(o => o.fld_JamOT).First();
                if (KerjaData2.fld_JamOT != 0)
                {
                    OTPay = KerjaData2.fld_JamOT * Math.Round(decimal.Parse(OTRate.ToString()), 2);

                    KerjaOTData.fld_KerjaID = KerjaData2.fld_ID;
                    KerjaOTData.fld_JamOT = KerjaData2.fld_JamOT;
                    KerjaOTData.fld_Kadar = Math.Round(decimal.Parse(OTRate.ToString()), 2);
                    KerjaOTData.fld_Jumlah = Math.Round(decimal.Parse(OTPay.ToString()), 2);
                    KerjaOTData.fld_Kum = KerjaData2.fld_Kum;
                    KerjaOTData.fld_LadangID = LadangID;
                    KerjaOTData.fld_WilayahID = WilayahID;
                    KerjaOTData.fld_SyarikatID = SyarikatID;
                    KerjaOTData.fld_NegaraID = NegaraID;
                    KerjaOTData.fld_Nopkj = NoPkj;
                    KerjaOTData.fld_Tarikh = tarikh;
                    KerjaOTData.fld_CreatedBy = UserID;
                    KerjaOTData.fld_CreatedDT = DTProcess;
                    KerjaOT = KerjaOTData;
                }
                else
                {
                    KerjaOT = null;
                }
            }
            else
            {
                KerjaOT = null;
            }
            //db.Dispose();
            db2.Dispose();
        }

        //added by faeza 26.02.2023 - special insentif for 2nd payslip
        public void GetSpecialInsentifFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, out tbl_SpecialInsentif SpecialInsentif, List<tbl_JenisInsentif> tbl_JenisInsentifSpecial, List<tbl_Insentif> tbl_Insentif)
        {
            string host, catalog, user, pass = "";
            GetConnectFunc conn = new GetConnectFunc();
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            tbl_SpecialInsentif SpecialInsentifData = new tbl_SpecialInsentif();

            var KodInsentifSpecial = tbl_JenisInsentifSpecial.Select(s => s.fld_KodInsentif).ToList();
            var SpecialInsentifList = tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Month == Month && x.fld_Year == Year && x.fld_Deleted == false && KodInsentifSpecial.Contains(x.fld_KodInsentif)).ToList();

            if (SpecialInsentifList.Count != 0)
            {
                var SpecialInsentifList2 = SpecialInsentifList.First();
                SpecialInsentifData.fld_Nopkj = SpecialInsentifList2.fld_Nopkj;
                SpecialInsentifData.fld_KodInsentif = SpecialInsentifList2.fld_KodInsentif;
                SpecialInsentifData.fld_NilaiInsentif = SpecialInsentifList2.fld_NilaiInsentif;
                SpecialInsentifData.fld_Year = SpecialInsentifList2.fld_Year;
                SpecialInsentifData.fld_Month = SpecialInsentifList2.fld_Month;
                SpecialInsentifData.fld_NegaraID = NegaraID;
                SpecialInsentifData.fld_SyarikatID = SyarikatID;
                SpecialInsentifData.fld_WilayahID = WilayahID;
                SpecialInsentifData.fld_LadangID = LadangID;
                SpecialInsentifData.fld_Deleted = SpecialInsentifList2.fld_Deleted;
                SpecialInsentifData.fld_CreatedBy = UserID;
                SpecialInsentifData.fld_CreatedDT = DTProcess;
                SpecialInsentifData.fld_InsentifID = SpecialInsentifList2.fld_InsentifID;
                SpecialInsentif = SpecialInsentifData;
            }
            else
            {
                SpecialInsentif = null;
            }
            //db.Dispose();
            db2.Dispose();
        }

        public async Task AddTo_tbl_KerjaOT(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, List<tbl_KerjaOT> KerjaOT)
        {
            GetConnectFunc conn = new GetConnectFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            db2.tbl_KerjaOT.AddRange(KerjaOT);
            await db2.SaveChangesAsync();
            db2.Dispose();
        }

        public async Task AddTo_tbl_KerjahdrCuti(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, List<tbl_KerjahdrCuti> KerjahdrCuti)
        {
            GetConnectFunc conn = new GetConnectFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            db2.tbl_KerjahdrCuti.AddRange(KerjahdrCuti);
            await db2.SaveChangesAsync();
            db2.Dispose();
        }

        public bool GetAttendStatusFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, DateTime tarikh, out string AttCode, List<tblOptionConfigsWeb> tblOptionConfigsWeb, List<tbl_Kerjahdr> tbl_Kerjahdr, out byte? PaidPeriod, out tbl_Kerjahdr WorkingAtt, out string KumCode)
        {
            bool AttendStatus = false;

            var Attandance = tblOptionConfigsWeb.Where(x => x.fldOptConfFlag1 == "cuti" && x.fldOptConfFlag2 == "hadirkerja" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).ToList();
            var checkattendance = tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Tarikh == tarikh).ToList();
            var checkattendancestatus = checkattendance.Where(x => Attandance.Contains(x.fld_Kdhdct)).FirstOrDefault();
            WorkingAtt = checkattendancestatus;
            if (checkattendancestatus != null)
            {
                AttendStatus = true;
                AttCode = checkattendancestatus.fld_Kdhdct;
                PaidPeriod = 1;
                KumCode = checkattendancestatus.fld_Kum;
            }
            else
            {
                AttCode = checkattendance.Select(s => s.fld_Kdhdct).Take(1).FirstOrDefault();
                PaidPeriod = 0;
                KumCode = "";
            }

            return AttendStatus;
        }

        //added by faeza 26.02.2023
        public async Task AddTo_tbl_SpecialInsentif(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, List<tbl_SpecialInsentif> SpecialInsentif)
        {            
            GetConnectFunc conn = new GetConnectFunc();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            db2.tbl_SpecialInsentif.AddRange(SpecialInsentif);
            await db2.SaveChangesAsync();
            db2.Dispose();          
        }

        //comment by faeza 26.09.2022 - original code
        //public bool GetAttendStatusFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, DateTime tarikh, out string AttCode, List<tblOptionConfigsWeb> tblOptionConfigsWeb, List<tbl_Kerjahdr> tbl_Kerjahdr)
        //{
        //    //GenSalaryModelHQ db = new GenSalaryModelHQ();
        //    //GetConnectFunc conn = new GetConnectFunc();

        //    bool AttendStatus = false;

        //    //string host, catalog, user, pass = "";
        //    //conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
        //    //GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

        //    var Attandance = tblOptionConfigsWeb.Where(x => x.fldOptConfFlag1 == "cuti" && x.fldOptConfFlag2 == "hadirkerja" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).ToList();
        //    var checkattendance = tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Tarikh == tarikh).ToList();
        //    var checkattendancestatus = checkattendance.Where(x => Attandance.Contains(x.fld_Kdhdct)).FirstOrDefault();
        //    if (checkattendancestatus != null)
        //    {
        //        AttendStatus = true;
        //        AttCode = checkattendancestatus.fld_Kdhdct;
        //    }
        //    else
        //    {
        //        AttCode = checkattendance.Select(s => s.fld_Kdhdct).Take(1).FirstOrDefault();
        //    }

        //    //db.Dispose();
        //    //db2.Dispose();

        //    return AttendStatus;
        //}
    }
}
