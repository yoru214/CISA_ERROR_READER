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
        List<Models.Error.PrevalidationDetails> prevalidationdetails = new List<Models.Error.PrevalidationDetails>();
        List<Models.Error.Subject> subjects = new List<Models.Error.Subject>();
        List<Models.Error.SubjectDetails> subjectdetails = new List<Models.Error.SubjectDetails>();
        List<Models.Error.Contract> contracts = new List<Models.Error.Contract>();
        List<Models.Error.ContractDetails> contractdetails = new List<Models.Error.ContractDetails>();

        Boolean isReady = false;

        String ErrorFilePath, ExcelPath;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void browse_file_Click(object sender, RoutedEventArgs e)
        {
            isReady = false;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CIC ERROR FILES|*_ERROR.zip";
            if (openFileDialog.ShowDialog() == true)
            {
                errorFile.IsReadOnly = false;
                errorFile.Text = openFileDialog.FileName;
                errorFile.ScrollToEnd();
                errorFile.IsReadOnly = true;

                ErrorFilePath = openFileDialog.FileName;

                ExcelPath = ErrorFilePath.Replace("_ERROR.zip", ".xlsx");

                if(File.Exists(ExcelPath))
                {
                    excelFile.Text = ExcelPath;
                    isReady = true;
                }
                else
                {
                    ExcelPath = ErrorFilePath.Replace("_ERROR.zip", ".xls");
                    if (File.Exists(ExcelPath))
                    {
                        excelFile.Text = ExcelPath;
                        isReady = true;
                    }
                    else
                    {
                        MessageBox.Show("Unable to find Excel File!\n\nPlease place the excel file on the same directory/folder of the error zip file.", "ERROR!!!", MessageBoxButton.OK,MessageBoxImage.Error);
                    }
                }

            }
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {

            using (var zipStream = new FileStream(ErrorFilePath, FileMode.Open))
            {
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                {
                    foreach (var entry in archive.Entries)
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
                                    while ((line = reader.ReadLine()) != null)
                                    {
                                        //Console.WriteLine(line);
                                        if (linenumber > 0)
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

                                if (entry.FullName.Contains("ERROR_PRE"))
                                {
                                    int linenumber = 0;
                                    while ((line = reader.ReadLine()) != null)
                                    {
                                        //Console.WriteLine(line);
                                        if (linenumber > 0)
                                        {
                                            String[] LineData = line.Split('|');
                                            Models.Error.PrevalidationDetails p = new Models.Error.PrevalidationDetails();

                                            p.ProviderCode = LineData[1];
                                            p.ReferrenceDate = LineData[2];
                                            p.ErrorType = Convert.ToInt32(LineData[6]);
                                            p.ErrorCode = Convert.ToInt32(LineData[7]);
                                            p.RowNumber = Convert.ToInt32(LineData[9]);

                                            this.prevalidationdetails.Add(p);
                                        }
                                        linenumber++;
                                    }
                                }

                                else if (entry.FullName.Contains("_ERROR_SUMMARY_LPS"))
                                {
                                    int linenumber = 0;
                                    while ((line = reader.ReadLine()) != null)
                                    {
                                        //Console.WriteLine(line);
                                        if (linenumber > 0)
                                        {
                                            String[] LineData = line.Split('|');
                                            Models.Error.Subject s = new Models.Error.Subject();

                                            s.ErrorType = Convert.ToInt32(LineData[1]);
                                            s.ErrorCode = Convert.ToInt32(LineData[2]);
                                            s.ErrorCount = Convert.ToInt32(LineData[3]);
                                            s.ErrorDescription = LineData[4];

                                            this.subjects.Add(s);
                                        }
                                        linenumber++;
                                    }
                                }


                                else if (entry.FullName.Contains("_ERROR_LPS"))
                                {
                                    int linenumber = 0;
                                    while ((line = reader.ReadLine()) != null)
                                    {
                                        //Console.WriteLine(line);
                                        if (linenumber > 0)
                                        {
                                            String[] LineData = line.Split('|');
                                            Models.Error.SubjectDetails s = new Models.Error.SubjectDetails();

                                            s.ProviderCode = LineData[1];
                                            s.ReferrenceDate = LineData[2];
                                            s.SubjectCode = LineData[3];
                                            s.ErrorType = Convert.ToInt32(LineData[6]);
                                            s.ErrorCode = Convert.ToInt32(LineData[7]);

                                            this.subjectdetails.Add(s);
                                        }
                                        linenumber++;
                                    }
                                }


                                else if (entry.FullName.Contains("_ERROR_SUMMARY_LPC"))
                                {
                                    int linenumber = 0;
                                    while ((line = reader.ReadLine()) != null)
                                    {
                                        //Console.WriteLine(line);
                                        if (linenumber > 0)
                                        {
                                            String[] LineData = line.Split('|');
                                            Models.Error.Contract c = new Models.Error.Contract();

                                            c.RecordType = LineData[0];
                                            c.ErrorType = Convert.ToInt32(LineData[1]);
                                            c.ErrorCode = Convert.ToInt32(LineData[2]);
                                            c.ErrorCount = Convert.ToInt32(LineData[3]);
                                            c.ErrorDescription = LineData[4];

                                            this.contracts.Add(c);
                                        }
                                        linenumber++;
                                    }
                                }



                                else if (entry.FullName.Contains("_ERROR_LPC"))
                                {
                                    int linenumber = 0;
                                    while ((line = reader.ReadLine()) != null)
                                    {
                                        //Console.WriteLine(line);
                                        if (linenumber > 0)
                                        {
                                            String[] LineData = line.Split('|');
                                            Models.Error.ContractDetails c = new Models.Error.ContractDetails();

                                            c.RecordType = LineData[0];
                                            c.ProviderCode = LineData[1];
                                            c.ReferrenceDate = LineData[2];
                                            c.SubjectCode = LineData[3];
                                            c.ContractCode = LineData[5];
                                            c.ErrorType = Convert.ToInt32(LineData[6]);
                                            c.ErrorCode = Convert.ToInt32(LineData[7]);

                                            this.contractdetails.Add(c);
                                        }
                                        linenumber++;
                                    }
                                }

                            }
                        }

                    }


                    if (this.prevalidation.Count > 0)
                    {
                        Console.WriteLine("PRE VALIDATION ERRORS START");
                        foreach (Models.Error.Prevalidation p in this.prevalidation)
                        {
                            Console.WriteLine(p.ErrorDescription + " count=" + p.ErrorCount + " rows=" + this.prevalidationdetails.Count);
                            foreach (Models.Error.PrevalidationDetails d in this.prevalidationdetails)
                            {
                                Console.WriteLine(d.RowNumber);
                            }
                        }
                        Console.WriteLine("PRE VALIDATION ERRORS END");

                    }
                    else
                    {

                        Console.WriteLine("SUBJECT ERRORS START");
                        foreach (Models.Error.Subject s in this.subjects)
                        {
                            Console.WriteLine(s.ErrorDescription + " count=" + s.ErrorCount + " rows=" + this.subjectdetails.Count);
                            foreach (Models.Error.SubjectDetails d in this.subjectdetails)
                            {
                                Console.WriteLine(d.SubjectCode);
                            }
                        }
                        Console.WriteLine("SUBJECT ERRORS END");

                        Console.WriteLine("CONTRACT ERRORS START");
                        foreach (Models.Error.Contract s in this.contracts)
                        {
                            Console.WriteLine(s.ErrorDescription + " count=" + s.ErrorCount + " rows=" + this.contractdetails.Count);
                            foreach (Models.Error.ContractDetails d in this.contractdetails)
                            {
                                Console.WriteLine(d.ContractCode);
                            }
                        }
                        Console.WriteLine("CONTRACT ERRORS END");
                    }
                }
            }
        }
    }
}
