using System;

namespace Server
{
    public static class Log
    {
        private static string InfoTag = "[INFO] ";
        private static string DebugTag = "[DEBUG] ";
        private static string WarnTag = "[WARN] ";
        private static string ErrorTag = "[ERROR] ";

        public static void Info(string msg)
        {
            Console.WriteLine(InfoTag + msg);
        }
        public static void Debug(string msg)
        {
            Console.WriteLine(DebugTag + msg);
        }
        public static void Warn(string msg)
        {
            Console.WriteLine(WarnTag + msg);
        }
        public static void Error(string msg)
        {
            Console.WriteLine(ErrorTag + msg);
        }
    }
}
