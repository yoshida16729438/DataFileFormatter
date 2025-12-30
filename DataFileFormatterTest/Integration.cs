using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DataFileFormatterTest {

    /// <summary>
    /// integration test by calling process
    /// </summary>
    [TestClass]
    public class Integration {

        [TestMethod]
        public async Task FormatJson1() {
            Process process = new Process();
            process.StartInfo.FileName = TestContextHandler.ExePath;
            process.StartInfo.RedirectStandardInput = true;
            string[] param = new string[] { "--json", "--format", "--indentSpacesCount", "4", "--space" };
            process.StartInfo.Arguments = string.Join(" ", param);
            string data = File.ReadAllText(TestContextHandler.GetTestDataPath("unindented.json"));

            Task task = new Task(async () => {
                await process.StandardInput.WriteAsync(data).ConfigureAwait(false);
                process.StandardInput.Close();
            });

            ResultData resultData = await GetResultFromStdout(process, task).ConfigureAwait(false);
            Assert.AreEqual(0, resultData.ExitCode);
            Assert.AreEqual(File.ReadAllText(TestContextHandler.GetTestDataPath("indentWithFourSpaces.json")), resultData.Output);
            Assert.AreEqual(string.Empty, resultData.Error);
        }

        [TestMethod]
        public async Task FormatXml() {
            Process process = new Process();
            process.StartInfo.FileName = TestContextHandler.ExePath;
            string infile = TestContextHandler.GetTestDataPath("indentWithFourSpaces.xml");
            string outfile = TestContextHandler.GetOutputFilePath("testIndentWithTab.xml");
            string[] param = new string[] { "--xml", "--format", "--tab", QuoteIfRequred(infile), "--outfile", QuoteIfRequred(outfile) };
            process.StartInfo.Arguments = string.Join(" ", param);

            ResultData resultData = await GetResultFromStdout(process, Task.CompletedTask);

            string expected = File.ReadAllText(TestContextHandler.GetTestDataPath("indentWithTab.xml"));
            Assert.AreEqual(0, resultData.ExitCode);
            Assert.AreEqual(string.Empty, resultData.Output);
            Assert.AreEqual(string.Empty, resultData.Error);
            Assert.AreEqual(expected, File.ReadAllText(outfile));
        }

        private async Task<ResultData> GetResultFromStdout(Process process, Task task) {
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

            if (task.Status == TaskStatus.Created) task.Start();

            await task.ConfigureAwait(false);

            process.WaitForExit();
            int exitCode = process.ExitCode;
            process.Close();

            return new ResultData(exitCode, sbOut.ToString().Trim(), sbErr.ToString().Trim());
        }

        [TestMethod]
        public async Task IndentSpaceNotAvailable() {
            Process process = new Process();
            process.StartInfo.FileName = TestContextHandler.ExePath;
            string[] param = new string[] { "--indentSpacesCount" };
            process.StartInfo.Arguments = string.Join(" ", param);
            ResultData resultData = await GetResultFromStdout(process, Task.Delay(1));

            Assert.AreEqual(1, resultData.ExitCode);
        }

        [TestMethod]
        public async Task OutputFileNameNotSet() {
            Process process = new Process();
            process.StartInfo.FileName = TestContextHandler.ExePath;
            string[] param = new string[] { "--outfile" };
            process.StartInfo.Arguments = string.Join(" ", param);
            ResultData resultData = await GetResultFromStdout(process, Task.CompletedTask);

            Assert.AreEqual(2, resultData.ExitCode);
        }

        [TestMethod]
        public async Task FileNotFound() {
            Process process = new Process();
            process.StartInfo.FileName = TestContextHandler.ExePath;
            string[] param = new string[] { TestContextHandler.GetOutputFilePath("hoge.json") };
            process.StartInfo.Arguments = string.Join(" ", param);
            ResultData resultData = await GetResultFromStdout(process, Task.CompletedTask);

            Assert.AreEqual(51, resultData.ExitCode);
        }

        [TestMethod]
        public async Task NotAvailableCharset() {
            Process process = new Process();
            process.StartInfo.FileName = TestContextHandler.ExePath;
            string[] param = new string[] { "--charset", "shift-sjis" };
            process.StartInfo.Arguments = string.Join(" ", param);
            ResultData resultData = await GetResultFromStdout(process, Task.CompletedTask);

            Assert.AreEqual(53, resultData.ExitCode);
        }

        [TestMethod]
        public async Task FailedToLoadFromStdin() {
            Process process = new Process();
            process.StartInfo.FileName = TestContextHandler.ExePath;
            process.StartInfo.RedirectStandardInput = true;
            string input = File.ReadAllText(TestContextHandler.GetTestDataPath("unindented.json"));

            Task task = Task.Run(async () => {
                int len = (input.Length + 9) / 10;
                await Task.Delay(1000);
                for (int i = 0; i < 10; i++) {
                    await process.StandardInput.WriteAsync(input.Substring(i * len, Math.Min(len, input.Length - i * len)));
                    await Task.Delay(1000);
                }
                process.StandardInput.Close();
            });

            ResultData resultData = await GetResultFromStdout(process, Task.CompletedTask);
            Assert.AreEqual(54, resultData.ExitCode);
        }

        [TestMethod]
        public async Task FailedToOutputFile() {
            Process process = new Process();
            process.StartInfo.FileName = TestContextHandler.ExePath;
            string infile = TestContextHandler.GetTestDataPath("unindented.json");
            string[] param = new string[] { infile, "--outfile", QuoteIfRequred(TestContextHandler.GetOutputFilePath(string.Empty)) };
            process.StartInfo.Arguments = string.Join(" ", param);

            ResultData resultData = await GetResultFromStdout(process, Task.CompletedTask);
            Assert.AreEqual(55, resultData.ExitCode);
        }

        private class ResultData {
            public readonly int ExitCode;
            public readonly string Output;
            public readonly string Error;
            internal ResultData(int exitCode, string output, string err) {
                ExitCode = exitCode;
                Output = output;
                Error = err;
            }
        }

        private string QuoteIfRequred(string path) {
            if (path.Contains(" ") && !path.StartsWith("\"") && !path.EndsWith("\"")) return $"\"{path}\"";
            else return path;
        }
    }
}
