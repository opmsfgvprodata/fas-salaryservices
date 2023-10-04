namespace SalaryGeneratorServices.ModelsEstate
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_SpecialInsentif
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid fld_SpecialInsentifID { get; set; }

        [StringLength(20)]
        public string fld_Nopkj { get; set; }

        [StringLength(5)]
        public string fld_KodInsentif { get; set; }

        public decimal? fld_NilaiInsentif { get; set; }

        public int? fld_Year { get; set; }

        public int? fld_Month { get; set; }

        public int? fld_NegaraID { get; set; }

        public int? fld_SyarikatID { get; set; }

        public int? fld_WilayahID { get; set; }

        public int? fld_LadangID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fld_KWSPPkj { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fld_KWSPMjk { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fld_SocsoPkj { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fld_SocsoMjk { get; set; }

        public bool? fld_Deleted { get; set; }

        public int? fld_CreatedBy { get; set; }

        public DateTime? fld_CreatedDT { get; set; }
        public Guid fld_InsentifID { get; set; }
    }
}
