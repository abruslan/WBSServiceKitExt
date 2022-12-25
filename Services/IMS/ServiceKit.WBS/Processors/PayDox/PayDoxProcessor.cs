using Microsoft.EntityFrameworkCore;
using ServiceKit.ExternalSystem.Common;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.WBS.Processors.PayDox
{
    public class PayDoxProcessor
    {
        protected readonly AppDbContext _context;
        protected readonly WBS_Project _project;
        public readonly List<ServiceKit.PayDox.PayDoxProvider> providers;
        
        public PayDoxProcessor(AppDbContext context, WBS_Project project)
        {
            _context = context;
            _project = project;

            providers = new List<ServiceKit.PayDox.PayDoxProvider>();
            foreach(var sync in _context.WBS_ProjectPublications.Include(r => r.SyncSystem).Where(r => r.ProjectId == project.Id))
                providers.Add(new ServiceKit.PayDox.PayDoxProvider(new PayDoxConfiguration(sync.SyncSystem)));
        }

        public void CheckConnect()
        {
            foreach (var provider in providers)
                provider.CheckConnect();
        }

        public List<WBS_SyncLogItem> CheckInfoMessages()
        {
            var ret = new List<WBS_SyncLogItem>();
            foreach (var provider in providers)
                foreach (var log in provider.CheckInfo.Log)
                    ret.Add(new WBS_SyncLogItem() { Level = log.Level, Message = $"{log.Source}: {log.Message}", SyncSystemId = log.SystemId });
            return ret;
        }

        public List<WBS_SyncLogItem> SyncInfoMessages()
        {
            var ret = new List<WBS_SyncLogItem>();
            foreach (var provider in providers)
                foreach (var log in provider.SyncInfo.Log)
                    ret.Add(new WBS_SyncLogItem() { Level = log.Level, Message = $"{log.Source}: {log.Message}", SyncSystemId = log.SystemId });
            return ret;
        }

        internal void Sync(Guid requestId)
        {
            foreach (var provider in providers)
                provider.SyncProject(_project, requestId);

        }
    }
}
