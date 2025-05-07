using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DataFileFormatterTest {

    [TestClass]
    public class TestContextHandler {

        private static string _outputDir;
        private static string _exePath;
        private static string _runDir;

        private static Dictionary<string, string> _testDataPath;

        [AssemblyInitialize]
        public static void Initialize(TestContext context) {
            _outputDir = context.TestResultsDirectory;
            _runDir = Environment.CurrentDirectory;

            string selfLocation = Assembly.GetExecutingAssembly().Location;
            string selfDirectory = Path.GetDirectoryName(selfLocation);
            _exePath = Path.Combine(selfDirectory, "DataFileFormatter.exe");

            string testDataDir = Path.GetFullPath(Path.Combine(selfDirectory, @"..\..\..\testdata"));

            LoadTestData(testDataDir);
        }

        private static void LoadTestData(string folderPath) {
            _testDataPath = new Dictionary<string, string>();
            foreach (string path in Directory.EnumerateFiles(folderPath, "*", SearchOption.AllDirectories)) {
                _testDataPath.Add(Path.GetFileName(path), path);
            }
        }

        internal static string GetTestDataPath(string filename) {
            return _testDataPath[filename];
        }

        internal static string GetOutputFilePath(string filename) {
            return Path.Combine(_outputDir, filename);
        }

        internal static string ExePath {
            get { return _exePath; }
        }

        internal static string RunDirectory {
            get { return _runDir; }
        }
    }
}
