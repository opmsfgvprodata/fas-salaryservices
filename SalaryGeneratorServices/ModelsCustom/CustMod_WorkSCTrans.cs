using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SalaryGeneratorServices.CustomModels
{
    public partial class CustMod_WorkSCTrans
    {
        [Key]
        public int fld_ID { get; set; }

        public byte? fld_JnsPkt { get; set; }

        [StringLength(10)]
        public string fld_KodPkt { get; set; }

        [StringLength(2)]
        public string fld_JnisAktvt { get; set; }

        [StringLength(4)]
        public string fld_KodAktvt { get; set; }

        [StringLength(5)]
        public string fld_KodGL { get; set; }
        
        public string fld_SAPGL { get; set; }

        public string fld_SAPIO { get; set; }

        public string fld_NNCC { get; set; }

        public string fld_SapActCode { get; set; }

        public string fld_Keterangan { get; set; }
    }
}