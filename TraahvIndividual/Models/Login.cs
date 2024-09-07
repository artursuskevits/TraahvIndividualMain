using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TraahvIndividual.Models
{
    public class Login
    {
        public int Id { get; set; }
        public string KasutajaNimi { get; set; }
        public string Salasona { get; set; }

        public Double Role { get; set; }

        [ForeignKey("Traahv")]
        public int Soidukenumber { get; set; }
        public virtual Traahv Traahv { get; set; }
    }
}