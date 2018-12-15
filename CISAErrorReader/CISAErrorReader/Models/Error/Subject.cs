using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CISAErrorReader.Models.Error
{
    public class Subject
    {
        public int ErrorType { get; set; }
        public int ErrorCode { get; set; }
        public int ErrorCount { get; set; }
        public String ErrorDescription { get; set; }
    }
}
