using System;
using System.Linq;
using BedLinenStore.WEB.Data;
using BedLinenStore.WEB.Models.Entities;
using BedLinenStore.WEB.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BedLinenStore.WEB.Services.Implementations
{
    public class ConsultationDateService : IConsultationDateService
    {
        private readonly ApplicationDbContext context;

        public ConsultationDateService(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        public ConsultationDate GetByDate(DateTime date)
        {
            return context.ConsultationDates
                .Include(consultationDate => consultationDate.ConsultationInfos)
                .FirstOrDefault(consultationDate => consultationDate.Date.Equals(date));
        }

        public void CreateConsultation(ConsultationDate info)
        {
            context.ConsultationDates.Add(info);
            context.SaveChanges();
        }
    }
}