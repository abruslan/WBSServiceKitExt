using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;

namespace ServiceKit.CSIT.CSP.Pages
{
    public class WordModel : PageModel
    {
        IHostEnvironment _environment;
        public WordModel(IHostEnvironment environment)
        {
            _environment = environment;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost(string WordContent, string FileName, string TemplateName)
        {
            if (string.IsNullOrEmpty(TemplateName))
                return File(WordProcessing.Word.FromHtml(WordContent), WordProcessing.ContentTypes.Word, FileName);

            var fullTemplatePath = Path.Combine(_environment.ContentRootPath, "wwwroot", "resources", TemplateName);
            return File(WordProcessing.Word.FromHtml(WordContent, fullTemplatePath), WordProcessing.ContentTypes.Word, FileName);
        }

    }
}
