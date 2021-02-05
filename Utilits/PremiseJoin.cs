using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Signature.Utilits
{
    /// <summary>
    /// Сопоставление квартир с со списком 2018
    /// </summary>
    public static class PremiseJoin
    {
        public static void Run()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.Write("Обработка...");

                var signatures = db.Signatures.ToList();

                var appartaments = (from a in db.Appartaments
                                    join h in db.Houses on a.f_house equals h.id
                                    join s in db.Streets on h.f_street equals s.id
                                    where a.f_signature == null && a.b_range != true
                                    orderby s.c_name, h.c_house_num, h.c_build_num, a.c_number
                                    select new
                                    {
                                        c_appartament = a.c_number,
                                        f_appartament = a.id,
                                        h.c_house_num,
                                        h.c_build_num,
                                        s.c_name,
                                        s.c_short_type,
                                        s.c_type
                                    }).ToList();

                Console.WriteLine("\r\nДоступно {0} квартир которые нужно проверить", appartaments.Count());

                int idx = 0;
                int search = 0;
                int count = appartaments.Count();
                // дальше по каждей квартире нужно искать
                // первое - проверим есть ли улица
                foreach (var item in appartaments)
                {
                    // возможные варианты улицы
                    string[] streetVar = GetSteetVar(item.c_type, item.c_short_type, item.c_name);

                    // ищем в списке улиц и находим совпадение
                    var innerStreets = signatures
                        .Where(t => GetSteetVar(t.c_street_type, t.c_street_short_type, t.c_street_local).Intersect(streetVar).Count() > 0).ToList();

                    if (innerStreets.Count > 0)
                    {
                        // улица найдена и в них находим требуемый дом
                        string[] houseVar = GetHouseVar(item.c_house_num, item.c_build_num, item.c_appartament);

                        var innerHouses = innerStreets.Where(t => houseVar.Contains(t.c_house)).ToList();
                        // тут если кол-во больше 0 дом найден.
                        if (innerHouses.Count > 0)
                        {
                            // улица найдена и в них находим требуемый дом
                            string[] appartamentVar = GetAppartamentVar(item.c_house_num, item.c_appartament);

                            var innerAppartaments = innerHouses.Where(t => GetAppartamentVar(t.c_house, t.c_appartament).Intersect(appartamentVar).Count() > 0).ToList();
                            // тут если кол-во больше 0 квартира найден.
                            if (innerAppartaments.Count == 1)
                            {
                                int signatureID = innerAppartaments.First().id;
                                var sign = db.Signatures.First(t => t.id == signatureID);
                                sign.f_appartament = item.f_appartament;
                                sign.b_search_appartament = true;

                                db.Signatures.Update(sign);

                                var premise = db.Appartaments.First(t => t.id == item.f_appartament);
                                premise.f_signature = signatureID;
                                premise.c_tag = "signature";

                                db.Appartaments.Update(premise);

                                db.SaveChanges();

                                search++;
                            }
                        }
                    }

                    Console.WriteLine("({0}/{1}/{2})",
                        search,
                        idx,
                        count);

                    idx++;
                }
            }
        }


        static string[] GetSteetVar(string type, string shortType, string name)
        {
            if (type != null &&
                            shortType != null &&
                            name != null)
            {
                // возможные варианты улицы
                string[] streetName = new string[5] {
                        string.Format("{0}", name).ToLower(),
                        string.Format("{0} {1}", type, name).ToLower(),
                        string.Format("{0} {1}", shortType, name).ToLower(),
                        string.Format("{0} {1}", shortType.Replace(".", ""), name).ToLower(),
                        string.Format("{0}", name.Replace("ул. ", "").Replace("п. ", "")).ToLower()
                    };
                return streetName;
            }
            else
            {
                return new string[0];
            }
        }

        static string[] GetHouseVar(string number, string build, string appartament)
        {
            List<string> items = new List<string>();

            if (!string.IsNullOrEmpty(number) && string.IsNullOrEmpty(build))
            {
                items.Add(string.Format("{0}", GetHouseNumberNormal(number)).ToLower());
            }

            if (!string.IsNullOrEmpty(number) && !string.IsNullOrEmpty(build))
            {
                if (GetHouseBuildNormal(build).Length == 0 && !char.IsDigit(GetHouseBuildNormal(build), 0))
                {
                    items.Add(string.Format("{0}{1}", GetHouseNumberNormal(number), GetHouseBuildNormal(build)));
                }

                items.Add(string.Format("{0}/{1}", GetHouseNumberNormal(number), GetHouseBuildNormal(build)));
                items.Add(string.Format("{0} корп.{1}", GetHouseNumberNormal(number), GetHouseBuildNormal(build)));
            }
            if (string.IsNullOrEmpty(number) && string.IsNullOrEmpty(build) && !string.IsNullOrEmpty(appartament))
            {
                items.Add(string.Format("{0}", appartament.Trim()).ToLower());
                items.Add(string.Format("{0}", GetHouseNumberNormal(number)).ToLower());
            }

            return items.ToArray();
        }

        /// <summary>
        /// поиск возможных вариаций квартиры
        /// </summary>
        /// <param name="house"></param>
        /// <param name="appartament"></param>
        /// <returns></returns>
        static string[] GetAppartamentVar(string house, string appartament)
        {
            List<string> items = new List<string>();

            if (!string.IsNullOrEmpty(house) && !string.IsNullOrEmpty(appartament))
            {
                items.Add(string.Format("{0}", GetAppartamentNormal(appartament)).ToLower());
            }

            // этот вариант для частного сектора
            if (string.IsNullOrEmpty(house) && !string.IsNullOrEmpty(appartament))
            {
                items.Add(string.Format("{0}", "0"));
                items.Add(string.Format("{0}", "1"));
            }

            return items.ToArray();
        }

        static string GetHouseNumberNormal(string number)
        {
            return number.ToLower().Replace("бн", "").Replace("б/н", "").Replace(" ", "").Trim();
        }

        static string GetHouseBuildNormal(string build)
        {
            return build.Replace(" ", "").Trim().ToLower();
        }

        static string GetAppartamentNormal(string number)
        {
            return number.ToLower().Replace("бн", "").Replace("б/н", "").Replace(" ", "").Trim();
        }
    }
}
