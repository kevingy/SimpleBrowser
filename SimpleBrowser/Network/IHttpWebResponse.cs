// -----------------------------------------------------------------------
// <copyright file="IHttpWebResponse.cs" company="SimpleBrowser">
// Copyright © 2010 - 2020, Nathan Ridley and the SimpleBrowser contributors.
// See https://github.com/SimpleBrowserDotNet/SimpleBrowser/blob/master/readme.md
// </copyright>
// -----------------------------------------------------------------------

namespace SimpleBrowser.Network
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    /// <summary>
    /// An HTTP web response interface.
    /// </summary>
    public interface IHttpWebResponse : IDisposable
    {
        /// <summary>
        /// Get the response stream.
        /// </summary>
        /// <returns></returns>
        Stream GetResponseStream();

        /// <summary>
        /// Gets the character set.
        /// </summary>
        string CharacterSet { get; }

        /// <summary>
        /// Gets the content encoding.
        /// </summary>
        /// <remarks>
        /// The ability to set this value outside of the response is a HACK to allow for poorly formed unit tests.
        /// When the unit tests are fixed, the ability to set the status code will be removed.
        /// </remarks>
        Encoding ContentEncoding { get; set; }

        /// <summary>
        /// Gets the content length.
        /// </summary>
        long ContentLength { get; }

        /// <summary>
        /// Gets or sets content type.
        /// </summary>
        string ContentType { get; set; }


        /// <summary>
        /// Gets the cookies in the response.
        /// </summary>
        CookieCollection Cookies { get; }

        /// <summary>
        /// Gets or set the response headers.
        /// </summary>
        /// <remarks>
        /// The ability to set this value outside of the response is a HACK to allow for poorly formed unit tests.
        /// When the unit tests are fixed, the ability to set the status code will be removed.
        /// </remarks>
        WebHeaderCollection Headers { get; set;  }

        /// <summary>
        /// Gets the response URI.
        /// </summary>
        Uri ResponseUri { get; }

        /// <summary>
        /// Gets the HTTP status code.
        /// </summary>
        /// <remarks>
        /// The ability to set this value outside of the response is a HACK to allow for poorly formed unit tests.
        /// When the unit tests are fixed, the ability to set the status code will be removed.
        /// </remarks>
        HttpStatusCode StatusCode { get; set; }
    }
}