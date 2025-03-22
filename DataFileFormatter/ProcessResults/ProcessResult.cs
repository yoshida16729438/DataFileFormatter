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
        private List<string> messages;

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
            this.messages = new List<string>();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="resultCode">result code</param>
        /// <param name="message">message</param>
        private ProcessResult(ResultCode resultCode, string message) {
            this.resultCode = (int)resultCode;
            this.messages = new List<string>();
            this.messages.Add(message);
        }

        /// <summary>
        /// allowed to continue process or not
        /// </summary>
        /// <returns></returns>
        internal bool canContinueProcess() {
            return resultCode != (int)ResultCode.OK;
        }

        /// <summary>
        /// create instance for continue
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult normal() {
            return new ProcessResult(ResultCode.OK);
        }

        /// <summary>
        /// create instance for continue with message
        /// </summary>
        /// <param name="message">message</param>
        /// <returns></returns>
        internal static ProcessResult normal(string message) {
            return new ProcessResult(ResultCode.OK, message);
        }

        /// <summary>
        /// create instance for padding space option
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult optionErrorForPaddingSpace() {
            return new ProcessResult(ResultCode.NG_PADDING_SPACE_NOT_AVAILABLE_VALUE, "padding spaces count was not set / not number/ not greater than 0");
        }

        /// <summary>
        /// create instance for output file name not set
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult optionErrorForOutputFileNotSet() {
            return new ProcessResult(ResultCode.NG_OUTPUT_FILE_NAME_NOT_SET, "output file name is not set");
        }

        /// <summary>
        /// create instance for input file not found
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult inputFileNotFound() {
            return new ProcessResult(ResultCode.NG_FILE_NOT_FOUND, "input file not found");
        }

        /// <summary>
        /// create instance for no input data
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult inputDataNotSpecified() {
            return new ProcessResult(ResultCode.NG_NO_INPUT_DATA_SPECIFIED, "no data is set to both of input file and stdin");
        }

        /// <summary>
        /// create instance for not available charset
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult notAvailableCharset() {
            return new ProcessResult(ResultCode.NG_NOT_AVAILABLE_CHARSET, "charset was not specified or not available");
        }

        /// <summary>
        /// create instance for file output failed
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult failedToOutputFile(string filename) {
            return new ProcessResult(ResultCode.NG_FAILED_TO_OUTPUT_FILE, $"failed to output file: {filename}");
        }

        /// <summary>
        /// create instance for failed to load json
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult failedToLoadJson() {
            return new ProcessResult(ResultCode.NG_FAILED_TO_LOAD_JSON, "failed to load json");
        }

        /// <summary>
        /// create instance for failed to load xml
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult failedToLoadXml() {
            return new ProcessResult(ResultCode.NG_FAILED_TO_LOAD_XML, "failed to load xml");
        }

        /// <summary>
        /// create instance for failed to load csv
        /// </summary>
        /// <returns></returns>
        internal static ProcessResult failedToLoadCsv() {
            return new ProcessResult(ResultCode.NG_FAILED_TO_LOAD_CSV, "failed to load csv");
        }
    }
}
