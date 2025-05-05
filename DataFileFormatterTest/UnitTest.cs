using DataFileFormatter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataFileFormatterTest {

    [TestClass]
    public class UnitTest {

        //[TestMethod]
        //public async Task WhenValidJsonRequestedFromStdin() {
        //    string output = TestContextHandler.GetOutputFilePath("formatted.json");
        //    string[] props = new string[] { "--json", "--format", "--charset", "shift-jis", "--indentSpacesCount", "4", "--outfile", output };

        //    using (MemoryStream ms = new MemoryStream()) {
        //        Encoding sjis = Encoding.GetEncoding("shift-jis");
        //        foreach (string line in File.ReadAllLines(TestContextHandler.GetTestDataPath("unindented.json"), Encoding.GetEncoding("UTF-8"))) {
        //            byte[] data = sjis.GetBytes(line);
        //            ms.Write(data, 0, data.Length);
        //        }
        //        ms.Position = 0;

        //        using (TextReader sr = new StreamReader(ms)) {
        //            Console.SetIn(sr);

        //            // to mark as redirected
        //            SetConsoleRedirect(true);

        //            int result = await Program.Main(props);
        //            Assert.AreEqual(0, result);

        //            //reset redirected
        //            SetConsoleRedirect(false);
        //        }
        //    }

        //    string expected = File.ReadAllText(TestContextHandler.GetTestDataPath("indentWithFourSpaces.json"));
        //    string actual = File.ReadAllText(output);
        //    Assert.AreEqual(expected, actual);
        //}

        [TestMethod]
        public async Task NoInputDataSpecified() {
            SetConsoleRedirect(false);
            Assert.AreEqual(52, await Program.Main(new string[0]));
        }

        private void SetConsoleRedirect(bool isRedirected) {
            FieldInfo queried = typeof(Console).GetField("_stdInRedirectQueried", BindingFlags.Static | BindingFlags.NonPublic);
            queried.SetValue(null, true);

            FieldInfo redirected = typeof(Console).GetField("_isStdInRedirected", BindingFlags.Static | BindingFlags.NonPublic);
            redirected.SetValue(null, isRedirected);
        }

    }
}
