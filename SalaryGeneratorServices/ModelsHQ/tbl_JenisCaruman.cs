namespace SalaryGeneratorServices.ModelsHQ
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_JenisCaruman
    {
        [Key]
        public int fld_JenisCarumanID { get; set; }

        [StringLength(5)]
        public string fld_KodCaruman { get; set; }

        [StringLength(30)]
        public string fld_JenisCaruman { get; set; }

        public int? fld_UmurLower { get; set; }

        public int? fld_UmurUpper { get; set; }

        public decimal? fld_PeratusCarumanPekerja { get; set; }

        public decimal? fld_PeratusCarumanMajikanBawah5000 { get; set; }

        public decimal? fld_PeratusCarumanMajikanAtas5000 { get; set; }

        [StringLength(70)]
        public string fld_Keterangan { get; set; }

        public bool? fld_Deleted { get; set; }

        public int? fldSyarikatID { get; set; }

        public int? fldNegaraID { get; set; }

        public bool? fld_Default { get; set; }
    }
}
