using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBagScanner.Models
{
    public class ScanRecord
    {
        public DateTime Timestamp { get; set; }
        public string BagCode1 { get; set; }
        public string BagCode2 { get; set; }
        public bool IsMatch { get; set; }
    }
}
