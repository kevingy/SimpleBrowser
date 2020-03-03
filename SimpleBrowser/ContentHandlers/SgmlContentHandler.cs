// -----------------------------------------------------------------------
// <copyright file="SgmlContentHandler.cs" company="SimpleBrowser">
// Copyright © 2010 - 2020, Nathan Ridley and the SimpleBrowser contributors.
// See https://github.com/SimpleBrowserDotNet/SimpleBrowser/blob/master/readme.md
// </copyright>
// -----------------------------------------------------------------------

namespace SimpleBrowser.ContentHandlers
{
    using System.Collections.Generic;
    using System.Composition;
    using System.IO;
    using SimpleBrowser.Network;

    [Export(typeof(IContentHandler))]
    public class SgmlContentHandler : IContentHandler
    {
        private static readonly string HandlerName = "SimpleBrowser SGML Handler";

        public IEnumerable<string> ContentTypes => new HashSet<string>()
        {
            "application/xml",
            "application/xml-dtd",
            "application/xhtml+xml",
            "text/html",
            "text/sgml",
        };

        public string Name => HandlerName;

        public string HandleResponse(IHttpWebResponse response)
        {
            string html;

            // ensure the stream is disposed
            using (Stream rs = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(rs, response.ContentEncoding))
            {
                html = reader.ReadToEnd();
            }

            return html;
        }
    }
}