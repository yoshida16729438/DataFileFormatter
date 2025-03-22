﻿using DataFileFormatter.Command;
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
            var analyzer = new CommandLineArgsAnalyzer();
            var res = analyzer.analyze(new string[] {
                "--xml",
                "--unformat",
                "--outFile",
                "outdir\\outfile.xml",
                "--paddingSpacesCount",
                "10",
                "--charset",
                "shift-jis"
            });
            Assert.IsTrue(res.canContinueProcess());
            Assert.AreEqual(0,res.resultCode);
            Assert.AreEqual(string.Empty, res.message);

            Assert.AreEqual(string.Empty, analyzer.commandLineData.fileName);
            Assert.AreEqual("outdir\\outfile.xml",analyzer.commandLineData.outputFileName);
            Assert.AreEqual(CommandLineData.FileType.xml,analyzer.commandLineData.fileType);
            Assert.AreEqual(CommandLineData.FormatStyle.unformat,analyzer.commandLineData.formatStyle);
            Assert.AreEqual(10,analyzer.commandLineData.paddingSpacesCount);
            Assert.AreEqual(932,analyzer.commandLineData.encoding.WindowsCodePage);
            Assert.AreEqual(CommandLineData.PaddingChar.space,analyzer.commandLineData.paddingChar);
        }

        [TestMethod]
        public void analyzeTest2() {
            var analyzer = new CommandLineArgsAnalyzer();
            var res = analyzer.analyze(new string[] {
                "--csv",
                "--format",
                "--tab",
                "DataFileFormatterTest.dll"
            });

            Assert.AreEqual(0, res.resultCode);

            Assert.AreEqual(string.Empty, analyzer.commandLineData.outputFileName);
            Assert.AreEqual(CommandLineData.FileType.csv, analyzer.commandLineData.fileType);
            Assert.AreEqual(CommandLineData.FormatStyle.format, analyzer.commandLineData.formatStyle);
            Assert.AreEqual(CommandLineData.PaddingChar.tab, analyzer.commandLineData.paddingChar);
            Assert.AreEqual("DataFileFormatterTest.dll", analyzer.commandLineData.fileName);
        }

        [TestMethod]
        public void analyzeTest3() {
            var analyzer=new CommandLineArgsAnalyzer();
            analyzer.analyze(new string[]{
                "--json",
                "--space",
                "--charset",
                "utf-32"
            });

            Assert.AreEqual(CommandLineData.FileType.json, analyzer.commandLineData.fileType);
            Assert.AreEqual(CommandLineData.PaddingChar.space, analyzer.commandLineData.paddingChar);
            Assert.AreEqual(12000, analyzer.commandLineData.encoding.CodePage);
        }

        [TestMethod]
        public void analyzeDefaultTest() {
            var analyzer=new CommandLineArgsAnalyzer();
            analyzer.analyze(new string[] { });

            Assert.AreEqual(string.Empty, analyzer.commandLineData.fileName);
            Assert.AreEqual(CommandLineData.FileType.json, analyzer.commandLineData.fileType);
            Assert.AreEqual(CommandLineData.FormatStyle.format, analyzer.commandLineData.formatStyle);
            Assert.AreEqual(string.Empty, analyzer.commandLineData.outputFileName);
            Assert.AreEqual(4, analyzer.commandLineData.paddingSpacesCount);
            Assert.AreEqual(CommandLineData.PaddingChar.space, analyzer.commandLineData.paddingChar);
            Assert.AreEqual(65001, analyzer.commandLineData.encoding.CodePage);
        }
    }
}
