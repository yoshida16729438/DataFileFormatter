using DataFileFormatter.Process;
using System;
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
        /// <returns></returns>
        internal async Task<(ProcessResult, string)> ReadAsync() {
            StringBuilder sb = new StringBuilder();
            char[] buffer = new char[4096];
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            try {
                while (!cts.Token.IsCancellationRequested) {

                    int readLength = await _textReader.ReadAsync(buffer, 0, buffer.Length);

                    if (readLength > 0) {
                        sb.Append(buffer, 0, readLength);
                    } else {
                        return (ProcessResult.Normal(), sb.ToString());
                    }
                }
            } catch (OperationCanceledException) {
                return (ProcessResult.FailedToLoadFromStdin(), string.Empty);
            }

            return (ProcessResult.FailedToLoadFromStdin(), string.Empty);
        }
    }
}
