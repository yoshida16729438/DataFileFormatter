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

        ProcessResult LoadFromText(string text);

        ProcessResult LoadFromFile(string fileName, Encoding encoding);

        void Format(IndentChar indentChar,int indent);

        void Unformat();

        ProcessResult SaveToFile(string fileName,Encoding encoding);

        void OutputToStdout();

    }
}
