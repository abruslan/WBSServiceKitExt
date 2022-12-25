using IMS.ERP1C.WBS;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.WBS.Processors
{
    public class WBSItemProcessor
    {
        private readonly AppDbContext _context;
        private int count = 0;
        public WBSItemProcessor(AppDbContext context)
        {
            _context = context;
        }

        public async Task Import(List<ERP1C_WBSItem> list)
        {
            count = 0;
            // read 1 level
            foreach (var item in list.Where(r => !string.IsNullOrEmpty(r.Id) && string.IsNullOrEmpty(r.ParentId)))
            {
                count++;
                WBS_Item wbs = UpdateOrCreate(item, null);
                ImportBranch(list, wbs);
            }
            await _context.SaveChangesAsync();
        }

        private void ImportBranch(List<ERP1C_WBSItem> list, WBS_Item parent)
        {
            foreach (var item in list.Where(r => r.ParentId == parent.ExtId))
            {
                count++;
                WBS_Item wbs = UpdateOrCreate(item, parent);
                ImportBranch(list, wbs);
            }
        }

        private WBS_Item UpdateOrCreate(ERP1C_WBSItem item, WBS_Item parent)
        {
            WBS_Item wbs = _context.WBS_Items.FirstOrDefault(r => r.ExtId == item.Id);
            if (wbs == null)
            {
                wbs = new WBS_Item();
                _context.WBS_Items.Add(wbs);
            }
            wbs.Code = item.Code;
            wbs.Name = item.Name;
            wbs.ShortName = item.ShortName;
            wbs.FullName = item.FullName;
            wbs.ExtId = item.Id;
            wbs.IsDeleted = item.IsDeleted;
            wbs.Level = (parent == null ? 1 : parent.Level + 1);
            wbs.ParentId = parent?.Id;
            return wbs;
        }


    }
}
