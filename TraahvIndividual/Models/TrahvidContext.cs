using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TraahvIndividual.Models
{
    public class TrahvidContext : DbContext
    {

        public DbSet<Traahv> Traahv { get; set; }
        public DbSet<Login> Login { get; set; }
    }
}