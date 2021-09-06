using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BedLinenStore.WEB.Models.Entities;

namespace BedLinenStore.WEB.Services.Interfaces
{
    public interface IExcelService
    {
        Task<byte[]> GetExcelReportAsync(IEnumerable<Order> orders);
    }
}