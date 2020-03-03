// -----------------------------------------------------------------------
// <copyright file="WebResponseWrapper.cs" company="SimpleBrowser">
// Copyright © 2010 - 2020, Nathan Ridley and the SimpleBrowser contributors.
// See https://github.com/SimpleBrowserDotNet/SimpleBrowser/blob/master/readme.md
// </copyright>
// -----------------------------------------------------------------------

namespace SimpleBrowser.Network
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;

    /// <summary>
    /// An HTTP web response wrapper.
    /// </summary>
    internal class WebResponseWrapper : IHttpWebResponse
    {
        private readonly HttpWebResponse response;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebResponseWrapper"/> class.
        /// </summary>
        /// <param name="response">The HTTP web response.</param>
        public WebResponseWrapper(HttpWebResponse response) => this.response = response;

        /// <summary>
        /// Gets the response stream.
        /// </summary>
        /// <returns>The response stream.</returns>
        public Stream GetResponseStream() => this.response.GetResponseStream();

        /// <summary>
        /// Gets the character set.
        /// </summary>
        public string CharacterSet
        {
            get
            {
                return this.response.CharacterSet;
            }
        }

        /// <summary>
        /// Gets or sets the content encoding.
        /// </summary>
        /// <remarks>
        /// The ability to set this value outside of the response is a HACK to allow for poorly formed unit tests.
        /// When the unit tests are fixed, the ability to set the status code will be removed.
        /// </remarks>
        public Encoding ContentEncoding
        {
            get
            {
                if (Browser.ResponseEncoding == null)
                {
                    if (!string.IsNullOrWhiteSpace(this.response.CharacterSet) && this.ParseEncoding(this.response.CharacterSet) != null)
                    {
                        return Encoding.GetEncoding(this.response.CharacterSet);
                    }
                    // Is the encoding specified in the Content-Type header, but for whatever reason was not in the CharacterSet?
                    else if ((this.response.Headers.AllKeys.Contains("Content-Type", StringComparer.OrdinalIgnoreCase) &&
                         this.response.Headers["Content-Type"].IndexOf("charset", 0, StringComparison.OrdinalIgnoreCase) > -1))
                    {
                        string[] tokens = response.ContentType.Split(';');
                        foreach(string token in tokens)
                        {
                            if (token.Contains("charset"))
                            {
                                string innerTokens = token.Split('=').LastOrDefault();
                                if (this.ParseEncoding(innerTokens.Replace(";", "")) != null)
                                {
                                    return this.ParseEncoding(innerTokens.Replace(";", ""));
                                }
                            }
                        }
                    }

                    return Encoding.UTF8; // try using utf8
                }
                else
                {
                    // Use the explicitly specified encoding.
                    return Browser.ResponseEncoding;
                }
            }

            set
            {

            }
        }

        /// <summary>
        /// Gets the content length.
        /// </summary>
        public long ContentLength
        {
            get
            {
                return this.response.ContentLength;
            }
        }

        /// <summary>
        /// Gets or sets the content type;
        /// </summary>
        public string ContentType
        {
            get
            {
                if (this.response.ContentType.Contains(";"))
                {
                    string[] tokens = response.ContentType.Split(';');
                    if (tokens.Count() > 0)
                    {
                        return tokens.Where(t => t.Contains('/')).FirstOrDefault();
                    }
                }

                return this.response.ContentType;
            }

            set
            {
                this.response.ContentType = value;
            }
        }

        /// <summary>
        /// Gets the collection of cookies.
        /// </summary>
        public CookieCollection Cookies { get; }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        /// <remarks>
        /// The ability to set this value outside of the response is a HACK to allow for poorly formed unit tests.
        /// When the unit tests are fixed, the ability to set the status code will be removed.
        /// </remarks>
        public WebHeaderCollection Headers
        {
            get
            {
                return this.response.Headers;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get
            {
                return this.response.StatusCode;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets the response URI.
        /// </summary>
        public Uri ResponseUri
        {
            get
            {
                return this.response.ResponseUri;
            }
        }

        /// <summary>
        /// Disposes of unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            (this.response as IDisposable)?.Dispose();
        }

        private Encoding ParseEncoding(string encoding)
        {
            try
            {
                return Encoding.GetEncoding(encoding);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}