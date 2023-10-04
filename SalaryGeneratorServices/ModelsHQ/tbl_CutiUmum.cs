namespace SalaryGeneratorServices.ModelsHQ
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_CutiUmum
    {
        [Key]
        public int fld_CutiUmumID { get; set; }

        [StringLength(100)]
        public string fld_KeteranganCuti { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fld_TarikhCuti { get; set; }

        public int? fld_Negeri { get; set; }

        public short? fld_Tahun { get; set; }

        public int? fld_NegaraID { get; set; }

        public int? fld_SyarikatID { get; set; }

        public bool? fld_Deleted { get; set; }
    }
}
