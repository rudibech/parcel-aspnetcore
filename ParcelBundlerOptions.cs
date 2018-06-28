using System.Collections.Generic;

namespace RudiBech.AspNetCore.SpaServices.Parcel
{
    /// <summary>
    /// More or less one-to-one mapping for the Bundler options <see cref="https://parceljs.org/api.html"/>.
    /// </summary>
    public class ParcelBundlerOptions
    {
        public ParcelBundlerOptions()
        {
            Port = 0;
            EnvironmentVariables = new Dictionary<string, string>();
            OutDir = "wwwroot/debug";
            PublicUrl = "/";
            Cache = true;
            Target = "browser";
            LogLevel = ParcelBundlerLogLevel.Everything;
            Watch = true;
            EntryPoint = "index.html";
        }

        //
        // Summary:
        //     The path to the file parcel should use the entry point
        public string EntryPoint { get; set; }


        //
        // Summary:
        //     The port to use for Parcel serve. Defaults to 0 resulting in a random port.
        public int Port { get; set; }
        
        //
        // Summary:
        //     The root path of your project. Webpack runs in this context.
        public string ProjectPath { get; set; }
        //
        // Summary:
        //     Specifies additional environment variables to be passed to the Node instance
        //     hosting Parcel.
        public IDictionary<string, string> EnvironmentVariables { get; set; }

        /// <summary>
        /// https://parceljs.org/api.html, Bundler => options => watch
        /// Whether to watch the files and rebuild them on change, defaults to true
        /// </summary>
        public bool Watch { get; set; }

        /// <summary>
        /// https://parceljs.org/api.html, Bundler => options => outDir
        /// The out directory to put the build files in, defaults to wwwroot
        /// </summary>
        public string OutDir { get; set; }


        /// <summary>
        /// https://parceljs.org/api.html, Bundler => options => outFile
        /// The name of the outputFile. Defaults to index.html.
        /// </summary>
        public string OutFile { get; set; }


        /// <summary>
        /// https://parceljs.org/api.html, Bundler => options => publicUrl
        /// The url to server on, defaults to /
        /// </summary>
        public string PublicUrl { get; set; }

        /// <summary>
        /// https://parceljs.org/api.html, Bundler => options => cache
        /// Enabled or disables caching, defaults to true
        /// </summary>
        public bool Cache { get; set; }


        /// <summary>
        /// https://parceljs.org/api.html, Bundler => options => cacheDir 
        /// The directory cache gets put in, defaults to .cache
        /// </summary>
        public string CacheDir { get; set; }


        /// <summary>
        /// https://parceljs.org/api.html, Bundler => options => minify 
        /// Minify files - disabled by default as you should not use this middelware in production, and you would generally want to avoid minification while developing.
        /// </summary>
        public bool Minify { get; set; }

        /// <summary>
        /// https://parceljs.org/api.html, Bundler => options => target 
        /// browser/node/electron, defaults to browser
        /// </summary>
        public string Target { get; set; }


        /// <summary>
        /// https://parceljs.org/api.html, Bundler => options => logLevel
        /// </summary>
        public ParcelBundlerLogLevel LogLevel { get; set; }


        /// <summary>
        /// https://parceljs.org/api.html, Bundler => options => sourceMaps
        /// Enable or disable sourcemaps, defaults to enabled (not supported in minified builds yet)
        /// </summary>
        public bool SourceMaps { get; set; }


        /// <summary>
        /// https://parceljs.org/api.html, Bundler => options => detailedReport
        /// // Prints a detailed report of the bundles, assets, filesizes and times, defaults to false, reports are only printed if watch is disabled
        /// </summary>
        public bool DetailedReport { get; set; }
    }

}