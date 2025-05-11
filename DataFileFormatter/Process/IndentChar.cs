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
                case IndentChar.tab:
                    return "\t";
                default:
                    return new string(' ', indentCount);
            }
        }
    }
}
