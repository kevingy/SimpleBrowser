// -----------------------------------------------------------------------
// <copyright file="DefaultContentHandler.cs" company="SimpleBrowser">
// Copyright © 2010 - 2020, Nathan Ridley and the SimpleBrowser contributors.
// See https://github.com/SimpleBrowserDotNet/SimpleBrowser/blob/master/readme.md
// </copyright>
// -----------------------------------------------------------------------

namespace SimpleBrowser.ContentHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml;
    using SimpleBrowser.Internal;
    using SimpleBrowser.Network;

    [Export(typeof(IContentHandler))]
    internal class DefaultContentHandler : IContentHandler
    {
        private static readonly string HandlerName = "Default";
        private string Filename { get; set; }

        public IEnumerable<string> ContentTypes
        {
            get
            {
                return new HashSet<string>() { "*/*" };
            }
        }

        public string Name
        {
            get
            {
                return HandlerName;
            }
        }

        public string HandleResponse(IHttpWebResponse response)
        {
            // If there's nothing to write, don't write.
            // (This also a hack that hides the busted way that redirects are handled.)
            if (response.ContentLength == 0)
            {
                return string.Empty;
            }

            // Try to determine the file name from the response.
            this.Filename = this.DetermineFilename(response.ResponseUri);

            using (Stream output = File.OpenWrite(this.Filename))
            using (Stream input = response.GetResponseStream())
            {
                input.CopyTo(output);
            }

            return this.CreateReturnDataPayload();
        }

        private string DetermineFilename(Uri uri)
        {
            string filename = Path.GetFileName(uri.LocalPath);

            // Try to use the filename from the URI.
            if (string.IsNullOrEmpty(filename))
            {
                filename = uri.Segments.Last();
            }

            string regularExpression = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex regex = new Regex(string.Format("[{0}]", Regex.Escape(regularExpression)));
            filename = regex.Replace(filename, "");

            // If that didn't work, use the domain.
            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = uri.Host;
                filename = regex.Replace(filename, "");
            }

            // If that didn't work, just create a random one.
            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = Guid.NewGuid().ToString();
            }

            string fullyQualified = FileHelpers.NextAvailableFilename(Path.Combine(Browser.DownloadDirectory, filename));

            return fullyQualified;
        }

        private string CreateReturnDataPayload()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = xmlDoc.DocumentElement;
            xmlDoc.InsertBefore(xmlDeclaration, root);

            XmlElement body = xmlDoc.CreateElement(string.Empty, "body", string.Empty);
            xmlDoc.AppendChild(body);

            XmlNode filenameNode = xmlDoc.CreateElement("Filename");
            filenameNode.InnerText = this.Filename;
            body.AppendChild(filenameNode);

            return xmlDoc.ToString();
        }
    }
}