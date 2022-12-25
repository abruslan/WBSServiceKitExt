using ServiceKit.CSIT.CSP.Data;
using ServiceKit.CSIT.CSP.Data.Entry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.IO;
using System.Text;
using ServiceKit.Helpers.Common;

namespace ServiceKit.CSIT.CSP.Processors
{
    public class ZipFor1CGenerator
    {
        private readonly ApplicationDbContext _context;
        private const string file_version = "v1.2";
        public ZipFor1CGenerator(ApplicationDbContext context)
        {
            _context = context;
        }

        public byte[] Zip(Register register)
        {
            var files = new List<ZipArchiveGenerator.InMemoryFile>();
            foreach (var client in register.Items.GroupBy(r => new { r.Client, r.ClientService?.ClientAnnex }))
            {
                // игнорируем организации, где нет ни одной выставленной услуги
                if (client.All(r => r.Total() == 0))
                    continue;

                // переменные для записи в файл
                var data = "";
                int file_count = 0;
                decimal file_total = 0;

                foreach(var row in client.OrderBy(r => r.ClientService.Name))
                {
                    data += $"{row.ClientService.Name.Trim()}|{row.WorkPlaceCount}|{row.Total()}|{PeriodName(row.RegisterRatePeriod)}|{row.RegisterRate}\r\n";

                    file_count++;
                    file_total += row.Total();
                }

                // добавляем сводную информацию в первую строку файла
                data = $"{file_count}|{file_total}|{file_version}\r\n" + data;

                // созраняем данные в файле
                files.Add(new ZipArchiveGenerator.InMemoryFile()
                {
                    FileName = $"{client.Key.Client.GetFileName()}{GetClientAnnexFileName(client.Key.ClientAnnex)}.txt",
                    Content = Encoding.GetEncoding(1251).GetBytes(data)
                });
            }

            return ZipArchiveGenerator.GetZipArchive(files);

        }

        private object GetClientAnnexFileName(ClientAnnex clientAnnex)
        {
            return clientAnnex == null ? "" : "-"+ clientAnnex.Name.RemoveInvalidFileNameChars();
        }

        private string PeriodName(PeriodType periodtype)
        {
            switch (periodtype)
            {
                case PeriodType.Month:
                    return "M";
                case PeriodType.Year:
                    return "Y";
            }
            return "";

        }

        private List<ClientAnnex> GetAnnexList(Register register, Guid clientid, PeriodType periodType)
        {
            var _services = register.Items.Where(r => r.Client.Id == clientid && r.RegisterRatePeriod == periodType).Select(r => r.ClientService).ToList();
            var _annexes = _services.Select(r => r.ClientAnnex).Distinct().ToList();
            return _annexes;
        }

    }
}
