using SalaryGeneratorServices.CustomModels;
using SalaryGeneratorServices.ModelsCustom;
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
    class Step3Func
    {
        //add by Shah 01.01.2024
        public List<tbl_Pkjmast> tbl_Pkjmasts;

        public void GetPkjMastsData(List<tbl_Pkjmast> Pkjmasts)
        {
            tbl_Pkjmasts = Pkjmasts;
        }
        //add by Shah 01.01.2024

        public async Task<CustMod_PaidWorking> GetPaidWorkingFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, List<tbl_Kerja> tbl_Kerja)
        {
            GetConnectFunc conn = new GetConnectFunc();
            Guid MonthSalaryID = new Guid();
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            decimal? WorkingPayment = 0m;
            decimal? DiffAreaPayment = 0m;
            var DataKerja = db2.tbl_Kerja.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_Nopkj == NoPkj).ToList();//.Sum(s => s.fld_Amount);
            var PaidWorking = DataKerja.Sum(s => s.fld_OverallAmount);
            if (PaidWorking == null)
            {
                PaidWorking = 0;
            }
            WorkingPayment = PaidWorking;
            MonthSalaryID = await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 1, PaidWorking, DTProcess, UserID, GajiBulanan);

            var PaidDifficultArea = DataKerja.Sum(s => s.fld_HrgaKwsnSkar);
            if (PaidDifficultArea == null)
            {
                PaidDifficultArea = 0;
            }
            DiffAreaPayment = PaidDifficultArea;

            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 19, PaidDifficultArea, DTProcess, UserID, GajiBulanan);

            var CustMod_PaidWorking = new CustMod_PaidWorking
            {
                SalaryID = MonthSalaryID,
                WorkingPayment = WorkingPayment,
                DiffAreaPayment = DiffAreaPayment
            };
            db2.Dispose();

            return CustMod_PaidWorking;
        }

        //add by faeza 10.11.2020
        public async Task<decimal?> GetPaidWorkingORPFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, Guid Guid, List<tblOptionConfigsWeb> tblOptionConfigsWebs, List<tbl_Kerja> tbl_Kerja, List<tbl_Kerjahdr> tbl_Kerjahdr)
        {
            GetConnectFunc conn = new GetConnectFunc();
            //Guid MonthSalaryID = new Guid();
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            decimal? WorkingPaymentORP = 0m;
            decimal? SingleWorkingPaymentORP = 0m;

            //attendance = H01
            var Attandance = tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "cuti" && x.fldOptConfFlag2 == "hadirkerja" && x.fldOptConfValue == "H01" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).ToList();
            //var DateWorkingPresentDay = tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_Nopkj == NoPkj && Attandance.Contains(x.fld_Kdhdct)).ToList();//.Select(s => s.fld_Tarikh).ToList();
            var DateWorkingPresentDay = tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Year == Year && x.fld_Tarikh.Value.Month == Month && x.fld_Nopkj == NoPkj && Attandance.Contains(x.fld_Kdhdct)).OrderBy(o => o.fld_Tarikh).Select(s => s.fld_Tarikh).ToList();

            //data kerja
            if (DateWorkingPresentDay.Count() != 0)
            {
                foreach (var DateWorkingPresentDays in DateWorkingPresentDay)
                {
                    var DataKerja = tbl_Kerja.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Day == DateWorkingPresentDays.Value.Day && x.fld_Tarikh.Value.Month == DateWorkingPresentDays.Value.Month && x.fld_Tarikh.Value.Year == DateWorkingPresentDays.Value.Year && x.fld_Nopkj == NoPkj).ToList();
                    SingleWorkingPaymentORP = DataKerja.Sum(s => s.fld_OverallAmount);

                    if (SingleWorkingPaymentORP == null)
                    {
                        SingleWorkingPaymentORP = 0;
                    }
                    WorkingPaymentORP = WorkingPaymentORP + SingleWorkingPaymentORP;
                }
            }
            else
            {
                WorkingPaymentORP = 0;
            }

            GajiBulanan = db2.tbl_GajiBulanan.Find(Guid);
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 22, WorkingPaymentORP, DTProcess, UserID, GajiBulanan);

            db2.Dispose();
            return WorkingPaymentORP;
        }

        public async Task<decimal?> GetAveragePaidFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, Guid Guid, List<tblOptionConfigsWeb> tblOptionConfigsWebs, List<tbl_Kerjahdr> tbl_Kerjahdr, List<tbl_Kerjahdr> tbl_KerjahdrYearly)
        {
            GetConnectFunc conn = new GetConnectFunc();
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            decimal? AverageSalary = 0;
            decimal? AveragePaid = 0;
            decimal? TotalSalary = 0;
            decimal? TotalSalaryBeforeORP = 0;
            decimal? TotalSalaryAfterORP = 0;
            int YearC = int.Parse(Year.ToString());
            int MonthC = int.Parse(Month.ToString());
            DateTime EndSelectDateOri = new DateTime(YearC, MonthC, 15);
            DateTime EndSelectDate = EndSelectDateOri.AddMonths(-1);
            DateTime StartSelectDate = EndSelectDate.AddMonths(-6);
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            GajiBulanan = db2.tbl_GajiBulanan.Find(Guid);

            //modified by faeza 10.11.2020 - implement full ORP 

            //attendance - select H01
            var Attandance = tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "cuti" && x.fldOptConfFlag2 == "hadirkerja" && x.fldOptConfValue == "H01" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).ToList();
            var TotalWorkingDay = tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_Nopkj == NoPkj && Attandance.Contains(x.fld_Kdhdct)).Count();

            //total payment ORP
            var TotalPaymentORP = GajiBulanan.fld_ByrKerjaORP + GajiBulanan.fld_BonusHarianORP + GajiBulanan.fld_LainInsentifORP;
            if (TotalPaymentORP > 0 && TotalWorkingDay > 0)
            {
                AveragePaid = TotalPaymentORP / TotalWorkingDay;
            }
            else
            {
                AveragePaid = 0;
                TotalPaymentORP = 0;
            }

            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 6, Math.Round(decimal.Parse(AveragePaid.ToString()), 2), DTProcess, UserID, GajiBulanan);
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 25, Math.Round(decimal.Parse(TotalPaymentORP.ToString()), 2), DTProcess, UserID, GajiBulanan);

            //kiraan average yearly - terbahagi 2 kiraan sebab kiraan ORP bermula bulan 11/2020. kiraan bayaran kerja ORP tersimpan dalam column baru tbl_TotalByrKerjaORP bermula 11/2020
            //var GetAverageSalary = db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && (x.fld_Year == StartSelectDate.Year && x.fld_Month >= StartSelectDate.Month) && (x.fld_Year == EndSelectDate.Year && x.fld_Month <= EndSelectDate.Month)).ToList();
            var TotalAtt = tbl_KerjahdrYearly.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Year == Year && x.fld_Nopkj == NoPkj && Attandance.Contains(x.fld_Kdhdct)).Count();
            if (Year == 2020)
            {
                //if (Month >= 1 && Month <= 10)
                //{
                TotalSalaryBeforeORP = db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && (x.fld_Month >= 1 && x.fld_Month <= 10) && x.fld_Year == Year).Sum(s => s.fld_ByrKerja);
                //}
                //else
                //{
                TotalSalaryAfterORP = db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && (x.fld_Month >= 11 && x.fld_Month <= 12) && x.fld_Year == Year).Sum(s => s.fld_TotalByrKerjaORP);

                //}
                TotalSalary = TotalSalaryBeforeORP + TotalSalaryAfterORP;
            }
            else
            {
                TotalSalary = db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && (x.fld_Month >= 1 && x.fld_Month <= Month) && x.fld_Year == Year).Sum(s => s.fld_TotalByrKerjaORP);
            }

            if (TotalSalary > 0 && TotalAtt > 0)
            {
                AverageSalary = TotalSalary / TotalAtt;
            }
            else
            {
                AverageSalary = 0;
            }
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 18, Math.Round(decimal.Parse(AverageSalary.ToString()), 2), DTProcess, UserID, GajiBulanan);


            //*********************************************************************************************************************
            //original code

            ////modified by faeza on 24.09.2020 - implement ORP - select H01
            //var Attandance = tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "cuti" && x.fldOptConfFlag2 == "hadirkerja" && x.fldOptConfValue == "H01" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).ToList();

            //var TotalWorkingDay = tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_Nopkj == NoPkj && Attandance.Contains(x.fld_Kdhdct)).Count();
            //if (GajiBulanan.fld_ByrKerja > 0 && TotalWorkingDay > 0)
            //{
            //    AveragePaid = GajiBulanan.fld_ByrKerja / TotalWorkingDay;
            //}
            //else
            //{
            //    AveragePaid = 0;
            //}
            //await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 6, Math.Round(decimal.Parse(AveragePaid.ToString()), 2), DTProcess, UserID, GajiBulanan);

            ////var GetAverageSalary = db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && (x.fld_Year == StartSelectDate.Year && x.fld_Month >= StartSelectDate.Month) && (x.fld_Year == EndSelectDate.Year && x.fld_Month <= EndSelectDate.Month)).ToList();
            //var TotalSalary = db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && (x.fld_Month >= 1 && x.fld_Month <= Month) && x.fld_Year == Year).Sum(s => s.fld_ByrKerja);
            //var TotalAtt = tbl_KerjahdrYearly.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Year == Year && x.fld_Nopkj == NoPkj && Attandance.Contains(x.fld_Kdhdct)).Count();

            //if (TotalSalary > 0 && TotalAtt > 0)
            //{
            //    AverageSalary = TotalSalary / TotalAtt;
            //}
            //else
            //{
            //    AverageSalary = 0;
            //}
            //await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 18, Math.Round(decimal.Parse(AverageSalary.ToString()), 2), DTProcess, UserID, GajiBulanan);

            db.Dispose();
            db2.Dispose();
            return Math.Round(decimal.Parse(AveragePaid.ToString()), 2);
        }

        //modify by Faeza on 25/2/2020
        //calc average salary rate - ORP
        public async Task<decimal?> GetAveragePaidORPFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, Guid Guid, List<tblOptionConfigsWeb> tblOptionConfigsWebs, List<tbl_Kerjahdr> tbl_Kerjahdr)
        {
            GetConnectFunc conn = new GetConnectFunc();
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            decimal? AverageSalary = 0;
            decimal? AveragePaid = 0;
            int YearC = int.Parse(Year.ToString());
            int MonthC = int.Parse(Month.ToString());
            DateTime EndSelectDateOri = new DateTime(YearC, MonthC, 15);
            DateTime EndSelectDate = EndSelectDateOri.AddMonths(-1);
            DateTime StartSelectDate = EndSelectDate.AddMonths(-6);
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            GajiBulanan = db2.tbl_GajiBulanan.Find(Guid);

            var TotalPayment = GajiBulanan.fld_ByrKerja + GajiBulanan.fld_BonusHarian + GajiBulanan.fld_LainInsentif;
            var Attandance = tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "cuti" && x.fldOptConfFlag2 == "hadirkerja" && x.fldOptConfValue == "H01" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).ToList();
            var TotalWorkingDay = tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_Nopkj == NoPkj && Attandance.Contains(x.fld_Kdhdct)).Count();

            //if (GajiBulanan.fld_ByrKerja > 0) //*commented by faeza on 30.07.20
            if (TotalPayment > 0 && TotalWorkingDay > 0)  //*added by faeza on 30.07.20
            {
                //AveragePaid = GajiBulanan.fld_ByrKerja / TotalWorkingDay;  //*commented by faeza on 30.07.20
                AveragePaid = TotalPayment / TotalWorkingDay;   //*added by faeza on 30.07.20
            }
            else
            {
                AveragePaid = 0;
            }
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 20, Math.Round(decimal.Parse(AveragePaid.ToString()), 2), DTProcess, UserID, GajiBulanan);

            var TotalSalary = db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && (x.fld_Month >= 1 && x.fld_Month <= Month) && x.fld_Year == Year).Sum(s => s.fld_ByrKerja + s.fld_BonusHarian + s.fld_LainInsentif);
            var TotalAtt = tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Year == Year && x.fld_Nopkj == NoPkj && Attandance.Contains(x.fld_Kdhdct)).Count();

            if (TotalSalary > 0 && TotalAtt > 0)
            {
                AverageSalary = TotalSalary / TotalAtt;
            }
            else
            {
                AverageSalary = 0;
            }

            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 21, Math.Round(decimal.Parse(AverageSalary.ToString()), 2), DTProcess, UserID, GajiBulanan);
            db.Dispose();
            db2.Dispose();
            return Math.Round(decimal.Parse(AveragePaid.ToString()), 2);
        }

        public async Task<decimal?> GetPaidDailyBonusFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, Guid Guid)
        {
            GetConnectFunc conn = new GetConnectFunc();
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            var PaidDailyBonus = await db2.tbl_KerjaBonus.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_Nopkj == NoPkj).SumAsync(s => s.fld_Jumlah);
            if (PaidDailyBonus == null)
            {
                PaidDailyBonus = 0;
            }
            GajiBulanan = await db2.tbl_GajiBulanan.FindAsync(Guid);
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 2, PaidDailyBonus, DTProcess, UserID, GajiBulanan);

            db2.Dispose();
            return PaidDailyBonus;
        }

        //add by faeza 10.11.2020
        public async Task<decimal?> GetPaidDailyBonusORPFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, Guid Guid, List<tblOptionConfigsWeb> tblOptionConfigsWebs, List<tbl_Kerjahdr> tbl_Kerjahdr)
        {
            GetConnectFunc conn = new GetConnectFunc();
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            decimal? SingleDailyBonusPaymentORP = 0m;
            decimal? DailyBonusPaymentORP = 0m;

            //attendance = H01
            var Attandance = tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "cuti" && x.fldOptConfFlag2 == "hadirkerja" && x.fldOptConfValue == "H01" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).ToList();
            var DateWorkingPresentDay = tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Year == Year && x.fld_Tarikh.Value.Month == Month && x.fld_Nopkj == NoPkj && Attandance.Contains(x.fld_Kdhdct)).OrderBy(o => o.fld_Tarikh).Select(s => s.fld_Tarikh).ToList();

            //data bonus harian
            if (DateWorkingPresentDay.Count() != 0)
            {
                foreach (var DateWorkingPresentDays in DateWorkingPresentDay)
                {
                    var DailyBonus = db2.tbl_KerjaBonus.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Day == DateWorkingPresentDays.Value.Day && x.fld_Tarikh.Value.Month == DateWorkingPresentDays.Value.Month && x.fld_Tarikh.Value.Year == DateWorkingPresentDays.Value.Year && x.fld_Nopkj == NoPkj).FirstOrDefault();
                    if (DailyBonus != null)
                    {
                        SingleDailyBonusPaymentORP = DailyBonus.fld_Jumlah;
                    }
                    else
                    {
                        SingleDailyBonusPaymentORP = 0;
                    }
                    DailyBonusPaymentORP = DailyBonusPaymentORP + SingleDailyBonusPaymentORP;
                }
            }
            else
            {
                DailyBonusPaymentORP = 0;
            }

            GajiBulanan = await db2.tbl_GajiBulanan.FindAsync(Guid);
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 23, DailyBonusPaymentORP, DTProcess, UserID, GajiBulanan);

            db2.Dispose();
            return DailyBonusPaymentORP;
        }

        public async Task<decimal?> GetPaidOTFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, Guid Guid)
        {
            GetConnectFunc conn = new GetConnectFunc();
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            var PaidOT = db2.tbl_KerjaOT.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_Nopkj == NoPkj).Sum(s => s.fld_Jumlah);
            if (PaidOT == null)
            {
                PaidOT = 0;
            }
            GajiBulanan = db2.tbl_GajiBulanan.Find(Guid);
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 3, PaidOT, DTProcess, UserID, GajiBulanan);

            db2.Dispose();
            return PaidOT;
        }

        public async Task<decimal?> GetPaidInsentifFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, Guid Guid, List<tbl_JenisInsentif> tbl_JenisInsentif, List<tbl_Insentif> tbl_Insentif, List<tbl_Kerjahdr> tbl_Kerjahdr)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            decimal? TotalGetInsentif = 0;

            var InsCd = tbl_JenisInsentif.Where(x => x.fld_JenisInsentif == "P" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false).Select(s => new { s.fld_KodInsentif, s.fld_TetapanNilai, s.fld_DailyFixedValue }).ToList();
            var InsCdForDailyValue = InsCd.Where(x => x.fld_TetapanNilai == 2).ToList();
            var InsCdForOtherValue = InsCd.Where(x => x.fld_TetapanNilai != 2).ToList();
            var PaidInsentif = tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && x.fld_Nopkj == NoPkj && x.fld_Deleted == false).ToList();
            var PaidInsentifOthers = PaidInsentif.Where(x => InsCdForOtherValue.Select(s => s.fld_KodInsentif).Contains(x.fld_KodInsentif)).Sum(s => s.fld_NilaiInsentif);

            if (PaidInsentifOthers == null)
            {
                PaidInsentifOthers = 0;
            }

            decimal? PeruntukkanInsentif = 0;
            decimal? TerimaInsentif = 0;
            decimal? DeductPerDay = 0;
            decimal? TotalDeduct = 0;
            decimal? TotalTerimaInsentif = 0;
            if (InsCdForDailyValue != null)
            {
                var PaidInsentifDaily = PaidInsentif.Where(x => InsCdForDailyValue.Select(s => s.fld_KodInsentif).Contains(x.fld_KodInsentif)).ToList();
                if (PaidInsentifDaily != null)
                {
                    foreach (var PaidInsentifDay in PaidInsentifDaily)
                    {
                        var PaidInsentifForDailyDec = PaidInsentif.Where(x => x.fld_KodInsentif == PaidInsentifDay.fld_KodInsentif).FirstOrDefault();
                        PeruntukkanInsentif = PaidInsentifForDailyDec.fld_NilaiInsentif;
                        DeductPerDay = InsCdForDailyValue.Where(x => x.fld_KodInsentif == PaidInsentifDay.fld_KodInsentif).Select(s => s.fld_DailyFixedValue).FirstOrDefault();
                        var GetTotalPonteng = tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_Kdhdct == "P01").Count();
                        if (GetTotalPonteng > 0)
                        {
                            TotalDeduct = DeductPerDay * GetTotalPonteng;
                            TerimaInsentif = PeruntukkanInsentif - TotalDeduct;
                            PaidInsentifForDailyDec.fld_NilaiInsentif = TerimaInsentif;
                            db2.Entry(PaidInsentifForDailyDec).State = EntityState.Modified;
                            db2.SaveChanges();
                        }
                        else
                        {
                            TerimaInsentif = PeruntukkanInsentif;
                        }
                        TotalTerimaInsentif = TotalTerimaInsentif + TerimaInsentif;
                        TerimaInsentif = 0;
                    }
                }
            }

            TotalGetInsentif = PaidInsentifOthers + TotalTerimaInsentif;

            GajiBulanan = db2.tbl_GajiBulanan.Find(Guid);
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 4, TotalGetInsentif, DTProcess, UserID, GajiBulanan);

            db.Dispose();
            db2.Dispose();
            return TotalGetInsentif;
        }

        //add by faeza 10.11.2020
        public async Task<decimal?> GetPaidInsentifORPFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, Guid Guid, List<tbl_JenisInsentif> tbl_JenisInsentif, List<tbl_Insentif> tbl_Insentif, List<tbl_Kerjahdr> tbl_Kerjahdr)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            decimal? TotalGetInsentif = 0;

            //original code
            //var InsCd = tbl_JenisInsentif.Where(x => x.fld_JenisInsentif == "P" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false && x.fld_KodInsentif != "P47" && x.fld_KodInsentif != "P48").Select(s => new { s.fld_KodInsentif, s.fld_TetapanNilai, s.fld_DailyFixedValue }).ToList();
            //added by faeza 26.09.2022 - implement ORP
            var InsCd = tbl_JenisInsentif.Where(x => x.fld_JenisInsentif == "P" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false && x.fld_AdaORP == true).Select(s => new { s.fld_KodInsentif, s.fld_TetapanNilai, s.fld_DailyFixedValue }).ToList();
            var InsCdForDailyValue = InsCd.Where(x => x.fld_TetapanNilai == 2).ToList();
            var InsCdForOtherValue = InsCd.Where(x => x.fld_TetapanNilai != 2).ToList();
            var PaidInsentif = tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && x.fld_Nopkj == NoPkj && x.fld_Deleted == false).ToList();
            var PaidInsentifOthers = PaidInsentif.Where(x => InsCdForOtherValue.Select(s => s.fld_KodInsentif).Contains(x.fld_KodInsentif)).Sum(s => s.fld_NilaiInsentif);

            if (PaidInsentifOthers == null)
            {
                PaidInsentifOthers = 0;
            }

            decimal? PeruntukkanInsentif = 0;
            decimal? TerimaInsentif = 0;
            decimal? DeductPerDay = 0;
            decimal? TotalDeduct = 0;
            decimal? TotalTerimaInsentif = 0;
            if (InsCdForDailyValue != null)
            {
                var PaidInsentifDaily = PaidInsentif.Where(x => InsCdForDailyValue.Select(s => s.fld_KodInsentif).Contains(x.fld_KodInsentif)).ToList();
                if (PaidInsentifDaily != null)
                {
                    foreach (var PaidInsentifDay in PaidInsentifDaily)
                    {
                        var PaidInsentifForDailyDec = PaidInsentif.Where(x => x.fld_KodInsentif == PaidInsentifDay.fld_KodInsentif).FirstOrDefault();
                        PeruntukkanInsentif = PaidInsentifForDailyDec.fld_NilaiInsentif;
                        DeductPerDay = InsCdForDailyValue.Where(x => x.fld_KodInsentif == PaidInsentifDay.fld_KodInsentif).Select(s => s.fld_DailyFixedValue).FirstOrDefault();
                        var GetTotalPonteng = tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_Kdhdct == "P01").Count();
                        if (GetTotalPonteng > 0)
                        {
                            TotalDeduct = DeductPerDay * GetTotalPonteng;
                            TerimaInsentif = PeruntukkanInsentif - TotalDeduct;
                            PaidInsentifForDailyDec.fld_NilaiInsentif = TerimaInsentif;
                            db2.Entry(PaidInsentifForDailyDec).State = EntityState.Modified;
                            db2.SaveChanges();
                        }
                        else
                        {
                            TerimaInsentif = PeruntukkanInsentif;
                        }
                        TotalTerimaInsentif = TotalTerimaInsentif + TerimaInsentif;
                        TerimaInsentif = 0;
                    }
                }
            }

            TotalGetInsentif = PaidInsentifOthers + TotalTerimaInsentif;

            GajiBulanan = db2.tbl_GajiBulanan.Find(Guid);
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 24, TotalGetInsentif, DTProcess, UserID, GajiBulanan);

            db.Dispose();
            db2.Dispose();
            return TotalGetInsentif;
        }

        public async Task<decimal?> GetDeductInsentifFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, Guid Guid, List<tbl_JenisInsentif> tbl_JenisInsentif, List<tbl_Insentif> tbl_Insentif)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            var InsCd = tbl_JenisInsentif.Where(x => x.fld_JenisInsentif == "T" && x.fld_KelayakanKepada != 9 && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false).Select(s => s.fld_KodInsentif).ToList();
            var DeductInsentif = tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && x.fld_Nopkj == NoPkj && InsCd.Contains(x.fld_KodInsentif) && x.fld_Deleted == false).Sum(s => s.fld_NilaiInsentif);
            if (DeductInsentif == null)
            {
                DeductInsentif = 0;
            }
            GajiBulanan = db2.tbl_GajiBulanan.Find(Guid);
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 5, DeductInsentif, DTProcess, UserID, GajiBulanan);

            db.Dispose();
            db2.Dispose();
            return DeductInsentif;
        }

        public async Task<decimal?> GetCompanyDeductFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, Guid Guid, decimal? TotalSalary, tbl_HutangPekerjaJumlah HutangPekerja, List<tbl_JenisInsentif> tbl_JenisInsentif, List<tbl_Insentif> tbl_Insentif, List<tblOptionConfigsWeb> tblOptionConfigsWebs)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            var InsCd = tbl_JenisInsentif.Where(x => x.fld_JenisInsentif == "T" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_Deleted == false && x.fld_KelayakanKepada == 9).Select(s => s.fld_KodInsentif).FirstOrDefault();
            var DeductInsentif = db2.tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && x.fld_Nopkj == NoPkj && x.fld_KodInsentif == InsCd && x.fld_Deleted == false).FirstOrDefault();
            if (DeductInsentif != null)
            {
                //Remove dulu data
                db2.tbl_Insentif.Remove(DeductInsentif);
                await db2.SaveChangesAsync();
            }
            decimal? GetMinSalary = decimal.Parse(tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "gajiMinima" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).FirstOrDefault());
            decimal? GetPercntDeduct = decimal.Parse(tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "percentdeduction" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).FirstOrDefault());
            decimal? TotalDeductionCompany = 0;
            decimal? DeductCompanyInsentifCollection = tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_KodInsentif == InsCd && x.fld_Deleted == false).Sum(s => s.fld_NilaiInsentif);
            DeductCompanyInsentifCollection = DeductCompanyInsentifCollection == null ? 0 : DeductCompanyInsentifCollection;
            var GetBalanceToPay = HutangPekerja.fld_JumlahHutang - DeductCompanyInsentifCollection;
            //if(NoPkj == "PI941518284")
            //{
            //    LogFunc LogFunc = new LogFunc();
            //    LogFunc.DataScTransChecking("GetBalanceToPay = " + GetBalanceToPay.ToString());
            //}
            if (GetBalanceToPay > 0)
            {
                decimal? BalanceAfterMinSalary = TotalSalary - GetMinSalary;

                if (BalanceAfterMinSalary > 0)
                {
                    decimal? AfterPercentDeduction = Math.Round(((GetPercntDeduct / 100) * TotalSalary).Value, 2);
                    if (BalanceAfterMinSalary > AfterPercentDeduction && GetBalanceToPay > AfterPercentDeduction)
                    {
                        TotalDeductionCompany = AfterPercentDeduction;
                    }
                    else
                    {
                        if (GetBalanceToPay > BalanceAfterMinSalary)
                        {
                            TotalDeductionCompany = BalanceAfterMinSalary;
                        }
                        else
                        {
                            TotalDeductionCompany = GetBalanceToPay;
                        }
                    }
                }

                if (TotalDeductionCompany > 0)
                {
                    tbl_Insentif tbl_Insentif2 = new tbl_Insentif();

                    tbl_Insentif2.fld_KodInsentif = InsCd;
                    tbl_Insentif2.fld_Month = Month;
                    tbl_Insentif2.fld_Year = Year;
                    tbl_Insentif2.fld_NilaiInsentif = TotalDeductionCompany;
                    tbl_Insentif2.fld_Nopkj = NoPkj;
                    tbl_Insentif2.fld_NegaraID = NegaraID;
                    tbl_Insentif2.fld_SyarikatID = SyarikatID;
                    tbl_Insentif2.fld_WilayahID = WilayahID;
                    tbl_Insentif2.fld_LadangID = LadangID;
                    tbl_Insentif2.fld_CreatedBy = UserID;
                    tbl_Insentif2.fld_CreatedDT = DTProcess;
                    tbl_Insentif2.fld_Deleted = false;

                    db2.tbl_Insentif.Add(tbl_Insentif2);
                    db2.SaveChanges();

                    HutangPekerja.fld_JumlahBayar = DeductCompanyInsentifCollection + TotalDeductionCompany;
                    db2.Entry(HutangPekerja).State = EntityState.Modified;
                    await db2.SaveChangesAsync();
                }

                GajiBulanan = await db2.tbl_GajiBulanan.FindAsync(Guid);

                decimal? TotalDeductInsentif = GajiBulanan.fld_LainPotongan;

                decimal? TotalAllDeductInsentif = TotalDeductInsentif + TotalDeductionCompany;

                await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 5, TotalAllDeductInsentif, DTProcess, UserID, GajiBulanan);

                db.Dispose();
                db2.Dispose();
            }

            return TotalDeductionCompany;
        }

        public async Task<decimal?> GetAIPSFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, Guid Guid, List<tbl_Produktiviti> tbl_Produktiviti, List<tblOptionConfigsWeb> tblOptionConfigsWebs, List<tbl_Kerjahdr> tbl_Kerjahdr, List<tbl_Kerja> tbl_Kerja)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            decimal? AIPSPrice = 0;
            decimal? AttInsentif = 0;
            decimal? QualityInsentif = 0;
            decimal? ProdInsentif = 0;
            int? AttTarget = 0;
            int? AttCapai = 0;
            short? QuaTarget = 0;
            short? QuaCapai = 0;
            decimal? ProdTarget = 0;
            decimal? ProdCapai = 0;


            var GetWorkerProdPlan = tbl_Produktiviti.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == Month && x.fld_Year == Year && x.fld_Nopkj == NoPkj).FirstOrDefault();

            if (GetWorkerProdPlan != null)
            {
                switch (GetWorkerProdPlan.fld_JenisPelan)
                {
                    case "A":
                        AttInsentif = GetAttInsentifFunc(db, db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, GetWorkerProdPlan, out AttTarget, out AttCapai, tblOptionConfigsWebs, tbl_Kerjahdr);
                        QualityInsentif = GetQualityInsentifFunc(db, db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, out QuaTarget, out QuaCapai, tblOptionConfigsWebs, tbl_Kerja);
                        ProdInsentif = GetProductivityInsentifFunc(db, db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, GetWorkerProdPlan, out ProdTarget, out ProdCapai, tblOptionConfigsWebs, tbl_Kerja);
                        break;
                    case "B":
                        AttInsentif = GetAttInsentifFunc(db, db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, GetWorkerProdPlan, out AttTarget, out AttCapai, tblOptionConfigsWebs, tbl_Kerjahdr);
                        QualityInsentif = 0;
                        ProdInsentif = GetProductivityInsentifFunc(db, db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, GetWorkerProdPlan, out ProdTarget, out ProdCapai, tblOptionConfigsWebs, tbl_Kerja);
                        break;
                }
            }

            AIPSPrice = AttInsentif + QualityInsentif + ProdInsentif;

            GajiBulanan = await db2.tbl_GajiBulanan.FindAsync(Guid);
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 7, AIPSPrice, DTProcess, UserID, GajiBulanan);
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 15, AttInsentif, DTProcess, UserID, GajiBulanan);
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 16, QualityInsentif, DTProcess, UserID, GajiBulanan);
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 17, ProdInsentif, DTProcess, UserID, GajiBulanan);

            AttTarget = AttTarget == null ? 0 : AttTarget;
            AttCapai = AttCapai == null ? 0 : AttCapai;
            QuaTarget = QuaTarget == null ? 0 : QuaTarget;
            QuaCapai = QuaCapai == null ? 0 : QuaCapai;
            ProdTarget = ProdTarget == null ? 0 : ProdTarget;
            ProdCapai = ProdCapai == null ? 0 : ProdCapai;

            await AddTo_tbl_GajiBulanan2(db2, 1, AttTarget, AttCapai, QuaTarget, QuaCapai, ProdTarget, ProdCapai, GajiBulanan);

            db.Dispose();
            db2.Dispose();

            return AIPSPrice;
        }

        public decimal? GetAttInsentifFunc(GenSalaryModelHQ db, GenSalaryModelEstate db2, int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? Month, int? Year, string NoPkj, tbl_Produktiviti GetTargetWorking, out int? AttTarget, out int? AttCapai, List<tblOptionConfigsWeb> tblOptionConfigsWebs, List<tbl_Kerjahdr> tbl_Kerjahdr)
        {
            decimal? AttInsentif = 0;

            var GetAbsent = tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "pontengaips" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).ToList();

            var GetWorking = tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_Nopkj == NoPkj && !GetAbsent.Contains(x.fld_Kdhdct)).ToList();

            if (GetWorking.Count >= GetTargetWorking.fld_HadirKerja)
            {
                AttInsentif = decimal.Parse(tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "aipskehadiranF" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).FirstOrDefault());
            }
            else if (GetWorking.Count == GetTargetWorking.fld_HadirKerja - 1)
            {
                AttInsentif = decimal.Parse(tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "aipskehadiranNF" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).FirstOrDefault());
            }
            else
            {
                AttInsentif = 0;
            }

            AttTarget = GetTargetWorking.fld_HadirKerja;

            AttCapai = GetWorking.Count;

            return AttInsentif;
        }

        public decimal? GetQualityInsentifFunc(GenSalaryModelHQ db, GenSalaryModelEstate db2, int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? Month, int? Year, string NoPkj, out short? QuaTarget, out short? QuaCapai, List<tblOptionConfigsWeb> tblOptionConfigsWebs, List<tbl_Kerja> tbl_Kerja)
        {
            decimal? QualityInsentif = 0;

            var GetQualityStatus = tbl_Kerja.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_Nopkj == NoPkj && x.fld_Quality != null && x.fld_Unit == "TAN").Sum(s => s.fld_Quality);

            if (GetQualityStatus <= 0 || GetQualityStatus == null)
            {
                QualityInsentif = decimal.Parse(tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "aipskualiti" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).FirstOrDefault());
                QuaCapai = 0;
            }
            else
            {
                QualityInsentif = 0;
                QuaCapai = null;
            }

            QuaTarget = 0;

            return QualityInsentif;
        }

        public decimal? GetProductivityInsentifFunc(GenSalaryModelHQ db, GenSalaryModelEstate db2, int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? Month, int? Year, string NoPkj, tbl_Produktiviti GetTargetWorking, out decimal? ProdTarget, out decimal? ProdCapai, List<tblOptionConfigsWeb> tblOptionConfigsWebs, List<tbl_Kerja> tbl_Kerja)
        {
            decimal? ProdInsentif = 0;
            decimal? ActualDailyTarget = 0;
            decimal? HalfDailyTarget = 0;

            var GetProductivityStatus = tbl_Kerja.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_Nopkj == NoPkj && x.fld_Unit.Contains(GetTargetWorking.fld_Unit)).Sum(s => s.fld_JumlahHasil);

            var CountWorkDay = tbl_Kerja.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == Month && x.fld_Tarikh.Value.Year == Year && x.fld_Nopkj == NoPkj && x.fld_Unit.Contains(GetTargetWorking.fld_Unit)).Select(s => s.fld_Tarikh).Distinct().Count();

            ActualDailyTarget = GetProductivityStatus / CountWorkDay;

            HalfDailyTarget = GetTargetWorking.fld_Targetharian / 2;

            if (ActualDailyTarget >= GetTargetWorking.fld_Targetharian)
            {
                ProdInsentif = GetTargetWorking.fld_JenisPelan == "A" ? decimal.Parse(tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "aipsprodFA" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).FirstOrDefault()) : decimal.Parse(tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "aipsprodFB" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).FirstOrDefault());
            }
            else if (ActualDailyTarget >= HalfDailyTarget && ActualDailyTarget < GetTargetWorking.fld_Targetharian)
            {
                ProdInsentif = GetTargetWorking.fld_JenisPelan == "A" ? decimal.Parse(tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "aipsprodHA" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).FirstOrDefault()) : decimal.Parse(tblOptionConfigsWebs.Where(x => x.fldOptConfFlag1 == "aipsprodHB" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).FirstOrDefault());
            }
            else
            {
                ProdInsentif = 0;
            }

            ProdTarget = GetTargetWorking.fld_Targetharian;

            ProdCapai = ActualDailyTarget;

            return ProdInsentif;
        }

        public async Task<decimal?> GetPaidLeaveFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, Guid Guid, List<CustMod_WorkerPaidLeave> WorkerPaidLeaveLists, DateTime? StartWorkDate, bool NoLeave, List<tbl_CutiKategori> CutiKategoriList, tbl_Pkjmast tbl_Pkjmast, List<tblOptionConfigsWeb> tblOptionConfigsWeb, List<tbl_Kerjahdr> tbl_Kerjahdr, List<tbl_CutiPeruntukan> tbl_CutiPeruntukan, List<tbl_PkjIncrmntSalary> tbl_PkjIncrmntSalary)
        {
            GetConnectFunc conn = new GetConnectFunc();
            Step2Func Step2Func = new Step2Func();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            int YearC = int.Parse(Year.ToString());
            int MonthC = int.Parse(Month.ToString());
            DateTime SelectDateOri = new DateTime(YearC, MonthC, 15);
            DateTime LastSelectMonthDate = SelectDateOri.AddMonths(-1);
            GenSalaryModelHQ db = new GenSalaryModelHQ();

            decimal? LeavePayment = 0;
            decimal? TotalPaidLeave = 0;
            decimal? TotalPaidLeave2 = 0;
            decimal? TotalPaidLeave3 = 0;
            decimal? OverAllPaidLeave = 0;
            decimal? AverageSalary = 0;
            decimal? AverageSalary12Month = 0;
            int? CheckPeruntukkan = 0;

            List<tbl_KerjahdrCuti> KerjahdrCutiList = new List<tbl_KerjahdrCuti>();
            tbl_KerjahdrCutiTahunan KerjahdrCutiTahunan = new tbl_KerjahdrCutiTahunan();

            //AverageSalary = db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Year == LastSelectMonthDate.Year && x.fld_Month == LastSelectMonthDate.Month).Select(s => s.fld_PurataGaji).FirstOrDefault();
            //AverageSalary = AverageSalary == null ? db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Year == Year && x.fld_Month == Month).Select(s => s.fld_PurataGaji).FirstOrDefault() : AverageSalary;
            var Krytn = tbl_Pkjmast.fld_Kdrkyt;
            var tbl_GajiBulanan = await db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_ID == Guid).FirstOrDefaultAsync();
            AverageSalary = tbl_GajiBulanan.fld_PurataGaji;
            AverageSalary12Month = tbl_GajiBulanan.fld_PurataGaji12Bln;
            var GetLastMonthAverageSalary = await db2.tbl_GajiBulanan.Where(x => x.fld_Nopkj == NoPkj && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == LastSelectMonthDate.Month && x.fld_Year == LastSelectMonthDate.Year).Select(s => s.fld_PurataGaji).FirstOrDefaultAsync();
            var GetMinPayforLeave = tblOptionConfigsWeb.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldOptConfFlag1 == "leavepay" && x.fldDeleted == false).ToList();
            if (Krytn == "MA")
            {
                var GetTKTPayment = GetMinPayforLeave.Where(x => x.fldOptConfFlag2 == "2").Select(s => s.fldOptConfValue).FirstOrDefault();
                GetLastMonthAverageSalary = decimal.Parse(GetTKTPayment);

                //add by faeza on 02.08.2021
                decimal? SalaryIncrement = tbl_PkjIncrmntSalary.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_AppStatus == true).Select(s => s.fld_IncrmntSalary).FirstOrDefault();

                if (SalaryIncrement != null)
                {
                    GetLastMonthAverageSalary = GetLastMonthAverageSalary + SalaryIncrement;
                }

            }
            else
            {
                var GetTKAPayment = GetMinPayforLeave.Where(x => x.fldOptConfFlag2 == "1").Select(s => s.fldOptConfValue).FirstOrDefault();
                var GetMinPay = decimal.Parse(GetTKAPayment);
                GetLastMonthAverageSalary = GetLastMonthAverageSalary == null ? AverageSalary : GetLastMonthAverageSalary;
                GetLastMonthAverageSalary = GetLastMonthAverageSalary == 0 ? AverageSalary : GetLastMonthAverageSalary;

                if (GetLastMonthAverageSalary < GetMinPay)
                {
                    GetLastMonthAverageSalary = GetMinPay;
                }
            }


            foreach (var WorkerPaidLeaveList in WorkerPaidLeaveLists)
            {
                if (WorkerPaidLeaveList.fld_PaidPeriod != 0) // if 0 kira hujung
                {
                    if (WorkerPaidLeaveList.fld_Kdhdct == "C01")
                    {
                        DateTime? OneDayBefore = WorkerPaidLeaveList.fld_Tarikh.Value.AddDays(-1);
                        DateTime? OneDayAfter = WorkerPaidLeaveList.fld_Tarikh.Value.AddDays(1);
                        var GetOneAftBefDayHdrCts = tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Tarikh >= OneDayBefore && x.fld_Tarikh <= OneDayAfter).Select(s => new { s.fld_Tarikh, s.fld_Kdhdct }).OrderBy(o => o.fld_Tarikh).ToList();

                        //if (GetOneAftBefDayHdrCts.Select(s => s.fld_Kdhdct).Contains("C07") == true)
                        //{
                        //    DateTime? TwoDayBefore = WorkerPaidLeaveList.fld_Tarikh.Value.AddDays(-2);
                        //    DateTime? TwoDayAfter = WorkerPaidLeaveList.fld_Tarikh.Value.AddDays(2);
                        //    var GetTwoAftBefDayHdrCts = db2.tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Tarikh >= TwoDayBefore && x.fld_Tarikh <= TwoDayAfter).Select(s => new { s.fld_Tarikh, s.fld_Kdhdct }).OrderBy(o => o.fld_Tarikh).ToList();
                        //    int loopcount = 1;
                        //    foreach (var GetTwoAftBefDayHdrCt in GetTwoAftBefDayHdrCts)
                        //    {
                        //        if (GetTwoAftBefDayHdrCt.fld_Tarikh != WorkerPaidLeaveList.fld_Tarikh)
                        //        {
                        //            if (GetTwoAftBefDayHdrCt.fld_Kdhdct == "P01")
                        //            {
                        //                DateTime? DateForward = new DateTime();
                        //                string KodHdrForward = "";
                        //                switch (loopcount)
                        //                {
                        //                    case 1:
                        //                        DateForward = GetTwoAftBefDayHdrCt.fld_Tarikh.Value.AddDays(1);
                        //                        KodHdrForward = GetTwoAftBefDayHdrCts.Where(x => x.fld_Tarikh == DateForward).Select(s => s.fld_Kdhdct).FirstOrDefault();
                        //                        if (KodHdrForward == "C07")
                        //                        {
                        //                            LeavePayment = 0;
                        //                        }
                        //                        else
                        //                        {
                        //                            LeavePayment = Math.Round(decimal.Parse(GetLastMonthAverageSalary.ToString()), 2);
                        //                            KerjahdrCutiList.Add(new tbl_KerjahdrCuti() { fld_Kadar = GetLastMonthAverageSalary, fld_Jumlah = LeavePayment, fld_Nopkj = WorkerPaidLeaveList.fld_Nopkj, fld_KerjahdrID = WorkerPaidLeaveList.fld_KerjahdrID, fld_Kum = WorkerPaidLeaveList.fld_Kum, fld_Tarikh = WorkerPaidLeaveList.fld_Tarikh, fld_NegaraID = NegaraID, fld_SyarikatID = SyarikatID, fld_WilayahID = WilayahID, fld_LadangID = LadangID, fld_CreatedBy = UserID, fld_CreatedDT = DTProcess });
                        //                        }
                        //                        break;
                        //                    case 2:
                        //                        LeavePayment = 0;
                        //                        break;
                        //                    case 3:
                        //                        LeavePayment = 0;
                        //                        break;
                        //                    case 4:
                        //                        DateForward = GetTwoAftBefDayHdrCt.fld_Tarikh.Value.AddDays(-1);
                        //                        KodHdrForward = GetTwoAftBefDayHdrCts.Where(x => x.fld_Tarikh == DateForward).Select(s => s.fld_Kdhdct).FirstOrDefault();
                        //                        if (KodHdrForward == "C07")
                        //                        {
                        //                            LeavePayment = 0;
                        //                        }
                        //                        else
                        //                        {
                        //                            LeavePayment = Math.Round(decimal.Parse(GetLastMonthAverageSalary.ToString()), 2);
                        //                            KerjahdrCutiList.Add(new tbl_KerjahdrCuti() { fld_Kadar = GetLastMonthAverageSalary, fld_Jumlah = LeavePayment, fld_Nopkj = WorkerPaidLeaveList.fld_Nopkj, fld_KerjahdrID = WorkerPaidLeaveList.fld_KerjahdrID, fld_Kum = WorkerPaidLeaveList.fld_Kum, fld_Tarikh = WorkerPaidLeaveList.fld_Tarikh, fld_NegaraID = NegaraID, fld_SyarikatID = SyarikatID, fld_WilayahID = WilayahID, fld_LadangID = LadangID, fld_CreatedBy = UserID, fld_CreatedDT = DTProcess });
                        //                        }
                        //                        break;
                        //                }
                        //            }
                        //        }
                        //        loopcount++;
                        //    }
                        //}
                        //else
                        //{
                        if (GetOneAftBefDayHdrCts.Select(s => s.fld_Kdhdct).Contains("P01") == true)
                        {
                            LeavePayment = 0;
                        }
                        else
                        {
                            LeavePayment = Math.Round(decimal.Parse(GetLastMonthAverageSalary.ToString()), 2);
                            KerjahdrCutiList.Add(new tbl_KerjahdrCuti() { fld_Kadar = GetLastMonthAverageSalary, fld_Jumlah = LeavePayment, fld_Nopkj = WorkerPaidLeaveList.fld_Nopkj, fld_KerjahdrID = WorkerPaidLeaveList.fld_KerjahdrID, fld_Kum = WorkerPaidLeaveList.fld_Kum, fld_Tarikh = WorkerPaidLeaveList.fld_Tarikh, fld_NegaraID = NegaraID, fld_SyarikatID = SyarikatID, fld_WilayahID = WilayahID, fld_LadangID = LadangID, fld_CreatedBy = UserID, fld_CreatedDT = DTProcess });
                        }
                        //}

                    }
                    else
                    {
                        if (WorkerPaidLeaveLists.Where(x => x.fld_Kdhdct == WorkerPaidLeaveList.fld_Kdhdct).Count() > 15)
                        {
                            //GetLastMonthAverageSalary = db2.tbl_GajiBulanan.Where(x => x.fld_Nopkj == NoPkj && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == LastSelectMonthDate.Month && x.fld_Year == LastSelectMonthDate.Year).Select(s => s.fld_PurataGaji).FirstOrDefault();
                            if (GetLastMonthAverageSalary != null)
                            {
                                LeavePayment = Math.Round(decimal.Parse(GetLastMonthAverageSalary.ToString()), 2);
                            }
                            else
                            {
                                var GetDailyRate = tblOptionConfigsWeb.Where(x => x.fldOptConfFlag1 == "kadarot" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).FirstOrDefault();
                                LeavePayment = Math.Round(decimal.Parse(GetDailyRate.ToString()), 2);
                            }
                        }
                        else
                        {
                            //GetLastMonthAverageSalary = db2.tbl_GajiBulanan.Where(x => x.fld_Nopkj == NoPkj && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == LastSelectMonthDate.Month && x.fld_Year == LastSelectMonthDate.Year).Select(s => s.fld_PurataGaji).FirstOrDefault();
                            //GetLastMonthAverageSalary = GetLastMonthAverageSalary == null ? AverageSalary : GetLastMonthAverageSalary;
                            LeavePayment = Math.Round(decimal.Parse(GetLastMonthAverageSalary.ToString()), 2);
                        }
                        KerjahdrCutiList.Add(new tbl_KerjahdrCuti() { fld_Kadar = GetLastMonthAverageSalary, fld_Jumlah = LeavePayment, fld_Nopkj = WorkerPaidLeaveList.fld_Nopkj, fld_KerjahdrID = WorkerPaidLeaveList.fld_KerjahdrID, fld_Kum = WorkerPaidLeaveList.fld_Kum, fld_Tarikh = WorkerPaidLeaveList.fld_Tarikh, fld_NegaraID = NegaraID, fld_SyarikatID = SyarikatID, fld_WilayahID = WilayahID, fld_LadangID = LadangID, fld_CreatedBy = UserID, fld_CreatedDT = DTProcess });
                    }
                }
                TotalPaidLeave = TotalPaidLeave + LeavePayment;
                LeavePayment = 0;
            }

            var GetStatusXActv = tblOptionConfigsWeb.Where(x => x.fldOptConfFlag1 == "sbbTakAktif" && x.fldOptConfFlag2 == "1" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).ToArray();
            var PkjStatus = tbl_Pkjmast; //.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && GetStatusXActv.Contains(x.fld_Sbtakf) && x.fld_Kdaktf =="0").FirstOrDefault();
            var KodCutiTahunan = CutiKategoriList.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WaktuBayaranCuti == 0 && x.fld_KodCuti == "C02").FirstOrDefault();

            // cuti tahunan sahaja
            if (GetStatusXActv.Contains(PkjStatus.fld_Sbtakf) == true && PkjStatus.fld_Kdaktf == "0" && Month <= 12)
            {
                int TotalWorkingDay = (PkjStatus.fld_Trtakf - PkjStatus.fld_Trmlkj).Value.Days;
                decimal Years = TotalWorkingDay / 365.25m;
                DateTime? MulaKerja = new DateTime();
                if (Years > 0)
                {
                    MulaKerja = new DateTime(YearC, 1, 1);
                }
                else
                {
                    MulaKerja = PkjStatus.fld_Trmlkj;
                }

                TotalWorkingDay = (PkjStatus.fld_Trtakf - MulaKerja).Value.Days;

                var GetCutiPkj = tbl_CutiPeruntukan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_NoPkj == NoPkj && x.fld_KodCuti == KodCutiTahunan.fld_KodCuti).FirstOrDefault();
                int? Peruntukkan = GetCutiPkj.fld_JumlahCuti;
                int? DahAmbil = GetCutiPkj.fld_JumlahCutiDiambil;
                //var TakeLeaves = db2.tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Tarikh.Value.Year == Year && PaidLeaveCode.Contains(x.fld_Kdhdct)).ToList();
                int? MonthWorking = TotalWorkingDay / 30;
                int? PeruntukkanSbulan = Peruntukkan / 12;

                if (MonthWorking <= 1 && PeruntukkanSbulan == 0)
                {
                    CheckPeruntukkan = 0;
                }
                else if (MonthWorking > 1 && PeruntukkanSbulan == 0)
                {
                    CheckPeruntukkan = MonthWorking * (Peruntukkan / 12);
                    //if (CheckPeruntukkan > DahAmbil)
                    //{
                    //    CheckPeruntukkan = CheckPeruntukkan - DahAmbil;
                    //}
                    //else
                    //{
                    //    CheckPeruntukkan = 0;
                    //    //kira berapa dh amek
                    //}
                }
                else if (MonthWorking > 1 && PeruntukkanSbulan > 0)
                {
                    CheckPeruntukkan = MonthWorking * PeruntukkanSbulan;
                    //if (CheckPeruntukkan > DahAmbil)
                    //{
                    //    CheckPeruntukkan = CheckPeruntukkan - DahAmbil;
                    //}
                    //else
                    //{
                    //    CheckPeruntukkan = 0;
                    //    //kira berapa dh amek
                    //}
                }

                if (CheckPeruntukkan > 0)
                {
                    LeavePayment = GetLastMonthAverageSalary;
                    TotalPaidLeave3 = LeavePayment * CheckPeruntukkan;
                    KerjahdrCutiTahunan.fld_Kadar = LeavePayment;
                    KerjahdrCutiTahunan.fld_KodCuti = KodCutiTahunan.fld_KodCuti;
                    KerjahdrCutiTahunan.fld_Kum = WorkerPaidLeaveLists.Select(s => s.fld_Kum).FirstOrDefault();
                    KerjahdrCutiTahunan.fld_JumlahCuti = CheckPeruntukkan;
                    KerjahdrCutiTahunan.fld_JumlahAmt = TotalPaidLeave3;
                    KerjahdrCutiTahunan.fld_NegaraID = NegaraID;
                    KerjahdrCutiTahunan.fld_SyarikatID = SyarikatID;
                    KerjahdrCutiTahunan.fld_WilayahID = WilayahID;
                    KerjahdrCutiTahunan.fld_LadangID = LadangID;
                    KerjahdrCutiTahunan.fld_Nopkj = NoPkj;
                    KerjahdrCutiTahunan.fld_Month = Month;
                    KerjahdrCutiTahunan.fld_Year = Year;
                    KerjahdrCutiTahunan.fld_CreatedBy = UserID;
                    KerjahdrCutiTahunan.fld_CreatedDT = DTProcess;
                    KerjahdrCutiTahunan.fld_StatusAmbil = false;

                    db2.tbl_KerjahdrCutiTahunan.Add(KerjahdrCutiTahunan);
                    await db2.SaveChangesAsync();
                }
            }

            // cuti tahunan sahaja
            else if (GetStatusXActv.Contains(PkjStatus.fld_Sbtakf) == false && PkjStatus.fld_Kdaktf == "1" && Month == 12 && KodCutiTahunan != null)
            {
                //var PaidLeaveCode = CutiKategoriList.Where(x => x.fld_WaktuBayaranCuti == 0).Select(s => s.fld_KodCuti).ToList();
                var TakeLeaves = tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Tarikh.Value.Year == Year && x.fld_Kdhdct == KodCutiTahunan.fld_KodCuti).ToList();
                var PeruntukkanCtTahunan = tbl_CutiPeruntukan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_NoPkj == NoPkj && x.fld_Tahun == Year && x.fld_KodCuti == KodCutiTahunan.fld_KodCuti).Select(s => s.fld_JumlahCuti).FirstOrDefault();

                foreach (var TakeLeave in TakeLeaves)
                {
                    LeavePayment = GetLastMonthAverageSalary;
                    KerjahdrCutiList.Add(new tbl_KerjahdrCuti() { fld_Kadar = GetLastMonthAverageSalary, fld_Jumlah = LeavePayment, fld_Nopkj = TakeLeave.fld_Nopkj, fld_KerjahdrID = TakeLeave.fld_UniqueID, fld_Kum = TakeLeave.fld_Kum, fld_Tarikh = TakeLeave.fld_Tarikh, fld_NegaraID = NegaraID, fld_SyarikatID = SyarikatID, fld_WilayahID = WilayahID, fld_LadangID = LadangID, fld_CreatedBy = UserID, fld_CreatedDT = DTProcess });
                    TotalPaidLeave2 = TotalPaidLeave2 + LeavePayment;
                    LeavePayment = 0;
                }

                if (PeruntukkanCtTahunan > TakeLeaves.Count)
                {
                    CheckPeruntukkan = PeruntukkanCtTahunan - TakeLeaves.Count;
                    LeavePayment = GetLastMonthAverageSalary;
                    TotalPaidLeave3 = LeavePayment * CheckPeruntukkan;
                    KerjahdrCutiTahunan.fld_Kadar = LeavePayment;
                    KerjahdrCutiTahunan.fld_KodCuti = KodCutiTahunan.fld_KodCuti;
                    KerjahdrCutiTahunan.fld_Kum = WorkerPaidLeaveLists.Select(s => s.fld_Kum).FirstOrDefault();
                    KerjahdrCutiTahunan.fld_JumlahCuti = CheckPeruntukkan;
                    KerjahdrCutiTahunan.fld_JumlahAmt = TotalPaidLeave3;
                    KerjahdrCutiTahunan.fld_NegaraID = NegaraID;
                    KerjahdrCutiTahunan.fld_SyarikatID = SyarikatID;
                    KerjahdrCutiTahunan.fld_WilayahID = WilayahID;
                    KerjahdrCutiTahunan.fld_LadangID = LadangID;
                    KerjahdrCutiTahunan.fld_Nopkj = NoPkj;
                    KerjahdrCutiTahunan.fld_Month = Month;
                    KerjahdrCutiTahunan.fld_Year = Year;
                    KerjahdrCutiTahunan.fld_CreatedBy = UserID;
                    KerjahdrCutiTahunan.fld_CreatedDT = DTProcess;
                    KerjahdrCutiTahunan.fld_StatusAmbil = false;

                    db2.tbl_KerjahdrCutiTahunan.Add(KerjahdrCutiTahunan);
                    await db2.SaveChangesAsync();
                }
            }

            await Step2Func.AddTo_tbl_KerjahdrCuti(NegaraID, SyarikatID, WilayahID, LadangID, KerjahdrCutiList);

            if (NoLeave)
            {
                TotalPaidLeave = 0;
                TotalPaidLeave2 = 0;
                TotalPaidLeave3 = 0;
            }

            OverAllPaidLeave = TotalPaidLeave + TotalPaidLeave2 + TotalPaidLeave3;

            GajiBulanan = db2.tbl_GajiBulanan.Find(Guid);
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 8, OverAllPaidLeave, DTProcess, UserID, GajiBulanan);

            db2.Dispose();

            return OverAllPaidLeave;
        }

        //added by faeza 26.09.2022 - implement ORP
        public async Task<decimal?> GetPaidLeaveORPFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, Guid Guid, List<CustMod_WorkerPaidLeave> WorkerPaidLeaveLists, DateTime? StartWorkDate, bool NoLeave, List<tbl_CutiKategori> CutiKategoriList, tbl_Pkjmast tbl_Pkjmast, List<tblOptionConfigsWeb> tblOptionConfigsWeb, List<tbl_Kerjahdr> tbl_Kerjahdr, List<tbl_CutiPeruntukan> tbl_CutiPeruntukan, List<tbl_PkjIncrmntSalary> tbl_PkjIncrmntSalary, List<tbl_Kerjahdr> tbl_KerjahdrYearly)
        {
            GetConnectFunc conn = new GetConnectFunc();
            Step2Func Step2Func = new Step2Func();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            int YearC = int.Parse(Year.ToString());
            int MonthC = int.Parse(Month.ToString());
            DateTime SelectDateOri = new DateTime(YearC, MonthC, 15);
            DateTime LastSelectMonthDate = SelectDateOri.AddMonths(-1);
            GenSalaryModelHQ db = new GenSalaryModelHQ();

            decimal? LeavePayment = 0;
            decimal? TotalPaidLeave = 0;
            decimal? TotalPaidLeave2 = 0;
            decimal? TotalPaidLeave3 = 0;
            decimal? OverAllPaidLeave = 0;
            decimal? AverageSalary = 0;
            decimal? AverageSalary12Month = 0;
            int? CheckPeruntukkan = 0;

            List<tbl_KerjahdrCuti> KerjahdrCutiList = new List<tbl_KerjahdrCuti>();
            tbl_KerjahdrCutiTahunan KerjahdrCutiTahunan = new tbl_KerjahdrCutiTahunan();

            //AverageSalary = db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Year == LastSelectMonthDate.Year && x.fld_Month == LastSelectMonthDate.Month).Select(s => s.fld_PurataGaji).FirstOrDefault();
            //AverageSalary = AverageSalary == null ? db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Year == Year && x.fld_Month == Month).Select(s => s.fld_PurataGaji).FirstOrDefault() : AverageSalary;
            var Krytn = tbl_Pkjmast.fld_Kdrkyt;
            //Modified by Shah 23.12.2023 - Baki Cuti Tahunan
            var tbl_GajiBulananList = await db2.tbl_GajiBulanan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Year == Year && x.fld_Month <= 12).ToListAsync();
            var tbl_GajiBulanan = tbl_GajiBulananList.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_ID == Guid).FirstOrDefault();
            //Modified by Shah 23.12.2023 - Baki Cuti Tahunan
            AverageSalary = tbl_GajiBulanan.fld_PurataGaji;
            AverageSalary12Month = tbl_GajiBulanan.fld_PurataGaji12Bln;
            var GetLastMonthAverageSalary = await db2.tbl_GajiBulanan.Where(x => x.fld_Nopkj == NoPkj && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == LastSelectMonthDate.Month && x.fld_Year == LastSelectMonthDate.Year).Select(s => s.fld_PurataGaji).FirstOrDefaultAsync();
            var GetMinPayforLeave = tblOptionConfigsWeb.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldOptConfFlag1 == "leavepay" && x.fldDeleted == false).ToList();

            if (Krytn == "MA")
            {
                var GetTKTPayment = GetMinPayforLeave.Where(x => x.fldOptConfFlag2 == "2").Select(s => s.fldOptConfValue).FirstOrDefault();
                var GetMinPay = decimal.Parse(GetTKTPayment);

                if (WorkerPaidLeaveLists.Select(s => s.fld_Kdhdct).Contains("C04") == true) //c04 = cuti bersalin
                {
                    for (int i = 1; i < 5; i++)// allocation cuti bersalin 3 bulan
                    {
                        //modified by faeza 05.09.2023
                        DateTime SelectDateOriBersalin = new DateTime(YearC, MonthC, 15);
                        DateTime LastSelectMonthDateBersalin = SelectDateOriBersalin.AddMonths(-(i));
                        var attendancelastmonth = db2.tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == LastSelectMonthDateBersalin.Month && x.fld_Tarikh.Value.Year == LastSelectMonthDateBersalin.Year && x.fld_Nopkj == NoPkj).ToList();
                        if (attendancelastmonth.Select(s => s.fld_Kdhdct).Contains("C04") == false)
                        {
                            GetLastMonthAverageSalary = await db2.tbl_GajiBulanan.Where(x => x.fld_Nopkj == NoPkj && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == LastSelectMonthDateBersalin.Month && x.fld_Year == LastSelectMonthDateBersalin.Year).Select(s => s.fld_PurataGaji).FirstOrDefaultAsync();
                            if (GetLastMonthAverageSalary < GetMinPay)
                            {
                                GetLastMonthAverageSalary = GetMinPay;
                            }
                            break;
                        }

                        //commented by faeza 03.08.2023
                        //decimal? SalaryIncrement = tbl_PkjIncrmntSalary.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_AppStatus == true).Select(s => s.fld_IncrmntSalary).FirstOrDefault();
                        //if (SalaryIncrement != null)
                        //{
                        //    GetLastMonthAverageSalary = GetLastMonthAverageSalary + SalaryIncrement;
                        //}
                    }

                }
                else
                {
                    //var GetTKTPayment = GetMinPayforLeave.Where(x => x.fldOptConfFlag2 == "2").Select(s => s.fldOptConfValue).FirstOrDefault();
                    //var GetMinPay = decimal.Parse(GetTKTPayment);
                    //GetLastMonthAverageSalary = GetLastMonthAverageSalary == null ? AverageSalary : GetLastMonthAverageSalary;
                    //GetLastMonthAverageSalary = GetLastMonthAverageSalary == 0 ? AverageSalary : GetLastMonthAverageSalary;

                    GetLastMonthAverageSalary = GetLastMonthAverageSalary == null ? GetMinPay : GetLastMonthAverageSalary;
                    GetLastMonthAverageSalary = GetLastMonthAverageSalary == 0 ? GetMinPay : GetLastMonthAverageSalary;

                    //commented by faeza 03.08.2023
                    //decimal? SalaryIncrement = tbl_PkjIncrmntSalary.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_AppStatus == true).Select(s => s.fld_IncrmntSalary).FirstOrDefault();

                    //if (SalaryIncrement != null)
                    //{
                    //    GetLastMonthAverageSalary = GetLastMonthAverageSalary + SalaryIncrement;
                    //}

                    if (GetLastMonthAverageSalary < GetMinPay)
                    {
                        GetLastMonthAverageSalary = GetMinPay;
                    }
                }

            }
            else //worker asing
            {
                var GetTKAPayment = GetMinPayforLeave.Where(x => x.fldOptConfFlag2 == "1").Select(s => s.fldOptConfValue).FirstOrDefault();
                var GetMinPay = decimal.Parse(GetTKAPayment);

                if (WorkerPaidLeaveLists.Select(s => s.fld_Kdhdct).Contains("C04") == true)
                {
                    //if (AverageSalary == null || AverageSalary == 0)
                    //{
                    for (int i = 1; i < 5; i++)
                    {
                        DateTime SelectDateOriBersalin = new DateTime(YearC, MonthC, 15);
                        DateTime LastSelectMonthDateBersalin = SelectDateOriBersalin.AddMonths(-(i));
                        var attendancelastmonth = db2.tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Month == LastSelectMonthDateBersalin.Month && x.fld_Tarikh.Value.Year == LastSelectMonthDateBersalin.Year && x.fld_Nopkj == NoPkj).ToList();
                        if (attendancelastmonth.Select(s => s.fld_Kdhdct).Contains("C04") == false)
                        {
                            GetLastMonthAverageSalary = await db2.tbl_GajiBulanan.Where(x => x.fld_Nopkj == NoPkj && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == LastSelectMonthDateBersalin.Month && x.fld_Year == LastSelectMonthDateBersalin.Year).Select(s => s.fld_PurataGaji).FirstOrDefaultAsync();
                            if (GetLastMonthAverageSalary < GetMinPay)
                            {
                                GetLastMonthAverageSalary = GetMinPay;
                            }
                            break;
                        }
                        //var GetTKTPayment = GetMinPayforLeave.Where(x => x.fldOptConfFlag2 == "2").Select(s => s.fldOptConfValue).FirstOrDefault();
                        //var GetMinPay = decimal.Parse(GetTKTPayment);

                        //decimal? SalaryIncrement = tbl_PkjIncrmntSalary.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_AppStatus == true).Select(s => s.fld_IncrmntSalary).FirstOrDefault();

                        //if (SalaryIncrement != null)
                        //{
                        //    GetLastMonthAverageSalary = GetLastMonthAverageSalary + SalaryIncrement;
                        //}                            
                    }
                }
                else
                {
                    //var GetTKAPayment = GetMinPayforLeave.Where(x => x.fldOptConfFlag2 == "1").Select(s => s.fldOptConfValue).FirstOrDefault();
                    //var GetMinPay = decimal.Parse(GetTKAPayment);
                    //GetLastMonthAverageSalary = GetLastMonthAverageSalary == null ? AverageSalary : GetLastMonthAverageSalary;
                    //GetLastMonthAverageSalary = GetLastMonthAverageSalary == 0 ? AverageSalary : GetLastMonthAverageSalary;

                    GetLastMonthAverageSalary = GetLastMonthAverageSalary == null ? GetMinPay : GetLastMonthAverageSalary;
                    GetLastMonthAverageSalary = GetLastMonthAverageSalary == 0 ? GetMinPay : GetLastMonthAverageSalary;

                    if (GetLastMonthAverageSalary < GetMinPay)
                    {
                        GetLastMonthAverageSalary = GetMinPay;
                    }
                }
            }

            foreach (var WorkerPaidLeaveList in WorkerPaidLeaveLists)
            {
                if (WorkerPaidLeaveList.fld_PaidPeriod != 0) // if 0 kira hujung
                {
                    if (WorkerPaidLeaveList.fld_Kdhdct == "C01")
                    {

                        DateTime? OneDayBefore = WorkerPaidLeaveList.fld_Tarikh.Value.AddDays(-1);
                        DateTime? OneDayAfter = WorkerPaidLeaveList.fld_Tarikh.Value.AddDays(1);
                        var GetOneAftBefDayHdrCts = tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Tarikh >= OneDayBefore && x.fld_Tarikh <= OneDayAfter).Select(s => new { s.fld_Tarikh, s.fld_Kdhdct }).OrderBy(o => o.fld_Tarikh).ToList();

                        //if (GetOneAftBefDayHdrCts.Select(s => s.fld_Kdhdct).Contains("C07") == true)
                        //{
                        //    DateTime? TwoDayBefore = WorkerPaidLeaveList.fld_Tarikh.Value.AddDays(-2);
                        //    DateTime? TwoDayAfter = WorkerPaidLeaveList.fld_Tarikh.Value.AddDays(2);
                        //    var GetTwoAftBefDayHdrCts = db2.tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Tarikh >= TwoDayBefore && x.fld_Tarikh <= TwoDayAfter).Select(s => new { s.fld_Tarikh, s.fld_Kdhdct }).OrderBy(o => o.fld_Tarikh).ToList();
                        //    int loopcount = 1;
                        //    foreach (var GetTwoAftBefDayHdrCt in GetTwoAftBefDayHdrCts)
                        //    {
                        //        if (GetTwoAftBefDayHdrCt.fld_Tarikh != WorkerPaidLeaveList.fld_Tarikh)
                        //        {
                        //            if (GetTwoAftBefDayHdrCt.fld_Kdhdct == "P01")
                        //            {
                        //                DateTime? DateForward = new DateTime();
                        //                string KodHdrForward = "";
                        //                switch (loopcount)
                        //                {
                        //                    case 1:
                        //                        DateForward = GetTwoAftBefDayHdrCt.fld_Tarikh.Value.AddDays(1);
                        //                        KodHdrForward = GetTwoAftBefDayHdrCts.Where(x => x.fld_Tarikh == DateForward).Select(s => s.fld_Kdhdct).FirstOrDefault();
                        //                        if (KodHdrForward == "C07")
                        //                        {
                        //                            LeavePayment = 0;
                        //                        }
                        //                        else
                        //                        {
                        //                            LeavePayment = Math.Round(decimal.Parse(GetLastMonthAverageSalary.ToString()), 2);
                        //                            KerjahdrCutiList.Add(new tbl_KerjahdrCuti() { fld_Kadar = GetLastMonthAverageSalary, fld_Jumlah = LeavePayment, fld_Nopkj = WorkerPaidLeaveList.fld_Nopkj, fld_KerjahdrID = WorkerPaidLeaveList.fld_KerjahdrID, fld_Kum = WorkerPaidLeaveList.fld_Kum, fld_Tarikh = WorkerPaidLeaveList.fld_Tarikh, fld_NegaraID = NegaraID, fld_SyarikatID = SyarikatID, fld_WilayahID = WilayahID, fld_LadangID = LadangID, fld_CreatedBy = UserID, fld_CreatedDT = DTProcess });
                        //                        }
                        //                        break;
                        //                    case 2:
                        //                        LeavePayment = 0;
                        //                        break;
                        //                    case 3:
                        //                        LeavePayment = 0;
                        //                        break;
                        //                    case 4:
                        //                        DateForward = GetTwoAftBefDayHdrCt.fld_Tarikh.Value.AddDays(-1);
                        //                        KodHdrForward = GetTwoAftBefDayHdrCts.Where(x => x.fld_Tarikh == DateForward).Select(s => s.fld_Kdhdct).FirstOrDefault();
                        //                        if (KodHdrForward == "C07")
                        //                        {
                        //                            LeavePayment = 0;
                        //                        }
                        //                        else
                        //                        {
                        //                            LeavePayment = Math.Round(decimal.Parse(GetLastMonthAverageSalary.ToString()), 2);
                        //                            KerjahdrCutiList.Add(new tbl_KerjahdrCuti() { fld_Kadar = GetLastMonthAverageSalary, fld_Jumlah = LeavePayment, fld_Nopkj = WorkerPaidLeaveList.fld_Nopkj, fld_KerjahdrID = WorkerPaidLeaveList.fld_KerjahdrID, fld_Kum = WorkerPaidLeaveList.fld_Kum, fld_Tarikh = WorkerPaidLeaveList.fld_Tarikh, fld_NegaraID = NegaraID, fld_SyarikatID = SyarikatID, fld_WilayahID = WilayahID, fld_LadangID = LadangID, fld_CreatedBy = UserID, fld_CreatedDT = DTProcess });
                        //                        }
                        //                        break;
                        //                }
                        //            }
                        //        }
                        //        loopcount++;
                        //    }
                        //}
                        //else
                        //{
                        if (GetOneAftBefDayHdrCts.Select(s => s.fld_Kdhdct).Contains("P01") == true)
                        {
                            LeavePayment = 0;
                        }
                        else
                        {
                            LeavePayment = Math.Round(decimal.Parse(GetLastMonthAverageSalary.ToString()), 2);
                            KerjahdrCutiList.Add(new tbl_KerjahdrCuti() { fld_Kadar = GetLastMonthAverageSalary, fld_Jumlah = LeavePayment, fld_Nopkj = WorkerPaidLeaveList.fld_Nopkj, fld_KerjahdrID = WorkerPaidLeaveList.fld_KerjahdrID, fld_Kum = WorkerPaidLeaveList.fld_Kum, fld_Tarikh = WorkerPaidLeaveList.fld_Tarikh, fld_NegaraID = NegaraID, fld_SyarikatID = SyarikatID, fld_WilayahID = WilayahID, fld_LadangID = LadangID, fld_CreatedBy = UserID, fld_CreatedDT = DTProcess });
                        }
                        //}

                    }
                    else if (WorkerPaidLeaveList.fld_Kdhdct == "C04") //cuti bersalin
                    {
                        //var DateStartHoliday = tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Tarikh.Value.Year == Year && x.fld_Tarikh.Value.Month == Month && x.fld_Nopkj == NoPkj && x.fld_Kdhdct == "C04").OrderBy(o => o.fld_Tarikh).Select(s => s.fld_Tarikh).FirstOrDefault();
                        //DateTime dateHoliday = Convert.ToDateTime(DateStartHoliday);
                        //int monthdateHoliday = int.Parse(dateHoliday.ToString("MM"));

                        //if (Krytn == "MA")
                        //{
                        //    var GetTKTPayment = GetMinPayforLeave.Where(x => x.fldOptConfFlag2 == "2").Select(s => s.fldOptConfValue).FirstOrDefault();
                        //    var GetMinPay = decimal.Parse(GetTKTPayment);
                        //    GetLastMonthAverageSalary = GetLastMonthAverageSalary == null ? AverageSalary : GetLastMonthAverageSalary;
                        //    GetLastMonthAverageSalary = GetLastMonthAverageSalary == 0 ? AverageSalary : GetLastMonthAverageSalary;

                        //    if (AverageSalary == null || AverageSalary == 0)
                        //    {
                        //        for (int i = 1; i < 4; i++)
                        //        {
                        //            DateTime SelectDateOriBersalin = new DateTime(YearC, MonthC, 15);
                        //            DateTime LastSelectMonthDateBersalin = SelectDateOri.AddMonths(-(i));
                        //            GetLastMonthAverageSalary = await db2.tbl_GajiBulanan.Where(x => x.fld_Nopkj == NoPkj && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == LastSelectMonthDateBersalin.Month && x.fld_Year == LastSelectMonthDateBersalin.Year).Select(s => s.fld_PurataGaji).FirstOrDefaultAsync();
                        //            if (GetLastMonthAverageSalary == null || GetLastMonthAverageSalary == 0)
                        //            {
                        //                //i++;
                        //            }
                        //            else
                        //            {
                        //                //add by faeza on 02.08.2021
                        //                decimal? SalaryIncrement = tbl_PkjIncrmntSalary.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_AppStatus == true).Select(s => s.fld_IncrmntSalary).FirstOrDefault();

                        //                if (SalaryIncrement != null)
                        //                {
                        //                    GetLastMonthAverageSalary = GetLastMonthAverageSalary + SalaryIncrement;
                        //                }

                        //                if (GetLastMonthAverageSalary < GetMinPay)
                        //                {
                        //                    GetLastMonthAverageSalary = GetMinPay;
                        //                }
                        //                break;
                        //            }
                        //        }
                        //        LeavePayment = Math.Round(decimal.Parse(GetLastMonthAverageSalary.ToString()), 2);
                        //    }
                        //    else
                        //    {
                        //        LeavePayment = Math.Round(decimal.Parse(GetLastMonthAverageSalary.ToString()), 2);
                        //    }
                        //}
                        //else
                        //{
                        //    var GetTKAPayment = GetMinPayforLeave.Where(x => x.fldOptConfFlag2 == "1").Select(s => s.fldOptConfValue).FirstOrDefault();
                        //    var GetMinPay = decimal.Parse(GetTKAPayment);
                        //    GetLastMonthAverageSalary = GetLastMonthAverageSalary == null ? AverageSalary : GetLastMonthAverageSalary;
                        //    GetLastMonthAverageSalary = GetLastMonthAverageSalary == 0 ? AverageSalary : GetLastMonthAverageSalary;

                        //    if (GetLastMonthAverageSalary < GetMinPay)
                        //    {
                        //        GetLastMonthAverageSalary = GetMinPay;
                        //    }
                        //    LeavePayment = Math.Round(decimal.Parse(GetLastMonthAverageSalary.ToString()), 2);
                        //}
                        LeavePayment = Math.Round(decimal.Parse(GetLastMonthAverageSalary.ToString()), 2);
                        KerjahdrCutiList.Add(new tbl_KerjahdrCuti() { fld_Kadar = GetLastMonthAverageSalary, fld_Jumlah = LeavePayment, fld_Nopkj = WorkerPaidLeaveList.fld_Nopkj, fld_KerjahdrID = WorkerPaidLeaveList.fld_KerjahdrID, fld_Kum = WorkerPaidLeaveList.fld_Kum, fld_Tarikh = WorkerPaidLeaveList.fld_Tarikh, fld_NegaraID = NegaraID, fld_SyarikatID = SyarikatID, fld_WilayahID = WilayahID, fld_LadangID = LadangID, fld_CreatedBy = UserID, fld_CreatedDT = DTProcess });
                    }
                    else if (WorkerPaidLeaveList.fld_Kdhdct == "H03")
                    {
                        LeavePayment = Math.Round(decimal.Parse(GetLastMonthAverageSalary.ToString()), 2);
                        KerjahdrCutiList.Add(new tbl_KerjahdrCuti() { fld_Kadar = GetLastMonthAverageSalary, fld_Jumlah = LeavePayment, fld_Nopkj = WorkerPaidLeaveList.fld_Nopkj, fld_KerjahdrID = WorkerPaidLeaveList.fld_KerjahdrID, fld_Kum = WorkerPaidLeaveList.fld_Kum, fld_Tarikh = WorkerPaidLeaveList.fld_Tarikh, fld_NegaraID = NegaraID, fld_SyarikatID = SyarikatID, fld_WilayahID = WilayahID, fld_LadangID = LadangID, fld_CreatedBy = UserID, fld_CreatedDT = DTProcess });
                    }
                    else
                    {
                        if (WorkerPaidLeaveLists.Where(x => x.fld_Kdhdct == WorkerPaidLeaveList.fld_Kdhdct).Count() > 15)
                        {

                            //GetLastMonthAverageSalary = db2.tbl_GajiBulanan.Where(x => x.fld_Nopkj == NoPkj && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == LastSelectMonthDate.Month && x.fld_Year == LastSelectMonthDate.Year).Select(s => s.fld_PurataGaji).FirstOrDefault();
                            if (GetLastMonthAverageSalary != null)
                            {
                                LeavePayment = Math.Round(decimal.Parse(GetLastMonthAverageSalary.ToString()), 2);
                            }
                            else
                            {
                                var GetDailyRate = tblOptionConfigsWeb.Where(x => x.fldOptConfFlag1 == "kadarot" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).FirstOrDefault();
                                LeavePayment = Math.Round(decimal.Parse(GetDailyRate.ToString()), 2);
                            }
                        }
                        else
                        {
                            //GetLastMonthAverageSalary = db2.tbl_GajiBulanan.Where(x => x.fld_Nopkj == NoPkj && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Month == LastSelectMonthDate.Month && x.fld_Year == LastSelectMonthDate.Year).Select(s => s.fld_PurataGaji).FirstOrDefault();
                            //GetLastMonthAverageSalary = GetLastMonthAverageSalary == null ? AverageSalary : GetLastMonthAverageSalary;
                            LeavePayment = Math.Round(decimal.Parse(GetLastMonthAverageSalary.ToString()), 2);
                        }
                        KerjahdrCutiList.Add(new tbl_KerjahdrCuti() { fld_Kadar = GetLastMonthAverageSalary, fld_Jumlah = LeavePayment, fld_Nopkj = WorkerPaidLeaveList.fld_Nopkj, fld_KerjahdrID = WorkerPaidLeaveList.fld_KerjahdrID, fld_Kum = WorkerPaidLeaveList.fld_Kum, fld_Tarikh = WorkerPaidLeaveList.fld_Tarikh, fld_NegaraID = NegaraID, fld_SyarikatID = SyarikatID, fld_WilayahID = WilayahID, fld_LadangID = LadangID, fld_CreatedBy = UserID, fld_CreatedDT = DTProcess });
                    }
                }
                TotalPaidLeave = TotalPaidLeave + LeavePayment;
                LeavePayment = 0;
            }

            var GetStatusXActv = tblOptionConfigsWeb.Where(x => x.fldOptConfFlag1 == "sbbTakAktif" && x.fldOptConfFlag2 == "1" && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fldDeleted == false).Select(s => s.fldOptConfValue).ToArray();
            var PkjStatus = tbl_Pkjmast; //.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && GetStatusXActv.Contains(x.fld_Sbtakf) && x.fld_Kdaktf =="0").FirstOrDefault();
            var KodCutiTahunan = CutiKategoriList.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WaktuBayaranCuti == 0 && x.fld_KodCuti == "C02").FirstOrDefault();

            var gajiSatuTahun = tbl_GajiBulananList.Where(x => x.fld_Year == Year && x.fld_Month <= 12).ToList();
            LeavePayment = gajiSatuTahun.Sum(s => s.fld_PurataGaji) / gajiSatuTahun.Count;
            LeavePayment = decimal.Round(LeavePayment.Value, 2);
            GajiBulanan = db2.tbl_GajiBulanan.Find(Guid);

            // cuti tahunan sahaja
            if (GetStatusXActv.Contains(PkjStatus.fld_Sbtakf) == true && PkjStatus.fld_Kdaktf == "0" && Month <= 12)
            {
                int TotalWorkingDay = (PkjStatus.fld_Trtakf - PkjStatus.fld_Trmlkj).Value.Days;
                decimal Years = TotalWorkingDay / 365.25m;
                DateTime? MulaKerja = new DateTime();
                if (Years > 0)
                {
                    MulaKerja = new DateTime(YearC, 1, 1);
                }
                else
                {
                    MulaKerja = PkjStatus.fld_Trmlkj;
                }

                TotalWorkingDay = (PkjStatus.fld_Trtakf - MulaKerja).Value.Days;

                var GetCutiPkj = tbl_CutiPeruntukan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_NoPkj == NoPkj && x.fld_KodCuti == KodCutiTahunan.fld_KodCuti).FirstOrDefault();
                int? Peruntukkan = GetCutiPkj.fld_JumlahCuti;
                int? DahAmbil = GetCutiPkj.fld_JumlahCutiDiambil;
                //var TakeLeaves = db2.tbl_Kerjahdr.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Tarikh.Value.Year == Year && PaidLeaveCode.Contains(x.fld_Kdhdct)).ToList();
                int? MonthWorking = TotalWorkingDay / 30;
                int? PeruntukkanSbulan = Peruntukkan / 12;

                if (MonthWorking <= 1 && PeruntukkanSbulan == 0)
                {
                    CheckPeruntukkan = 0;
                }
                else if (MonthWorking > 1 && PeruntukkanSbulan == 0)
                {
                    CheckPeruntukkan = MonthWorking * (Peruntukkan / 12);
                    //if (CheckPeruntukkan > DahAmbil)
                    //{
                    //    CheckPeruntukkan = CheckPeruntukkan - DahAmbil;
                    //}
                    //else
                    //{
                    //    CheckPeruntukkan = 0;
                    //    //kira berapa dh amek
                    //}
                }
                else if (MonthWorking > 1 && PeruntukkanSbulan > 0)
                {
                    CheckPeruntukkan = MonthWorking * PeruntukkanSbulan;
                    //if (CheckPeruntukkan > DahAmbil)
                    //{
                    //    CheckPeruntukkan = CheckPeruntukkan - DahAmbil;
                    //}
                    //else
                    //{
                    //    CheckPeruntukkan = 0;
                    //    //kira berapa dh amek
                    //}
                }

                var TakeLeaves = tbl_KerjahdrYearly.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Tarikh.Value.Year == Year && x.fld_Kdhdct == KodCutiTahunan.fld_KodCuti).ToList();
                var bakiCuti = CheckPeruntukkan - TakeLeaves.Count;
                if (bakiCuti > 0)
                {
                    TotalPaidLeave3 = LeavePayment * bakiCuti;
                    KerjahdrCutiTahunan.fld_Kadar = LeavePayment;
                    KerjahdrCutiTahunan.fld_KodCuti = KodCutiTahunan.fld_KodCuti;
                    KerjahdrCutiTahunan.fld_Kum = WorkerPaidLeaveLists.Select(s => s.fld_Kum).FirstOrDefault();
                    KerjahdrCutiTahunan.fld_JumlahCuti = CheckPeruntukkan;
                    KerjahdrCutiTahunan.fld_JumlahAmt = TotalPaidLeave3;
                    KerjahdrCutiTahunan.fld_NegaraID = NegaraID;
                    KerjahdrCutiTahunan.fld_SyarikatID = SyarikatID;
                    KerjahdrCutiTahunan.fld_WilayahID = WilayahID;
                    KerjahdrCutiTahunan.fld_LadangID = LadangID;
                    KerjahdrCutiTahunan.fld_Nopkj = NoPkj;
                    KerjahdrCutiTahunan.fld_Month = Month;
                    KerjahdrCutiTahunan.fld_Year = Year;
                    KerjahdrCutiTahunan.fld_CreatedBy = UserID;
                    KerjahdrCutiTahunan.fld_CreatedDT = DTProcess;
                    KerjahdrCutiTahunan.fld_StatusAmbil = false;

                    db2.tbl_KerjahdrCutiTahunan.Add(KerjahdrCutiTahunan);
                    await db2.SaveChangesAsync();
                }
            }

            // cuti tahunan sahaja
            //Modified by Shah 23.12.2023 - Baki Cuti Tahunan
            else if (GetStatusXActv.Contains(PkjStatus.fld_Sbtakf) == false && PkjStatus.fld_Kdaktf == "1" && Month == 12)
            {
                //var PaidLeaveCode = CutiKategoriList.Where(x => x.fld_WaktuBayaranCuti == 0).Select(s => s.fld_KodCuti).ToList();
                var TakeLeaves = tbl_KerjahdrYearly.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && x.fld_Tarikh.Value.Year == Year && x.fld_Kdhdct == "C02").ToList();
                var PeruntukkanCtTahunan = tbl_CutiPeruntukan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_NoPkj == NoPkj && x.fld_Tahun == Year && x.fld_KodCuti == "C02").Select(s => s.fld_JumlahCuti).FirstOrDefault();

                var bakiCuti = PeruntukkanCtTahunan - TakeLeaves.Count;
                if (bakiCuti > 0)
                {
                    TotalPaidLeave3 = LeavePayment * bakiCuti;
                    KerjahdrCutiTahunan.fld_Kadar = LeavePayment;
                    KerjahdrCutiTahunan.fld_KodCuti = "C99";
                    KerjahdrCutiTahunan.fld_Kum = WorkerPaidLeaveLists.Select(s => s.fld_Kum).FirstOrDefault();
                    KerjahdrCutiTahunan.fld_JumlahCuti = bakiCuti;
                    KerjahdrCutiTahunan.fld_JumlahAmt = TotalPaidLeave3;
                    KerjahdrCutiTahunan.fld_NegaraID = NegaraID;
                    KerjahdrCutiTahunan.fld_SyarikatID = SyarikatID;
                    KerjahdrCutiTahunan.fld_WilayahID = WilayahID;
                    KerjahdrCutiTahunan.fld_LadangID = LadangID;
                    KerjahdrCutiTahunan.fld_Nopkj = NoPkj;
                    KerjahdrCutiTahunan.fld_Month = Month;
                    KerjahdrCutiTahunan.fld_Year = Year;
                    KerjahdrCutiTahunan.fld_CreatedBy = UserID;
                    KerjahdrCutiTahunan.fld_CreatedDT = DTProcess;
                    KerjahdrCutiTahunan.fld_StatusAmbil = false;

                    db2.tbl_KerjahdrCutiTahunan.Add(KerjahdrCutiTahunan);
                    await db2.SaveChangesAsync();
                    await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 26, TotalPaidLeave3, DTProcess, UserID, GajiBulanan);
                }
                else
                {
                    await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 26, 0, DTProcess, UserID, GajiBulanan);
                }
            }
            GajiBulanan = db2.tbl_GajiBulanan.Find(Guid);
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 18, LeavePayment, DTProcess, UserID, GajiBulanan);
            await Step2Func.AddTo_tbl_KerjahdrCuti(NegaraID, SyarikatID, WilayahID, LadangID, KerjahdrCutiList);
            //Modified by Shah 23.12.2023 - Baki Cuti Tahunan
            if (NoLeave)
            {
                TotalPaidLeave = 0;
                TotalPaidLeave2 = 0;
                TotalPaidLeave3 = 0;
            }

            OverAllPaidLeave = TotalPaidLeave + TotalPaidLeave2;

            GajiBulanan = db2.tbl_GajiBulanan.Find(Guid);
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 8, OverAllPaidLeave, DTProcess, UserID, GajiBulanan);

            db2.Dispose();

            return OverAllPaidLeave;
        }

        public async Task<CustMod_KWSP> GetKWSPFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, Guid Guid, string KodCaruman, bool NoKWSP, List<tbl_JenisInsentif> tbl_JenisInsentif, List<tbl_Insentif> tbl_Insentif, List<tbl_Kwsp> tbl_Kwsp)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            decimal? TotalSalaryForKWSP = 0;
            decimal? TotalInsentifEfected = 0;
            decimal? BakiCutiTahunan = 0;
            GajiBulanan = await db2.tbl_GajiBulanan.FindAsync(Guid);
            decimal? KWSPMjk = 0;
            decimal? KWSPPkj = 0;
            if (NoKWSP)
            {
                KWSPMjk = 0;
                KWSPPkj = 0;
            }
            else
            {
                var GetInsetifEffectCode = tbl_JenisInsentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_JenisInsentif == "P" && x.fld_AdaCaruman == true && x.fld_Deleted == false).Select(s => s.fld_KodInsentif).ToArray();
                TotalInsentifEfected = tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && GetInsetifEffectCode.Contains(x.fld_KodInsentif) && x.fld_Deleted == false && x.fld_Month == Month && x.fld_Year == Year).Sum(s => s.fld_NilaiInsentif);
                TotalInsentifEfected = TotalInsentifEfected == null ? 0 : TotalInsentifEfected;
                //Modified by Shah 23.12.2023 - Baki Cuti Tahunan
                //Modified by faeza 09.01.2024
                BakiCutiTahunan = GajiBulanan.fld_BakiCutiTahunan == null ? 0 : GajiBulanan.fld_BakiCutiTahunan;
                TotalSalaryForKWSP = GajiBulanan.fld_ByrKerja + GajiBulanan.fld_ByrCuti + GajiBulanan.fld_BonusHarian + TotalInsentifEfected + GajiBulanan.fld_AIPS + GajiBulanan.fld_ByrKwsnSkr + BakiCutiTahunan;
                //Modified by Shah 23.12.2023 - Baki Cuti Tahunan

                var GetCarumanKWSP = db.tbl_Kwsp.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodCaruman == KodCaruman && TotalSalaryForKWSP >= x.fld_KdrLower && TotalSalaryForKWSP <= x.fld_KdrUpper).FirstOrDefault();
                if (GetCarumanKWSP != null)
                {
                    KWSPMjk = GetCarumanKWSP.fld_Mjkn;
                    KWSPPkj = GetCarumanKWSP.fld_Pkj;
                }
                else
                {
                    KWSPMjk = 0;
                    KWSPPkj = 0;
                }
            }

            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 9, KWSPMjk, DTProcess, UserID, GajiBulanan);
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 10, KWSPPkj, DTProcess, UserID, GajiBulanan);

            var CustMod_KWSP = new CustMod_KWSP
            {
                KWSPMjk = KWSPMjk,
                KWSPPkj = KWSPPkj
            };
            db2.Dispose();
            db.Dispose();
            return CustMod_KWSP;
        }

        public async Task<CustMod_Socso> GetSocsoFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, Guid Guid, string KodCaruman, bool NoSocso, List<tbl_JenisInsentif> tbl_JenisInsentif, List<tbl_Insentif> tbl_Insentif, List<tbl_Socso> tbl_Socso)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            decimal? TotalSalaryForSocso = 0;
            decimal? TotalInsentifEfected = 0;
            decimal? BakiCutiTahunan = 0;
            GajiBulanan = await db2.tbl_GajiBulanan.FindAsync(Guid);
            decimal? SocsoMjk = 0;
            decimal? SocsoPkj = 0;
            if (NoSocso)
            {
                SocsoMjk = 0;
                SocsoPkj = 0;
            }
            else
            {
                var GetInsetifEffectCode = tbl_JenisInsentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_JenisInsentif == "P" && x.fld_AdaCaruman == true && x.fld_Deleted == false).Select(s => s.fld_KodInsentif).ToArray();
                TotalInsentifEfected = tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && GetInsetifEffectCode.Contains(x.fld_KodInsentif) && x.fld_Deleted == false && x.fld_Month == Month && x.fld_Year == Year).Sum(s => s.fld_NilaiInsentif);
                TotalInsentifEfected = TotalInsentifEfected == null ? 0 : TotalInsentifEfected;
                //Modified by Shah 23.12.2023 - Baki Cuti Tahunan
                //modified by faeza 09.01.2024
                BakiCutiTahunan = GajiBulanan.fld_BakiCutiTahunan == null ? 0 : GajiBulanan.fld_BakiCutiTahunan;
                TotalSalaryForSocso = GajiBulanan.fld_ByrKerja + GajiBulanan.fld_ByrCuti + GajiBulanan.fld_OT + TotalInsentifEfected + GajiBulanan.fld_AIPS + GajiBulanan.fld_ByrKwsnSkr + GajiBulanan.fld_BonusHarian + BakiCutiTahunan;
                //Modified by Shah 23.12.2023 - Baki Cuti Tahunan

                var GetCarumanSocso = db.tbl_Socso.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodCaruman == KodCaruman && TotalSalaryForSocso >= x.fld_KdrLower && TotalSalaryForSocso <= x.fld_KdrUpper).FirstOrDefault();
                if (GetCarumanSocso != null)
                {
                    SocsoMjk = GetCarumanSocso.fld_SocsoMjkn;
                    SocsoPkj = GetCarumanSocso.fld_SocsoPkj;
                }
                else
                {
                    SocsoMjk = 0;
                    SocsoPkj = 0;
                }
            }

            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 11, SocsoMjk, DTProcess, UserID, GajiBulanan);
            await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 12, SocsoPkj, DTProcess, UserID, GajiBulanan);

            var CustMod_Socso = new CustMod_Socso
            {
                SocsoMjk = SocsoMjk,
                SocsoPkj = SocsoPkj
            };
            db2.Dispose();
            db.Dispose();
            return CustMod_Socso;
        }

        public async Task<CustMod_OthrCon> GetOtherContributionsFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, Guid Guid, List<tbl_PkjCarumanTambahan> tbl_PkjCarumanTambahan, List<tbl_JenisInsentif> tbl_JenisInsentif, List<tbl_Insentif> tbl_Insentif, List<tbl_CarumanTambahan> tbl_CarumanTambahan, List<tbl_SubCarumanTambahan> tbl_SubCarumanTambahan, List<tbl_JadualCarumanTambahan> tbl_JadualCarumanTambahanList, List<tbl_TaxRelief> tbl_TaxRelief, tbl_TaxWorkerInfo tbl_TaxWorkerInfo)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            GetConnectFunc conn = new GetConnectFunc();
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            List<tbl_ByrCarumanTambahan> ByrCarumanTambahanList = new List<tbl_ByrCarumanTambahan>();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);
            tbl_JadualCarumanTambahan GetContributionAmnt = new tbl_JadualCarumanTambahan();

            decimal? TotalMjkCont = 0;
            decimal? TotalPkjCont = 0;
            TotalMjkCont = 0;
            TotalPkjCont = 0;
            decimal? TotalSalaryForOtherContribution = 0;
            decimal? TotalInsentifEfected = 0;
            decimal? BakiCutiTahunan = 0;
            GajiBulanan = await db2.tbl_GajiBulanan.FindAsync(Guid);

            var GetOtherContributions = tbl_PkjCarumanTambahan.Where(x => x.fld_Nopkj == NoPkj && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Deleted == false).ToList();

            var GetInsetifEffectCode = tbl_JenisInsentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_JenisInsentif == "P" && x.fld_AdaCaruman == true && x.fld_Deleted == false).Select(s => s.fld_KodInsentif).ToArray();
            TotalInsentifEfected = tbl_Insentif.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_Nopkj == NoPkj && GetInsetifEffectCode.Contains(x.fld_KodInsentif) && x.fld_Deleted == false && x.fld_Month == Month && x.fld_Year == Year).Sum(s => s.fld_NilaiInsentif);
            TotalInsentifEfected = TotalInsentifEfected == null ? 0 : TotalInsentifEfected;
            //Modified by Shah 23.12.2023 - Baki Cuti Tahunan
            //modified by faeza 09.01.2024
            BakiCutiTahunan = GajiBulanan.fld_BakiCutiTahunan == null ? 0 : GajiBulanan.fld_BakiCutiTahunan;
            TotalSalaryForOtherContribution = GajiBulanan.fld_ByrKerja + GajiBulanan.fld_ByrCuti + GajiBulanan.fld_OT + TotalInsentifEfected + GajiBulanan.fld_AIPS + GajiBulanan.fld_ByrKwsnSkr + GajiBulanan.fld_BonusHarian + BakiCutiTahunan;
            //Modified by Shah 23.12.2023 - Baki Cuti Tahunan

            decimal? ContriMjk = 0;
            decimal? ContriPkj = 0;
            foreach (var GetOtherContribution in GetOtherContributions)
            {
                //Added by Shah 01_01_2024
                if (GetOtherContribution.fld_KodCaruman == "PCB" && GetOtherContribution.fld_KodSubCaruman == "PCB02")
                {
                    if (tbl_TaxWorkerInfo.fld_TaxResidency == "1")
                    {
                        var noPkjPermanent = tbl_Pkjmasts.Where(x => x.fld_Nopkj == NoPkj).Select(s => s.fld_NopkjPermanent).FirstOrDefault();
                        var tbl_GajiBulanan = db2.tbl_GajiBulanan.Where(x => x.fld_NopkjPermanent == noPkjPermanent && x.fld_Year == Year).ToList();
                        var byrCarumanTambahan = PCBResident(tbl_GajiBulanan, Month, Year, tbl_TaxRelief, tbl_TaxWorkerInfo, tbl_JadualCarumanTambahanList, ByrCarumanTambahanList, false, Guid, GetOtherContribution, NegaraID, SyarikatID, WilayahID, LadangID, db2);
                        ByrCarumanTambahanList.Add(byrCarumanTambahan);
                    }
                    else
                    {
                        var kadar = tbl_SubCarumanTambahan.Where(x => x.fld_KodCaruman == GetOtherContribution.fld_KodCaruman && x.fld_KodSubCaruman == GetOtherContribution.fld_KodSubCaruman).Select(s => s.fld_KadarPekerja).FirstOrDefault();
                        ContriPkj = kadar * TotalSalaryForOtherContribution;
                        ContriPkj = Round(ContriPkj.Value);
                        ByrCarumanTambahanList.Add(new tbl_ByrCarumanTambahan() { fld_GajiID = Guid, fld_KodCaruman = GetOtherContribution.fld_KodCaruman, fld_KodSubCaruman = GetOtherContribution.fld_KodSubCaruman, fld_CarumanPekerja = ContriPkj, fld_CarumanMajikan = 0, fld_Month = Month, fld_Year = Year, fld_LadangID = LadangID, fld_WilayahID = WilayahID, fld_SyarikatID = SyarikatID, fld_NegaraID = NegaraID });
                    }
                    //Added by Shah 01_01_2024
                }
                else
                {
                    ContriMjk = 0;
                    ContriPkj = 0;
                    var GetContributionDetail = tbl_CarumanTambahan.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodCaruman == GetOtherContribution.fld_KodCaruman).FirstOrDefault();
                    var GetSubContributionDetail = tbl_SubCarumanTambahan.Where(x => x.fld_KodSubCaruman == GetOtherContribution.fld_KodSubCaruman && x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID).FirstOrDefault();
                    GetContributionAmnt = tbl_JadualCarumanTambahanList.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_KodSubCaruman == GetOtherContribution.fld_KodSubCaruman && TotalSalaryForOtherContribution >= x.fld_GajiLower && TotalSalaryForOtherContribution <= x.fld_GajiUpper).FirstOrDefault();
                    if (GetContributionDetail.fld_Berjadual == true)
                    {
                        switch (GetContributionDetail.fld_CarumanOleh)
                        {
                            case 1:
                                ContriPkj = GetContributionAmnt.fld_CarumanPekerja;
                                break;
                            case 2:
                                ContriMjk = GetContributionAmnt.fld_CarumanMajikan;
                                break;
                            case 3:
                                ContriPkj = GetContributionAmnt.fld_CarumanPekerja;
                                ContriMjk = GetContributionAmnt.fld_CarumanMajikan;
                                break;
                        }
                    }
                    else
                    {
                        switch (GetContributionDetail.fld_CarumanOleh)
                        {
                            case 1:
                                ContriPkj = TotalSalaryForOtherContribution * GetSubContributionDetail.fld_KadarPekerja;
                                break;
                            case 2:
                                ContriMjk = TotalSalaryForOtherContribution * GetSubContributionDetail.fld_KadarMajikan;
                                break;
                            case 3:
                                ContriPkj = TotalSalaryForOtherContribution * GetSubContributionDetail.fld_KadarPekerja;
                                ContriMjk = TotalSalaryForOtherContribution * GetSubContributionDetail.fld_KadarMajikan;
                                break;
                        }
                    }
                    ByrCarumanTambahanList.Add(new tbl_ByrCarumanTambahan() { fld_GajiID = Guid, fld_KodCaruman = GetOtherContribution.fld_KodCaruman, fld_KodSubCaruman = GetOtherContribution.fld_KodSubCaruman, fld_CarumanPekerja = ContriPkj, fld_CarumanMajikan = ContriMjk, fld_Month = Month, fld_Year = Year, fld_LadangID = LadangID, fld_WilayahID = WilayahID, fld_SyarikatID = SyarikatID, fld_NegaraID = NegaraID });
                }
            }

            if (ByrCarumanTambahanList.Count > 0)
            {
                db2.tbl_ByrCarumanTambahan.AddRange(ByrCarumanTambahanList);
                await db2.SaveChangesAsync();

                TotalMjkCont = ByrCarumanTambahanList.Sum(s => s.fld_CarumanMajikan);
                TotalPkjCont = ByrCarumanTambahanList.Sum(s => s.fld_CarumanPekerja);
            }
            var CustMod_OthrCon = new CustMod_OthrCon
            {
                TotalMjkCont = TotalMjkCont,
                TotalPkjCont = TotalPkjCont
            };
            db2.Dispose();
            db.Dispose();
            return CustMod_OthrCon;
        }

        //Added by Shah 01_01_2024
        public tbl_ByrCarumanTambahan PCBResident(List<tbl_GajiBulanan> tbl_GajiBulanan, int? month, int? year, List<tbl_TaxRelief> tbl_TaxRelief, tbl_TaxWorkerInfo tbl_TaxWorkerInfo, List<tbl_JadualCarumanTambahan> tbl_JadualCarumanTambahan, List<tbl_ByrCarumanTambahan> tbl_ByrCarumanTambahan, bool isAdditional, Guid Guid, tbl_PkjCarumanTambahan GetOtherContribution, int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, GenSalaryModelEstate db2)
        {
            string maritulStatus = tbl_TaxWorkerInfo.fld_TaxMaritalStatus;
            decimal? KLimit = tbl_TaxRelief.Where(x => x.fld_VariableCode == "K").Select(s => s.fld_TaxReliefLimit).FirstOrDefault();
            decimal? K = tbl_GajiBulanan.Sum(s => s.fld_KWSPPkj) > KLimit ? KLimit : tbl_GajiBulanan.Sum(s => s.fld_KWSPPkj);
            decimal? K1 = tbl_GajiBulanan.Where(x => x.fld_Month == month).Select(s => s.fld_KWSPPkj).FirstOrDefault();
            decimal? Kt = 0;
            decimal? n = 12m - decimal.Parse(month.ToString());
            decimal? n1 = 12m - decimal.Parse(month.ToString()) + 1;
            decimal? K2 = (KLimit - (K + K1 + Kt)) / n;
            K2 = K2 > K1 ? K1 : K2;

            decimal? Y = tbl_GajiBulanan.Sum(s => s.fld_GajiKasar);
            decimal? Y1 = tbl_GajiBulanan.Where(x => x.fld_Month == month).Select(s => s.fld_GajiKasar).FirstOrDefault();
            decimal? Y2 = Y1;
            decimal? Yt = 0;

            decimal? D = tbl_TaxRelief.Where(x => x.fld_VariableCode == "D").Select(s => s.fld_TaxReliefLimit).FirstOrDefault();
            decimal? Du = tbl_TaxWorkerInfo.fld_IsIndividuOKU == "1" ? tbl_TaxRelief.Where(x => x.fld_VariableCode == "DU").Select(s => s.fld_TaxReliefLimit).FirstOrDefault() : 0;
            decimal? Su = tbl_TaxWorkerInfo.fld_IsSpouseOKU == "1" ? tbl_TaxRelief.Where(x => x.fld_VariableCode == "SU").Select(s => s.fld_TaxReliefLimit).FirstOrDefault() : 0;
            decimal? S = 0;
            decimal? C = 0;
            decimal? Q = tbl_TaxRelief.Where(x => x.fld_VariableCode == "Q").Select(s => s.fld_TaxReliefLimit).FirstOrDefault();
            decimal? QC = 0;
            decimal? LP = 0;
            decimal? LP1 = 0;
            decimal? P = 0;
            decimal? M = 0;
            decimal? R = 0;
            decimal? B = 0;
            decimal? Z = 0;
            decimal? X = 0;
            decimal? PCBY = 0;
            decimal? PCBM = 0;
            decimal? PCB = 0;

            decimal? CB18F = tbl_TaxWorkerInfo.fld_ChildBelow18Full;
            decimal? CB18H = tbl_TaxWorkerInfo.fld_ChildBelow18Half * (decimal)0.5;

            decimal? CA18CF = tbl_TaxWorkerInfo.fld_ChildAbove18CertFull;
            decimal? CA18CH = tbl_TaxWorkerInfo.fld_ChildAbove18CertHalf * (decimal)0.5;

            decimal? CA18HF = tbl_TaxWorkerInfo.fld_ChildAbove18HigherFull * (decimal)4;
            decimal? CA18HH = tbl_TaxWorkerInfo.fld_ChildAbove18HigherHalf * (decimal)2;

            decimal? DCHF = tbl_TaxWorkerInfo.fld_DisabledChildFull * (decimal)3;
            decimal? DCHH = tbl_TaxWorkerInfo.fld_DisabledChildHalf * (decimal)1.5;

            decimal? DCHSF = tbl_TaxWorkerInfo.fld_DisabledChildStudyFull * (decimal)7;
            decimal? DCHSH = tbl_TaxWorkerInfo.fld_DisabledChildStudyHalf * (decimal)3.5;

            decimal? totalFull = CB18F + CA18CF + CA18HF + DCHF + DCHSF;
            decimal? totalHalf = CB18H + CA18CH + CA18HH + DCHH + DCHSH;

            if (maritulStatus == "2")
            {
                S = tbl_TaxRelief.Where(x => x.fld_VariableCode == "S").Select(s => s.fld_TaxReliefLimit).FirstOrDefault();
                C = totalFull + totalHalf;
                QC = Q * C;
            }
            else if (maritulStatus == "3" || maritulStatus == "4")
            {
                C = totalFull + totalHalf;
                QC = Q * C;
            }

            P = ((Y - K) + (Y1 - K1) + ((Y2 - K2) * n) + (Yt - Kt)) - (D + S + Du + Su + QC + (LP + LP1));
            var MRB = tbl_JadualCarumanTambahan.Where(x => x.fld_GajiUpper >= P && x.fld_GajiLower <= P && x.fld_KodSubCaruman == GetOtherContribution.fld_KodSubCaruman).FirstOrDefault();
            M = MRB.fld_CarumanPekerja;
            R = MRB.fld_TaxPercent_R;
            if (maritulStatus == "1" || maritulStatus == "3" || maritulStatus == "4")
            {
                B = MRB.fld_Category1_B;
            }
            else if (maritulStatus == "2")
            {
                B = MRB.fld_Category2_B;
            }
            X = tbl_ByrCarumanTambahan.Sum(s => s.fld_CarumanPekerja);

            PCBM = (((P - M) * R + B) - (Z + X)) / (n + 1);
            PCBM = PCBM < 0 ? 0 : PCBM;
            PCBM = Round(PCBM.Value);
            PCB = PCBM - Z;
            PCB = PCB < 0 ? 0 : PCB;
            PCB = Round(PCB.Value);
            PCBY = ((P - M) * R + B);
            PCBY = PCBY < 0 ? 0 : PCBY;
            PCBY = Round(PCBY.Value);

            var byrCarumanTambahan = new tbl_ByrCarumanTambahan
            {
                fld_GajiID = Guid,
                fld_KodCaruman = GetOtherContribution.fld_KodCaruman,
                fld_KodSubCaruman = GetOtherContribution.fld_KodSubCaruman,
                fld_CarumanPekerja = PCBM,
                fld_CarumanMajikan = 0,
                fld_Month = month,
                fld_Year = year,
                fld_LadangID = LadangID,
                fld_WilayahID = WilayahID,
                fld_SyarikatID = SyarikatID,
                fld_NegaraID = NegaraID,
                fld_B = B,
                fld_C = int.Parse(C.ToString()),
                fld_CarumanPekerjaNet = PCB,
                fld_CarumanPekerjaYearly = PCBY,
                fld_D = D,
                fld_Du = Du,
                fld_K = K,
                fld_K1 = K1,
                fld_K2 = K2,
                fld_Kt = Kt,
                fld_LP = LP,
                fld_LP1 = LP1,
                fld_M = M,
                fld_n = int.Parse(n.ToString()),
                fld_n1 = int.Parse(n1.ToString()),
                fld_P = P,
                fld_Q = Q,
                fld_R = R,
                fld_S = S,
                fld_Su = Su,
                fld_X = X,
                fld_Y = Y,
                fld_Y1 = Y1,
                fld_Y2 = Y2,
                fld_Yt = Yt,
                fld_Z = Z,
                fld_ChildAbove18CertFull = tbl_TaxWorkerInfo.fld_ChildAbove18CertFull,
                fld_ChildAbove18CertHalf = tbl_TaxWorkerInfo.fld_ChildAbove18CertHalf,
                fld_ChildAbove18HigherFull = tbl_TaxWorkerInfo.fld_ChildAbove18HigherFull,
                fld_ChildAbove18HigherHalf = tbl_TaxWorkerInfo.fld_ChildAbove18HigherHalf,
                fld_ChildBelow18Full = tbl_TaxWorkerInfo.fld_ChildBelow18Full,
                fld_ChildBelow18Half = tbl_TaxWorkerInfo.fld_ChildBelow18Half,
                fld_DisabledChildFull = tbl_TaxWorkerInfo.fld_DisabledChildFull,
                fld_DisabledChildHalf = tbl_TaxWorkerInfo.fld_DisabledChildHalf,
                fld_DisabledChildStudyFull = tbl_TaxWorkerInfo.fld_DisabledChildStudyFull,
                fld_DisabledChildStudyHalf = tbl_TaxWorkerInfo.fld_DisabledChildStudyHalf,
                fld_IsIndividuOKU = tbl_TaxWorkerInfo.fld_IsIndividuOKU,
                fld_IsSpouseOKU = tbl_TaxWorkerInfo.fld_IsSpouseOKU,
                fld_TaxMaritalStatus = tbl_TaxWorkerInfo.fld_TaxMaritalStatus
            };
            return byrCarumanTambahan;
        }
        public static decimal Round(decimal value)
        {
            var ceiling = Math.Ceiling(value * 20);
            if (ceiling == 0)
            {
                return 0;
            }
            return ceiling / 20;
        }
        //Added by Shah 01_01_2024

        public async Task<CustMod_OverallSlry> GetOverallSalaryFunc(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, Guid Guid, List<tbl_JenisInsentif> tbl_JenisInsentif, List<tbl_Insentif> tbl_Insentif, List<tblOptionConfigsWeb> tblOptionConfigsWebs, List<tbl_HutangPekerjaJumlah> tbl_HutangPekerjaJumlah, bool isToGetRemuneration)
        {
            GetConnectFunc conn = new GetConnectFunc();
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            decimal? OtherContr = 0;
            decimal? TotalDebtDeduction = 0;
            decimal? BakiCutiTahunan = 0;
            GajiBulanan = await db2.tbl_GajiBulanan.FindAsync(Guid);
            //Modified by faeza 09.01.2024
            BakiCutiTahunan = GajiBulanan.fld_BakiCutiTahunan == null ? 0 : GajiBulanan.fld_BakiCutiTahunan;
            decimal? OverallSalary = GajiBulanan.fld_ByrKerja + GajiBulanan.fld_BonusHarian + GajiBulanan.fld_ByrCuti + GajiBulanan.fld_LainInsentif + GajiBulanan.fld_AIPS + GajiBulanan.fld_OT + BakiCutiTahunan; // + GajiBulanan.fld_ByrKwsnSkr; + GajiBulanan.fld_KWSPMjk + GajiBulanan.fld_SocsoMjk;
            OtherContr = db2.tbl_ByrCarumanTambahan.Where(x => x.fld_GajiID == Guid).Sum(s => s.fld_CarumanPekerja);
            if (OtherContr == null)
            {
                OtherContr = 0;
            }
            decimal? Salary = (OverallSalary) - (GajiBulanan.fld_KWSPPkj + GajiBulanan.fld_SocsoPkj + GajiBulanan.fld_LainPotongan + OtherContr);
            var HutangPekerja = tbl_HutangPekerjaJumlah.Where(x => x.fld_NegaraID == NegaraID && x.fld_SyarikatID == SyarikatID && x.fld_WilayahID == WilayahID && x.fld_LadangID == LadangID && x.fld_NoPkj == NoPkj).FirstOrDefault();
            if (HutangPekerja != null)
            {
                //if (HutangPekerja.fld_JumlahHutang > HutangPekerja.fld_JumlahBayar)
                //{
                TotalDebtDeduction = await GetCompanyDeductFunc(NegaraID, SyarikatID, WilayahID, LadangID, UserID, DTProcess, Month, Year, processname, servicesname, ClientID, NoPkj, Guid, Salary, HutangPekerja, tbl_JenisInsentif, tbl_Insentif, tblOptionConfigsWebs);
                Salary = Salary - TotalDebtDeduction;
                //}
            }
            await db2.Entry(GajiBulanan).ReloadAsync();
            GajiBulanan = await db2.tbl_GajiBulanan.FindAsync(Guid);
            if (isToGetRemuneration)
            {
                await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 13, OverallSalary, DTProcess, UserID, GajiBulanan);
            }
            else
            {
                await AddTo_tbl_GajiBulanan(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 14, Salary, DTProcess, UserID, GajiBulanan);
            }
            var CustMod_OverallSlry = new CustMod_OverallSlry
            {
                OverallSalary = OverallSalary,
                Salary = Salary,
                TotalDebtDeduction = TotalDebtDeduction
            };
            db2.Dispose();
            return CustMod_OverallSlry;
        }

        //added by faeza 09.06.2021
        public async Task UpdatePaymentMode(int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? UserID, DateTime DTProcess, int? Month, int? Year, string processname, string servicesname, int? ClientID, string NoPkj, string PaymentMode, Guid Guid)
        {
            GetConnectFunc conn = new GetConnectFunc();
            tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            string host, catalog, user, pass = "";
            conn.GetConnection(out host, out catalog, out user, out pass, WilayahID, SyarikatID, NegaraID);
            GenSalaryModelEstate db2 = GenSalaryModelEstate.ConnectToSqlServer(host, catalog, user, pass);

            GajiBulanan = db2.tbl_GajiBulanan.Find(Guid);
            await AddTo_tbl_GajiBulanan3(db2, NegaraID, SyarikatID, WilayahID, LadangID, Month, Year, NoPkj, 1, PaymentMode, DTProcess, UserID, GajiBulanan);

            db2.Dispose();
        }

        public async Task<Guid> AddTo_tbl_GajiBulanan(GenSalaryModelEstate db, int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? Month, int? Year, string NoPkj, byte? UpdateFlag, decimal? PaymentAmount, DateTime DTProcess, int? UserID, tbl_GajiBulanan GajiBulanan)
        {
            // UpdateFlag = 1 - Add Gaji bulanan utk Byran Kerja, 
            //tbl_GajiBulanan GajiBulanan = new tbl_GajiBulanan();
            Guid MonthSalaryID = new Guid();

            //GajiBulanan = UpdateFlag >= 2 ? db.tbl_GajiBulanan.Find(Guid) : GajiBulanan;

            switch (UpdateFlag)
            {
                case 1: // untuk add new gaji kerja harian
                    GajiBulanan.fld_NegaraID = NegaraID;
                    GajiBulanan.fld_SyarikatID = SyarikatID;
                    GajiBulanan.fld_WilayahID = WilayahID;
                    GajiBulanan.fld_LadangID = LadangID;
                    GajiBulanan.fld_Nopkj = NoPkj;
                    GajiBulanan.fld_DTCreated = DTProcess;
                    GajiBulanan.fld_CreatedBy = UserID;
                    GajiBulanan.fld_ByrKerja = PaymentAmount;
                    GajiBulanan.fld_Month = Month;
                    GajiBulanan.fld_Year = Year;
                    GajiBulanan.fld_NopkjPermanent = tbl_Pkjmasts.Where(x => x.fld_Nopkj == NoPkj).Select(s => s.fld_NopkjPermanent).FirstOrDefault();
                    db.tbl_GajiBulanan.Add(GajiBulanan);
                    await db.SaveChangesAsync();
                    MonthSalaryID = GajiBulanan.fld_ID;
                    break;
                case 2: // untuk add bonus harian
                    GajiBulanan.fld_BonusHarian = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
                case 3: // untuk add ot harian
                    GajiBulanan.fld_OT = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
                case 4: // untuk add Lain - lain insentif
                    GajiBulanan.fld_LainInsentif = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
                case 5: // untuk add Potongan insentif
                    GajiBulanan.fld_LainPotongan = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
                case 6: // untuk add Purata gaji
                    GajiBulanan.fld_PurataGaji = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
                case 7: // untuk add AIPS insentif
                    GajiBulanan.fld_AIPS = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
                case 8: // untuk add Leave Payment
                    GajiBulanan.fld_ByrCuti = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
                case 9: // untuk add KWSP Mjkn
                    GajiBulanan.fld_KWSPMjk = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
                case 10: // untuk add KWSP Pkj
                    GajiBulanan.fld_KWSPPkj = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
                case 11: // untuk add Socso Mjkn
                    GajiBulanan.fld_SocsoMjk = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
                case 12: // untuk add Socso Pkj
                    GajiBulanan.fld_SocsoPkj = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
                case 13: // untuk add Gaji Kasar
                    GajiBulanan.fld_GajiKasar = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
                case 14: // untuk add Gaji Bersih
                    GajiBulanan.fld_GajiBersih = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
                case 15: // untuk add Kehadiran Insentif
                    GajiBulanan.fld_HdrInsentif = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
                case 16: // untuk add Kualiti Insentif
                    GajiBulanan.fld_KuaInsentif = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
                case 17: // untuk add Produktiviti Insentif
                    GajiBulanan.fld_ProdInsentif = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;

                case 18: // untuk add Purata 12 bulan Gaji
                    GajiBulanan.fld_PurataGaji12Bln = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;

                case 19: // untuk add Bayar Kawasan Sukar
                    GajiBulanan.fld_ByrKwsnSkr = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;

                //modify by Faeza on 25/2/2020
                //calc average salary rate - ORP
                case 20: // untuk add ORP
                    GajiBulanan.fld_PurataGajiORP = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;

                case 21: // untuk add Purata 12 bulan Gaji ORP
                    GajiBulanan.fld_PurataGajiORP12Bln = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;

                case 22: // untuk add bayar kerja ORP
                    GajiBulanan.fld_ByrKerjaORP = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;

                case 23: // untuk add bonus harian ORP
                    GajiBulanan.fld_BonusHarianORP = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;

                case 24: // untuk add insentif ORP
                    GajiBulanan.fld_LainInsentifORP = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;

                case 25: // untuk add total bayar kerja ORP
                    GajiBulanan.fld_TotalByrKerjaORP = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
                //Modified by Shah 23.12.2023 - Baki Cuti Tahunan
                case 26: // untuk baki cuti tahunan
                    GajiBulanan.fld_BakiCutiTahunan = PaymentAmount;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
                    //Modified by Shah 23.12.2023 - Baki Cuti Tahunan
            }

            return MonthSalaryID;
        }

        public async Task<Guid> AddTo_tbl_GajiBulanan2(GenSalaryModelEstate db, byte? UpdateFlag, int? AttTarget, int? AttCapai, short? QuaTarget, short? QuaCapai, decimal? ProdTarget, decimal? ProdCapai, tbl_GajiBulanan GajiBulanan)
        {
            Guid MonthSalaryID = new Guid();
            switch (UpdateFlag)
            {
                case 1: // untuk add target capai AIPS
                    GajiBulanan.fld_HdrTarget = AttTarget;
                    GajiBulanan.fld_HdrCapai = AttCapai;
                    GajiBulanan.fld_KuaTarget = QuaTarget;
                    GajiBulanan.fld_KuaCapai = QuaCapai;
                    GajiBulanan.fld_TargetProd = ProdTarget;
                    GajiBulanan.fld_CapaiProd = ProdCapai;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
            }

            return MonthSalaryID;
        }

        public async Task<Guid> AddTo_tbl_GajiBulanan3(GenSalaryModelEstate db, int? NegaraID, int? SyarikatID, int? WilayahID, int? LadangID, int? Month, int? Year, string NoPkj, byte? UpdateFlag, string PaymentMode, DateTime DTProcess, int? UserID, tbl_GajiBulanan GajiBulanan)
        {
            Guid MonthSalaryID = new Guid();
            switch (UpdateFlag)
            {
                case 1: // update payment mode
                    GajiBulanan.fld_PaymentMode = PaymentMode;
                    db.Entry(GajiBulanan).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    break;
            }

            return MonthSalaryID;
        }

    }
}
