using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Signature
{
    class Program
    {
        static void Main(string[] args)
        {
            using(ApplicationContext db = new ApplicationContext())
            {
                var signatures = db.Signatures.ToList();

                var appartaments = from a in db.Appartaments
                                   join h in db.Houses on a.f_house equals h.id
                                   join s in db.Streets on h.f_street equals s.id
                                   where a.f_premise == null
                                   orderby a.id
                                   select new
                                   {
                                       c_appartament = a.c_number,
                                       f_appartament = a.id,
                                       h.c_house_num,
                                       h.c_build_num,
                                       s.c_name,
                                       s.c_short_type,
                                       s.c_type
                                   };

                Console.WriteLine("Доступно {0} квартир которые нужно проверить", appartaments.Count());
                // дальше по каждей квартире нужно искать
                // первое - проверим есть ли улица
                foreach(var item in appartaments)
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
                        if(innerHouses.Count > 0)
                        {

                        }
                    }
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
                string[] streetName = new string[3] {
                        string.Format("{0} {1}", type, name).ToLower(),
                        string.Format("{0} {1}", shortType, name).ToLower(),
                        string.Format("{0} {1}", shortType.Replace(".", ""), name).ToLower()
                    };
                return streetName;
            } else
            {
                return new string[0];
            }
        }

        static string[] GetHouseVar(string number, string build, string appartament)
        {
            List<string> items = new List<string>();

            if(!string.IsNullOrEmpty(number) && string.IsNullOrEmpty(build))
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

        /*static string[] GetAppartamentVar(string house, string appartament)
        {
            List<string> items = new List<string>();

            if (string.IsNullOrEmpty(house))
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
        }*/

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
