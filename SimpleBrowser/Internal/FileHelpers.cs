// -----------------------------------------------------------------------
// <copyright file="FileHelpers.cs" company="SimpleBrowser">
// Copyright © 2010 - 2020, Nathan Ridley and the SimpleBrowser contributors.
// See https://github.com/SimpleBrowserDotNet/SimpleBrowser/blob/master/readme.md
// </copyright>
// -----------------------------------------------------------------------

namespace SimpleBrowser.Internal
{
    using System;
    using System.IO;
    using System.Threading;

    public static class FileHelpers
    {
        private readonly static string numberPattern = " ({0})";
        private readonly static Mutex mutex = new Mutex();

        public static string NextAvailableFilename(string path)
        {
            // Short-cut if already available
            if (!File.Exists(path))
            {
                return path;
            }

            // If path has extension then insert the number pattern just before the extension and return next filename
            if (Path.HasExtension(path))
            {
                return GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path)), numberPattern));
            }

            // Otherwise just append the pattern to the path and return next filename
            return GetNextFilename(path + numberPattern);
        }

        private static string GetNextFilename(string pattern)
        {
            string filename = string.Format(pattern, 1);
            if (filename == pattern)
            {
                throw new ArgumentException("The pattern must include an index place-holder", "pattern");
            }

            if (!File.Exists(filename))
            {
                return filename; // short-circuit if no matches
            }

            mutex.WaitOne();

            int min = 1, max = 2; // min is inclusive, max is exclusive/untested
            while (File.Exists(string.Format(pattern, max)))
            {
                min = max;
                max *= 2;
            }

            while (max != min + 1)
            {
                int pivot = (max + min) / 2;
                if (File.Exists(string.Format(pattern, pivot)))
                {
                    min = pivot;
                }
                else
                {
                    max = pivot;
                }
            }

            string result = string.Format(pattern, max);

            mutex.ReleaseMutex();

            return result;
        }
    }
}