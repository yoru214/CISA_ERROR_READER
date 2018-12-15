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
        Models.LocalData Data = new Models.LocalData();

       

        Boolean isReady = false;

        String ErrorFilePath, ExcelPath;
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this.Data;
        }

        private void browse_file_Click(object sender, RoutedEventArgs e)
        {

            isReady = false;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CIC ERROR FILES|*_ERROR.zip";
            if (openFileDialog.ShowDialog() == true)
            {
                //errorFile.IsReadOnly = false;
                errorFile.Text = openFileDialog.FileName;
                errorFile.ScrollToEnd();
                //errorFile.IsReadOnly = true;

                ErrorFilePath = openFileDialog.FileName;

                ExcelPath = ErrorFilePath.Replace("_ERROR.zip", ".csv");

                if(File.Exists(ExcelPath))
                {
                    excelFile.Text = ExcelPath;
                    isReady = true;
                }
                else
                {
                    ExcelPath = ErrorFilePath.Replace("_ERROR.zip", ".csv");
                    if (File.Exists(ExcelPath))
                    {
                        excelFile.Text = ExcelPath;
                        isReady = true;
                    }
                    else
                    {
                        MessageBox.Show("Unable to find CSV File!\n\nPlease place the CSV File on the same Directory/Folder of the error zip file.", "ERROR!!!", MessageBoxButton.OK,MessageBoxImage.Error);
                    }
                }

            }
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {

            this.Data.prevalidation = new List<Models.Error.Prevalidation>();
            this.Data.subjects = new List<Models.Error.Subject>();
            this.Data.contracts = new List<Models.Error.Contract>();


            this.Data.prevalidationdetails = new List<Models.Error.PrevalidationDetails>();
            this.Data.subjectdetails = new List<Models.Error.SubjectDetails>();
            this.Data.contractdetails = new List<Models.Error.ContractDetails>();


            this.Data.subjectdetailsData = new List<Models.Error.SubjectRows>();

            preTab.IsEnabled = false;
            preGrid.IsEnabled = false;
            subTab.IsEnabled = false;
            subGrid.IsEnabled = false;
            preTab.IsEnabled = false;
            subTab.IsEnabled = false;

            tabControl.SelectedIndex = 0;
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

                                            this.Data.prevalidation.Add(p);
                                        }
                                        linenumber++;
                                    }
                                    prevalidationSummaryData.Items.Refresh();
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

                                            this.Data.prevalidationdetails.Add(p);
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

                                            this.Data.subjects.Add(s);
                                        }
                                        linenumber++;
                                    }

                                    subjectnSummaryData.Items.Refresh();
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

                                            this.Data.subjectdetails.Add(s);
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

                                            c.ProviderCode = LineData[0];
                                            c.ErrorType = Convert.ToInt32(LineData[1]);
                                            c.ErrorCode = Convert.ToInt32(LineData[2]);
                                            c.ErrorCount = Convert.ToInt32(LineData[3]);
                                            c.ErrorDescription = LineData[4];

                                            this.Data.contracts.Add(c);
                                        }
                                        linenumber++;
                                    }

                                    contractDetailData.Items.Refresh();
                                }



                                else if (entry.FullName.Contains("_ERROR_LPC"))
                                {
                                    int linenumber = 0;
                                    while ((line = reader.ReadLine()) != null)
                                    {
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

                                            this.Data.contractdetails.Add(c);
                                        }
                                        linenumber++;
                                    }
                                }

                            }
                        }

                    }


                    if (this.Data.prevalidation.Count > 0)
                    {
                        preTab.IsEnabled = true;
                        preGrid.IsEnabled = true;

                    }
                    else
                    {
                        if(this.Data.contracts.Count>0)
                        {
                            conTab.IsEnabled = true;
                            conGrid.IsEnabled = true;
                            tabControl.SelectedIndex = 2;
                        }

                        if (this.Data.subjects.Count > 0)
                        {
                            subTab.IsEnabled = true;
                            subGrid.IsEnabled = true;
                            tabControl.SelectedIndex = 1;
                        }
                    }
                }
            }

            //FILE READ AREA

            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(excelFile.Text))
            { 
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {

                    if (this.Data.prevalidation.Count > 0)
                    {
                        int LineNumber = 0;
                        String line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            LineNumber++;
                            if(this.Data.prevalidationdetails.FindIndex(o=>o.RowNumber == LineNumber) >= 0)
                            {
                                //Models.Error.PrevalidationRows pf = new Models.Error.PrevalidationRows();
                                this.Data.prevalidationdetails.Find(o => o.RowNumber == LineNumber).ErrorData = line;
                                //String[] columns = line.Split('|');
                                //pf.ErrorCode = Convert.ToInt32(columns[7]);
                                //pf.ErrorType = Convert.ToInt32(columns[6]);
                                //pf.RowNumber = LineNumber;
                                //this.Data.prevalidationdetailsData.Add(pf);
                            }

                        }
                    }
                    else
                    {
                        int LineNumber = 0;
                        String line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            String[] columns = line.Split('|');
                            if (columns[0] == "ID")
                            {
                                int i = this.Data.subjectdetails.FindIndex(o => o.SubjectCode == columns[4]);
                                if (i >= 0)
                                {
                                    this.Data.subjectdetails.Find(o => o.SubjectCode == columns[4]).ErrorData = line;
                                    this.Data.subjectdetails.Find(o => o.SubjectCode == columns[4]).RowNumber = (LineNumber + 1);


                                }
                            }
                            if (columns[0] == "CI")
                            {
                                int i = this.Data.contractdetails.FindIndex(o => o.ContractCode == columns[6]);
                                if (i >= 0)
                                {
                                    this.Data.contractdetails.Find(o => o.ContractCode == columns[6]).ErrorData = line;
                                    this.Data.contractdetails.Find(o => o.ContractCode == columns[6]).RowNumber = (LineNumber + 1);
                                }
                            }
                            LineNumber++;
                        }
                        // Process line
                    }
                }

            }
        }


        private void Prevalidation_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;

            //MessageBox.Show("test");
            // Some operations with this row
            this.Data.prevalidationdetailsData = new List<Models.Error.PrevalidationRows>();
            Models.Error.Prevalidation contract = (Models.Error.Prevalidation)prevalidationSummaryData.SelectedItem;

            int ErrorCode = contract.ErrorCode;
            int ErrorType = contract.ErrorType;

            foreach (Models.Error.PrevalidationDetails cd in this.Data.prevalidationdetails.FindAll(o => o.ErrorType == ErrorType && o.ErrorCode == ErrorCode))
            {
                Models.Error.PrevalidationRows prevalidationrow = new Models.Error.PrevalidationRows();

                prevalidationrow.Line = cd.RowNumber;
                prevalidationrow.ErrorType = cd.ErrorType;
                prevalidationrow.ErrorCode = cd.ErrorCode;


                String[] columns = cd.ErrorData.Split('|');


                prevalidationrow.RecordType = columns[0];
                prevalidationrow.BranchCode = columns[2];
                prevalidationrow.PSubjectRefDate = columns[3];
                prevalidationrow.PSubjectNumber = columns[4];

                this.Data.prevalidationdetailsData.Add(prevalidationrow);

            }

            List<Models.Error.PrevalidationRows> pr = this.Data.prevalidationdetailsData.OrderBy(o=>o.Line).ToList<Models.Error.PrevalidationRows>();
            this.Data.prevalidationdetailsData = pr;
            prevalidationDetailData.Items.Refresh();
        }

        private void Subjects_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            // Some operations with this row
            this.Data.subjectdetailsData = new List<Models.Error.SubjectRows>();
            Models.Error.Subject subject = (Models.Error.Subject) subjectnSummaryData.SelectedItem;

            int ErrorCode = subject.ErrorCode;
            int ErrorType = subject.ErrorType;

            foreach (Models.Error.SubjectDetails sd in this.Data.subjectdetails.FindAll(o => o.ErrorType == ErrorType && o.ErrorCode == ErrorCode))
            {
                Console.WriteLine(sd.RowNumber);
                Models.Error.SubjectRows subjectrow = new Models.Error.SubjectRows();

                subjectrow.Line = sd.RowNumber;
                subjectrow.RecordType = "ID";
                subjectrow.SubjectReferenceDate = sd.ReferrenceDate;


                String[] columns = sd.ErrorData.Split('|');


                subjectrow.BranchCode = columns[2];
                subjectrow.PSubjectNumber = columns[4];
                subjectrow.FirstName = columns[6];
                subjectrow.LastName = columns[7];
                subjectrow.Gender = columns[12];
                subjectrow.DateofBirth = columns[13];
                subjectrow.CivilStatus = columns[18];

                this.Data.subjectdetailsData.Add(subjectrow);

            }
            subjectDetailData.Items.Refresh();
        }


        private void Contracts_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            // Some operations with this row
            this.Data.contractdetailsData = new List<Models.Error.ContractRows>();
            Models.Error.Contract contract = (Models.Error.Contract)contractSummaryData.SelectedItem;

            int ErrorCode = contract.ErrorCode;
            int ErrorType = contract.ErrorType;

            foreach (Models.Error.ContractDetails cd in this.Data.contractdetails.FindAll(o => o.ErrorType == ErrorType && o.ErrorCode == ErrorCode))
            {
                Models.Error.ContractRows contractrow = new Models.Error.ContractRows();

                contractrow.Line = cd.RowNumber;
                contractrow.RecordType = "CI";
                contractrow.SubjectReferenceDate = cd.ReferrenceDate;


                String[] columns = cd.ErrorData.Split('|');


                contractrow.BranchCode = columns[2];
                contractrow.PSubjectNumber = columns[4];
                contractrow.Role = columns[5];
                contractrow.PContractNumber = columns[6];
                contractrow.ContractPhase = columns[8];

                this.Data.contractdetailsData.Add(contractrow);

            }
            contractDetailData.Items.Refresh();
        }
    }
}
