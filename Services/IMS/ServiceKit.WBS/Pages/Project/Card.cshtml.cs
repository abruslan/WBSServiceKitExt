using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;
using ServiceKit.WBS.Processors.PayDox;

namespace ServiceKit.WBS.Pages.Project
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = Common.RoleProvider.Writer)]
    public class CardModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public CardModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        
        public WBS_Project WBS_Project { get; set; }
        public IList<WBS_SyncLog> WBS_SyncLog { get; set; }
        public List<WBS_SyncSystem> SyncedSystems { get; set; }

        public string PublishList { get; set; }

        public string StatusMessage { get; set; } = "";

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return await CurrentPage(id);
        }

        private async Task<IActionResult> CurrentPage(Guid? id)
        {
            WBS_Project = await _context.WBS_Projects.FirstOrDefaultAsync(m => m.Id == id);
            if (WBS_Project == null)
                return NotFound();

            WBS_SyncLog = await _context.WBS_SyncLogs
                .Include(r => r.Items).ThenInclude(r => r.SyncSystem)
                .Where(r => r.ProjectId == id).ToListAsync();

            SyncedSystems = _context.WBS_ProjectPublications
                .Include(r => r.SyncSystem)
                .Where(r => r.ProjectId == id && !r.SyncSystem.IsDeleted)
                .Select(r => r.SyncSystem).ToList();


            PublishList = string.Join(", ", SyncedSystems.Select(r => r.Name).ToList());
            return Page();
        }

        public async Task<IActionResult> OnPostSyncAsync(Guid? id)
        {
            if (id == null)
                return NotFound();

            WBS_Project = await _context.WBS_Projects.Include(r => r.ProjectItems).FirstOrDefaultAsync(m => m.Id == id);
            if (WBS_Project == null)
                return NotFound();

            StatusMessage = Check_WBS_Project(WBS_Project);
            if (!string.IsNullOrEmpty(StatusMessage))
            {
                return await CurrentPage(id);
            }

            var log = new WBS_SyncLog() { Items = new List<WBS_SyncLogItem>()};
            log.Project = WBS_Project;
            log.RequestId = Guid.NewGuid();
            _context.WBS_SyncLogs.Add(log);

            var processor = new PayDoxProcessor(_context, WBS_Project);
            processor.CheckConnect();
            log.Items.AddRange(processor.CheckInfoMessages());
            log.Success = processor.providers.All(r => r.CheckInfo.Success);
            if (!log.Success)
                log.Message = "Ошибка проверки соединения";

            if (log.Success)
            {
                processor.Sync(log.RequestId??Guid.Empty);
                log.Items.AddRange(processor.SyncInfoMessages());
                log.Success = processor.providers.All(r => r.SyncInfo.Success);

                log.Success = processor.providers.All(r => r.SyncInfo.Success);
                log.Message = log.Success ? "Публикация выполнена успешно" : "Ошибка публикации";
            }

            _context.SaveChanges();
            return RedirectToPage("./Card", new { id });
        }

        public async Task<IActionResult> OnPostExportJsonAsync(Guid? id)
        {
            if (id == null)
                return NotFound();

            WBS_Project = await _context.WBS_Projects.Include(r => r.ProjectItems).FirstOrDefaultAsync(m => m.Id == id);
            if (WBS_Project == null)
                return NotFound();

            var data = _context.WBS_Projects.Include(r => r.ProjectItems).ToList()
                .Where(p => p.Id == id)
                .Select(p => new {
                    p.Id,
                    p.ProjectCode,
                    p.ProjectName,
                    p.ProjectShortName,
                    p.Created,
                    p.Modified,
                    Items = p.ProjectItems.OrderBy(i => i.FullCode).Where(i => !i.IsDeleted).Select(i => new {
                        i.Level,
                        i.ShortCode,
                        i.FullCode,
                        i.ShortName,
                        Full1CName = Full1CName(WBS_Project,i),
                        i.Comment,
                        i.Created,
                        i.Modified
                    }).ToList()
                })
                .FirstOrDefault();

            var jsonstr = System.Text.Json.JsonSerializer.Serialize(data);
            byte[] byteArray = System.Text.ASCIIEncoding.ASCII.GetBytes(jsonstr);
            return File(byteArray, "application/force-download", $"{WBS_Project.ProjectCode}.json");
        }
        private string Check_WBS_Project(WBS_Project project)
        {
            foreach(var item in project.ProjectItems)
            {
                if (string.IsNullOrWhiteSpace(item.ShortName))
                    return $"Ошибка: для элемента {item.FullCode} не указано название. Операция не может быть выполнена.";
            }
            return "";
        }

        public string Full1CName(WBS_Project project, WBS_ProjectItem item)
        {
            if (item.ParentId == null)
                return $"{project.ProjectShortName}_{item.ShortName}";

            var parent = project.ProjectItems.FirstOrDefault(r => !r.IsDeleted && r.Id == item.ParentId);
            if (parent == null || parent.Level >= item.Level)
                return $"{project.ProjectShortName}_{item.ShortName}";

            return $"{Full1CName(project, parent)}_{item.ShortName}";
        }
    }
}
