using DataFileFormatter.Command;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFileFormatterTest.Command {

    [TestClass]
    public class CommandLineArgsAnalyzerTest {

        [TestMethod]
        public void analyzeTest() {
            var ana = new CommandLineArgsAnalyzer();
            ana.analyze(new string[] { "--charset", "shift-jis" });
            Assert.AreEqual(ana.commandLineData.encoding.CodePage, 932);
        }
    }
}
