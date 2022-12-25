using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.Helpers.Common
{
    public static class FileNameHelper
    {
        public static string RemoveInvalidFileNameChars(this string filename)
        {
            var invalid = Path.GetInvalidFileNameChars();
            foreach (char c in invalid)
                filename = filename.Replace(c.ToString(), "");

            return filename;
        }
    }
}
