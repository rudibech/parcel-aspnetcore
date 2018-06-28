"use strict";
// Pass through the invocation to the 'parcel-bundler' package, verifying that it can be loaded
function createParcelDevServer(callback, options) {
    try {
        const Bundler = require('parcel-bundler');
        
        const parcelOptions = JSON.parse(options);

        const bundler = new Bundler(parcelOptions.entryPoint, parcelOptions);

        bundler.serve(parcelOptions.hmrPort).then((server) => {
            callback(null, {
                Port: server.address().port,
                PublicPaths: [parcelOptions.publicUrl]
            });
        });
    }
    catch (ex) {
        // Developers sometimes have trouble with badly-configured Node installations, where it's unable
        // to find node_modules. Or they accidentally fail to deploy node_modules, or even to run 'npm install'.
        // Make sure such errors are reported back to the .NET part of the app.
        callback('Webpack dev middleware failed because of an error while loading \'parcel-bundler\'. Error was: '
            + ex.stack
            + '\nCurrent directory is: '
            + process.cwd());
    }
}
exports.createParcelDevServer = createParcelDevServer;