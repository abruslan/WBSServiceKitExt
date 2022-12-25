using ServiceKit.Model.WBS.Entry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.WBS.Processors.Request
{
    public interface IRequestProcessor
    {
        public List<string> ErrorMessage { get; }
        public WBS_Project Eval(WBS_Request request);
        public bool Check(WBS_Request request);
    }
}
