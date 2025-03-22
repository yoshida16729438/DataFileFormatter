using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFileFormatter.Command {

    internal class CommandLineData {

        //properties

        internal FileType fileType { get; set; }
        internal FormatStyle formatStyle { get; set; }
        internal string fileName { get; set; }
        internal string outputFileName { get; set; }
        internal int paddingSpacesCount { get; set; }
        internal PaddingChar paddingChar { get; set; }
        internal Encoding encoding { get; set; }


        //enums

        /// <summary>
        /// target file type
        /// </summary>
        internal enum FileType {
            json,
            xml,
            csv
        }

        /// <summary>
        /// format style
        /// </summary>
        internal enum FormatStyle {
            format,
            unformat
        }

        /// <summary>
        /// padding char
        /// </summary>
        internal enum PaddingChar {
            space,
            tab
        }

        internal CommandLineData() { }
    }
}
