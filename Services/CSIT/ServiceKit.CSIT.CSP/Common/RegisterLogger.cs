using Microsoft.Extensions.Logging;
using ServiceKit.CSIT.CSP.Data;
using ServiceKit.CSIT.CSP.Data.Entry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.CSIT.CSP.Common
{
    public class RegisterLogger
    {
        private readonly ApplicationDbContext _context;
        public RegisterLogger(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Log(Register register, LogLevel level, string area, string message)
        {
            _context.Logs.Add(new Log() { 
                Source = register.Id,
                LogLevel = level,
                Message = message,
                Area = area
            });
        }

        public void Log(Register register, string message)
            => Log(register, LogLevel.Information, "", message);
        
        public void Log(Register register, string area, string message)
            => Log(register, LogLevel.Information, area, message);
        
    }
}
