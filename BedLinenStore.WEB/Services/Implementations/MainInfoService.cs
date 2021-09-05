using System.Collections.Generic;
using System.Linq;
using BedLinenStore.WEB.Data;
using BedLinenStore.WEB.Models.Entities;
using BedLinenStore.WEB.Services.Interfaces;

namespace BedLinenStore.WEB.Services.Implementations
{
    public class MainInfoService : IMainInfoService
    {
        private readonly ApplicationDbContext context;

        public MainInfoService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<MainInfo> GetAll()
        {
            return context.MainInfos.ToList();
        }

        public MainInfo GetById(int id)
        {
            return context.MainInfos
                .FirstOrDefault(item => item.Id == id);
        }
    }
}