using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;

namespace ServiceKit.WBS.Pages.Project
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = Common.RoleProvider.Reader)]
    public class ItemModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public ItemModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        public IList<WBS_ProjectItem> WBS_ProjectItem { get;set; }
        public WBS_Project WBS_Project { get;set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
                return NotFound();

            WBS_Project = await _context.WBS_Projects.Include(w => w.ProjectItems).Where(r => r.Id == id).FirstAsync();
            if (WBS_Project == null)
                return NotFound();

            WBS_ProjectItem = WBS_Project.ProjectItems.Where(r => !r.IsDeleted).ToList();

            return Page();
        }

        public string Full1CName(WBS_ProjectItem item)
        {
            if (item.ParentId == null)
                return $"{WBS_Project.ProjectShortName}_{item.ShortName}";

            var parent = WBS_ProjectItem.FirstOrDefault(r => r.Id == item.ParentId);
            if (parent == null || parent.Level >= item.Level)
                return $"{WBS_Project.ProjectShortName}_{item.ShortName}";

            return $"{Full1CName(parent)}_{item.ShortName}";
        }
    }
}
