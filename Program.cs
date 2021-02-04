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
                var appartaments = from a in db.Appartaments
                                   join h in db.Houses on a.f_house equals h.id
                                   join s in db.Streets on h.f_street equals s.id
                                   where a.f_premise == null
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
            }
        }
    }
}
