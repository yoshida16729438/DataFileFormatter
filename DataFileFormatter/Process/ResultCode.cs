using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFileFormatter.Process {

    /// <summary>
    /// result codes
    /// </summary>
    internal enum ResultCode {

        //common for continue
        OK = 0,

        //common for command line options error
        NG_INDENT_SPACES_COUNT_NOT_AVAILABLE_VALUE = 1,
        NG_OUTPUT_FILE_NAME_NOT_SET = 2,

        //for other common failure
        NG_FILE_NOT_FOUND = 51,
        NG_NO_INPUT_DATA_SPECIFIED = 52,
        NG_NOT_AVAILABLE_CHARSET = 53,
        NG_FAILED_TO_LOAD_FROM_STDIN = 54,
        NG_FAILED_TO_OUTPUT_FILE = 55,

        //for json
        NG_FAILED_TO_LOAD_JSON = 101,

        //for xml
        NG_FAILED_TO_LOAD_XML = 201,

        //for csv
        NG_FAILED_TO_LOAD_CSV = 301
    }
}
