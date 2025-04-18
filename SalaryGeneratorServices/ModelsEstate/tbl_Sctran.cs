namespace SalaryGeneratorServices.ModelsEstate
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Sctran
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid fld_ID { get; set; }

        [StringLength(1)]
        public string fld_KdCaj { get; set; }

        public byte? fld_Kategori { get; set; }

        [StringLength(5)]
        public string fld_KodGL { get; set; }

        public byte? fld_JnsPkt { get; set; }

        [StringLength(10)]
        public string fld_KodPkt { get; set; }

        [StringLength(2)]
        public string fld_JnisAktvt { get; set; }

        [StringLength(5)]
        public string fld_KodAktvt { get; set; }

        [StringLength(300)]
        public string fld_Keterangan { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fld_Amt { get; set; }

        public int? fld_Year { get; set; }

        public int? fld_Month { get; set; }

        public int? fld_NegaraID { get; set; }

        public int? fld_SyarikatID { get; set; }

        public int? fld_WilayahID { get; set; }

        public int? fld_LadangID { get; set; }

        public int? fld_DivisionID { get; set; }

        public int? fld_CreatedBy { get; set; }

        public DateTime? fld_CreatedDT { get; set; }

        [StringLength(20)]
        public string fld_IO { get; set; }

        [StringLength(10)]
        public string fld_GL { get; set; }

        [StringLength(50)]
        public string fld_NNCC { get; set; }

        [StringLength(10)]
        public string fld_SapActCode { get; set; }

        public bool? fld_IsCitizen { get; set; }
    }
}
