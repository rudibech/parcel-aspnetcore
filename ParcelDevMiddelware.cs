using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.NodeServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
          
namespace RudiBech.AspNetCore.SpaServices.Parcel
{
    /// <summary>
    /// Extension methods that can be used to enable Parcel dev middleware support.
    /// 
    /// Based on https://github.com/aspnet/JavaScriptServices/tree/dev/src/Microsoft.AspNetCore.SpaServices/Webpack with minor modifications
    /// </summary>
    public static partial class ParcelDevMiddelware
    {
        private static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver(),

            TypeNameHandling = TypeNameHandling.None
        };

        /// <summary>
        /// Enables Parcel dev middleware support. This hosts an instance of Parcel in memory
        /// in your application so that you can always serve up-to-date Parcel-built resources without having
        /// to run it manually. Since Parcel is retained in memory, incremental
        /// compilation is vastly faster that re-running it from scratch.
        ///
        /// Incoming requests that match Parcel-built files will be handled by returning the Parcel
        /// output directly, regardless of files on disk. If compilation is in progress when the request arrives,
        /// the response will pause until updated compiler output is ready.
        /// </summary>
        /// <param name="appBuilder">The <see cref="IApplicationBuilder"/>.</param>
        /// <param name="options">Options for configuring Parcel.</param>
        public static void UseParcelDevMiddleware(
            this IApplicationBuilder appBuilder,
            ParcelBundlerOptions options = null)
        {
            // Prepare options
            if (options == null)
            {
                options = new ParcelBundlerOptions();
            }
     
            // Unlike other consumers of NodeServices, ParcelDevMiddleware dosen't share Node instances, nor does it
            // use your DI configuration. It's important for ParcelDevMiddleware to have its own private Node instance
            // because it must *not* restart when files change (if it did, you'd lose all the benefits of Parcel
            // middleware). And since this is a dev-time-only feature, it doesn't matter if the default transport isn't
            // as fast as some theoretical future alternative.
            var nodeServicesOptions = new NodeServicesOptions(appBuilder.ApplicationServices);
            nodeServicesOptions.WatchFileExtensions = new string[] { }; // Don't watch anything
            if (!string.IsNullOrEmpty(options.ProjectPath))
            {
                nodeServicesOptions.ProjectPath = options.ProjectPath;
            }

            if (options.EnvironmentVariables != null)
            {
                foreach (var kvp in options.EnvironmentVariables)
                {
                    nodeServicesOptions.EnvironmentVariables[kvp.Key] = kvp.Value;
                }
            }

            var nodeServices = NodeServicesFactory.CreateNodeServices(nodeServicesOptions);

            // Get a filename matching the middleware Node script
            //Actual resource name: RudiBech.AspNetCore.SpaServices.Parcel.parcel-dev-middleware.js
            var script = EmbeddedResourceReader.Read(typeof(ParcelDevMiddelware), "/parcel-dev-middleware.js");            

            // Will be cleaned up on process exit
            var nodeScript = new StringAsTempFile(script, nodeServicesOptions.ApplicationStoppingToken); 

            // Tell Node to start the server hosting webpack-dev-middleware
            var parcelOptions = new
            {
                watch = options.Watch,
                hmrPort = options.Port,
                outDir = options.OutDir,
                outFile = options.OutFile,
                publicUrl = options.PublicUrl,
                cache = options.Cache,
                cacheDir = options.CacheDir,
                minify = options.Minify,
                target = options.Target,
                logLevel = (int) options.LogLevel,
                sourceMaps = options.SourceMaps,
                detailedReport = options.DetailedReport,
                entryPoint = options.EntryPoint
            };

            var devServerInfo =
                nodeServices.InvokeExportAsync<ParcelDevServerInfo>(nodeScript.FileName, "createParcelDevServer",
                    JsonConvert.SerializeObject(parcelOptions, jsonSerializerSettings)).Result;

            // Proxy the corresponding requests through ASP.NET and into the Node listener
            // Anything under /<publicpath> (e.g., /dist) is proxied as a normal HTTP request with a typical timeout (100s is the default from HttpClient)
            foreach (var publicPath in devServerInfo.PublicPaths)
            {
                appBuilder.UseProxyToLocalParcelDevMiddleware(publicPath, devServerInfo.Port, TimeSpan.FromSeconds(100));
            }
        }

        private static void UseProxyToLocalParcelDevMiddleware(this IApplicationBuilder appBuilder, string publicPath, int proxyToPort, TimeSpan requestTimeout)
        {
            // Note that this is hardcoded to make requests to "localhost" regardless of the hostname of the
            // server as far as the client is concerned. This is because ConditionalProxyMiddlewareOptions is
            // the one making the internal HTTP requests, and it's going to be to some port on this machine
            // because aspnet-webpack hosts the dev server there. We can't use the hostname that the client
            // sees, because that could be anything (e.g., some upstream load balancer) and we might not be
            // able to make outbound requests to it from here.
            // Also note that the webpack HMR service always uses HTTP, even if your app server uses HTTPS,
            // because the HMR service has no need for HTTPS (the client doesn't see it directly - all traffic
            // to it is proxied), and the HMR service couldn't use HTTPS anyway (in general it wouldn't have
            // the necessary certificate).
            var proxyOptions = new ConditionalProxyMiddlewareOptions("http", "localhost", proxyToPort.ToString(), requestTimeout);

            appBuilder.UseMiddleware<ConditionalProxyMiddleware>(publicPath, proxyOptions);
        }
    }
}