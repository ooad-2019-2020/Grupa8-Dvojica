using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGames.Models
{
    public class NalogIgrice
    {
        public int NalogIgriceId { get; set; }
        public int IgricaId { get; set; }
        public string NalogId { get; set; }
        public virtual KorisnickiNalog KorisnickiNalog { get; set; }
        public virtual Igrica Igrica { get; set; }
    }
}
