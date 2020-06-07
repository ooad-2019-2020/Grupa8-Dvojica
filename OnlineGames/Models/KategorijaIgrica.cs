using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGames.Models
{
    public class KategorijaIgrica
    {
        public int KategorijaIgricaId { get; set; }
        [Display(Name = "Igrica")]
        [Required]
        public int IgricaId { get; set; }
        [Display(Name = "Kategorija")]
        [Required]
        public int KategorijaId { get; set; }
        public virtual Kategorija Kategorija { get; set; }
        public virtual Igrica Igrica { get; set; }
    }
}
