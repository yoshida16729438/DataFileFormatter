using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFileFormatter.Process;
using DataFileFormatter.Process;
using System.Globalization;

namespace DataFileFormatter.Command {

    /// <summary>
    /// analyze command line arguments
    /// </summary>
    internal class CommandLineArgsAnalyzer {

        /// <summary>
        /// properties
        /// </summary>
        internal readonly ProcessParameter _commandLineData;

        /// <summary>
        /// constructor
        /// </summary>
        internal CommandLineArgsAnalyzer() {
            _commandLineData = new ProcessParameter();
        }

        /// <summary>
        /// analyze
        /// </summary>
        /// <param name="args">command line args</param>
        /// <returns></returns>
        internal ProcessResult Analyze(string[] args) {

            SetDefault();

            return AnalyzeInternal(args);
        }

        /// <summary>
        /// initialize
        /// </summary>
        private void SetDefault() {
            _commandLineData.ProcessType = ProcessType.json;
            _commandLineData.FormatStyle = FormatStyle.format;
            _commandLineData.FileName = string.Empty;
            _commandLineData.OutputFileName = string.Empty;
            _commandLineData.IndentSpacesCount = 4;
            _commandLineData.IndentChar = IndentChar.space;
            _commandLineData.Encoding = Encoding.UTF8;
        }

        /// <summary>
        /// internal analyze process
        /// </summary>
        /// <param name="args">command line args</param>
        /// <returns></returns>
        private ProcessResult AnalyzeInternal(string[] args) {
            for (int i = 0; i < args.Length; i++) {
                switch (args[i]) {
                    case CommandLineOptions.JSON:
                        _commandLineData.ProcessType = ProcessType.json;
                        break;

                    case CommandLineOptions.XML:
                        _commandLineData.ProcessType = ProcessType.xml;
                        break;

                    case CommandLineOptions.CSV:
                        _commandLineData.ProcessType = ProcessType.csv;
                        break;

                    case CommandLineOptions.FORMAT:
                        _commandLineData.FormatStyle = FormatStyle.format;
                        break;

                    case CommandLineOptions.UNFORMAT:
                        _commandLineData.FormatStyle = FormatStyle.unformat;
                        break;

                    case CommandLineOptions.OUTPUTFILE:
                        if (i < args.Length - 1) {
                            _commandLineData.OutputFileName = args[i + 1];
                            i++;
                        } else {
                            return ProcessResult.OptionErrorForOutputFileNotSet();
                        }
                        break;

                    case CommandLineOptions.INDENT_SPACES_COUNT:
                        if (i < args.Length - 1 && int.TryParse(args[i + 1], out int count) && count > 0) {
                            _commandLineData.IndentSpacesCount = count;
                            i++;
                        } else {
                            return ProcessResult.OptionErrorForPaddingSpace();
                        }
                        break;

                    case CommandLineOptions.INDENT_SPACE:
                        _commandLineData.IndentChar = IndentChar.space;
                        break;

                    case CommandLineOptions.INDENT_TAB:
                        _commandLineData.IndentChar = IndentChar.tab;
                        break;

                    case CommandLineOptions.CHARSET:
                        if (i < args.Length - 1) {
                            try {
                                _commandLineData.Encoding = System.Text.Encoding.GetEncoding(args[i + 1]);
                                i++;
                            } catch (ArgumentException) {
                                return ProcessResult.NotAvailableCharset();
                            }
                        } else {
                            return ProcessResult.NotAvailableCharset();
                        }
                        break;

                    default:
                        if (System.IO.File.Exists(args[i])) {
                            _commandLineData.FileName = args[i];
                        } else {
                            return ProcessResult.InputFileNotFound(args[i]);
                        }
                        break;
                }
            }

            return ProcessResult.Normal();
        }
    }
}
