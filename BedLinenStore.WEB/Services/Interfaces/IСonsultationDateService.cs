using System;
using BedLinenStore.WEB.Models.Entities;

namespace BedLinenStore.WEB.Services.Interfaces
{
    public interface IConsultationDateService
    {
        ConsultationDate GetByDate(DateTime date);

        void CreateConsultation(ConsultationDate info);
    }
}