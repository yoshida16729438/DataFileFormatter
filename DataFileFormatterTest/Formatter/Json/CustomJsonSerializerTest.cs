using DataFileFormatter.Formatter.Json;
using DataFileFormatter.Process;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DataFileFormatterTest.Formatter.Json {

    [TestClass]
    public class CustomJsonSerializerTest {
        [TestMethod]
        public void TryTest() {
            JsonNode jsonNode = JsonNode.Parse("{\"stringProp\":\"aaa\",\"numberProp\":1,\"objectProp\":{\"objectPropChild\":\"wei\"},\"arrayProp\":[{\"arrayChild\":\"soiya\"}],\"rawArrayProp\":[1,2,3]}");
            var ser = new CustomJsonSerializer();
            Debug.WriteLine(ser.FormatIndented(jsonNode, IndentChar.space, 2));
        }
    }
}
