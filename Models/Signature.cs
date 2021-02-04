using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Signature.Models
{
    [Table("cs_signature", Schema = "imp")]
    public class Signature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        /// <summary>
        /// Район
        /// </summary>
        public string c_area { get; set; }
        /// <summary>
        /// УИК
        /// </summary>
        public int n_uik { get; set; }
        public string c_street { get; set; }
        public string c_house { get; set; }
        public int? n_appartament_start { get; set; }
        public int? n_appartament_end { get; set; }
        public string c_appartaments { get; set; }
        public string c_appartament { get; set; }
        public Guid? f_appartament { get; set; }
        public string c_street_local { get; set; }
        public string c_street_type { get; set; }
        public string c_street_short_type { get; set; }
    }
}
