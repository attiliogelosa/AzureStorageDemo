using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;

namespace AzureStorageDemo.Utilities
{
    public static class Helpers
    {

        #region Enums

        public enum ExitOrContinue
        {
            Continue,
            Exit
        }

        #endregion

        #region Methods

        public static void HelpOnLine()
        {
            string helpText = "This demo shows the basic concepts of Microsoft Azure Storage. Before to use these demos, please set up the settings \"accountName\" and \"accountKey\" in the app.config file.";
            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            foreach (string line in WordWrap(helpText))
            {
                Console.WriteLine(line);
            }
            sb.AppendLine();
            sb.AppendLine("Usage: AzureStorageDemo.exe [#]");
            sb.AppendLine("/? - This help");
            sb.AppendLine("1  - Blob Storage demo");
            sb.AppendLine("2  - Table Storage demo");
            sb.AppendLine("3  - Queue Storage demo");
            sb.AppendLine("4  - File Storage demo");
            Console.Write(sb);
        }

        public static List<string> WordWrap(string paragraph)
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            List<string> lines = new List<string>();

            paragraph = new Regex(@" {2,}").Replace(paragraph.Trim(), @" ") + " ";
            for (var i = 0; paragraph.Length > 0; i++)
            {
                lines.Add(paragraph.Substring(0, Math.Min(Console.WindowWidth, paragraph.Length)));

                int length = lines[i].LastIndexOf(" ", StringComparison.Ordinal);

                if (length > 0)
                {
                    lines[i] = lines[i].Remove(length);
                }
                paragraph = paragraph.Substring(Math.Min(lines[i].Length + 1, paragraph.Length));
                //Console.SetCursorPosition(left, top + i);
                //Console.WriteLine(lines[i]);
            }
            return lines;
        }

        public static string BuildKeyString(string key)
        {
            string result = string.Empty;

            int lenght = key.Length < 10 ? key.Length : 10;
            if (key.Length <= 10)
            {
                result = key;
            }
            else
            {
                result = key.Substring(0, 4) + "..." + key.Substring(key.Length - 4);
            }
            return result;
        }

        public static void StopAndGo(ExitOrContinue exitOrContinue = ExitOrContinue.Continue)
        {
            Console.WriteLine();
            Console.Write(string.Format("Press any key to {0}...", exitOrContinue == ExitOrContinue.Continue ? "continue" : "exit"));
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine();
        }

        public static string GetConfigurationValue(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString();
        }

        #endregion

    }

}
