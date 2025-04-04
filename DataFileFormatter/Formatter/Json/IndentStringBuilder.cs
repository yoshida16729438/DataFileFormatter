using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFileFormatter.Formatter.Json {

    /// <summary>
    /// custom StringBuilder for indent json
    /// </summary>
    internal class IndentStringBuilder {

        private readonly StringBuilder _builder;
        private readonly string _baseIndentation;
        private readonly List<string> _indentStrings;
        private int _indentLevel;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="baseIndentation">string to be used for one-level indentation</param>
        internal IndentStringBuilder(string baseIndentation) {
            _builder = new StringBuilder();
            _baseIndentation = baseIndentation;
            _indentStrings = new List<string>() { string.Empty };
            _indentLevel = 0;
        }

        /// <summary>
        /// append
        /// </summary>
        /// <param name="value">append value</param>
        internal void Append(string value) {
            _builder.Append(value);
        }

        /// <summary>
        /// move to new line and add indent
        /// </summary>
        internal void MoveToNewLine() {
            _builder.AppendLine();
            _builder.Append(_indentStrings[_indentLevel]);
        }

        /// <summary>
        /// increment indent level and generate indent string if required
        /// </summary>
        internal void IncrementIndentLevel() {
            _indentLevel++;
            if (_indentLevel == _indentStrings.Count) _indentStrings.Add(_indentStrings[_indentLevel - 1] + _baseIndentation);
        }

        /// <summary>
        /// decrement indent level
        /// </summary>
        internal void DecrementIndentLevel() {
            _indentLevel--;
        }

        /// <summary>
        /// get built text
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return _builder.ToString();
        }
    }
}
