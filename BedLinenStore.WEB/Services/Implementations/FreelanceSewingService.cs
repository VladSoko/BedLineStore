using System.Collections.Generic;
using System.Linq;
using BedLinenStore.WEB.Data;
using BedLinenStore.WEB.Models.Entities;
using BedLinenStore.WEB.Services.Interfaces;

namespace BedLinenStore.WEB.Services.Implementations
{
    public class FreelanceSewingService : IFreelanceSewingService
    {
        private readonly ApplicationDbContext context;

        public FreelanceSewingService(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        public IEnumerable<FreelanceSewing> GetAll()
        {
            return context.FreelanceSewings.ToList();
        }

        public void DeleteById(int id)
        {
            var freelanceSewing = context.FreelanceSewings
                .FirstOrDefault(item => item.Id == id);
            context.FreelanceSewings.Remove(freelanceSewing);
            context.SaveChanges();
        }
    }
}