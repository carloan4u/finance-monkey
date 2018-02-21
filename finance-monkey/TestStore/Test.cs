using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceMonkey.TestStore
{
    class Test
    {
        public string ServiceName { get; set; }
        public DateTime DateStarted { get; set; }
        public string OriginalInstanceSize { get; set; }
        public string NewInstanceSize { get; set; }
        public DateTime DateEnded { get; set; }
        public bool WasSuccessful { get; set; }
    }
}
