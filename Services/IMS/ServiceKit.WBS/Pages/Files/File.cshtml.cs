using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using ServiceKit.Model.WBS;

namespace ServiceKit.WBS.Pages.Files
{
    /// <summary>
    /// Общая логика работы с файлами
    /// </summary>
    [Authorize]
    public class FileModel : PageModel
    {
        private readonly AppDbContext _context;

        public FileModel(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Скачать файл по Guid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGet(Guid id)
        {
            return NotFound();
        }

        public IActionResult OnGetRegister(Guid id)
        {
            var register = _context.WBS_Requests.FirstOrDefault(r => r.Id == id);
            if (register == null || register.Data == null)
                return NotFound();
            return File(register.Data, register.ContentType, register.FileName);
        }

    }
}
