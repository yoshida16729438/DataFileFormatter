using DataFileFormatter.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DataFileFormatter.Formatter.Json {

    /// <summary>
    /// Json indented serializer
    /// </summary>
    internal class CustomJsonSerializer {

        internal CustomJsonSerializer() { }

        internal string FormatIndented(JsonNode jsonNode, IndentChar indentChar, int indent) {
            IndentStringBuilder builder = new IndentStringBuilder(GetIndentString(indentChar, indent));
            BuildJsonNode(jsonNode, builder);
            return builder.ToString();
        }

        /// <summary>
        /// get indent string
        /// </summary>
        /// <param name="indentChar"></param>
        /// <param name="indent"></param>
        /// <returns></returns>
        internal string GetIndentString(IndentChar indentChar, int indent) {
            switch (indentChar) {
                case IndentChar.space:
                    return new string(' ', indent);
                case IndentChar.tab:
                    return new string('\t', 1);
                default:
                    return string.Empty;
            }
        }

        private void BuildJsonNode(JsonNode jsonNode, IndentStringBuilder builder) {
            switch (jsonNode.GetValueKind()) {
                case JsonValueKind.Object:
                    BuildJsonObject(jsonNode.AsObject(), builder);
                    break;

                case JsonValueKind.Array:
                    BuildJsonArray(jsonNode.AsArray(), builder);
                    break;

                default:
                    BuildJsonValue(jsonNode.AsValue(), builder);
                    break;
            }
        }

        private void BuildJsonObject(JsonObject jsonObject, IndentStringBuilder builder) {
            builder.Append("{");
            if(jsonObject.Count > 0) {
                builder.IncrementIndentLevel();
                builder.MoveToNewLine();

                KeyValuePair<string,JsonNode> element = jsonObject.First();
                builder.Append($"\"{element.Key}\": ");
                BuildJsonNode(element.Value, builder);

                for(int i=1; i < jsonObject.Count; i++) {
                    element=jsonObject.ElementAt(i);
                    builder.Append(",");
                    builder.MoveToNewLine();
                    builder.Append($"\"{element.Key}\": ");
                    BuildJsonNode(element.Value, builder);
                }
                
                builder.DecrementIndentLevel();
                builder.MoveToNewLine();
            }
            builder.Append("}");
        }

        private void BuildJsonArray(JsonArray jsonArray, IndentStringBuilder builder) {
            builder.Append("[");
            if (jsonArray.Count > 0) {
                builder.IncrementIndentLevel();
                builder.MoveToNewLine();
                BuildJsonNode(jsonArray[0], builder);
                for (int i = 1; i < jsonArray.Count; i++) {
                    builder.Append(",");
                    builder.MoveToNewLine();
                    BuildJsonNode(jsonArray[i], builder);
                }
                builder.DecrementIndentLevel();
                builder.MoveToNewLine();
            }
            builder.Append("]");
        }

        private void BuildJsonValue(JsonValue jsonValue, IndentStringBuilder builder) {
            if (jsonValue.GetValueKind() == JsonValueKind.String) {
                builder.Append($"\"{jsonValue.GetValue<string>()}\"");
            } else {
                builder.Append(jsonValue.ToString());
            }
        }
    }
}
