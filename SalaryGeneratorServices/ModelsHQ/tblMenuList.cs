namespace SalaryGeneratorServices.ModelsHQ
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblMenuList")]
    public partial class tblMenuList
    {
        [Key]
        public int fld_ID { get; set; }

        [StringLength(50)]
        public string fld_Val { get; set; }

        [StringLength(50)]
        public string fld_Desc { get; set; }

        [StringLength(50)]
        public string fld_Flag { get; set; }

        [StringLength(50)]
        public string fld_Sub { get; set; }

        public bool? fldDeleted { get; set; }
    }
}
