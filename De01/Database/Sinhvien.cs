namespace De01.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sinhvien")]
    public partial class Sinhvien
    {
        [Key]
        [StringLength(10)]
        public string MaSV { get; set; }

        [Required]
        [StringLength(50)]
        public string HoTenSV { get; set; }

        public DateTime NgaySinh { get; set; }

        [StringLength(10)]
        public string MaLop { get; set; }

        public virtual Lop Lop { get; set; }
    }
}
