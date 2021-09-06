using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BedLinenStore.WEB.Services.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace BedLinenStore.WEB.Services.Implementations
{
    public class ExcelService : IExcelService
    {
        public async Task<byte[]> GetExcelReportAsync()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                await CreateReportWorksheet(excelPackage);

                await CreateSynchronizationsWorksheet(excelPackage);

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return stream.ToArray();
                }
            }
        }

        private async Task CreateReportWorksheet(ExcelPackage excelPackage)
        {
            var worksheet = CheckAndCreateWorkSheet(
                excelPackage,
                "Report");

            CreateReportWorksheetHead(worksheet);

            var users = await _dbContext.Users
                .Include(user => user.ThreatFiles)
                    .ThenInclude(file => file.Rule)
                .Include(user => user.ThreatFiles)
                    .ThenInclude(file => file.Synchronization)
                        .ThenInclude(sync => sync.AuditType)
                .ToListAsync();

            (int X, int Y) pos = (2, 1);

            foreach (var user in users)
            {
                if (!(user.ThreatFiles is null)
                    || user.ThreatFiles.Count != 0)
                {
                    var userFileGroups = user.ThreatFiles
                        .GroupBy(file => new
                        {
                            Filename = file.Filename,
                            FilePath = file.FilePath,
                            FileSize = file.FileSize,
                            Description = file.Description,
                            Workstation = file.Workstation
                        });

                    foreach (var fileGroup in userFileGroups)
                    {
                        var synchronization = fileGroup.LastOrDefault().Synchronization;

                        var isAllowed = fileGroup.LastOrDefault().Rule?.IsAllowed ?? false;

                        worksheet.Cells[pos.X, pos.Y++].Value = user.Username;

                        worksheet.Cells[pos.X, pos.Y++].Value = user.Email;

                        worksheet.Cells[pos.X, pos.Y++].Value = user
                            .ResourceManagerEmail
                            .Replace('_', ' ')
                            .Replace("@epam.com", string.Empty);

                        worksheet.Cells[pos.X, pos.Y++].Value = user.ResourceManagerEmail;

                        worksheet.Cells[pos.X, pos.Y++].Value = fileGroup.Key.Filename;

                        worksheet.Cells[pos.X, pos.Y++].Value = fileGroup.Key.FileSize;

                        worksheet.Cells[pos.X, pos.Y++].Value = fileGroup.Key.Description;

                        worksheet.Cells[pos.X, pos.Y++].Value = fileGroup.LastOrDefault().Status.ToString();

                        worksheet.Cells[pos.X, pos.Y++].Value = isAllowed
                            ? "Approved"
                            : "Declined";

                        worksheet.Cells[pos.X, pos.Y++].Value = fileGroup.Count();

                        worksheet.Cells[pos.X, pos.Y++].Value = fileGroup.Key.Workstation;

                        worksheet.Cells[pos.X, pos.Y++].Value = synchronization.AuditType.Name;

                        worksheet.Cells[pos.X, pos.Y++].Value = $"{synchronization.SynchronizationDate:dd/MM/yy HH:mm:ss}";

                        worksheet.Cells[pos.X, pos.Y++].Value = await IsSameAudit(fileGroup.LastOrDefault());

                        worksheet.Cells[pos.X, pos.Y++].Value = GetScanDateFromThreatFile(fileGroup.LastOrDefault());

                        worksheet.Cells[pos.X, pos.Y++].Value = await GetUserProjectNames(user.PmcId);

                        worksheet.Cells[pos.X, pos.Y++].Value = fileGroup.LastOrDefault().Comment;

                        worksheet.Cells[pos.X, pos.Y++].Value = fileGroup.LastOrDefault().Id;

                        pos.X++;

                        pos.Y = 1;
                    }
                }
            }

            #region Autofit
            AutoFit(worksheet);
            AutoFit(worksheet);
            AutoFit(worksheet);
            AutoFit(worksheet);
            AutoFit(worksheet);
            AutoFit(worksheet);
            #endregion
        }

        private async Task CreateSynchronizationsWorksheet(ExcelPackage excelPackage)
        {
            var worksheet = CheckAndCreateWorkSheet(
                excelPackage,
                "Synchronizations");

            CreateSynchronizationsWorksheetHead(worksheet);

            var synchronizations = await _dbContext.Synchronizations
                .Include(sync => sync.AuditType)
                .OrderByDescending(sync => sync.SynchronizationDate)
                .ToListAsync();

            (int X, int Y) pos = (2, 1);

            foreach (var synchronization in synchronizations)
            {
                var starter = await _dbContext.Users.FindAsync(synchronization.StarterId);

                worksheet.Cells[pos.X, pos.Y++].Value = synchronization.AuditType.Name;

                worksheet.Cells[pos.X, pos.Y++].Value = $"{synchronization.SynchronizationDate:dd/MM/yy HH:mm:ss}";

                worksheet.Cells[pos.X, pos.Y++].Value = starter?.Username;

                worksheet.Cells[pos.X, pos.Y++].Value = synchronization.SuccessfulNotifications;

                pos.X++;

                pos.Y = 1;
            }

            worksheet.View.FreezePanes(2, 1);

            #region Autofit
            AutoFit(worksheet);
            AutoFit(worksheet);
            AutoFit(worksheet);
            AutoFit(worksheet);
            AutoFit(worksheet);
            AutoFit(worksheet);
            #endregion
        }

        private ExcelWorksheet CheckAndCreateWorkSheet(
            ExcelPackage excelPackage,
            string sheetName)
        {
            ExcelWorksheet workSheet = excelPackage
                .Workbook
                .Worksheets
                .FirstOrDefault(x => x.Name.Equals(sheetName));

            if (workSheet == null)
            {
                workSheet = excelPackage.Workbook.Worksheets.Add(sheetName);
            }
            else
            {
                if (workSheet.Dimension != null)
                {
                    workSheet.Cells[
                        workSheet.Dimension.Start.Row,
                        workSheet.Dimension.Start.Column,
                        workSheet.Dimension.End.Row,
                        workSheet.Dimension.End.Column]
                        .Clear();
                }
            }

            return workSheet;
        }

        private void CreateReportWorksheetHead(
            ExcelWorksheet worksheet,
            int startPosition = 1)
        {
            #region Head text
            worksheet.Cells[1, startPosition++].Value = "Employee Name";

            worksheet.Cells[1, startPosition++].Value = "Employee Email";

            worksheet.Cells[1, startPosition++].Value = "Manager";

            worksheet.Cells[1, startPosition++].Value = "Manager Email";

            worksheet.Cells[1, startPosition++].Value = "File Name";

            worksheet.Cells[1, startPosition++].Value = "File Size";

            worksheet.Cells[1, startPosition++].Value = "File Description";

            worksheet.Cells[1, startPosition++].Value = "Status";

            worksheet.Cells[1, startPosition++].Value = "Rule Type";

            worksheet.Cells[1, startPosition++].Value = "Repeated Count";

            worksheet.Cells[1, startPosition++].Value = "Workstation";

            worksheet.Cells[1, startPosition++].Value = "Audit Type";

            worksheet.Cells[1, startPosition++].Value = "Synchronization Date";

            worksheet.Cells[1, startPosition++].Value = "Present in the same Last Audit";

            worksheet.Cells[1, startPosition++].Value = "Scan Date";

            worksheet.Cells[1, startPosition++].Value = "Projects";

            worksheet.Cells[1, startPosition++].Value = "Comment";

            worksheet.Cells[1, startPosition++].Value = "File Id";
            #endregion

            for (int i = 1; i < startPosition; i++)
            {
                using (var excelRange = worksheet.Cells[1, i])
                {
                    excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    excelRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    excelRange.Style.Font.Bold = true;

                    excelRange.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                }
            }
        }

        private void CreateSynchronizationsWorksheetHead(
            ExcelWorksheet worksheet,
            int startPosition = 1)
        {
            #region Head text
            worksheet.Cells[1, startPosition++].Value = "Type";

            worksheet.Cells[1, startPosition++].Value = "Date";

            worksheet.Cells[1, startPosition++].Value = "Who Started";

            worksheet.Cells[1, startPosition++].Value = "Successful notifications";
            #endregion

            for (int i = 1; i < startPosition; i++)
            {
                using (var excelRange = worksheet.Cells[1, i])
                {
                    excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    excelRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    excelRange.Style.Font.Bold = true;

                    excelRange.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                }
            }
        }

        private void AutoFit(ExcelWorksheet worksheet)
        {
            worksheet.Cells[
                worksheet.Dimension.Start.Row,
                worksheet.Dimension.Start.Column,
                worksheet.Dimension.End.Row,
                worksheet.Dimension.End.Column]
                .AutoFitColumns();
        }

        private string GetScanDateFromThreatFile(ThreatFile threatFile)
        {
            return $"{threatFile.LastScanDate:dd/MM/yy HH:mm:ss}";
        }

        private async Task<bool> IsSameAudit(ThreatFile threatFile)
        {

            var synchronization = await _dbContext.Synchronizations
                .Include(sync => sync.ThreatFiles)
                .Where(sync => sync.AuditTypeId == threatFile.Synchronization.AuditTypeId)
                .OrderByDescending(sync => sync.SynchronizationDate)
                .FirstOrDefaultAsync();

            return synchronization?.ThreatFiles.Any(file =>
                        file.Filename == threatFile.Filename
                        && file.FilePath == threatFile.FilePath
                        && file.FileSize == threatFile.FileSize
                        && file.Description == threatFile.Description
                        && file.Workstation == threatFile.Workstation) ?? false;
        }

        private async Task<string> GetUserProjectNames(string userId)
        {
            var userProjects = await _storageContext.UsersProjectRoles
                .Include(userProjectRole => userProjectRole.Project)
                .Where(userProjectRole => userProjectRole.UserId == userId)
                .Select(userProjectRole => userProjectRole.Project.ProjectName)
                .ToListAsync();

            return string.Join(", ", userProjects);
        }
    }
}