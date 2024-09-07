using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TraahvIndividual.Models
{
    public class TrahvidDbInitializer : DropCreateDatabaseAlways<TrahvidContext>
    {
        protected override void Seed(TrahvidContext db)
        {
            db.Traahv.Add(new Traahv
            {
                SoidukeNumber = "AAA 777",
                OmanikuNimi = "Oleg Nechiparenko",
                OmanikuEpost = "Oleg@gmail.com",
                Rikkumisekuupaev = DateTime.Parse("2021-12-12"),
                KiiruseUletamine = 12,
                TrahviSuurus = 100
            });

            db.Traahv.Add(new Traahv
            {
                SoidukeNumber = "BBB 888",
                OmanikuNimi = "John Doe",
                OmanikuEpost = "john.doe@example.com",
                Rikkumisekuupaev = DateTime.Parse("2022-01-10"),
                KiiruseUletamine = 20,
                TrahviSuurus = 150
            });

            base.Seed(db);
        }
    }
}
