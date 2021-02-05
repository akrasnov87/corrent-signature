using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Signature.Models
{
    [Table("cs_appartament", Schema = "dbo")]
    public class Appartament
    {
        [Key]
        public Guid id { get; set; }
        public Guid f_house { get; set; }
        public string c_number { get; set; }
        public int? n_number { get; set; }
        public DateTime? dx_date { get; set; }
        public bool? b_disabled { get; set; }
        public int? f_user { get; set; }
        public int n_signature_2018 { get; set; }
        public int? f_main_user { get; set; }
        public int? f_premise { get; set; }
        public bool b_check { get; set; }
        public string c_tag { get; set; }
        public int? f_jkh_house { get; set; }
        public int? f_signature { get; set; }

        public bool? b_range { get; set; }
    }
}
