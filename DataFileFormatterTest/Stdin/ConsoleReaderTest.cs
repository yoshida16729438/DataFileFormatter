using DataFileFormatter.Process;
using DataFileFormatter.Stdin;
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
                Assert.AreEqual(ProcessResult.Normal(), result);
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
