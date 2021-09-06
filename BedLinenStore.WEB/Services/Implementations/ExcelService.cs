using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BedLinenStore.WEB.Models.Entities;
using BedLinenStore.WEB.Services.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;

namespace BedLinenStore.WEB.Services.Implementations
{
    public class ExcelService : IExcelService
    {
        public async Task<byte[]> GetExcelReportAsync(IEnumerable<Order> orders)
        {
            // using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            // {
            //     await CreateReportWorksheet(excelPackage);
            //
            //     await CreateSynchronizationsWorksheet(excelPackage);
            //
            //     using (var stream = new MemoryStream())
            //     {
            //         workbook.SaveAs(stream);
            //         stream.Flush();
            //
            //         return stream.ToArray();
            //     }
            // }
            
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

            // sheet.Cells[1, 1, contentPos.X, pos.Y].Style.Border. = ExcelBorderStyle.Thin;

            sheet.Column(dataOrderColumn).Width = 40;
            sheet.Column(dataOrderColumn).Style.WrapText = true;
            sheet.Cells[2, 1, contentPos.X, pos.Y].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[2, 1, contentPos.X, pos.Y].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            // sheet.Cells[8, 2, 8, 4].LoadFromArrays(new object[][]{ new []{"Capitalization", "SharePrice", "Date"} });
            // var row = 9;
            // var column = 2;
            // foreach (var item in report.History)
            // {
            //     sheet.Cells[row, column].Value = item.Capitalization;
            //     sheet.Cells[row, column + 1].Value = item.SharePrice;
            //     sheet.Cells[row, column + 2].Value = item.Date;    
            //     row++;
            // }
            //
            // sheet.Cells[1, 1, row, column + 2].AutoFitColumns();
            // sheet.Column(2).Width = 14;
            // sheet.Column(3).Width = 12;
            //
            // sheet.Cells[9, 4, 9 + report.History.Length, 4].Style.Numberformat.Format = "yyyy";
            // sheet.Cells[9, 2, 9 + report.History.Length, 2].Style.Numberformat.Format =  "### ### ### ##0";
            //
            // sheet.Column(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            // sheet.Cells[8, 3, 8 + report.History.Length, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //
            // sheet.Cells[8, 2, 8, 4].Style.Font.Bold = true;
            // sheet.Cells["B2:C4"].Style.Font.Bold = true;
            //
            // sheet.Cells[8, 2, 8 + report.History.Length, 4].Style.Border.BorderAround(ExcelBorderStyle.Double);
            // sheet.Cells[8, 2, 8, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            //
            // var capitalizationChart = sheet.Drawings.AddChart("FindingsChart", OfficeOpenXml.Drawing.Chart.eChartType.Line);
            // capitalizationChart.Title.Text = "Capitalization";
            // capitalizationChart.SetPosition(7, 0, 5, 0);
            // capitalizationChart.SetSize(800, 400);
            // var capitalizationData = (ExcelChartSerie)(capitalizationChart.Series.Add(sheet.Cells["B9:B28"], sheet.Cells["D9:D28"]));
            // capitalizationData.Header = report.Company.Currency;
            
            sheet.Protection.IsProtected = true;
            
            return package.GetAsByteArray();
        }

