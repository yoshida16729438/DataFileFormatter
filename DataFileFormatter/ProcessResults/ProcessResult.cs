using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFileFormatter.ProcessResults {

    /// <summary>
    /// result codes definitions
    /// </summary>
    internal class ProcessResult {

        /// <summary>
        /// result code
        /// </summary>
        internal int resultCode { get; }

        /// <summary>
        /// messages
        /// </summary>
        internal string Message { get; }

        /// <summary>
        /// result codes
        /// </summary>
        private enum ResultCode {

            //common for continue
            OK = 0,

            //common for command line options error
            NG_PADDING_SPACE_NOT_AVAILABLE_VALUE = 1,
            NG_OUTPUT_FILE_NAME_NOT_SET = 2,

            //for other common failure
            NG_FILE_NOT_FOUND = 51,
            NG_NO_INPUT_DATA_SPECIFIED = 52,
            NG_NOT_AVAILABLE_CHARSET = 53,
            NG_FAILED_TO_OUTPUT_FILE = 54,

            //for json
            NG_FAILED_TO_LOAD_JSON = 101,

            //for xml
            NG_FAILED_TO_LOAD_XML = 201,

            //for csv
            NG_FAILED_TO_LOAD_CSV = 301
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="resultCode">result code</param>
        private ProcessResult(ResultCode resultCode) {
            this.resultCode = (int)resultCode;
            Message = string.Empty;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="resultCode">result code</param>
        /// <param name="message">message</param>
        private ProcessResult(ResultCode resultCode, string message) {
            this.resultCode = (int)resultCode;
            Message = message;
        }

        /// <summary>
        /// allowed to continue process or not
        /// </summary>
        /// <returns></returns>
        internal bool CanContinueProcess() {
            return resultCode == (int)ResultCode.OK;
        }

        /// <summary>
        /// create instance for continue
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult Normal() {
            return new ProcessResult(ResultCode.OK);
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
            return new ProcessResult(ResultCode.NG_PADDING_SPACE_NOT_AVAILABLE_VALUE, "padding spaces count was not set / not number/ not greater than 0");
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
    }
}
