using DataFileFormatter.Process;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DataFileFormatter.Formatter {

    /// <summary>
    /// formatter for xml
    /// </summary>
    internal class XmlFormatter : IDataFormatter {

        /// <summary>
        /// constructor
        /// </summary>
        internal XmlFormatter() { }

        /// <summary>
        /// loaded document
        /// </summary>
        private XDocument _document;

        /// <summary>
        /// formatted/unformatted xml string
        /// </summary>
        private string _processedXml;

        /// <summary>
        /// input/output encoding from commandline or xml declaration
        /// </summary>
        private Encoding _encoding;

        /// <summary>
        /// is xml declaration exist in input xml, will be used to dicide output xml declaration or not
        /// </summary>
        private bool _isXmlDeclarationExisted;

        /// <inheritdoc/>
        public ProcessResult LoadFromText(string text) {
            using (StringReader reader = new StringReader(text)) {
                return LoadXmlInner(reader);
            }
        }

        /// <inheritdoc/>
        public ProcessResult LoadFromFile(string fileName, Encoding encoding) {
            try {
                using (StreamReader reader = new StreamReader(fileName, encoding)) {
                    var res = LoadXmlInner(reader);
                    _encoding = encoding;
                    return res;
                }
            } catch (FileNotFoundException) {
                return ProcessResult.InputFileNotFound(fileName);
            }
        }

        /// <inheritdoc/>
        private ProcessResult LoadXmlInner(TextReader reader) {
            try {
                _document = XDocument.Load(reader);
                _isXmlDeclarationExisted = _document.Declaration != null;
                _encoding = _isXmlDeclarationExisted ? Encoding.GetEncoding(_document.Declaration.Encoding) : Encoding.UTF8;
            } catch (XmlException) {
                _isXmlDeclarationExisted = false;
                _encoding = Encoding.UTF8;
                return ProcessResult.FailedToLoadXml();
            }
            return ProcessResult.Normal();
        }

        /// <inheritdoc/>
        public void Format(IndentChar indentChar, int indent) {
            XmlWriterSettings settings = CreateWriterSettings();
            settings.Indent = true;
            settings.IndentChars = IndentCharTranslator.GetIndentString(indentChar, indent);
            ProcessFormatInternal(settings);
        }

        /// <inheritdoc/>
        public void Unformat() {
            XmlWriterSettings settings = CreateWriterSettings();
            settings.Indent = false;
            ProcessFormatInternal(settings);
        }

        /// <summary>
        /// common method to create xml writer settings
        /// </summary>
        /// <returns></returns>
        private XmlWriterSettings CreateWriterSettings() {
            XmlWriterSettings settings = new XmlWriterSettings() { Encoding = _encoding, OmitXmlDeclaration = !_isXmlDeclarationExisted, CloseOutput = true };
            return settings;
        }

        /// <summary>
        /// formatting/unformatting implementation
        /// </summary>
        /// <param name="settings">writer settings</param>
        private void ProcessFormatInternal(XmlWriterSettings settings) {
            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(new EncodingStringWriter(sb, _encoding), settings)) {
                _document.WriteTo(writer);
                writer.Flush();
                _processedXml = sb.ToString();
            }
        }

        /// <inheritdoc/>
        public ProcessResult SaveToFile(string fileName, Encoding encoding) {
            try {
                using (StreamWriter writer = new StreamWriter(fileName, false, encoding)) {
                    writer.Write(_processedXml);
                }
                return ProcessResult.Normal();
            } catch (Exception) {
                return ProcessResult.FailedToOutputFile(fileName);
            }
        }

        /// <inheritdoc/>
        public string GetProcessedData() {
            return _processedXml;
        }

        /// <summary>
        /// Provides a StringWriter that uses a specified character encoding when writing to a StringBuilder.
        /// </summary>
        /// <remarks>Use this class when you need to control the encoding reported by a StringWriter, such
        /// as when working with APIs that require a specific encoding for text output. The encoding specified during
        /// construction is returned by the Encoding property.</remarks>
        private class EncodingStringWriter : StringWriter {

            /// <summary>
            /// text encoding to use with string builder
            /// </summary>
            private Encoding _encoding;

            /// <summary>
            /// Initializes a new instance of the EncodingStringWriter class using the specified StringBuilder and
            /// character encoding.
            /// </summary>
            /// <param name="sb">The StringBuilder instance that receives the written text. Cannot be null.</param>
            /// <param name="encoding">The character encoding to use for the writer. Determines how characters are encoded when written.</param>
            public EncodingStringWriter(StringBuilder sb, Encoding encoding) : base(sb) {
                _encoding = encoding;
            }

            /// <summary>
            /// Gets the character encoding used by the current stream.
            /// </summary>
            public override Encoding Encoding => _encoding;
        }
    }
}
