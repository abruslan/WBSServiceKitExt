using ServiceKit.Model.WBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.WBS.Processors.Request
{

    public class RequestProcessor
    {
        protected const int FULL_NAME_MAX_LENGTH = int.MaxValue;
        //protected const int FULL_NAME_MAX_LENGTH = 150;
        public List<string> ErrorMessage { get; protected set; } = new List<string>();

        protected readonly AppDbContext _context;
        public RequestProcessor(AppDbContext context)
        {
            _context = context;
        }
    }
}
