using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataFileFormatter.Command.CommandLineData;
using DataFileFormatter.ProcessResults;
using System.Globalization;

namespace DataFileFormatter.Command {

    /// <summary>
    /// analyze command line arguments
    /// </summary>
    internal class CommandLineArgsAnalyzer {

        /// <summary>
        /// properties
        /// </summary>
        internal readonly CommandLineData commandLineData;

        /// <summary>
        /// constructor
        /// </summary>
        internal CommandLineArgsAnalyzer() {
            this.commandLineData = new CommandLineData();
        }

        /// <summary>
        /// analyze
        /// </summary>
        /// <param name="args">command line args</param>
        /// <returns></returns>
        internal ProcessResult analyze(string[] args) {

            ProcessResult result;

            this.setDefault();

            return this.analyzeInternal(args);
        }

        /// <summary>
        /// initialize
        /// </summary>
        private void setDefault() {
            this.commandLineData.fileType = FileType.json;
            this.commandLineData.formatStyle = FormatStyle.format;
            this.commandLineData.fileName = string.Empty;
            this.commandLineData.outputFileName = string.Empty;
            this.commandLineData.paddingSpacesCount = 4;
            this.commandLineData.paddingChar = PaddingChar.space;
            this.commandLineData.encoding = Encoding.UTF8;
        }

        /// <summary>
        /// internal analyze process
        /// </summary>
        /// <param name="args">command line args</param>
        /// <returns></returns>
        private ProcessResult analyzeInternal(string[] args) {
            for (int i = 0; i < args.Length; i++) {
                switch (args[i]) {
                    case CommandLineOptions.JSON:
                        this.commandLineData.fileType = FileType.json;
                        break;

                    case CommandLineOptions.XML:
                        this.commandLineData.fileType = FileType.xml;
                        break;

                    case CommandLineOptions.CSV:
                        this.commandLineData.fileType = FileType.csv;
                        break;

                    case CommandLineOptions.FORMAT:
                        this.commandLineData.formatStyle = FormatStyle.format;
                        break;

                    case CommandLineOptions.UNFORMAT:
                        this.commandLineData.formatStyle = FormatStyle.unformat;
                        break;

                    case CommandLineOptions.OUTPUTFILE:
                        if (i < args.Length - 1) {
                            this.commandLineData.outputFileName = args[i + 1];
                            i++;
                        } else {
                            return ProcessResult.optionErrorForOutputFileNotSet();
                        }
                        break;

                    case CommandLineOptions.PADDING_SPACES_COUNT:
                        if (i < args.Length - 1 && int.TryParse(args[i + 1], out int count)) {
                            this.commandLineData.paddingSpacesCount = count;
                            i++;
                        } else {
                            return ProcessResult.optionErrorForPaddingSpace();
                        }
                        break;

                    case CommandLineOptions.PADDING_SPACE:
                        this.commandLineData.paddingChar = PaddingChar.space;
                        break;

                    case CommandLineOptions.PADDING_TAB:
                        this.commandLineData.paddingChar = PaddingChar.tab;
                        break;

                    case CommandLineOptions.CHARSET:
                        if (i < args.Length - 1) {
                            try {
                                this.commandLineData.encoding = System.Text.Encoding.GetEncoding(args[i + 1]);
                            } catch (ArgumentException ex) {
                                return ProcessResult.notAvailableCharset();
                            }
                        } else {
                            return ProcessResult.notAvailableCharset();
                        }
                        break;

                    default:
                        this.commandLineData.fileName = args[i];
                        break;
                }
            }

            return ProcessResult.normal();
        }
    }
}
