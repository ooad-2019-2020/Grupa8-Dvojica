using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGames.Models
{
    public class KorisnickiNalog
    {
        public string KorisnickiNalogId { get; set; }
        public virtual ICollection<NalogIgrice> NalogIgrice { get; set; }

    }
}
