using System.Collections;
using System.Collections.Generic;
using BedLinenStore.WEB.Models.Entities;

namespace BedLinenStore.WEB.Services.Interfaces
{
    public interface IMainInfoService
    {
        IEnumerable<MainInfo> GetAll();

        MainInfo GetById(int id);
    }
}