using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CISAErrorReader.Models
{
    public class LocalData : INotifyPropertyChanged
    {
        List<Models.Error.Prevalidation> _prevalidation = new List<Models.Error.Prevalidation>();
        List<Models.Error.PrevalidationDetails> _prevalidationdetails = new List<Models.Error.PrevalidationDetails>();
        List<Models.Error.PrevalidationRows> _prevalidationdetailsData = new List<Models.Error.PrevalidationRows>();
        List<Models.Error.Subject> _subjects = new List<Models.Error.Subject>();
        List<Models.Error.SubjectDetails> _subjectdetails = new List<Models.Error.SubjectDetails>();
        List<Models.Error.SubjectRows> _subjectdetailsData = new List<Models.Error.SubjectRows>();
        List<Models.Error.Contract> _contracts = new List<Models.Error.Contract>();
        List<Models.Error.ContractDetails> _contractdetails = new List<Models.Error.ContractDetails>();
        List<Models.Error.ContractRows> _contractdetailsData = new List<Models.Error.ContractRows>();


        public List<Models.Error.Prevalidation> prevalidation
        {
            get
            {
                return _prevalidation;
            }
            set
            {

                _prevalidation = value;
                OnPropertyChanged("prevalidation");
            }
        }
        public List<Models.Error.PrevalidationDetails> prevalidationdetails
        {
            get
            {
                return _prevalidationdetails;
            }
            set
            {

                _prevalidationdetails = value;
                OnPropertyChanged("prevalidationdetails");
            }
        }
        public List<Models.Error.PrevalidationRows> prevalidationdetailsData
        {
            get
            {
                return _prevalidationdetailsData;
            }
            set
            {

                _prevalidationdetailsData = value;
                OnPropertyChanged("prevalidationdetailsData");
            }
        }
        public List<Models.Error.Subject> subjects
        {
            get
            {
                return _subjects;
            }
            set
            {

                _subjects = value;
                OnPropertyChanged("subjects");
            }
        }
        public List<Models.Error.SubjectDetails> subjectdetails
        {
            get
            {
                return _subjectdetails;
            }
            set
            {

                _subjectdetails = value;
                OnPropertyChanged("subjectdetails");
            }
        }
        public List<Models.Error.SubjectRows> subjectdetailsData
        {
            get
            {
                return _subjectdetailsData;
            }
            set
            {

                _subjectdetailsData = value;
                OnPropertyChanged("subjectdetailsData");
            }
        }
        public List<Models.Error.Contract> contracts
        {
            get
            {
                return _contracts;
            }
            set
            {

                _contracts = value;
                OnPropertyChanged("contracts");
            }
        }
        public List<Models.Error.ContractDetails> contractdetails
        {
            get
            {
                return _contractdetails;
            }
            set
            {

                _contractdetails = value;
                OnPropertyChanged("contractdetails");
            }
        }
        public List<Models.Error.ContractRows> contractdetailsData
        {
            get
            {
                return _contractdetailsData;
            }
            set
            {

                _contractdetailsData = value;
                OnPropertyChanged("contractdetailsData");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            Console.WriteLine(property);
        }
    }
}
