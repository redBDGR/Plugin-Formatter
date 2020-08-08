using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Plugin_Formatter
{
    public static class Formatter
    {
        public static async Task<string> CompressStringAsync(string input)
        {
            StringBuilder final = new StringBuilder();
            string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            await Task.Run(async () =>
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i] == null)
                        continue;

                    var line = lines[i];

                    // Remove comments
                    line = await Task.Run(() => StripComments(line));

                    // Remove #region-ing
                    if (Regex.IsMatch(line, @"#region") || Regex.IsMatch(line, @"#endregion"))
                        line = string.Empty;

                    final.Append(line);
                }
            });

            return Regex.Replace(final.ToString(), @"\s+", " ");
        }

        private static string StripComments(string input)
        {
            for (int i = 0; i < input.TakeWhile(c => c == '*').Count(); i++)
                input = Regex.Replace(input, "\\/*.*\\*/", "");
            input = Regex.Replace(input, @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/", "$1");
            return input;
        }
    }
}
