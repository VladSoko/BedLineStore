using System.Collections;
using System.Collections.Generic;
using BedLinenStore.WEB.Models.Entities;

namespace BedLinenStore.WEB.Services.Interfaces
{
    public interface IFreelanceSewingService
    {
        IEnumerable<FreelanceSewing> GetAll();

        void DeleteById(int id);
    }
}