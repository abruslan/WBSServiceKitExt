using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ServiceKit.CSIT.CSP.Data;
using System.Linq;

namespace ServiceKit.CSIT.CSP.Pages.Files
{
    /// <summary>
    /// Общая логика работы с файлами
    /// </summary>
    [Authorize]
    public class FileModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FileModel(ApplicationDbContext context)
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
            var register = _context.Registers.FirstOrDefault(r => r.Id == id);
            if (register == null || register.Data == null)
                return NotFound();
            return File(register.Data, register.ContentType, register.FileName);
        }

    }
}
