using Aspose.Cells;
using Gmobile.Core.Inventory.Models.Dtos;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Domain.BusinessServices
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        public FileService(ILogger<FileService> logger)
        {
            _logger = logger;
        }

        public async Task<List<SettingItem>> ReadFileXls(Stream stream)
        {
            try
            {

                List<SettingItem> lst = new List<SettingItem>();
                var desFileCsv = await GetFilexls(stream, retry: 0);
                var compareDate = DateTime.Now;
                using (var sreader = System.IO.File.OpenRead(desFileCsv))
                {
                    var lines = sreader.ReadLines();
                    foreach (var item in lines)
                    {
                        if (!string.IsNullOrEmpty(item) && !item.StartsWith("Mobile") && !item.StartsWith("Evaluation Only"))
                        {
                            var line = item.Split(',');
                            if (line.Length >= 1)
                            {
                                lst.Add(new SettingItem()
                                {
                                    Mobile = line[0],
                                    Serial = line.Length >= 2 ? line[1] : string.Empty,
                                    Package = line.Length >= 3 ? line[2] : string.Empty,
                                });
                            }
                        }
                    }
                }
                System.IO.File.Delete(desFileCsv);
                return lst;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ReadFileXls error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                return null;
            }
        }
        private async Task<string> GetFilexls(Stream stream, int retry = 0)
        {
            string pathSave = Path.Combine(Directory.GetCurrentDirectory(), "files", DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls");
            using (var streamFile = System.IO.File.Create(pathSave))
            {
                await stream.CopyToAsync(streamFile);
                streamFile.Close();
            }
            try
            {
                var book = new Workbook(pathSave);
                _logger.LogInformation($"Continue GetFilexls Save sang file csv khoi tao");
                var desFileCsv = Path.Combine(Directory.GetCurrentDirectory(), "files", DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv");
                if (System.IO.File.Exists(desFileCsv))
                    return desFileCsv;

                // save XLSX as CSV
                _logger.LogInformation($"Continue GetFilexls Save sang file csv getPath");
                book.Save(desFileCsv, SaveFormat.CSV);
                _logger.LogInformation($"Continue GetFilexls Save sang file csv Save file Done");
                System.IO.File.Delete(pathSave);
                _logger.LogInformation($"Continue GetFilexls Save sang file csv xong: {desFileCsv}");
                return desFileCsv;
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetFilexls Save .csv error: {ex.Message}|{ex.InnerException}|{ex.StackTrace}");
                if (retry >= 1)
                    return string.Empty;
                await Task.Delay(new TimeSpan(0, 0, 1));
                return await GetFilexls(stream, retry: 1);
            }
        }
    }
}
