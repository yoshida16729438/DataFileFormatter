using DataFileFormatter.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFileFormatter.Process {

    /// <summary>
    /// result codes definitions
    /// </summary>
    internal class ProcessResult {

        /// <summary>
        /// result code
        /// </summary>
        internal ResultCode ResultCode { get; }

        /// <summary>
        /// messages
        /// </summary>
        internal string Message { get; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="resultCode">result code</param>
        /// <param name="message">message</param>
        private ProcessResult(ResultCode resultCode, string message) {
            ResultCode = resultCode;
            Message = message;
        }

        /// <summary>
        /// allowed to continue process or not
        /// </summary>
        /// <returns></returns>
        internal bool CanContinueProcess() {
            return ResultCode == ResultCode.OK;
        }

        /// <summary>
        /// create instance for continue
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult Normal() {
            return new ProcessResult(ResultCode.OK, string.Empty);
        }

        /// <summary>
        /// create instance for continue with message
        /// </summary>
        /// <param name="message">message</param>
        /// <returns></returns>
        internal static ProcessResult Normal(string message) {
            return new ProcessResult(ResultCode.OK, message);
        }

        /// <summary>
        /// create instance for padding space option
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult OptionErrorForPaddingSpace() {
            return new ProcessResult(ResultCode.NG_INDENT_SPACES_COUNT_NOT_AVAILABLE_VALUE, "padding spaces count was not set / not number/ not greater than 0");
        }

        /// <summary>
        /// create instance for output file name not set
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult OptionErrorForOutputFileNotSet() {
            return new ProcessResult(ResultCode.NG_OUTPUT_FILE_NAME_NOT_SET, "output file name is not set");
        }

        /// <summary>
        /// create instance for input file not found
        /// </summary>
        /// <param name="filepath">file path</param>
        /// <returns></returns>
        internal static ProcessResult InputFileNotFound(string filepath) {
            return new ProcessResult(ResultCode.NG_FILE_NOT_FOUND, $"input file not found:{filepath}");
        }

        /// <summary>
        /// create instance for no input data
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult InputDataNotSpecified() {
            return new ProcessResult(ResultCode.NG_NO_INPUT_DATA_SPECIFIED, "no data is set to both of input file and stdin");
        }

        /// <summary>
        /// create instance for not available charset
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult NotAvailableCharset() {
            return new ProcessResult(ResultCode.NG_NOT_AVAILABLE_CHARSET, "charset was not specified or not available");
        }

        /// <summary>
        /// create instance for failed to load from stdin
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult FailedToLoadFromStdin() {
            return new ProcessResult(ResultCode.NG_FAILED_TO_LOAD_FROM_STDIN, "failed to read data from stdin");
        }

        /// <summary>
        /// create instance for file output failed
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult FailedToOutputFile(string filename) {
            return new ProcessResult(ResultCode.NG_FAILED_TO_OUTPUT_FILE, $"failed to output file: {filename}");
        }

        /// <summary>
        /// create instance for failed to load json
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult FailedToLoadJson() {
            return new ProcessResult(ResultCode.NG_FAILED_TO_LOAD_JSON, "failed to load json");
        }

        /// <summary>
        /// create instance for failed to load xml
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult FailedToLoadXml() {
            return new ProcessResult(ResultCode.NG_FAILED_TO_LOAD_XML, "failed to load xml");
        }

        /// <summary>
        /// create instance for failed to load csv
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult FailedToLoadCsv() {
            return new ProcessResult(ResultCode.NG_FAILED_TO_LOAD_CSV, "failed to load csv");
        }

        public override bool Equals(object obj) {
            if (obj != null && obj is ProcessResult pr) {
                return ResultCode == pr.ResultCode && Message == pr.Message;
            }
            return false;
        }

        public override int GetHashCode() {
            return (ResultCode, Message).GetHashCode();
            
        }
    }
}
