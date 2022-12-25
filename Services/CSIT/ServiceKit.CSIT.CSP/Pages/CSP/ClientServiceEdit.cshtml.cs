using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceKit.CSIT.CSP.Data;
using ServiceKit.CSIT.CSP.Data.Entry;

namespace ServiceKit.CSIT.CSP.Pages.CSP
{
    [Authorize]
    public class ClientServiceEditModel : PageModel
    {
        private readonly ServiceKit.CSIT.CSP.Data.ApplicationDbContext _context;

        public ClientServiceEditModel(ServiceKit.CSIT.CSP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ClientService ClientService { get; set; }
        [BindProperty]
        public Guid registerId { get; set; }

        [BindProperty]
        public string ClientAnnex { get; set; }

        [BindProperty]
        public string NewAnnex { get; set; }

        public List<SelectListItem> ClientAnnexes { get; set; } = new List<SelectListItem>();

        public async Task<IActionResult> OnGetAsync(Guid id, Guid register)
        {
            registerId = register;
            ClientService = await _context.ClientServices
                .Include(r => r.Client).Include(r => r.ClientAnnex)
                .FirstOrDefaultAsync(m => m.Id == id);
            ClientAnnex = ClientService.ClientAnnex?.Id.ToString();
            if (ClientService.Client != null)
            {
                var clientid = ClientService.Client.Id;
                ClientAnnexes = await _context.ClientAnnexes
                    .Include(r => r.Client)
                    .Where(r => r.Client.Id == clientid)
                    .OrderBy(r => r.Name)
                    .Select(r => new SelectListItem(r.Name, r.Id.ToString()))
                    .ToListAsync();
            }
            
            if (ClientService == null)
                return NotFound();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            var _service = await _context.ClientServices
                .Include(r => r.Client).Include(r => r.ClientAnnex)
                .FirstOrDefaultAsync(m => m.Id == ClientService.Id);
            _service.Price = ClientService.Price;
            _service.YearPrice = ClientService.YearPrice;
            _service.ClientAnnex = ExtractClientAnnex(_service);

            if (_service.Price > 0)
                _service.YearPrice = 0;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientServiceExists(ClientService.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new { id = registerId });
        }

        private ClientAnnex ExtractClientAnnex(ClientService service)
        {
            if (string.IsNullOrEmpty(ClientAnnex))
                return null;
            if (ClientAnnex == "new")
            {
                var _annex = new ClientAnnex()
                {
                    Client = service.Client,
                    Name = NewAnnex
                };
                _context.ClientAnnexes.Add(_annex);
                return _annex;
            }
            if(Guid.TryParse(ClientAnnex, out var guid))
            {
                return _context.ClientAnnexes.FirstOrDefault(r => r.Id == guid);
            }
            return null;
        }

        private bool ClientServiceExists(Guid id)
        {
            return _context.ClientServices.Any(e => e.Id == id);
        }
    }
}
