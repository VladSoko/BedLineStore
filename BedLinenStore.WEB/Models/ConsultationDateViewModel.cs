using System;
using System.ComponentModel.DataAnnotations;

namespace BedLinenStore.WEB.Models
{
    public class ConsultationDateViewModel
    {
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public int ConsultationsNumber { get; set; }
    }
}