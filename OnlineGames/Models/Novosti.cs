using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGames.Models
{
    public class Novosti
    {
        public int Id { get; set; }
        [Display(Name = "Naslov novosti")]
        [Required]
        public String Naslov { get; set; }
        [Display(Name = "Tekst")]
        [Required]
        public String Tekst { get; set; }
        [Display(Name = "Link za vijest")]
        public String Link { get; set; }
    }
}
