using DataFileFormatter.Process;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DataFileFormatter.Formatter {

    /// <summary>
    /// formatter for json
    /// </summary>
    internal class JsonFormatter : IDataFormatter {

        /// <summary>
        /// constructor
        /// </summary>
        internal JsonFormatter() { }

        /// <summary>
        /// loaded json object
        /// </summary>
        private JsonNode _jsonNode;

        /// <summary>
        /// formatted/unformatted json string
        /// </summary>
        private string _processedJson;

        /// <inheritdoc/>
        public ProcessResult LoadFromText(string text) {
            try {
                _jsonNode = JsonNode.Parse(text);
            } catch (JsonException) {
                return ProcessResult.FailedToLoadJson();
            }
            return ProcessResult.Normal();
        }

        /// <inheritdoc/>
        public ProcessResult LoadFromFile(string fileName, Encoding encoding) {
            try {
                using (var sr = new StreamReader(fileName, encoding)) {
                    return LoadFromText(sr.ReadToEnd());
                }
            } catch (Exception) {
                return ProcessResult.FailedToLoadJson();
            }
        }

        /// <inheritdoc/>
        public void Format(IndentChar indentChar, int indent) {
            JsonSerializerOptions options = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true };
            string indentedJson = _jsonNode.ToJsonString(options);

            _processedJson = string.Join(Environment.NewLine, indentedJson.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Select(x => {
                int spaceLen = x.Length - x.TrimStart().Length;

                return string.Concat(Enumerable.Repeat(IndentCharTranslator.GetIndentString(indentChar, indent), spaceLen / 2)) + x.TrimStart();
            }));
        }

        /// <inheritdoc/>
        public void Unformat() {
            var options = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = false };
            _processedJson = _jsonNode.ToJsonString(options);
        }

        /// <inheritdoc/>
        public ProcessResult SaveToFile(string fileName, Encoding encoding) {
            try {
                using (var sw = new StreamWriter(fileName, false, encoding)) {
                    sw.Write(_processedJson);
                }
                return ProcessResult.Normal();
            } catch (Exception) {
                return ProcessResult.FailedToOutputFile(fileName);
            }
        }

        /// <inheritdoc/>
        public string GetProcessedData() {
            return _processedJson;
        }
    }
}
