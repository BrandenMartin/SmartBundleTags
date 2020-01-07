using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

namespace SmartBundleTags.BundleTagHelpers {

    public interface IBundleProvider {
        Bundle GetBundle(string name);
    }

    public class BundleProvider : IBundleProvider, IDisposable {

        private readonly object _lock = new object();
        private readonly string _configurationPath;
        private IList<Bundle> _bundles;
        private FileSystemWatcher _fileWatcher;

        public BundleProvider() : this("bundleconfig.json") {

        }

        public BundleProvider(string configurationPath) {
            if (configurationPath == null) throw new ArgumentNullException(nameof(configurationPath));

            var fullPath = Path.GetFullPath(configurationPath);
            var directory = Path.GetDirectoryName(fullPath);
            var fileName = Path.GetFileName(fullPath);
            _configurationPath = fullPath;

            if (directory != null)
            {
                var watcher = new FileSystemWatcher(directory);
                watcher.EnableRaisingEvents = true;
                watcher.IncludeSubdirectories = false;
                watcher.Filter = fileName;
                watcher.Changed += (sender, args) => Reset();
                watcher.Created += (sender, args) => Reset();
                watcher.Deleted += (sender, args) => Reset();
                _fileWatcher = watcher;
            }
        }

        private void Reset() {
            _bundles = null;
        }

        private void LoadBundles() {
            if (_bundles == null) {

                lock (_lock) {

                    if (_bundles == null) {
                        
                        if (!TryGetBundles(_configurationPath, out List<Bundle> bundles))
                            throw new Exception("Unable to load bundles.");

                        _bundles = bundles;
                    }
                }
            }
        }

        public bool TryGetBundles(string configPath, out List<Bundle> bundleList) {
            try {//Tries to read the bundleconfig and load the data into a managed structure, if anything goes wrong we null the output and quit
                bundleList = JsonConvert.DeserializeObject<List<Bundle>>(File.ReadAllText(_configurationPath)) ?? throw new FileLoadException();
                return true;
            } catch {
                bundleList = null;
                return false;
            }
        }

        public Bundle GetBundle(string name) {
            LoadBundles();

            var bundle = _bundles.FirstOrDefault(b => string.Equals(b.outputFileName, name, StringComparison.OrdinalIgnoreCase));
            if (bundle != null)
                return bundle;

            return null;
        }

        public void Dispose() {

            if (_fileWatcher != null) {
                _fileWatcher.Dispose();
                _fileWatcher = null;
            }
        }
    }
}