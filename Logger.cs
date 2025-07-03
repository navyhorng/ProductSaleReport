using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductSaleReport
{
    public class Logger
    {
        private static readonly string LogDirectory = "Logs";
        private static readonly string LogFile = Path.Combine(LogDirectory, "errors.txt");
        private const int MaxLogEntries = 100;
        public static void LogError(Exception ex)
        {
            try
            {
                Directory.CreateDirectory(LogDirectory);

                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {ex.Message}\n{ex.StackTrace}\n------------------------------\n";

                List<string> lines = new List<string>();
                if (File.Exists(LogFile))
                {
                    lines = File.ReadAllLines(LogFile).ToList();
                }

                lines.Add(logEntry);

                if (lines.Count > MaxLogEntries)
                {
                    lines = lines.Skip(lines.Count - MaxLogEntries).ToList();
                }

                File.WriteAllLines(LogFile, lines);
            }
            catch (Exception logEx)
            {
                System.Windows.Forms.MessageBox.Show("Failed to write log: " + logEx.Message);
            }
        }
        public static void LogInfo(string message)
        {
            try
            {
                Directory.CreateDirectory(LogDirectory);

                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] INFO: {message}\n------------------------------\n";

                File.AppendAllText(LogFile, logEntry);
            }
            catch (Exception logEx)
            {
                System.Windows.Forms.MessageBox.Show("Failed to write log: " + logEx.Message);
            }
        }
    }
}
