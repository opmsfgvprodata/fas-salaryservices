using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SalaryGeneratorServices.CustomModels
{
    public partial class CustMod_WorkerPaidLeave
    {
        [StringLength(10)]
        public string fld_Nopkj { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fld_Tarikh { get; set; }

        [StringLength(3)]
        public string fld_Kdhdct { get; set; }

        public byte? fld_PaidPeriod { get; set; }

        [StringLength(50)]
        public string fld_Kum { get; set; }

        public Guid fld_KerjahdrID { get; set; }
    }
}