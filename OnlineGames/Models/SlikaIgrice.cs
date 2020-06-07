using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineGames.Models
{
    public class SlikaIgrice
    {
        public int SlikaIgriceId { get; set; }
        [Display(Name = "Naziv")]
        [Required]
        public string ImageTitle { get; set; }
        [Display(Name = "Slika")]
        
        public byte[] ImageData { get; set; }
        
    }
}
