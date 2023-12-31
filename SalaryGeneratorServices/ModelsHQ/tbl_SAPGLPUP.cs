namespace SalaryGeneratorServices.ModelsHQ
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_SAPGLPUP
    {
        [Key]
        public Guid fld_ID { get; set; }

        [StringLength(4)]
        public string fld_CompanyCode { get; set; }

        [StringLength(10)]
        public string fld_GLCode { get; set; }

        [StringLength(50)]
        public string fld_GLDesc { get; set; }

        public int? fld_NegaraID { get; set; }

        public int? fld_SyarikatID { get; set; }

        public int? fld_WilayahID { get; set; }

        public int? fld_LadangID { get; set; }

        public bool? fld_IsSelected { get; set; }

        public bool? fld_Deleted { get; set; }

        [StringLength(50)]
        public string fld_CreatedBy { get; set; }

        public DateTime? fld_CreatedDT { get; set; }

        public DateTime? fld_ModifiedDt { get; set; }

        [StringLength(50)]
        public string fld_ModifiedBy { get; set; }
    }
}
