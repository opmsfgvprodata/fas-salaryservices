namespace SalaryGeneratorServices.ModelsEstate
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_SubPkt
    {
        [Key]
        public int fld_ID { get; set; }

        [StringLength(10)]
        public string fld_KodPktUtama { get; set; }

        [StringLength(10)]
        public string fld_Pkt { get; set; }

        [StringLength(50)]
        public string fld_NamaPkt { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fld_LsPkt { get; set; }

        public DateTime? fld_CreateDate { get; set; }

        public DateTime? fld_EndDate { get; set; }

        [StringLength(5)]
        public string fld_KodPktUtama_Sblm { get; set; }

        [StringLength(5)]
        public string fld_Pkt_Sblm { get; set; }

        [StringLength(50)]
        public string fld_NamaPkt_Sblm { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fld_LsPkt_Sblm { get; set; }

        public DateTime? fld_CreateDate_Sblm { get; set; }

        public DateTime? fld_EndDate_Sblm { get; set; }

        public int? fld_NegaraID { get; set; }

        public int? fld_SyarikatID { get; set; }

        public int? fld_WilayahID { get; set; }

        public int? fld_LadangID { get; set; }
    }
}
