using ServiceKit.CSIT.CSP.Common;
using ServiceKit.CSIT.CSP.Data;
using ServiceKit.CSIT.CSP.Data.Entry;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.CSIT.CSP.Processors
{
    public class ClientGenerator
    {
        const string LOG_AREA_SERVICE = "Формирование сервисов";
        const string LOG_AREA_REPORT = "Формирование отчета";

        private readonly ApplicationDbContext _context;
        private RegisterLogger _logger;
        public ClientGenerator(ApplicationDbContext context)
        {
            _context = context;
            _logger = new RegisterLogger(_context);
        }

        public void GenerateClient(Register register)
        {
            foreach(var item in register.Items)
            {
                // 1. client
                Client client = null;
                if (string.IsNullOrEmpty(item.ClientINN))
                    client = _context.Clients.Where(r => r.Name == item.ClientName).FirstOrDefault();
                else
                    client = _context.Clients.Where(r => r.INN == item.ClientINN).FirstOrDefault();
                if (client == null)
                {
                    client = new Client()
                    {
                        INN = item.ClientINN,
                        Name = item.ClientName
                    };
                    _context.Clients.Add(client);
                    _context.SaveChanges();
                }
                item.Client = client;

                // 2. client service
                ClientService service = _context.ClientServices.ToList().Where(r => r.Client == client && r.OriginalName.Trim() == item.ServiceName.Trim()).FirstOrDefault();
                if (service == null)
                {
                    service = new ClientService()
                    {
                        Client = client,
                        OriginalName = item.ServiceName,
                        Name = NorvalizeServiceName(item.ServiceName)
                    };
                    _context.ClientServices.Add(service);
                    _context.SaveChanges();
                }
                item.ClientService = service;
            }
        }

        public void GenerateReport(Register register)
        {
            var calendar = new System.Globalization.GregorianCalendar();
            int dayInPeriod = Convert.ToInt32(register.SaleTo.Subtract(register.SaleFrom).TotalDays) + 1;
            _logger.Log(register, LOG_AREA_REPORT, $"Период с {register.SaleFrom} по {register.SaleTo}. Всего дней.");
            foreach (var item in register.Items)
            {
                item.RegisterRatePeriod = PeriodType.None;
                if (item.ClientService != null)
                {
                    if (item.ClientService.Price > 0)
                    {
                        item.RegisterRatePeriod = PeriodType.Month;
                        item.ReportRate = Math.Round(item.ClientService.Price / dayInPeriod, 2);
                    }
                    if (item.ClientService.YearPrice > 0)
                    {
                        item.RegisterRatePeriod = PeriodType.Year;
                        item.ReportRate = Math.Round(item.ClientService.YearPrice / calendar.GetDaysInYear(register.SaleFrom.Year), 2);
                    }
                }
            }
        }

        private string NorvalizeServiceName(string serviceName)
        {
            if (serviceName.Split("(SUB-").Length > 0)
                return "Облачный сервис " + serviceName.Split("(SUB-")[0];
            return serviceName;
        }
    }
}
