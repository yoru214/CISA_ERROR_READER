using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CISAErrorReader.Models.Error
{
    public class PrevalidationRows
    {
        public int Line { get; set; }
        public int ErrorType { get; set; }
        public int ErrorCode { get; set; }
        public String BranchCode { get; set; }
        public String RecordType { get; set; }
        public String PSubjectRefDate { get; set; }
        public String PSubjectNumber { get; set; }
    }
}
