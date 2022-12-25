using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace ServiceKit.CSIT.CSP.Common
{

    public static class SystemConst
    {
        public static string SystemName = "ABILITY Автоматизация";
        
        private static string _Version = null;
        public static string Version => GetVersion();
        public static string GetVersion() 
        {
            if (_Version != null)
                return _Version;

            var dt = GetLinkerTime(Assembly.GetExecutingAssembly());
            _Version = $"версия 1.1 от {dt.ToShortDateString()}";
            return _Version;
        }

        private static DateTime GetLinkerTime(Assembly assembly)
        {
            DateTime buildDate = new FileInfo(assembly.Location).LastWriteTime;
            return buildDate;
        }
    }
}
