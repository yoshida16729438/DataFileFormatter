using DataFileFormatter.Formatter;
using DataFileFormatter.Process;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFileFormatterTest.Formatter {

    [TestClass]
    public class JsonFormatterTest {
        private static string _unindented;
        private static string _indentWithFourSpaces;
        private static string _indentWithTab;
        private static string _escapedJsonInput;
        private static string _escapedJsonOutput;
        private static string _workFolder;

        private const string TestDataFolderPath = "..\\..\\testdata\\json";

        [AssemblyInitialize]
        public static void LoadTestData(TestContext testContext) {
            _unindented = LoadFromFile("unindented.json");
            _indentWithFourSpaces = LoadFromFile("indentWithFourSpaces.json");
            _indentWithTab = LoadFromFile("indentWithTab.json");
            _escapedJsonInput = LoadFromFile("escapedJsonInput.json");
            _escapedJsonOutput = LoadFromFile("escapedJsonOutput.json");
            _workFolder = testContext.TestRunDirectory;
        }

        private static string LoadFromFile(string fileName) {
            using (StreamReader streamReader = new StreamReader(Path.Combine(TestDataFolderPath, fileName))) return streamReader.ReadToEnd();
        }

        [TestMethod]
        public void FormatWithFourSpacesTest() {
            JsonFormatter formatter = new JsonFormatter();
            Assert.AreEqual(ProcessResult.Normal(), formatter.LoadFromFile(Path.Combine(TestDataFolderPath, "unindented.json"), Encoding.UTF8));
            formatter.Format(IndentChar.space, 4);
            Assert.AreEqual(ProcessResult.Normal(), formatter.SaveToFile(Path.Combine(_workFolder, "output.json"), Encoding.UTF8));
            using (StreamReader sr = new StreamReader(Path.Combine(_workFolder, "output.json"))) Assert.AreEqual(_indentWithFourSpaces, sr.ReadToEnd());
        }

        [TestMethod]
        public void FormatWithTabTest() {
            JsonFormatter formatter = new JsonFormatter();
            Assert.AreEqual(ProcessResult.Normal(), formatter.LoadFromFile(Path.Combine(TestDataFolderPath, "indentWithFourSpaces.json"), Encoding.UTF8));
            formatter.Format(IndentChar.tab, 1);
            Assert.AreEqual(_indentWithTab, formatter.GetProcessedData());
        }

        [TestMethod]
        public void UnformatTest() {
            JsonFormatter formatter = new JsonFormatter();
            Assert.AreEqual(ProcessResult.Normal(), formatter.LoadFromFile(Path.Combine(TestDataFolderPath, "indentWithTab.json"), Encoding.UTF8));
            formatter.Unformat();
            Assert.AreEqual(_unindented, formatter.GetProcessedData());
        }

        [TestMethod]
        public void EmptyObjectTest() {
            JsonFormatter formatter = new JsonFormatter();
            Assert.AreEqual(ProcessResult.Normal(), formatter.LoadFromText("{}"));
            formatter.Format(IndentChar.space, 2);
            Assert.AreEqual("{}", formatter.GetProcessedData());
        }

        [TestMethod]
        public void EmptyArrayTest() {
            JsonFormatter formatter = new JsonFormatter();
            Assert.AreEqual(ProcessResult.Normal(), formatter.LoadFromText("[]"));
            formatter.Format(IndentChar.space, 2);
            Assert.AreEqual("[]", formatter.GetProcessedData());
        }

        [TestMethod]
        public void EscapedJsonTest() {
            JsonFormatter formatter = new JsonFormatter();
            Assert.AreEqual(ProcessResult.Normal(), formatter.LoadFromText(_escapedJsonInput));
            formatter.Format(IndentChar.space, 2);
            Assert.AreEqual(_escapedJsonOutput, formatter.GetProcessedData());
        }

        [TestMethod]
        public void NotJsonTest() {
            JsonFormatter formatter = new JsonFormatter();
            Assert.AreEqual(ProcessResult.FailedToLoadJson(), formatter.LoadFromText("<file>xml</file>"));
        }

        [TestMethod]
        public void CannotSaveTest() {
            JsonFormatter formatter = new JsonFormatter();
            formatter.LoadFromText("{}");
            formatter.Unformat();
            Assert.AreEqual(ProcessResult.FailedToOutputFile(_workFolder), formatter.SaveToFile(_workFolder, Encoding.UTF8));
        }

        [TestMethod]
        public void CannotLoadTest() {
            JsonFormatter formatter = new JsonFormatter();
            Assert.AreEqual(ProcessResult.FailedToLoadJson(), formatter.LoadFromFile(TestDataFolderPath, Encoding.UTF8));
        }
    }
}
