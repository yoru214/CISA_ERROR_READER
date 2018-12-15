using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CISAErrorReader.Models.Error
{
    public class ContractRows
    {
        public Int64 Line { get; set; }
        public String RecordType { get; set; }
        public String BranchCode { get; set; }
        public String SubjectReferenceDate { get; set; }
        public String PSubjectNumber { get; set; }
        public String PContractNumber { get; set; }
        public String Role { get; set; }
        public String ContractPhase { get; set; }
    }
}
