using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServiceKit.CSIT.CSP.Data;
using ServiceKit.CSIT.CSP.Data.Entry;

namespace ServiceKit.CSIT.CSP.Pages.CSP
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ServiceKit.CSIT.CSP.Data.ApplicationDbContext _context;
        public RegisterStatus[] AvailableStatuses =  { RegisterStatus.Imported, RegisterStatus.Checking, RegisterStatus.Checked, RegisterStatus.Closed, RegisterStatus.Archived };

        public DetailsModel(ServiceKit.CSIT.CSP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Register Register { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Register = await _context.Registers.Where(r => r.Id == id)
                .Include(r => r.Items)
                .ThenInclude(r => r.Client)
                .Include(r => r.Items)
                .ThenInclude(r => r.ClientService)
                .ThenInclude(r => r.ClientAnnex)
                .FirstOrDefaultAsync();

            if (Register == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnGetJsonAsync(Guid? register, Guid? client)
        {
            if (register == null)
            {
                return NotFound();
            }
            var _client = _context.Clients.FirstOrDefault(r => r.Id == client);
            var _report = _context.Registers.Include(r => r.Items)
                            .ThenInclude(r => r.ClientService)
                            .ThenInclude(r => r.ClientAnnex)
                            .Include(r => r.Items)
                            .ThenInclude(r => r.Client)
                            .FirstOrDefault(r => r.Id == register);
            var _items = _report.Items
                .Where(r => r.Client.Id == client)
                .Select(r => new { 
                ServiceName = r.ClientService.Name.Trim(),
                Total = r.ReportRate * r.WorkPlaceCount * r.DayCount
            } ).ToList();
            var _items2 = _items.GroupBy(r => r.ServiceName)
                .Select(r => new { ServiceName = r.Key, Total = r.Sum(m => m.Total) })
                .ToList();

            var ret = new {
                            _client.Name,
                            _client.INN,
                            Items = _items2
                        };
            var _data = JsonConvert.SerializeObject(ret);
            return File(Encoding.UTF8.GetBytes(_data), "application/json", "file.json");


            //var report = _cob item in register.Items.Where(r => r.Client == client && r.RegisterRatePeriod == PeriodType.Month)
            //var register = _context.Registers.FirstOrDefault(r => r.Id == id);
            //if (register == null || register.Data == null)
            //    return NotFound();
            //return File(register.Data, register.ContentType, register.FileName);
            //return NotFound();
        }

        public RegisterStatus NextStatus()
        {
            if (Register.Status == RegisterStatus.Archived)
                return RegisterStatus.Archived;
            return (RegisterStatus)(((int)Register.Status) + 1);
        }
        public RegisterStatus PrevStatus()
        {
            if (Register.Status == RegisterStatus.None || Register.Status == RegisterStatus.Imported)
                return RegisterStatus.Imported;
            return (RegisterStatus)(((int)Register.Status) - 1);
        }

        public List<ClientService> GetAppropriateServices()
        {
            var services = Register.Items.Select(r => r.ClientService).ToList();
            return _context.ClientServices.Where(r => services.Contains(r)).Include(r => r.Client).ToList();
        }

        //public List<Client> GetAppropriateClients()
        //{
        //    Register.Items.GroupBy(r => r.Client.Name).ToList();
        //    var services = Register.Items.Select(r => r.Client).ToList();
        //    return _context.Clients.Where(r => services.Contains(r)).Include(r => r.Client).ToList();
        //}

        public string EmptyClass(string value)
            => string.IsNullOrEmpty(value) ? "empty" : "full";
        public string EmptyClass(decimal value)
            => value <= 0 ? "empty" : "full";

        public List<ClientAnnex> GetAnnexList(Guid clientid, string clientname, PeriodType periodType)
        {
            var _services = Register.Items.Where(r => r.Client.Id == clientid && r.RegisterRatePeriod == periodType).Select(r => r.ClientService).ToList();
            var _annexes = _services.Select(r => r.ClientAnnex).Distinct().ToList();
            return _annexes;
        }
    }
}
