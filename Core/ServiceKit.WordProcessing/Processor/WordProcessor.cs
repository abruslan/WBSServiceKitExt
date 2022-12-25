using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ServiceKit.WordProcessing.Processor
{
    public class WordProcessor
    {
        private int _chunckId = 0;
        private string NextChunkId() => $"ltChunkNewId{++_chunckId}";
        private XNamespace xml_w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
        private XNamespace xml_r = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
        private const string html_template = @"<html><head><meta charset=utf-8><head/><body>{0}</body></html>";


        public WordProcessor()
        {

        }

        public byte[] Html(string html)
        {
            var htmlText = _verifiedHtml(html);
            
            using (MemoryStream stream = new MemoryStream())
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
            {
                AppendHtml(htmlText, wordDocument);
                return stream.ToArray();
            }
        }

        public byte[] Html(string html, string templatepath)
        {
            var htmlText = _verifiedHtml(html);

            // extract stream from file
            var stream = new MemoryStream();
            using (var fileStream = new FileStream(templatepath, FileMode.Open, FileAccess.Read))
                fileStream.CopyTo(stream);

            // standard way
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(stream, true))
            {
                wordDocument.ChangeDocumentType(WordprocessingDocumentType.Document); // change from template to document
                AppendHtml(htmlText, wordDocument);
                wordDocument.Close();
            }

            stream.Position = 0;
            return stream.ToArray();
        }

        private string _verifiedHtml(string html)
        {
            // verification for dummies: wrap in <html>
            if (html.Contains("<HTML>", StringComparison.InvariantCultureIgnoreCase))
                return html;
            return string.Format(html_template, html);

        }

        private void AppendHtml(string html, WordprocessingDocument wordDocument)
        {
            MainDocumentPart mainPart = wordDocument.MainDocumentPart;
            if (mainPart == null)
            {
                mainPart = wordDocument.AddMainDocumentPart();
                new Document(new Body()).Save(mainPart);
            }

            string altChunkId = NextChunkId();
            MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html));
            AlternativeFormatImportPart formatImportPart = wordDocument.MainDocumentPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, altChunkId);
            formatImportPart.FeedData(ms);
            AltChunk altChunk = new AltChunk();
            altChunk.Id = altChunkId;
            wordDocument.MainDocumentPart.Document.Body.Append(altChunk);
        }

    }
}
