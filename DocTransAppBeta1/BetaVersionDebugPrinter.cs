using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocTransAppBeta1
{
    /// <summary>
    /// 调试信息打印
    /// </summary>
    public static class BetaVersionDebugPrinter
    {
        /// <summary>
        /// 打印调试信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="callee"></param>
        public static void WriteLine(string message, string callee)
        {
            if (string.IsNullOrEmpty(callee)) callee = "null";
            Console.WriteLine("[{0}]<{1}>:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), callee, message);
        }
    }
}
