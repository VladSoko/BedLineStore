using System;
using System.Collections.Generic;

namespace BedLinenStore.WEB.Models.Entities
{
    public class ConsultationDate
    {
        public  int Id { get; set; }
        
        public DateTime Date { get; set; }
        
        public int ConsultationsNumber { get; set; }
        
        public ICollection<ConsultationInfo> ConsultationInfos { get; set; }
    }
}