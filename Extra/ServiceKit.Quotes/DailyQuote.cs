using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.Quotes
{
    public static partial class DailyQuote
    {
        public static string GetRandom()
        {
            var _Quotes = new List<Quote>();
            foreach (var line in _quotes)
                _Quotes.Add(new Quote() { Text = line });
            if (_Quotes.Count == 0) return "";

            var i = new Random().Next(0, _Quotes.Count - 1);
            return _Quotes[i].Text;
        }
    }
}
