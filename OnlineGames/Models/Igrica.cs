using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGames.Models
{
    public class Igrica
    {
        public int Id { get; set; }
        [Display(Name = "Naziv igrice")]
        [Required]
        public String Naziv { get; set; }
        [Required]
        [Display(Name = "Datum izlaska")]
        public DateTime DatumIzlaska { get; set; }
        [Display(Name = "Izdavač")]
        [Required]
        public String Izdavac { get; set; }
        [Display(Name = "Specifikacije")]
        public String Specifikacije { get; set; }
        [Display(Name = "Cijena")]
        [Required]
        public double Cijena { get; set; }
        [Display(Name = "Slika")]
        public int SlikaIgriceId {get; set;}
        [Display(Name = "Slika")]
        public virtual SlikaIgrice SlikaIgrice { get; set; }

        public virtual ICollection<KategorijaIgrica> KategorijaIgrica { get; set; }
        public virtual ICollection<NalogIgrice> NalogIgrice { get; set; }

    }
}
