using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelliumClient
{
    public class TestCase
    {
        public bool Enabled { get; set; }
        public int ID { get; set; }
        public int Seq { get; set; }
        public String TestCaseName { get; set; }
        public String Markers { get; set; }
        public int Itr { get; set; }
        public String XmlFileName { get; set; }
    }
}
