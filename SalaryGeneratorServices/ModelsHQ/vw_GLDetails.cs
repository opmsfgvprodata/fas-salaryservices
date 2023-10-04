namespace SalaryGeneratorServices.ModelsHQ
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vw_GLDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int fld_ID { get; set; }

        [StringLength(20)]
        public string fld_GLcode { get; set; }

        [StringLength(50)]
        public string fld_DescGL { get; set; }

        [StringLength(10)]
        public string fld_KodAktvt { get; set; }

        [StringLength(150)]
        public string fld_DescKodAktvt { get; set; }

        [StringLength(2)]
        public string fld_KodJenisAktvt { get; set; }

        [StringLength(50)]
        public string fld_DescJenisAktiviti { get; set; }

        public int? fld_NegaraID { get; set; }

        public int? fld_SyarikatID { get; set; }
    }
}
