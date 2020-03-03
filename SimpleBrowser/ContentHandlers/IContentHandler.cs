// -----------------------------------------------------------------------
// <copyright file="IContentHandler.cs" company="SimpleBrowser">
// Copyright © 2010 - 2020, Nathan Ridley and the SimpleBrowser contributors.
// See https://github.com/SimpleBrowserDotNet/SimpleBrowser/blob/master/readme.md
// </copyright>
// -----------------------------------------------------------------------

namespace SimpleBrowser.ContentHandlers
{
    using System.Collections.Generic;
    using SimpleBrowser.Network;

    /// <summary>
    /// An interface for a managed extensibility framework (MEF) content handler.
    /// </summary>
    /// <remarks>
    /// Microsoft Managed Extensibility Framework (MEF) documentation: https://docs.microsoft.com/en-us/dotnet/framework/mef/
    /// </remarks>
    public interface IContentHandler
    {
        /// <summary>
        /// Gets the collection of content types that are handled by the content handler.
        /// </summary>
        IEnumerable<string> ContentTypes { get; }

        /// <summary>
        /// Gets the human readable name of the content handler.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Handles the HTTP response.
        /// </summary>
        /// <param name="response">The HTTP response corresponding to the request.</param>
        /// <returns>A string representation of the response. This may be the actual content of the response (for example, cases of a SGML response) or a generated data payload specific to the respose.</returns>
        /// <remarks>IMPORTANT: The response MUST be parse-able by the HTML parser!</remarks>
        string HandleResponse(IHttpWebResponse response);
    }
}