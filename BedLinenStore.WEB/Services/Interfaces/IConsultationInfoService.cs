using BedLinenStore.WEB.Models.Entities;

namespace BedLinenStore.WEB.Services.Interfaces
{
    public interface IConsultationInfoService
    {
        
        void CreateConsultation(ConsultationInfo info);

        ConsultationInfo GetInfoByEmail(string email);
    }
}