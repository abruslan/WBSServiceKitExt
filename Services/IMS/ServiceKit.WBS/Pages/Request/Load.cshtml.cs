using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;
using ServiceKit.WBS.Processors;

namespace ServiceKit.WBS.Pages.Request
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = Common.RoleProvider.Writer)]
    public class LoadModel : PageModel
    {
        private readonly AppDbContext _context;

        public LoadModel(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        [Required(ErrorMessage = "Укажите тип файла")]
        public WBS_RequestType FileType { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Выберите файл для загрузки")]
        public IFormFile Upload { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(IFormFile Upload)
        {
            if (Upload == null || Upload.Length <= 0)
                ModelState.AddModelError("Upload", "Файл не выбран");
            else if (!Path.GetExtension(Upload.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                ModelState.AddModelError("Upload", $"Расширение файла {Path.GetExtension(Upload.FileName)} не поддерживается. Выберите файл с расширением xlsx."); 

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var processor = CreateExcelProcessor();
            if (processor == null)
            {
                ModelState.AddModelError("FileType", "Выберите тип загружаемого файла");
                return Page();
            }

            var request = processor.Eval(Upload);
            if (processor.ErrorMessage.Count > 0)
            {
                foreach(var err in processor.ErrorMessage)
                    ModelState.AddModelError("", err);
                return Page();
            }

            _context.WBS_Requests.Add(request);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Details", new { id = request.Id });
        }

        private IExcelProcessor CreateExcelProcessor()
        {
            switch (FileType)
            {
                case WBS_RequestType.Project:
                    return new ExcelProjectRequestProcessor(_context);
                case WBS_RequestType.Changes:
                    return new ExcelChangeRequestProcessor(_context);
            }
            return null;
        }
    }
}
