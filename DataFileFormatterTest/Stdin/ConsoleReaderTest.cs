using DataFileFormatter.Process;
using DataFileFormatter.Stdin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFileFormatterTest.Stdin {

    /// <summary>
    /// testing class for console reader
    /// </summary>
    [TestClass]
    public class ConsoleReaderTest {

        [TestMethod]
        public async Task TestRead() {
            string expected = File.ReadAllText(Path.Combine(Const.TestDataFolderPath, "unindented.json"));
            using (TextReader reader = new StreamReader(Path.Combine(Const.TestDataFolderPath, "unindented.json"))) {
                ConsoleReader consoleReader = new ConsoleReader(reader);
                (ProcessResult result, string actual) = await consoleReader.ReadAsync();
                Assert.AreEqual(ProcessResult.Normal(), result);
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
