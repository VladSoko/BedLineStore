namespace BedLinenStore.WEB.Models.Entities
{
    public class ConsultationInfo
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public  string Surname { get; set; }
        
        public  string Email { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public int ConsultationDateId { get; set; }
        
        public ConsultationDate ConsultationDate { get; set; }
    }
}