namespace SalaryGeneratorServices.ModelsHQ
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_SubCarumanTambahan
    {
        [Key]
        public int fld_JenisSubCarumanID { get; set; }

        [Required]
        [StringLength(10)]
        public string fld_KodCaruman { get; set; }

        [Required]
        [StringLength(10)]
        public string fld_KodSubCaruman { get; set; }

        [Required]
        [StringLength(30)]
        public string fld_KeteranganSubCaruman { get; set; }

        public int fld_UmurLower { get; set; }

        public int fld_UmurUpper { get; set; }

        public decimal fld_KadarMajikan { get; set; }

        public decimal fld_KadarPekerja { get; set; }

        public int fld_NegaraID { get; set; }

        public int fld_SyarikatID { get; set; }

        public bool fld_Deleted { get; set; }
    }
}
