using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Plugin_Formatter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string selectedFile = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void chooseFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog() { Filter = $"C# Class (*.cs)|*.cs" };
            if (fileDialog.ShowDialog(this) == true)
            {
                string fileName = fileDialog.FileName;

                if (!FileManip.IsValidFileName(fileName))
                {
                    return;
                }

                // Update UI
                selectedFile = fileDialog.FileName;
                chooseFileTextBox.Text = selectedFile;
                optionsGrid.Visibility = Visibility.Visible;
            }
        }

        private void singleLineRow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            singleLineCheckBox.IsChecked = !singleLineCheckBox.IsChecked;
        }

        private void obfuscateRow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            obfuscationCheckBox.IsChecked = !obfuscationCheckBox.IsChecked;
        }

        private async void formatButton_Click(object sender, RoutedEventArgs e)
        {
            if (!FileManip.IsValidFileName(selectedFile))
            {
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog() { AddExtension = true, CheckPathExists = true, Filter = $"C# Class (*.cs)|*.cs" };

            if (saveDialog.ShowDialog(this) == false)
            {
                return;
            }

            string saveDir = saveDialog.FileName;

            // Async file read
            string contents = await FileManip.ReadFileAsync(selectedFile);

            // Async compress file
            if ((bool)singleLineCheckBox.IsChecked) 
                contents = await Formatter.CompressStringAsync(contents);

            await FileManip.WriteFileAsync(saveDir, contents);

            MessageBox.Show(this, "File has been formatted", "Finished", MessageBoxButton.OK, MessageBoxImage.None);
        }
    }
}
