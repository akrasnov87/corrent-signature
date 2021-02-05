using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Signature.Utilits
{
    /// <summary>
    /// Проверка на диапазон квартир
    /// </summary>
    public static class RangePremise
    {
        public static void Run()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine("Обработка...");

                // проверяем что дом входит в допустимый диапазон
                var appartaments = (from a in db.Appartaments
                                    join h in db.HouseByJkhs on a.f_jkh_house equals h.link
                                    where a.f_signature == null && a.b_range == null
                                    orderby h.f_street, h.c_house_number, a.c_number
                                    select new { 
                                        f_appartament = a.id,
                                        c_appartament = a.c_number,
                                        h.n_appart_count
                                    }).ToList();

                int idx = 0;
                int rangeOff = 0;
                int bad = 0;

                int count = appartaments.Count();

                int limit = 0;

                foreach (var item in appartaments)
                {
                    var record = db.Appartaments.First(t => t.id == item.f_appartament);

                    int appartamentNumber = GetAppartamentNumber(item.c_appartament);
                    if(appartamentNumber > 0)
                    {
                        if(appartamentNumber > item.n_appart_count)
                        {
                            record.c_tag = "range_off";
                            record.b_range = true;
                            rangeOff++;
                        } else
                        {
                            record.b_range = false;
                        }
                    } else
                    {
                        // возможно это частный сектор или просто ошибка         
                        record.c_tag = "bag_premise_number";
                        bad++;
                    }

                    db.Appartaments.Update(record);
                    limit++;

                    if (limit >= 500)
                    {
                        limit = 0;
                        db.SaveChanges();
                    }
                    idx++;

                    Console.Write("\r{0}/{1}/{2}/{3}", rangeOff, bad, idx, count);
                }

                if(limit > 0)
                {
                    db.SaveChanges();
                }
            }
        }

        static int GetAppartamentNumber(string number)
        {
            Regex r = new Regex(@"\d+", RegexOptions.IgnoreCase);
            if(r.IsMatch(number))
            {
                return int.Parse(r.Match(number).Value);
            }

            return -1;
        }
    }
}
