using System.Text;

namespace DataFileFormatter.Process {

    /// <summary>
    /// Represents a set of parameters used to configure a processing operation, including process type, formatting
    /// options, file names, and encoding settings.
    /// </summary>
    internal class ProcessParameter {

        //properties

        /// <summary>
        /// file type or other process
        /// </summary>
        internal ProcessType ProcessType { get; set; }

        /// <summary>
        /// format of unformat
        /// </summary>
        internal FormatStyle FormatStyle { get; set; }

        /// <summary>
        /// input file path
        /// </summary>
        internal string FileName { get; set; }

        /// <summary>
        /// output file path
        /// </summary>
        internal string OutputFileName { get; set; }

        /// <summary>
        /// Gets or sets the number of spaces to use for each indentation level.
        /// </summary>
        internal int IndentSpacesCount { get; set; }

        /// <summary>
        /// character setting to use for indentation
        /// </summary>
        internal IndentChar IndentChar { get; set; }

        /// <summary>
        /// input or output file encoding
        /// </summary>
        internal Encoding Encoding { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        internal ProcessParameter() { }
    }
}
