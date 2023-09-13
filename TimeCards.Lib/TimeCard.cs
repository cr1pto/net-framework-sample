using System;
using System.Collections.Generic;
using System.Text;

namespace TimeCards.Lib
{
    public class TimeCard
    {
        public IEnumerable<TimeEntry> TimeEntries { get; set; }
        public Employee Employee { get; set; }
    }
}
