namespace SalaryGeneratorServices.ModelsEstate
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_HasilSawitPkt
    {
        [Key]
        public int fld_ID { get; set; }

        [StringLength(10)]
        public string fld_KodPeringkat { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fld_HasilTan { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fld_LuasHektar { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fld_Bulan { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fld_Tahun { get; set; }

        public int? fld_NegaraID { get; set; }

        public int? fld_SyarikatID { get; set; }

        public int? fld_WilayahID { get; set; }

        public int? fld_LadangID { get; set; }

        public bool? fld_Deleted { get; set; }
    }
}
