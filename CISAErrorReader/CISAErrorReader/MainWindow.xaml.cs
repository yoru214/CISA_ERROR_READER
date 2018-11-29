using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
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

namespace CISAErrorReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Models.Error.Prevalidation> prevalidation = new List<Models.Error.Prevalidation>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void browse_file_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CIC ERROR FILES|*_ERROR.zip";
            if (openFileDialog.ShowDialog() == true)
            {
                errorFile.IsReadOnly = false;
                errorFile.Text = openFileDialog.FileName;
                errorFile.ScrollToEnd();
                errorFile.IsReadOnly = true;

                using (var zipStream = new FileStream(openFileDialog.FileName,FileMode.Open))
                {
                    using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                    {
                        foreach(var entry in archive.Entries)
                        {
                            //Console.WriteLine(entry.FullName);
                            using (var stream = entry.Open())
                            {
                                using (var reader = new StreamReader(stream))
                                {
                                    String line = "";

                                    if (entry.FullName.Contains("ERROR_SUMMARY_PRE"))
                                    {
                                        int linenumber = 0;
                                        this.prevalidation = new List<Models.Error.Prevalidation>();
                                        while ((line = reader.ReadLine()) != null)
                                        {
                                            //Console.WriteLine(line);
                                            if(linenumber>0)
                                            {
                                                String[] LineData = line.Split('|');
                                                Models.Error.Prevalidation p = new Models.Error.Prevalidation();

                                                p.ErrorType = Convert.ToInt32(LineData[1]);
                                                p.ErrorCode = Convert.ToInt32(LineData[2]);
                                                p.ErrorCount = Convert.ToInt32(LineData[3]);
                                                p.ErrorDescription = LineData[4];

                                                this.prevalidation.Add(p);
                                            }
                                            linenumber++;
                                        }
                                    }
                                    
                                }
                            }
                           
                        }
                        Console.WriteLine("PRE VALIDATION ERROR");
                        foreach (Models.Error.Prevalidation p in this.prevalidation)
                        {
                            Console.WriteLine(p.ErrorDescription);
                        }
                    }
                }
            }
        }
    }
}
