using DataFileFormatter.Process;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataFileFormatter.Stdin {

    /// <summary>
    /// read data from console stream
    /// </summary>
    internal class ConsoleReader {

        private readonly TextReader _textReader;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="textReader">stream of console</param>
        internal ConsoleReader(TextReader textReader) {
            _textReader = textReader;
        }

        /// <summary>
        /// read from stream
        /// </summary>
        /// <param name="timeSpan">wait timeout</param>
        /// <returns></returns>
        internal (ProcessResult, string) Read(TimeSpan timeSpan) {

            Task timeoutTask = Task.Delay(timeSpan);
            Task<(bool, string)> readTask = ReadAsyncInternal();

            Task completedTask = Task.WhenAny(timeoutTask, readTask).GetAwaiter().GetResult();
            if (completedTask == readTask) {

                (bool isNormal, string text) = readTask.GetAwaiter().GetResult();

                if (isNormal) return (ProcessResult.Normal(), text);
                else return (ProcessResult.FailedToLoadFromStdin(), text);
            } else {
                return (ProcessResult.FailedToLoadFromStdin(), string.Empty);
            }

        }

        /// <summary>
        /// read from textreader async
        /// </summary>
        /// <returns></returns>
        private async Task<(bool, string)> ReadAsyncInternal() {
            StringBuilder sb = new StringBuilder();
            char[] buffer = new char[4096];

            try {
                while (true) {
                    int readLength = await _textReader.ReadAsync(buffer, 0, buffer.Length);
                    if (readLength > 0) {
                        sb.Append(buffer, 0, readLength);
                    } else {
                        break;
                    }
                }
                return (true, sb.ToString());

            } catch (Exception) {

                return (false, string.Empty);
            }
        }
    }
}
