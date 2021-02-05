using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Signature.Models
{
    [Table("jkh_houses", Schema = "imp")]
    public class HouseByJkh
    {
        [Key]
        public int link { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        public int? f_street { get; set; }

        /// <summary>
        /// номер дома - полностью
        /// </summary>
        public string c_house_number { get; set; }

        /// <summary>
        /// адрес
        /// </summary>
        public string c_address { get; set; }

        /// <summary>
        /// год строения
        /// </summary>
        public int? n_building_year { get; set; }

        /// <summary>
        /// количество лифтов
        /// </summary>
        public int? n_lift_count { get; set; }

        /// <summary>
        /// Количество этажей
        /// </summary>
        public string c_floor { get; set; }

        /// <summary>
        /// кол-во подъездов
        /// </summary>
        public int? n_entrance_count { get; set; }

        /// <summary>
        /// кол-во квартир
        /// </summary>
        public int? n_appart_count { get; set; }

        /// <summary>
        /// мин. кол-во этажей
        /// </summary>
        public string n_count_floor_min { get; set; }

        /// <summary>
        /// макс. кол-во этажей
        /// </summary>
        public string n_count_floor_max { get; set; }

        /// <summary>
        /// ФИАС
        /// </summary>
        public Guid? s_fias_guid { get; set; }

        /// <summary>
        /// номер дома
        /// </summary>
        public string c_house_num { get; set; }

        /// <summary>
        /// корпус
        /// </summary>
        public string c_house_corp { get; set; }

        /// <summary>
        /// литерала
        /// </summary>
        public string c_house_litera { get; set; }
    }
}
