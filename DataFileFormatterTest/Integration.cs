using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFileFormatterTest {

    /// <summary>
    /// integration test by calling process
    /// </summary>
    [TestClass]
    public class Integration {

        private const string EXE_PATH = ".\\DataFileFormatter.exe";
        private static string _workFolder;

        [ClassInitialize]
        public static void Initialize(TestContext testContext) {
            _workFolder = Path.Combine(testContext.TestRunDirectory, "Out");
        }

        [TestMethod]
        public void FormatJson1() {
            Process process = new Process();
            process.StartInfo.FileName = EXE_PATH;
            process.StartInfo.RedirectStandardInput = true;
            string[] param = new string[] { "--json", "--format", "--indentSpacesCount", "4", "--space" };
            process.StartInfo.Arguments = string.Join(" ", param);
            string data = File.ReadAllText(Path.Combine(Const.TestDataFolderPath, "unindented.json"));
            Action action = () => {
                process.StandardInput.Write(data);
                process.StandardInput.Close();
            };

            (int exitCode, string output, string error) = GetResultFromStdout(process, action);
            Assert.AreEqual(0, exitCode);
            Assert.AreEqual(File.ReadAllText(Path.Combine(Const.TestDataFolderPath, "indentWithFourSpaces.json")), output);
            Assert.AreEqual(string.Empty, error);
        }

        [TestMethod]
        public void FormatJson2() {
            Process process = new Process();
            process.StartInfo.FileName = EXE_PATH;
            string infile = Path.Combine(Const.TestDataFolderPath, "indentWithFourSpaces.json");
            string outfile = Path.Combine(_workFolder, "testIndentWithTab.json");
            string[] param = new string[] { "--json", "--format", "--tab", infile, "--outfile", $"\"{outfile}\"" };
            process.StartInfo.Arguments = string.Join(" ", param);
            process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;

            (int exitCode, string output, string error) = GetResultFromStdout(process, () => { });

            string expected = File.ReadAllText(Path.Combine(Const.TestDataFolderPath, "indentWithTab.json"));
            Assert.AreEqual(0, exitCode);
            Assert.AreEqual(string.Empty, output);
            Assert.AreEqual(string.Empty, error);
            Assert.AreEqual(expected, File.ReadAllText(outfile));
        }


        private (int, string, string) GetResultFromStdout(Process process, Action action) {
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;

            StringBuilder sbOut = new StringBuilder();
            StringBuilder sbErr = new StringBuilder();

            process.OutputDataReceived += (sender, e) => {
                sbOut.AppendLine(e.Data);
            };
            process.ErrorDataReceived += (sender, e) => {
                sbErr.AppendLine(e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            action();

            process.WaitForExit();
            int exitCode = process.ExitCode;
            process.Close();

            return (exitCode, sbOut.ToString().Trim(), sbErr.ToString().Trim());
        }

    }
}
