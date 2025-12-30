namespace DataFileFormatter.Process {

    /// <summary>
    /// character for indent
    /// </summary>
    internal enum IndentChar {
        space,
        tab
    }

    /// <summary>
    /// utility class to convert IndentChar enum to actual string
    /// </summary>
    internal class IndentCharTranslator {

        /// <summary>
        /// convert IndentChar enum to actual string
        /// </summary>
        /// <param name="indentChar">enum</param>
        /// <param name="indentCount">repeat count of indent char per indent</param>
        /// <returns>actual string for indent</returns>
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
