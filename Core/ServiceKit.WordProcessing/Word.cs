using System;
using ServiceKit.WordProcessing.Processor;

namespace ServiceKit.WordProcessing
{
    public static class Word
    {
        public static byte[] FromHtml(string html)
            => new WordProcessor().Html(html);
        
        public static byte[] FromHtml(string html, string templatepath)
            => new WordProcessor().Html(html, templatepath);
        
    }
}
