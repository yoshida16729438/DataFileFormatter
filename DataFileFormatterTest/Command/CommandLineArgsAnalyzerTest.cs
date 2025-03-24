using DataFileFormatter.Command;
using DataFileFormatter.Process;
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
                "--indentSpacesCount",
                "10",
                "--charset",
                "shift-jis"
            });
            Assert.IsTrue(res.CanContinueProcess());
            Assert.AreEqual(ResultCode.OK, res.ResultCode);
            Assert.AreEqual(string.Empty, res.Message);

            Assert.AreEqual(string.Empty, analyzer._commandLineData.FileName);
            Assert.AreEqual("outdir\\outfile.xml", analyzer._commandLineData.OutputFileName);
            Assert.AreEqual(ProcessType.xml, analyzer._commandLineData.ProcessType);
            Assert.AreEqual(FormatStyle.unformat, analyzer._commandLineData.FormatStyle);
            Assert.AreEqual(10, analyzer._commandLineData.IndentSpacesCount);
            Assert.AreEqual(932, analyzer._commandLineData.Encoding.WindowsCodePage);
            Assert.AreEqual(IndentChar.space, analyzer._commandLineData.IndentChar);
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

            Assert.AreEqual(ResultCode.OK, res.ResultCode);

            Assert.AreEqual(string.Empty, analyzer._commandLineData.OutputFileName);
            Assert.AreEqual(ProcessType.csv, analyzer._commandLineData.ProcessType);
            Assert.AreEqual(FormatStyle.format, analyzer._commandLineData.FormatStyle);
            Assert.AreEqual(IndentChar.tab, analyzer._commandLineData.IndentChar);
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

            Assert.AreEqual(ProcessType.json, analyzer._commandLineData.ProcessType);
            Assert.AreEqual(IndentChar.space, analyzer._commandLineData.IndentChar);
            Assert.AreEqual(12000, analyzer._commandLineData.Encoding.CodePage);
        }

        [TestMethod]
        public void AnalyzeDefaultTest() {
            var analyzer = new CommandLineArgsAnalyzer();
            analyzer.Analyze(new string[] { });

            Assert.AreEqual(string.Empty, analyzer._commandLineData.FileName);
            Assert.AreEqual(ProcessType.json, analyzer._commandLineData.ProcessType);
            Assert.AreEqual(FormatStyle.format, analyzer._commandLineData.FormatStyle);
            Assert.AreEqual(string.Empty, analyzer._commandLineData.OutputFileName);
            Assert.AreEqual(4, analyzer._commandLineData.IndentSpacesCount);
            Assert.AreEqual(IndentChar.space, analyzer._commandLineData.IndentChar);
            Assert.AreEqual(65001, analyzer._commandLineData.Encoding.CodePage);
        }

        [TestMethod]
        public void UnknownOptionTest() {
            var analyzer = new CommandLineArgsAnalyzer();
            var res = analyzer.Analyze(new string[] { "--unknown" });
            Assert.AreEqual(ResultCode.NG_FILE_NOT_FOUND, res.ResultCode);
            Assert.IsFalse(res.CanContinueProcess());
        }

        [TestMethod]
        public void UnknownCharsetTest() {
            var analyzer = new CommandLineArgsAnalyzer();
            var res = analyzer.Analyze(new string[] { "--charset", "unknown" });
            Assert.AreEqual(ResultCode.NG_NOT_AVAILABLE_CHARSET, res.ResultCode);
        }

        [TestMethod]
        public void CharsetNotSetTest() {
            var analyzer = new CommandLineArgsAnalyzer();
            var res = analyzer.Analyze(new string[] { "--charset" });
            Assert.AreEqual(ResultCode.NG_NOT_AVAILABLE_CHARSET, res.ResultCode);
        }

        [TestMethod]
        public void OutFileNotSetTest() {
            var analyzer = new CommandLineArgsAnalyzer();
            var res = analyzer.Analyze(new string[] { "--outfile" });
            Assert.AreEqual(ResultCode.NG_OUTPUT_FILE_NAME_NOT_SET, res.ResultCode);
        }

        [TestMethod]
        public void PaddingNotSetTest() {
            var analyzer = new CommandLineArgsAnalyzer();
            var res = analyzer.Analyze(new string[] { "--indentSpacesCount" });
            Assert.AreEqual(ResultCode.NG_INDENT_SPACES_COUNT_NOT_AVAILABLE_VALUE, res.ResultCode);
        }

        [TestMethod]
        public void PaddingNotNumberTest() {
            var analyzer = new CommandLineArgsAnalyzer();
            var res = analyzer.Analyze(new string[] { "--indentSpacesCount", "A" });
            Assert.AreEqual(ResultCode.NG_INDENT_SPACES_COUNT_NOT_AVAILABLE_VALUE, res.ResultCode);
        }

        [TestMethod]
        public void PaddingNotAvailableTest() {
            var analyzer = new CommandLineArgsAnalyzer();
            var res = analyzer.Analyze(new string[] { "--indentSpacesCount", "0" });
            Assert.AreEqual(ResultCode.NG_INDENT_SPACES_COUNT_NOT_AVAILABLE_VALUE, res.ResultCode);
        }
    }
}
