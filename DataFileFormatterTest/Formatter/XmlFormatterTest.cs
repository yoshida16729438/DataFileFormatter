using DataFileFormatter.Formatter;
using DataFileFormatter.Process;
using DataFileFormatterTest.ProcessExt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFileFormatterTest.Formatter {
    [TestClass]
    public class XmlFormatterTest {
        private static string _unindented;
        private static string _indentWithFourSpaces;
        private static string _indentWithTab;
        private static string _escaped;

        [ClassInitialize]
        public static void LoadTestContext(TestContext _) {
            _unindented = TestContextHandler.LoadTestDataFileContent("unindented.xml");
            _indentWithFourSpaces = TestContextHandler.LoadTestDataFileContent("indentWithFourSpaces.xml");
            _indentWithTab = TestContextHandler.LoadTestDataFileContent("indentWithTab.xml");
            _escaped = TestContextHandler.LoadTestDataFileContent("escaped.xml");
        }

        [TestMethod]
        public void FormatWithFourSpacesTest() {
            XmlFormatter formatter = new XmlFormatter();
            Assert.IsTrue(ProcessResult.Normal().IsEqualsTo(formatter.LoadFromFile(TestContextHandler.GetTestDataPath("unindented.xml"), Encoding.UTF8)));
            formatter.Format(IndentChar.space, 4);

            string outputFilePath = TestContextHandler.GetOutputFilePath("output.xml");
            Assert.IsTrue(ProcessResult.Normal().IsEqualsTo(formatter.SaveToFile(outputFilePath, Encoding.UTF8)));
            using (StreamReader sr = new StreamReader(outputFilePath)) Assert.AreEqual(_indentWithFourSpaces, sr.ReadToEnd());
        }

        [TestMethod]
        public void FormatWithTabTest() {
            XmlFormatter formatter = new XmlFormatter();
            Assert.IsTrue(ProcessResult.Normal().IsEqualsTo(formatter.LoadFromFile(TestContextHandler.GetTestDataPath("indentWithFourSpaces.xml"), Encoding.UTF8)));
            formatter.Format(IndentChar.tab, 1);
            Assert.AreEqual(_indentWithTab, formatter.GetProcessedData());
        }

        [TestMethod]
        public void UnformatTest() {
            XmlFormatter formatter = new XmlFormatter();
            Assert.IsTrue(ProcessResult.Normal().IsEqualsTo(formatter.LoadFromFile(TestContextHandler.GetTestDataPath("indentWithTab.xml"), Encoding.UTF8)));
            formatter.Unformat();
            Assert.AreEqual(_unindented, formatter.GetProcessedData());
        }

        [TestMethod]
        public void EmptyObjectTest() {
            XmlFormatter formatter = new XmlFormatter();
            string minimalXml = "<root />";
            Assert.IsTrue(ProcessResult.Normal().IsEqualsTo(formatter.LoadFromText(minimalXml)));
            formatter.Format(IndentChar.space, 1);
            Assert.AreEqual(minimalXml, formatter.GetProcessedData());
        }

        [TestMethod]
        public void EscapedXmlTest() {
            XmlFormatter formatter = new XmlFormatter();
            Assert.IsTrue(ProcessResult.Normal().IsEqualsTo(formatter.LoadFromText(_escaped)));
            formatter.Format(IndentChar.tab, 1);
            Assert.AreEqual(_escaped, formatter.GetProcessedData());
        }

        [TestMethod]
        public void NotXmlTest() {
            XmlFormatter formatter = new XmlFormatter();
            Assert.IsTrue(ProcessResult.FailedToLoadXml().IsEqualsTo(formatter.LoadFromText("{\"key\":\"json value\"}")));
        }

        [TestMethod]
        public void CannotSaveTest() {
            XmlFormatter formatter = new XmlFormatter();
            formatter.LoadFromText("<root />");
            formatter.Unformat();

            string outFilePath = TestContextHandler.GetOutputFilePath(string.Empty);
            Assert.IsTrue(ProcessResult.FailedToOutputFile(outFilePath).IsEqualsTo(formatter.SaveToFile(outFilePath, Encoding.UTF8)));
        }

        [TestMethod]
        public void CannotLoadTest() {
            XmlFormatter formatter = new XmlFormatter();
            string inputFilePath = TestContextHandler.GetOutputFilePath("hoge.xml");
            Assert.IsTrue(ProcessResult.InputFileNotFound(inputFilePath).IsEqualsTo(formatter.LoadFromFile(inputFilePath, Encoding.UTF8)));
        }
    }
}
