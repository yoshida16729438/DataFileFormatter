using System.Text;

namespace DataFileFormatter.Process {

    internal class ProcessParameter {

        //properties

        internal ProcessType ProcessType { get; set; }
        internal FormatStyle FormatStyle { get; set; }
        internal string FileName { get; set; }
        internal string OutputFileName { get; set; }
        internal int IndentSpacesCount { get; set; }
        internal IndentChar IndentChar { get; set; }
        internal Encoding Encoding { get; set; }

        internal ProcessParameter() { }
    }
}
