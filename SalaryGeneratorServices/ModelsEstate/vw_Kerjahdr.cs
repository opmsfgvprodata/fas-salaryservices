namespace SalaryGeneratorServices.ModelsEstate
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vw_Kerjahdr
    {
        [Key]
        public Guid fld_UniqueID { get; set; }

        [StringLength(10)]
        public string fld_Nopkj { get; set; }

        [StringLength(40)]
        public string fld_Nama { get; set; }

        [StringLength(50)]
        public string fld_Kum { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fld_Tarikh { get; set; }

        public int? fld_Hujan { get; set; }

        [StringLength(2)]
        public string fld_Kdhdct { get; set; }

        public int? fld_NegaraID { get; set; }

        public int? fld_SyarikatID { get; set; }

        public int? fld_WilayahID { get; set; }

        public int? fld_LadangID { get; set; }

        public int? fld_CreatedBy { get; set; }

        public DateTime? fld_CreatedDT { get; set; }

        public int? fld_StatusApproved { get; set; }
    }
}
