using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServiceKit.CSIT.CSP.Data;
using ServiceKit.CSIT.CSP.Data.Entry;

namespace ServiceKit.CSIT.CSP.Pages.Settings
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class LogModel : PageModel
    {
        private readonly ServiceKit.CSIT.CSP.Data.ApplicationDbContext _context;

        public LogModel(ServiceKit.CSIT.CSP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Log> Log { get;set; }

        public async Task OnGetAsync()
        {
            Log = await _context.Logs.ToListAsync();
        }
    }
}