        // private async Task CreateReportWorksheet(XLWorkbook workbook)
        // {
        //     var worksheet = CheckAndCreateWorkSheet(
        //         excelPackage,
        //         "Report");
        //
        //     CreateReportWorksheetHead(worksheet);
        //
        //     var users = await _dbContext.Users
        //         .Include(user => user.ThreatFiles)
        //             .ThenInclude(file => file.Rule)
        //         .Include(user => user.ThreatFiles)
        //             .ThenInclude(file => file.Synchronization)
        //                 .ThenInclude(sync => sync.AuditType)
        //         .ToListAsync();
        //
        //     (int X, int Y) pos = (2, 1);
        //
        //     foreach (var user in users)
        //     {
        //         if (!(user.ThreatFiles is null)
        //             || user.ThreatFiles.Count != 0)
        //         {
        //             var userFileGroups = user.ThreatFiles
        //                 .GroupBy(file => new
        //                 {
        //                     Filename = file.Filename,
        //                     FilePath = file.FilePath,
        //                     FileSize = file.FileSize,
        //                     Description = file.Description,
        //                     Workstation = file.Workstation
        //                 });
        //
        //             foreach (var fileGroup in userFileGroups)
        //             {
        //                 var synchronization = fileGroup.LastOrDefault().Synchronization;
        //
        //                 var isAllowed = fileGroup.LastOrDefault().Rule?.IsAllowed ?? false;
        //
        //                 worksheet.Cells[pos.X, pos.Y++].Value = user.Username;
        //
        //                 worksheet.Cells[pos.X, pos.Y++].Value = user.Email;
        //
        //                 worksheet.Cells[pos.X, pos.Y++].Value = user
        //                     .ResourceManagerEmail
        //                     .Replace('_', ' ')
        //                     .Replace("@epam.com", string.Empty);
        //
        //                 worksheet.Cells[pos.X, pos.Y++].Value = user.ResourceManagerEmail;
        //
        //                 worksheet.Cells[pos.X, pos.Y++].Value = fileGroup.Key.Filename;
        //
        //                 worksheet.Cells[pos.X, pos.Y++].Value = fileGroup.Key.FileSize;
        //
        //                 worksheet.Cells[pos.X, pos.Y++].Value = fileGroup.Key.Description;
        //
        //                 worksheet.Cells[pos.X, pos.Y++].Value = fileGroup.LastOrDefault().Status.ToString();
        //
        //                 worksheet.Cells[pos.X, pos.Y++].Value = isAllowed
        //                     ? "Approved"
        //                     : "Declined";
        //
        //                 worksheet.Cells[pos.X, pos.Y++].Value = fileGroup.Count();
        //
        //                 worksheet.Cells[pos.X, pos.Y++].Value = fileGroup.Key.Workstation;
        //
        //                 worksheet.Cells[pos.X, pos.Y++].Value = synchronization.AuditType.Name;
        //
        //                 worksheet.Cells[pos.X, pos.Y++].Value = $"{synchronization.SynchronizationDate:dd/MM/yy HH:mm:ss}";
        //
        //                 worksheet.Cells[pos.X, pos.Y++].Value = await IsSameAudit(fileGroup.LastOrDefault());
        //
        //                 worksheet.Cells[pos.X, pos.Y++].Value = GetScanDateFromThreatFile(fileGroup.LastOrDefault());
        //
        //                 worksheet.Cells[pos.X, pos.Y++].Value = await GetUserProjectNames(user.PmcId);
        //
        //                 worksheet.Cells[pos.X, pos.Y++].Value = fileGroup.LastOrDefault().Comment;
        //
        //                 worksheet.Cells[pos.X, pos.Y++].Value = fileGroup.LastOrDefault().Id;
        //
        //                 pos.X++;
        //
        //                 pos.Y = 1;
        //             }
        //         }
        //     }
        // }
        //
        // private async Task CreateSynchronizationsWorksheet(ExcelPackage excelPackage)
        // {
        //     var worksheet = CheckAndCreateWorkSheet(
        //         excelPackage,
        //         "Synchronizations");
        //
        //     CreateSynchronizationsWorksheetHead(worksheet);
        //
        //     var synchronizations = await _dbContext.Synchronizations
        //         .Include(sync => sync.AuditType)
        //         .OrderByDescending(sync => sync.SynchronizationDate)
        //         .ToListAsync();
        //
        //     (int X, int Y) pos = (2, 1);
        //
        //     foreach (var synchronization in synchronizations)
        //     {
        //         var starter = await _dbContext.Users.FindAsync(synchronization.StarterId);
        //
        //         worksheet.Cells[pos.X, pos.Y++].Value = synchronization.AuditType.Name;
        //
        //         worksheet.Cells[pos.X, pos.Y++].Value = $"{synchronization.SynchronizationDate:dd/MM/yy HH:mm:ss}";
        //
        //         worksheet.Cells[pos.X, pos.Y++].Value = starter?.Username;
        //
        //         worksheet.Cells[pos.X, pos.Y++].Value = synchronization.SuccessfulNotifications;
        //
        //         pos.X++;
        //
        //         pos.Y = 1;
        //     }
        //
        //     worksheet.View.FreezePanes(2, 1);
        // }
        // private void CreateReportWorksheetHead(
        //     ExcelWorksheet worksheet,
        //     int startPosition = 1)
        // {
        //     #region Head text
        //     worksheet.Cells[1, startPosition++].Value = "Employee Name";
        //
        //     worksheet.Cells[1, startPosition++].Value = "Employee Email";
        //
        //     worksheet.Cells[1, startPosition++].Value = "Manager";
        //
        //     worksheet.Cells[1, startPosition++].Value = "Manager Email";
        //
        //     worksheet.Cells[1, startPosition++].Value = "File Name";
        //
        //     worksheet.Cells[1, startPosition++].Value = "File Size";
        //
        //     worksheet.Cells[1, startPosition++].Value = "File Description";
        //
        //     worksheet.Cells[1, startPosition++].Value = "Status";
        //
        //     worksheet.Cells[1, startPosition++].Value = "Rule Type";
        //
        //     worksheet.Cells[1, startPosition++].Value = "Repeated Count";
        //
        //     worksheet.Cells[1, startPosition++].Value = "Workstation";
        //
        //     worksheet.Cells[1, startPosition++].Value = "Audit Type";
        //
        //     worksheet.Cells[1, startPosition++].Value = "Synchronization Date";
        //
        //     worksheet.Cells[1, startPosition++].Value = "Present in the same Last Audit";
        //
        //     worksheet.Cells[1, startPosition++].Value = "Scan Date";
        //
        //     worksheet.Cells[1, startPosition++].Value = "Projects";
        //
        //     worksheet.Cells[1, startPosition++].Value = "Comment";
        //
        //     worksheet.Cells[1, startPosition++].Value = "File Id";
        //     #endregion
        //
        //     for (int i = 1; i < startPosition; i++)
        //     {
        //         using (var excelRange = worksheet.Cells[1, i])
        //         {
        //             excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //
        //             excelRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //
        //             excelRange.Style.Font.Bold = true;
        //
        //             excelRange.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
        //         }
        //     }
        // }
        //
        // private void CreateSynchronizationsWorksheetHead(
        //     ExcelWorksheet worksheet,
        //     int startPosition = 1)
        // {
        //     #region Head text
        //     worksheet.Cells[1, startPosition++].Value = "Type";
        //
        //     worksheet.Cells[1, startPosition++].Value = "Date";
        //
        //     worksheet.Cells[1, startPosition++].Value = "Who Started";
        //
        //     worksheet.Cells[1, startPosition++].Value = "Successful notifications";
        //     #endregion
        //
        //     for (int i = 1; i < startPosition; i++)
        //     {
        //         using (var excelRange = worksheet.Cells[1, i])
        //         {
        //             excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //
        //             excelRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //
        //             excelRange.Style.Font.Bold = true;
        //
        //             excelRange.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
        //         }
        //     }
        // }
    }
}