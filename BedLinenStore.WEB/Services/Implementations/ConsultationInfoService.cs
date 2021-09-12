using System.Linq;
using BedLinenStore.WEB.Data;
using BedLinenStore.WEB.Models.Entities;
using BedLinenStore.WEB.Services.Interfaces;

namespace BedLinenStore.WEB.Services.Implementations
{
    public class ConsultationInfoService : IConsultationInfoService
    {
        private readonly ApplicationDbContext context;

        public ConsultationInfoService(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        public void CreateConsultation(ConsultationInfo info)
        {
            context.ConsultationInfos.Add(info);
            context.SaveChanges();
        }

        public ConsultationInfo GetInfoByEmail(string email)
        {
            return context.ConsultationInfos
                .FirstOrDefault(info => info.Email == email);
        }
    }
}