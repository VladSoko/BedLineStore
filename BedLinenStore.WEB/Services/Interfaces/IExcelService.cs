using System.Threading.Tasks;

namespace BedLinenStore.WEB.Services.Interfaces
{
    public interface IExcelService
    {
        Task<byte[]> GetExcelReportAsync();
    }
}