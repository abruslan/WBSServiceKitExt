using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceKit.CSIT.CSP.Data;

namespace ServiceKit.CSIT.CSP.Pages.Report
{
    [Authorize]
    public class DialogModel : PageModel
    {
        [BindProperty]
        public string ViewName { get; set; }
        [BindProperty]
        public Guid? RegisterId { get; set; }
        [BindProperty]
        public Guid? ClientId { get; set; }
        [BindProperty]
        public Guid? ClientAnnexId { get; set; }
        [BindProperty]
        public bool IsDebug { get; set; }


        public readonly ApplicationDbContext _context;

        public DialogModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet(string view, Guid? register, Guid? client, Guid? annex, string debug)
        {
            ViewName = view;
            RegisterId = register;
            ClientId = client;
            ClientAnnexId = annex;
            IsDebug = !string.IsNullOrEmpty(debug);
        }
    }
}
