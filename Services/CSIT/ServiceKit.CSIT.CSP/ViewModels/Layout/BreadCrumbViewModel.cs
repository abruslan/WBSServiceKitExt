using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.CSIT.CSP.ViewModels.Layout
{
    public class BreadCrumbViewModel
    {
        public List<BreadCrumbItem> Items { get; set; } = new List<BreadCrumbItem>();
        public BreadCrumbViewModel()
        {

        }

        public BreadCrumbViewModel(List<BreadCrumbItem> list)
        {
            Items = list;
        }

        public BreadCrumbViewModel(string Title, string Url)
        {
            Items.Add(new BreadCrumbItem() { Title = Title, Url = Url });
        }

        public BreadCrumbViewModel Add(string Title, string Url)
        {
            Items.Add(new BreadCrumbItem() { Title = Title, Url = Url });
            return this;
        }
    }

    public class BreadCrumbItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public BreadCrumbItem() { }
        public BreadCrumbItem(string Title, string Url)
        {
            this.Title = Title;
            this.Url = Url;
        }
    }
}
