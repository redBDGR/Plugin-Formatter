using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Plugin_Formatter
{
    public static class FileManip
    {
        public static async Task<string> ReadFileAsync(string dir)
        {
            using (var reader = File.OpenText(dir))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public static async Task WriteFileAsync(string dir, string messaage, bool append = true)
        {
            using (FileStream stream = new FileStream(dir, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            using (StreamWriter sw = new StreamWriter(stream))
            {
                await sw.WriteLineAsync(messaage);
            }
        }

        public static bool IsValidFileName(string name)
        {
            // Validate .cs file
            if (!Regex.IsMatch(name, @".cs"))
            {
                MessageBox.Show("This file must be a .cs file", "Invalid file type", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!File.Exists(name))
            {
                MessageBox.Show($"The file {name} does not exist or could not be found", "Could not find this file", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}
