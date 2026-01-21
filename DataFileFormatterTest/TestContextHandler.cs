using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DataFileFormatterTest {

    [TestClass]
    public static class TestContextHandler {

        private static string _outputDir;
        private static string _exePath;

        private static Dictionary<string, string> _testDataPath;

        [AssemblyInitialize]
        public static void Initialize(TestContext context) {
            _outputDir = context.TestResultsDirectory;

            string selfLocation = Assembly.GetExecutingAssembly().Location;
            string selfDirectory = Path.GetDirectoryName(selfLocation);
            _exePath = Path.Combine(selfDirectory, "DataFileFormatter.exe");

            string testDataDir = Path.GetFullPath(Path.Combine(selfDirectory, @"..\..\..\testdata"));

            LoadTestDirectory(testDataDir);
        }

        /// <summary>
        /// load test data file paths from test data directory of current context
        /// </summary>
        /// <param name="folderPath">directory path to load from</param>
        private static void LoadTestDirectory(string folderPath) {
            _testDataPath = new Dictionary<string, string>();
            foreach (string path in Directory.EnumerateFiles(folderPath, "*", SearchOption.AllDirectories)) {
                _testDataPath.Add(Path.GetFileName(path), path);
            }
        }

        /// <summary>
        /// load test data file content as string
        /// </summary>
        /// <param name="filename">file name to load</param>
        /// <returns></returns>
        internal static string LoadTestDataFileContent(string filename) {
            using (StreamReader streamReader = new StreamReader(GetTestDataPath(filename))) return streamReader.ReadToEnd();
        }

        /// <summary>
        /// Retrieves the full file system path for the specified test data file name.
        /// </summary>
        /// <param name="filename">The name of the test data file for which to retrieve the full path. Cannot be null.</param>
        /// <returns>The full file system path corresponding to the specified test data file name.</returns>
        internal static string GetTestDataPath(string filename) {
            return _testDataPath[filename];
        }

        /// <summary>
        /// Combines the specified file name with the output directory path to generate a full file path.
        /// </summary>
        /// <param name="filename">The name of the file to append to the output directory path. Cannot be null or contain invalid path
        /// characters.</param>
        /// <returns>A string containing the full path to the output file.</returns>
        internal static string GetOutputFilePath(string filename) {
            return Path.Combine(_outputDir, filename);
        }

        /// <summary>
        /// Gets the full path to the executable file for the current process.
        /// </summary>
        internal static string ExePath {
            get { return _exePath; }
        }

    }
}
