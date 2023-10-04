namespace SalaryGeneratorServices.ModelsHQ
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_MingguNegeri
    {
        [Key]
        public short fld_MingguNegeriID { get; set; }

        public short? fld_JenisMinggu { get; set; }

        public int? fld_NegeriID { get; set; }

        public int? fld_NegaraID { get; set; }

        public int? fld_SyarikatID { get; set; }

        public bool? fld_Deleted { get; set; }
    }
}
