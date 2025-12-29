using DataFileFormatter.Formatter;
using DataFileFormatter.Process;
using DataFileFormatterTest.ProcessExt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace DataFileFormatterTest.Formatter {

    [TestClass]
    public class JsonFormatterTest {
        private static string _unindented;
        private static string _indentWithFourSpaces;
        private static string _indentWithTab;
        private static string _escapedJsonInput;
        private static string _escapedJsonOutput;

        [ClassInitialize]
        public static void LoadTestContext(TestContext _) {
            _unindented = TestContextHandler.LoadTestDataFileContent("unindented.json");
            _indentWithFourSpaces = TestContextHandler.LoadTestDataFileContent("indentWithFourSpaces.json");
            _indentWithTab = TestContextHandler.LoadTestDataFileContent("indentWithTab.json");
            _escapedJsonInput = TestContextHandler.LoadTestDataFileContent("escapedJsonInput.json");
            _escapedJsonOutput = TestContextHandler.LoadTestDataFileContent("escapedJsonOutput.json");
        }

        [TestMethod]
        public void FormatWithFourSpacesTest() {
            JsonFormatter formatter = new JsonFormatter();
            Assert.IsTrue(ProcessResult.Normal().IsEqualsTo(formatter.LoadFromFile(TestContextHandler.GetTestDataPath("unindented.json"), Encoding.UTF8)));
            formatter.Format(IndentChar.space, 4);

            string outputFilePath = TestContextHandler.GetOutputFilePath("output.json");
            Assert.IsTrue(ProcessResult.Normal().IsEqualsTo(formatter.SaveToFile(outputFilePath, Encoding.UTF8)));
            using (StreamReader sr = new StreamReader(outputFilePath)) Assert.AreEqual(_indentWithFourSpaces, sr.ReadToEnd());
        }

        [TestMethod]
        public void FormatWithTabTest() {
            JsonFormatter formatter = new JsonFormatter();
            Assert.IsTrue(ProcessResult.Normal().IsEqualsTo(formatter.LoadFromFile(TestContextHandler.GetTestDataPath("indentWithFourSpaces.json"), Encoding.UTF8)));
            formatter.Format(IndentChar.tab, 1);
            Assert.AreEqual(_indentWithTab, formatter.GetProcessedData());
        }

        [TestMethod]
        public void UnformatTest() {
            JsonFormatter formatter = new JsonFormatter();
            Assert.IsTrue(ProcessResult.Normal().IsEqualsTo(formatter.LoadFromFile(TestContextHandler.GetTestDataPath("indentWithTab.json"), Encoding.UTF8)));
            formatter.Unformat();
            Assert.AreEqual(_unindented, formatter.GetProcessedData());
        }

        [TestMethod]
        public void EmptyObjectTest() {
            JsonFormatter formatter = new JsonFormatter();
            Assert.IsTrue(ProcessResult.Normal().IsEqualsTo(formatter.LoadFromText("{}")));
            formatter.Format(IndentChar.space, 2);
            Assert.AreEqual("{}", formatter.GetProcessedData());
        }

        [TestMethod]
        public void EmptyArrayTest() {
            JsonFormatter formatter = new JsonFormatter();
            Assert.IsTrue(ProcessResult.Normal().IsEqualsTo(formatter.LoadFromText("[]")));
            formatter.Format(IndentChar.space, 2);
            Assert.AreEqual("[]", formatter.GetProcessedData());
        }

        [TestMethod]
        public void EscapedJsonTest() {
            JsonFormatter formatter = new JsonFormatter();
            Assert.IsTrue(ProcessResult.Normal().IsEqualsTo(formatter.LoadFromText(_escapedJsonInput)));
            formatter.Format(IndentChar.space, 2);
            Assert.AreEqual(_escapedJsonOutput, formatter.GetProcessedData());
        }

        [TestMethod]
        public void NotJsonTest() {
            JsonFormatter formatter = new JsonFormatter();
            Assert.IsTrue(ProcessResult.FailedToLoadJson().IsEqualsTo(formatter.LoadFromText("<file>xml</file>")));
        }

        [TestMethod]
        public void CannotSaveTest() {
            JsonFormatter formatter = new JsonFormatter();
            formatter.LoadFromText("{}");
            formatter.Unformat();

            string outFilePath = TestContextHandler.GetOutputFilePath(string.Empty);
            Assert.IsTrue(ProcessResult.FailedToOutputFile(outFilePath).IsEqualsTo(formatter.SaveToFile(outFilePath, Encoding.UTF8)));
        }

        [TestMethod]
        public void CannotLoadTest() {
            JsonFormatter formatter = new JsonFormatter();
            Assert.IsTrue(ProcessResult.FailedToLoadJson().IsEqualsTo(formatter.LoadFromFile(TestContextHandler.GetOutputFilePath("hoge.json"), Encoding.UTF8)));
        }
    }
}
