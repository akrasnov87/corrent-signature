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
        /// <summary>
        /// ID волантера
        /// </summary>
        public string c_user_num { get; set; }
        /// <summary>
        /// ФИО волонтера
        /// </summary>
        public string c_user_fio { get; set; }
        public string c_street { get; set; }
        public string c_house { get; set; }
        public int? n_appartament_start { get; set; }
        public int? n_appartament_end { get; set; }
        public string c_appartaments { get; set; }
        public string c_appartament { get; set; }
        /// <summary>
        /// Дата обхода
        /// </summary>
        public DateTime d_date { get; set; }
        /// <summary>
        /// Статус контакта
        /// </summary>
        public string c_status { get; set; }
        /// <summary>
        /// Возрастная категория респондента
        /// </summary>
        public int? n_age { get; set; }
        /// <summary>
        /// Количество подписей
        /// </summary>
        public int? n_signature { get; set; }
        /// <summary>
        /// Лояльность
        /// </summary>
        public string c_loyalty { get; set; }
        /// <summary>
        /// Явка
        /// </summary>
        public string c_come { get; set; }
        public string c_notice { get; set; }
    }
}
