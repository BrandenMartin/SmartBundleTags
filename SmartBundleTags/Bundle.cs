using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;

namespace SmartBundleTags.BundleTagHelpers {

    public class Bundle {
        public string outputFileName { get; set; }
        public IList<string> inputFiles { get; set; }
    }

    public class BundleOptions {
        public BundleOptions(IHostingEnvironment env) {
            this.Configure(env);
        }

        public bool UseBundles { get; set; }
        public bool AppendVersion { get; set; }

        internal void Configure(IHostingEnvironment env) {
            if (env != null) {
                var isDevelopment = env.IsDevelopment();
                UseBundles = !isDevelopment;
                AppendVersion = !isDevelopment;
            }
        }
    }
}
