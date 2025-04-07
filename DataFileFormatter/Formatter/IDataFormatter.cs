using DataFileFormatter.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFileFormatter.Formatter {

    /// <summary>
    /// formatter interface
    /// </summary>
    internal interface IDataFormatter {

        /// <summary>
        /// load data from text
        /// </summary>
        /// <param name="text">data</param>
        /// <returns></returns>
        ProcessResult LoadFromText(string text);

        /// <summary>
        /// load from file with specified encoding
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        ProcessResult LoadFromFile(string fileName, Encoding encoding);

        /// <summary>
        /// format data
        /// </summary>
        /// <param name="indentChar">character to be used to indent</param>
        /// <param name="indent">indent character count per indent</param>
        void Format(IndentChar indentChar, int indent);

        /// <summary>
        /// unformat
        /// </summary>
        void Unformat();

        /// <summary>
        /// save to file in specified path
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        ProcessResult SaveToFile(string fileName, Encoding encoding);

        /// <summary>
        /// get formatted/unformatted text
        /// </summary>
        /// <returns></returns>
        string GetProcessedData();
    }
}
