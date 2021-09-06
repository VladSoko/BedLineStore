using System;
using System.ComponentModel.DataAnnotations;

namespace BedLinenStore.WEB.Models
{
    public class DownloadOrderViewModel
    {
        [Required]
        public DateTime FromDate { get; set; }
        
        [Required]
        public DateTime ToDate { get; set; }
    }
}