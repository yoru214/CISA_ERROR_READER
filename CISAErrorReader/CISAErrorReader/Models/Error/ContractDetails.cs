using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CISAErrorReader.Models.Error
{
    public class ContractDetails
    {
        public String RecordType { get; set; }
        public String ProviderCode { get; set; }
        public String SubjectCode { get; set; }
        public String ContractCode { get; set; }
        public String ReferrenceDate { get; set; }
        public int ErrorType { get; set; }
        public int ErrorCode { get; set; }
        public int RowNumber { get; set; }
    }
}
