using DataFileFormatter.Command;
using DataFileFormatter.Formatter;
using DataFileFormatter.Process;
using DataFileFormatter.Stdin;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DataFileFormatter {

    internal class Program {

        /// <summary>
        /// main
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static async Task<int> Main(string[] args) {

            CommandLineArgsAnalyzer analyzer = new CommandLineArgsAnalyzer();
            ProcessResult result = analyzer.Analyze(args);
            if (!result.CanContinueProcess()) return EndWithError(result);

            return await ProcessFormat(analyzer._commandLineData);
        }

        /// <summary>
        /// execute whole process of formatting
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static async Task<int> ProcessFormat(ProcessParameter param) {

            IDataFormatter formatter = GetFormatter(param.ProcessType);

            ProcessResult result;

            result = await LoadData(formatter, param.FileName, param.Encoding);
            if (!result.CanContinueProcess()) return EndWithError(result);

            ProcessFormatMain(formatter, param.FormatStyle, param.IndentChar, param.IndentSpacesCount);

            result = OutputData(formatter, param.OutputFileName, param.Encoding);
            if (!result.CanContinueProcess()) return EndWithError(result);

            return (int)ProcessResult.Normal().ResultCode;
        }

        /// <summary>
        /// load data from file or stdin
        /// </summary>
        /// <param name="formatter"></param>
        /// <param name="fileName"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private static async Task<ProcessResult> LoadData(IDataFormatter formatter, string fileName, Encoding encoding) {
            if (fileName == string.Empty) {

                if (!Console.IsInputRedirected) return ProcessResult.InputDataNotSpecified();

                if (Console.InputEncoding.CodePage != encoding.CodePage) Console.InputEncoding = encoding;

                ConsoleReader reader = new ConsoleReader(Console.In);
                (ProcessResult processResult, string data) = await reader.ReadAsync();

                if (processResult.CanContinueProcess()) return formatter.LoadFromText(data);
                else return processResult;

            } else {
                return formatter.LoadFromFile(fileName, encoding);
            }
        }

        /// <summary>
        /// core format process execution
        /// </summary>
        /// <param name="formatter"></param>
        /// <param name="formatStyle"></param>
        /// <param name="indentChar"></param>
        /// <param name="indentSpacesCount"></param>
        private static void ProcessFormatMain(IDataFormatter formatter, FormatStyle formatStyle, IndentChar indentChar, int indentSpacesCount) {
            if (formatStyle == FormatStyle.format) {
                formatter.Format(indentChar, indentSpacesCount);
            } else {
                formatter.Unformat();
            }
        }

        /// <summary>
        /// output data to file or stdout
        /// </summary>
        /// <param name="formatter"></param>
        /// <param name="fileName"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private static ProcessResult OutputData(IDataFormatter formatter, string fileName, Encoding encoding) {
            if (fileName == string.Empty) {
                OutputToStdout(formatter.GetProcessedData(), encoding);
                return ProcessResult.Normal();
            } else {
                return formatter.SaveToFile(fileName, encoding);
            }
        }

        /// <summary>
        /// output data to stdout
        /// </summary>
        /// <param name="outputData"></param>
        /// <param name="encoding"></param>
        private static void OutputToStdout(string outputData, Encoding encoding) {
            Console.OutputEncoding = encoding;
            Console.Write(outputData);
        }

        /// <summary>
        /// get formatter instance
        /// </summary>
        /// <param name="processType"></param>
        /// <returns></returns>
        private static IDataFormatter GetFormatter(ProcessType processType) {
            switch (processType) {
                case ProcessType.json:
                    return new JsonFormatter();
            }
            return null;
        }

        /// <summary>
        /// write error message to stderr and exit
        /// </summary>
        /// <param name="processResult"></param>
        /// <returns></returns>
        private static int EndWithError(ProcessResult processResult) {
            Console.Error.WriteLine(processResult.Message);
            return (int)processResult.ResultCode;
        }
    }
}
