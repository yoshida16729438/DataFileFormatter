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
    internal class JsonFormatterTest {
        private static string _unindented;
        private static string _indentWithFourSpaces;
        private static string _indentWithTab;
        private static string _escapedJsonInput;
        private static string _escapedJsonOutput;

        private const string _folderPath = "..\\..\\testdata\\json";

#pragma warning disable IDE0060
        [AssemblyInitialize]
        public static void LoadTestData(TestContext testContext) {
            _unindented = LoadFromFile("unindented.json");
            _indentWithFourSpaces = LoadFromFile("indentWithFourSpaces.json");
            _indentWithTab = LoadFromFile("indentWithTab.json");
            _escapedJsonInput = LoadFromFile("escapedJsonInput.json");
            _escapedJsonOutput = LoadFromFile("escapedJsonOutput.json");
        }

        private static string LoadFromFile(string fileName) {
            const string folderPath = "..\\..\\testdata\\json";
            using (StreamReader streamReader = new StreamReader(Path.Combine(folderPath, fileName))) return streamReader.ReadToEnd();
        }

        [TestMethod]
        public void FormatWithFourSpacesTest() {
            JsonFormatter formatter = new JsonFormatter();
            formatter.LoadFromFile(Path.Combine(_folderPath, "unindented.json"), Encoding.UTF8);
            formatter.Format(IndentChar.space, 4);
            Assert.AreEqual(_indentWithFourSpaces, formatter.GetProcessedData());
        }

        [TestMethod]
        public void FormatWithTabTest() {
            JsonFormatter formatter = new JsonFormatter();
            formatter.LoadFromFile(Path.Combine(_folderPath, "indentWithFourSpaces"), Encoding.UTF8);
            formatter.Format(IndentChar.tab, 1);
            Assert.AreEqual(_indentWithTab, formatter.GetProcessedData());
        }

        [TestMethod]
        public void UnformatTest() {
            JsonFormatter formatter = new JsonFormatter();
            formatter.LoadFromFile(Path.Combine(_folderPath, "indentWithTab.json"), Encoding.UTF8);
            formatter.Unformat();
            Assert.AreEqual(_unindented, formatter.GetProcessedData());
        }

        [TestMethod]
        public void EmptyObjectTest() {
            JsonFormatter formatter = new JsonFormatter();
            formatter.LoadFromText("{}");
            formatter.Format(IndentChar.space, 2);
            Assert.AreEqual("{}", formatter.GetProcessedData());
        }

        [TestMethod]
        public void EmptyArrayTest() {
            JsonFormatter formatter = new JsonFormatter();
            formatter.LoadFromText("[]");
            formatter.Format(IndentChar.space, 2);
            Assert.AreEqual("[]", formatter.GetProcessedData());
        }

        [TestMethod]
        public void EscapedJsonTest() {
            JsonFormatter formatter = new JsonFormatter();
            formatter.LoadFromText(_escapedJsonInput);
            formatter.Format(IndentChar.space, 2);
            Assert.AreEqual(_escapedJsonOutput, formatter.GetProcessedData());
        }

    }
}
