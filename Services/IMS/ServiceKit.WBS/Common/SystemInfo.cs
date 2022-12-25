using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ServiceKit.WBS
{
    public static class SystemInfo
    {
        private static DateTime BaseDate => new DateTime(2022, 01, 01);

        private static string _Version = null;
        public static string Version => GetVersion();

        public static string GetVersion()
        {
            if (_Version != null)
                return _Version;

            _Version = typeof(SystemInfo).Assembly.GetName().Version.ToString();

            var version = typeof(SystemInfo).Assembly.GetName().Version;
            var dt = GetLinkerTime(Assembly.GetExecutingAssembly());
            _Version = $"{version.Major}.{version.Minor}.{dt.Subtract(BaseDate).Days / 14}.{Convert.ToInt32(dt.Subtract(BaseDate).TotalHours / 3)}";

            return _Version;
        }

        private static DateTime? _BuildDate = null;
        public static DateTime BuildDate => GetBuildDate();
        public static DateTime GetBuildDate()
        {
            _BuildDate = _BuildDate ?? GetLinkerTime(Assembly.GetExecutingAssembly());
            return (DateTime)_BuildDate;
        }

        private static DateTime GetLinkerTime(Assembly assembly)
        {
            DateTime buildDate = new FileInfo(assembly.Location).LastWriteTime;
            return buildDate;
        }
    }
}
