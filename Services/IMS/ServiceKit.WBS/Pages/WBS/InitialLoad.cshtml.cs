using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMS.ERP1C.WBS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;
using ServiceKit.WBS.Processors;

namespace ServiceKit.WBS.Pages.WBS
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = Common.RoleProvider.SuperAdmin)]
    public class InitialLoadModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public InitialLoadModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public WBS_Item WBS_Item { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

#if DEBUG
            var json = System.IO.File.ReadAllText(@"C:\Code\AlvaIT\ServiceKit\Services\IMS\ServiceKit.WBS\Resources\1С.json");
            var list = JsonConvert.DeserializeObject<List<ERP1C_WBSItem>>(json);

            var processor = new WBSItemProcessor(_context);
            await processor.Import(list);

#endif

            //var syncsystem = _context.WBS_SyncSystems.Where(r => !r.IsDeleted && r.SystemType == WBS_SyncSystemType.Import1C).FirstOrDefault();
            //if (syncsystem == null)
            //    return BadRequest("Не найдена настройка внешней системы для первичной загрузки данных");

            //var _wsconfig = new ExternalSystem.Common.WebServiceConfiguration() { 
            //    BaseURL = syncsystem.ConnectionString,
            //    Login = syncsystem.Login,
            //    Password = syncsystem.Password
            //};
            //var adapter = new ExternalSystem.Common.WebServiceEx(_wsconfig);
            //var list = adapter.DownloadJson<List<ERP1C_WBSItem>>("SpisokISR");

            return RedirectToPage("./Index");
        }
    }
}
