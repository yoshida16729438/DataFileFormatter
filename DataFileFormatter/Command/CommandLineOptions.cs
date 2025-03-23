using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFileFormatter.Command {

    /// <summary>
    /// コマンドラインオプション定義
    /// </summary>
    internal static class CommandLineOptions {
        internal const string JSON = "--json";
        internal const string XML = "--xml";
        internal const string CSV = "--csv";
        internal const string FORMAT = "--format";
        internal const string UNFORMAT = "--unformat";
        internal const string OUTPUTFILE = "--outfile";
        internal const string PADDING_SPACES_COUNT = "--paddingSpacesCount";
        internal const string PADDING_TAB = "--tab";
        internal const string PADDING_SPACE = "--space";
        internal const string CHARSET = "--charset";
    }
}
