using System;
using System.ComponentModel.DataAnnotations;

namespace BedLinenStore.WEB.Models
{
    public class ConsultationInfoViewModel
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public  string Surname { get; set; }
        
        [Required]
        public  string Email { get; set; }
        
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}