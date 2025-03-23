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
        public void AnalyzeTest() {
            var analyzer = new CommandLineArgsAnalyzer();
            var res = analyzer.Analyze(new string[] {
                "--xml",
                "--unformat",
                "--outfile",
                "outdir\\outfile.xml",
                "--paddingSpacesCount",
                "10",
                "--charset",
                "shift-jis"
            });
            Assert.IsTrue(res.CanContinueProcess());
            Assert.AreEqual(0, res.resultCode);
            Assert.AreEqual(string.Empty, res.Message);

            Assert.AreEqual(string.Empty, analyzer._commandLineData.FileName);
            Assert.AreEqual("outdir\\outfile.xml", analyzer._commandLineData.OutputFileName);
            Assert.AreEqual(CommandLineData.FileType.xml, analyzer._commandLineData.fileType);
            Assert.AreEqual(CommandLineData.FormatStyle.unformat, analyzer._commandLineData.formatStyle);
            Assert.AreEqual(10, analyzer._commandLineData.PaddingSpacesCount);
            Assert.AreEqual(932, analyzer._commandLineData.Encoding.WindowsCodePage);
            Assert.AreEqual(CommandLineData.PaddingChar.space, analyzer._commandLineData.paddingChar);
        }

        [TestMethod]
        public void AnalyzeTest2() {
            var analyzer = new CommandLineArgsAnalyzer();
            var res = analyzer.Analyze(new string[] {
                "--csv",
                "--format",
                "--tab",
                "DataFileFormatterTest.dll"
            });

            Assert.AreEqual(0, res.resultCode);

            Assert.AreEqual(string.Empty, analyzer._commandLineData.OutputFileName);
            Assert.AreEqual(CommandLineData.FileType.csv, analyzer._commandLineData.fileType);
            Assert.AreEqual(CommandLineData.FormatStyle.format, analyzer._commandLineData.formatStyle);
            Assert.AreEqual(CommandLineData.PaddingChar.tab, analyzer._commandLineData.paddingChar);
            Assert.AreEqual("DataFileFormatterTest.dll", analyzer._commandLineData.FileName);
        }

        [TestMethod]
        public void AnalyzeTest3() {
            var analyzer = new CommandLineArgsAnalyzer();
            analyzer.Analyze(new string[]{
                "--json",
                "--space",
                "--charset",
                "utf-32"
            });

            Assert.AreEqual(CommandLineData.FileType.json, analyzer._commandLineData.fileType);
            Assert.AreEqual(CommandLineData.PaddingChar.space, analyzer._commandLineData.paddingChar);
            Assert.AreEqual(12000, analyzer._commandLineData.Encoding.CodePage);
        }

        [TestMethod]
        public void AnalyzeDefaultTest() {
            var analyzer = new CommandLineArgsAnalyzer();
            analyzer.Analyze(new string[] { });

            Assert.AreEqual(string.Empty, analyzer._commandLineData.FileName);
            Assert.AreEqual(CommandLineData.FileType.json, analyzer._commandLineData.fileType);
            Assert.AreEqual(CommandLineData.FormatStyle.format, analyzer._commandLineData.formatStyle);
            Assert.AreEqual(string.Empty, analyzer._commandLineData.OutputFileName);
            Assert.AreEqual(4, analyzer._commandLineData.PaddingSpacesCount);
            Assert.AreEqual(CommandLineData.PaddingChar.space, analyzer._commandLineData.paddingChar);
            Assert.AreEqual(65001, analyzer._commandLineData.Encoding.CodePage);
        }

        [TestMethod]
        public void UnknownOptionTest() {
            var analyzer = new CommandLineArgsAnalyzer();
            var res = analyzer.Analyze(new string[] { "--unknown" });
            Assert.AreEqual(51, res.resultCode);
            Assert.IsFalse(res.CanContinueProcess());
        }

        [TestMethod]
        public void UnknownCharsetTest() {
            var analyzer = new CommandLineArgsAnalyzer();
            var res = analyzer.Analyze(new string[] { "--charset", "unknown" });
            Assert.AreEqual(53, res.resultCode);
        }

        [TestMethod]
        public void CharsetNotSetTest() {
            var analyzer = new CommandLineArgsAnalyzer();
            var res = analyzer.Analyze(new string[] { "--charset" });
            Assert.AreEqual(53, res.resultCode);
        }

        [TestMethod]
        public void OutFileNotSetTest() {
            var analyzer = new CommandLineArgsAnalyzer();
            var res = analyzer.Analyze(new string[] { "--outfile" });
            Assert.AreEqual(2, res.resultCode);
        }

        [TestMethod]
        public void PaddingNotSetTest() {
            var analyzer = new CommandLineArgsAnalyzer();
            var res = analyzer.Analyze(new string[] { "--paddingSpacesCount" });
            Assert.AreEqual(1, res.resultCode);
        }

        [TestMethod]
        public void PaddingNotNumberTest() {
            var analyzer = new CommandLineArgsAnalyzer();
            var res = analyzer.Analyze(new string[] { "--paddingSpacesCount", "A" });
            Assert.AreEqual(1, res.resultCode);
        }

        [TestMethod]
        public void PaddingNotAvailableTest() {
            var analyzer = new CommandLineArgsAnalyzer();
            var res = analyzer.Analyze(new string[] { "--paddingSpacesCount", "0" });
            Assert.AreEqual(1, res.resultCode);
        }
    }
}
