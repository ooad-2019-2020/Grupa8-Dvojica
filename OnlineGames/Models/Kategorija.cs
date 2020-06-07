using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGames.Models
{
    public class Kategorija
    {
        public int KategorijaId { get; set; }
        [Display(Name = "Naziv kategorije")]
        [Required]
        public String Naziv { get; set; }
        public virtual ICollection<KategorijaIgrica> KategorijaIgrica { get; set; }
    }
}
