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

        // input from 

        [TestMethod]
        public void WhenValidJsonRequestedFromStdin() {
            string output = TestContextHandler.GetOutputFilePath("formatted.json");
            string[] props = new string[] { "--json", "--format", "--charset", "shift-jis", "--indentSpacesCount", "4", "--outfile", output, TestContextHandler.GetTestDataPath("unindented.json") };

            int result = Program.Main(props);
            Assert.AreEqual(0, result);

            string expected = File.ReadAllText(TestContextHandler.GetTestDataPath("indentWithFourSpaces.json"));
            string actual = File.ReadAllText(output);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NoInputDataSpecified() {
            SetConsoleRedirect(false);
            Assert.AreEqual(52, Program.Main(new string[0]));
        }

        private void SetConsoleRedirect(bool isRedirected) {
            FieldInfo queried = typeof(Console).GetField("_stdInRedirectQueried", BindingFlags.Static | BindingFlags.NonPublic);
            queried.SetValue(null, true);

            FieldInfo redirected = typeof(Console).GetField("_isStdInRedirected", BindingFlags.Static | BindingFlags.NonPublic);
            redirected.SetValue(null, isRedirected);
        }

    }
}
