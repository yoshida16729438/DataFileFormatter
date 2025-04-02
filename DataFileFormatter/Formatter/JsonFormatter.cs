using DataFileFormatter.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DataFileFormatter.Formatter {

    /// <summary>
    /// formatter for json
    /// </summary>
    internal class JsonFormatter : IDataFormatter {

        public JsonFormatter() { }

        private JsonNode _jsonNode;

        private string _processedJson;

        public ProcessResult LoadFromText(string text) {
            _jsonNode = JsonNode.Parse(text);
            if (_jsonNode == null) return ProcessResult.FailedToLoadJson();
            else return ProcessResult.Normal();
        }

        public ProcessResult LoadFromFile(string fileName, Encoding encoding) {
            try {
                using (var sr = new System.IO.StreamReader(fileName, encoding)) {
                    return LoadFromText(sr.ReadToEnd());
                }
            } catch (Exception) {
                return ProcessResult.FailedToLoadJson();
            }
        }

        public void Format(IndentChar indentChar, int indent) {

        }

        public void Unformat() {
            var options = new JsonSerializerOptions() { WriteIndented = false };
            _processedJson = _jsonNode.ToJsonString(options);
        }

        public ProcessResult SaveToFile(string fileName, Encoding encoding) {
            try {
                using (var sw = new System.IO.StreamWriter(fileName, false, encoding)) {
                    sw.Write(_processedJson);
                }
                return ProcessResult.Normal();
            } catch (Exception) {
                return ProcessResult.FailedToOutputFile(fileName);
            }
        }

        public void OutputToStdout() {
            Console.WriteLine(_processedJson);
        }

    }
}
