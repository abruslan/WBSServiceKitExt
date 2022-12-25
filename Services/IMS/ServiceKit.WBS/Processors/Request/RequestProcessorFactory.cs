using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.WBS.Processors.Request
{
    public static class RequestProcessorFactory
    {
        public static IRequestProcessor CreateRequestProcessor(this WBS_Request request, AppDbContext _context)
        {
            switch (request.RequestType)
            {
                case WBS_RequestType.Project:
                    return new ProjectRequestProcessor(_context);
                case WBS_RequestType.Changes:
                    return new ChangeRequestProcessor(_context);
            }
            throw new Exception($"Не определен тип заявки {request.RequestType}");
        }
    }
}
