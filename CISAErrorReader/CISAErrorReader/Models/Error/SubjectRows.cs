using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CISAErrorReader.Models.Error
{
    public class SubjectRows
    {
        public Int64 Line { get; set; }
        public String RecordType { get; set; }
        public String BranchCode { get; set; }
        public String SubjectReferenceDate { get; set; }
        public String PSubjectNumber { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Gender { get; set; }
        public String DateofBirth { get; set; }
        public String CivilStatus { get; set; }
    }
}
