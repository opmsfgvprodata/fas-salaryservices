namespace SalaryGeneratorServices.ModelsHQ
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Syarikat
    {
        [Key]
        public int fld_SyarikatID { get; set; }

        [StringLength(100)]
        public string fld_NamaSyarikat { get; set; }

        [StringLength(20)]
        public string fld_NamaPndkSyarikat { get; set; }

        public int? fld_NegaraID { get; set; }

        public bool? fld_Deleted { get; set; }

        [StringLength(50)]
        public string fld_NoSyarikat { get; set; }

        [StringLength(50)]
        public string fld_LogoName { get; set; }

        [StringLength(100)]
        public string fld_SyarikatEmail { get; set; }

        [StringLength(10)]
        public string fld_FrstNmeUsrNme { get; set; }

        [StringLength(10)]
        public string fld_RequestCode { get; set; }

        [StringLength(4)]
        public string fld_SAPComCode { get; set; }
    }
}
