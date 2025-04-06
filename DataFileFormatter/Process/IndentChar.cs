using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFileFormatter.Process {

    /// <summary>
    /// character for indent
    /// </summary>
    internal enum IndentChar {
        space,
        tab
    }

    internal class IndentCharTranslator {
        internal static string GetIndentString(IndentChar indentChar, int indentCount) {
            switch (indentChar) {
                case IndentChar.space:
                    return new string(' ', indentCount);
                case IndentChar.tab:
                    return "\t";
                default:
                    return string.Empty;
            }
        }
    }
}
