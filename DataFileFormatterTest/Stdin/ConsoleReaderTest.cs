using DataFileFormatter.Process;
using DataFileFormatter.Stdin;
using DataFileFormatterTest.ProcessExt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DataFileFormatterTest.Stdin {

    /// <summary>
    /// testing class for console reader
    /// </summary>
    [TestClass]
    public class ConsoleReaderTest {

        [TestMethod]
        public void TestRead() {

            string inFilePath = TestContextHandler.GetTestDataPath("unindented.json");
            string expected = File.ReadAllText(inFilePath);
            using (TextReader reader = new StreamReader(inFilePath)) {
                ConsoleReader consoleReader = new ConsoleReader(reader);
                (ProcessResult result, string actual) = consoleReader.Read(TimeSpan.FromSeconds(1));
                Assert.IsTrue(ProcessResult.Normal().IsEqualsTo(result));
                Assert.AreEqual(expected, actual);
            }
        }

        public void ThrowExceptionTest() {
            using (TextReader reader = new MockReader("test value")) {
                ConsoleReader consoleReader = new ConsoleReader(reader);
                (ProcessResult result, string actual) = consoleReader.Read(TimeSpan.FromSeconds(1));
                Assert.IsTrue(ProcessResult.FailedToLoadFromStdin().IsEqualsTo(result));
                Assert.AreEqual(string.Empty, actual);
            }
        }
        class MockReader : StringReader {
            public MockReader(string s) : base(s) { }
            public override Task<int> ReadAsync(char[] buffer, int index, int count) {
                throw new NotImplementedException();
            }
        }
    }
}
