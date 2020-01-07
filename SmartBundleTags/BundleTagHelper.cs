using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;

namespace SmartBundleTags.BundleTagHelpers {

    [HtmlTargetElement("bundle")]
    public class BundleTagHelper : TagHelper {
        private readonly IBundleProvider _bundleProvider;
        private readonly BundleOptions _options;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;
        private readonly HtmlEncoder _htmlEncoder;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private FileVersionProvider _fileVersionProvider;

        public BundleTagHelper(IHostingEnvironment hostingEnvironment, IMemoryCache cache, HtmlEncoder htmlEncoder, IUrlHelperFactory urlHelperFactory, BundleOptions options = null, IBundleProvider bundleProvider = null) {
            _bundleProvider = bundleProvider ?? new BundleProvider();
            _options = options ?? new BundleOptions(hostingEnvironment);
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _htmlEncoder = htmlEncoder ?? throw new ArgumentNullException(nameof(htmlEncoder));
            _urlHelperFactory = urlHelperFactory ?? throw new ArgumentNullException(nameof(urlHelperFactory));
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("name")]
        public string BundleName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.SuppressOutput();
            var bundle = _bundleProvider.GetBundle(BundleName);

            if (bundle != null) {
                var isScript = bundle.outputFileName.EndsWith(".js", StringComparison.OrdinalIgnoreCase);
                var isStylesheet = bundle.outputFileName.EndsWith(".css", StringComparison.OrdinalIgnoreCase);

                IEnumerable<string> files = GetFiles(bundle);
                foreach (string file in files) {
                    var path = file.Replace("wwwroot/", "");

                    if (isScript)
                        output.Content.AppendHtmlLine($"<script src=\"/{_htmlEncoder.Encode(path)}\" type=\"text/javascript\"></script>");
                    else if (isStylesheet)
                        output.Content.AppendHtmlLine($"<link href=\"/{_htmlEncoder.Encode(path)}\" rel=\"stylesheet\" />");
                }
            }
        }

        private IEnumerable<string> GetFiles(Bundle bundle) {

            if (_options.UseBundles) {
                if (_options.AppendVersion)
                    return new[] { GetVersionedSrc(bundle.outputFileName) };
                return new[] { bundle.outputFileName };
            } else
                return bundle.inputFiles;
        }

        private string GetVersionedSrc(string srcValue) {
            EnsureFileVersionProvider();

            if (_options.AppendVersion)
                srcValue = _fileVersionProvider.AddFileVersionToPath(srcValue);

            return srcValue;
        }

        private void EnsureFileVersionProvider() {

            if (_fileVersionProvider == null)
                _fileVersionProvider = new FileVersionProvider(_hostingEnvironment.WebRootFileProvider, _cache, ViewContext.HttpContext.Request.PathBase);
        }
    }
}