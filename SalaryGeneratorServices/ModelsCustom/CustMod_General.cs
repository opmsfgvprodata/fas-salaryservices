using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryGeneratorServices.ModelsCustom
{
    class CustMod_PaidWorking
    {
        public Guid SalaryID { get; set; }
        public decimal? WorkingPayment { get; set; }
        public decimal? DiffAreaPayment { get; set; }
    }

    class CustMod_KWSP
    {
        public decimal? KWSPMjk { get; set; }
        public decimal? KWSPPkj { get; set; }
    }
    class CustMod_Socso
    {
        public decimal? SocsoMjk { get; set; }
        public decimal? SocsoPkj { get; set; }
    }
    class CustMod_OthrCon
    {
        public decimal? TotalMjkCont { get; set; }
        public decimal? TotalPkjCont { get; set; }
    }
    class CustMod_OverallSlry
    {
        public decimal? OverallSalary { get; set; }
        public decimal? Salary { get; set; }
        public decimal? TotalDebtDeduction { get; set; }
    }
}
