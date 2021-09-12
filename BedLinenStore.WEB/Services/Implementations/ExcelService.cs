using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BedLinenStore.WEB.Models.Entities;
using BedLinenStore.WEB.Services.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace BedLinenStore.WEB.Services.Implementations
{
    public class ExcelService : IExcelService
    {
        public async Task<byte[]> GetExcelReportAsync(IEnumerable<Order> orders)
        {
            var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets
                .Add("Заказы");
            
            (int X, int Y) pos = (1, 1);
            
            sheet.Cells[pos.X, pos.Y++].Value = "Номер заказа";
            sheet.Cells[pos.X, pos.Y++].Value = "Дата заказа";
            int dataOrderColumn = pos.Y;
            sheet.Cells[pos.X, pos.Y++].Value = "Данные о заказе";
            sheet.Cells[pos.X, pos.Y++].Value = "ФИ заказчика";
            sheet.Cells[pos.X, pos.Y++].Value = "Почта";
            sheet.Cells[pos.X, pos.Y++].Value = "Адрес доставки";
            
            
            
            sheet.Cells[1, 1, pos.X, pos.Y].AutoFitColumns();
            
            sheet.Cells[1, 1, pos.X, pos.Y].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[1, 1, pos.X, pos.Y].Style.Font.Bold = true;

            (int X, int Y) contentPos = (2, 1);
            
            foreach (var order in orders)
            {
                sheet.Cells[contentPos.X, contentPos.Y++].Value = order.Id;
                sheet.Cells[contentPos.X, contentPos.Y++].Value = order.CreatedDate.ToString("d");
                //
                StringBuilder dataOrder = new StringBuilder();
                foreach (var data in order.Products)
                {
                    dataOrder.Append($"{data.MainInfo.Name} {data.Category.Description}");

                    if (data != order.Products.Last())
                    {
                        dataOrder.AppendLine();
                        dataOrder.AppendLine();
                    }
                   
                }

                sheet.Cells[contentPos.X, contentPos.Y++].Value = dataOrder.ToString();
                sheet.Cells[contentPos.X, contentPos.Y++].Value = order.Name;
                sheet.Cells[contentPos.X, contentPos.Y++].Value = order.Email;
                
                string address = $"{order.Country}, {order.City}, {order.Street}";
                sheet.Cells[contentPos.X, contentPos.Y++].Value = address;

                contentPos.X++;
                
                contentPos.Y = 1;
            }
            
            sheet.Cells[1, 1, contentPos.X - 1, pos.Y - 1].AutoFitColumns();
            sheet.Cells[1, 1, contentPos.X - 1, pos.Y - 1].Style.Border.BorderAround(ExcelBorderStyle.Double);

            for (int i = 1; i < contentPos.X; i++)
            {
                sheet.Cells[1, 1, i, pos.Y - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            }
            
            for (int i = 1; i < pos.Y - 1; i++)
            {
                sheet.Cells[1, 1, contentPos.X - 1, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }
            
            sheet.Cells[1, 1, 1, pos.Y - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

            sheet.Column(dataOrderColumn).Width = 40;
            sheet.Column(dataOrderColumn).Style.WrapText = true;
            sheet.Cells[2, 1, contentPos.X, pos.Y].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[2, 1, contentPos.X, pos.Y].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            
            sheet.Protection.IsProtected = true;
            
            return package.GetAsByteArray();
        }
    }
}