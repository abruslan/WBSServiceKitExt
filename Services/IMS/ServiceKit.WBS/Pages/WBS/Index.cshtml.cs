using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;
using ServiceKit.WBS.ViewModels.Layout;

namespace ServiceKit.WBS.Pages.WBS
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = Common.RoleProvider.Reader)]
    public class IndexModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public IndexModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        public IList<WBS_Item> Items { get;set; }
        public WBS_Item Current { get;set; }
        public IList<Guid> WBS_ItemChildren { get;set; }
        public BreadCrumbViewModel BreadCrumb;

        private IList<Guid?> Parents;

        public async Task OnGetAsync(Guid? id)
        {
            BreadCrumb = new BreadCrumbViewModel().Add("ИСР", "/WBS/Index");

            Current = await _context.WBS_Items.Where(r => !r.IsDeleted && r.Id == id).FirstOrDefaultAsync();

            if (id == null)
                Items = await _context.WBS_Items.Where(r => !r.IsDeleted && r.Level == 1).ToListAsync();
            else { 
                Items = await _context.WBS_Items.Where(r => !r.IsDeleted && r.ParentId == id).ToListAsync();
            }
            await AddBreadCrumbItem(id);

            Parents = await _context.WBS_Items.Where(r => r.ParentId != null).Select(r => r.ParentId).Distinct().ToListAsync();
        }

        public bool hasChildren(Guid id)
            => Parents.Contains(id);

        private async Task AddBreadCrumbItem(Guid? id)
        {
            if (id == null) 
                return;

            var item = await _context.WBS_Items.Where(r => r.Id == id).FirstOrDefaultAsync();
            if (item == null)
                return;
            
            await AddBreadCrumbItem(item.ParentId);
            BreadCrumb.Add(item.Code, "/WBS/Index?id=" + item.Id.ToString());
        }
    }
}
