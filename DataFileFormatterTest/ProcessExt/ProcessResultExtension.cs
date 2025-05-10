using DataFileFormatter.Process;

namespace DataFileFormatterTest.ProcessExt {
    internal static class ProcessResultExtension {
        public static bool IsEqualsTo(this ProcessResult self, object obj) {
            if (obj != null && obj is ProcessResult pr) {
                return self.ResultCode == pr.ResultCode && self.Message == pr.Message;
            }
            return false;
        }
    }
}
