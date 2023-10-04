namespace SalaryGeneratorServices.ModelsHQ
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vw_CutiUmumNegeri
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int fld_CutiUmumID { get; set; }

        [StringLength(100)]
        public string fld_KeteranganCuti { get; set; }

        [StringLength(10)]
        public string fld_TarikhCuti { get; set; }

        public int? fld_Negeri { get; set; }

        public short? fld_Tahun { get; set; }

        public int? fld_NegaraID { get; set; }

        public int? fld_SyarikatID { get; set; }

        public bool? fld_Deleted { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string fldOptConfDesc { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string fldOptConfFlag1 { get; set; }

        [StringLength(50)]
        public string fldOptConfFlag2 { get; set; }

        public bool? fldDeleted { get; set; }

        [StringLength(50)]
        public string fldOptConfFlag3 { get; set; }
    }
}
