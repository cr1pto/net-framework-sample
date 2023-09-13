using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimeCards.Lib
{
    public class TimeTrackingService : ITimeTrackingService
    {
        public TimeSpan CalculateTime(DateTime startTime, DateTime endTime)
        {
            return endTime - startTime;
        }

        public async Task<IEnumerable<TimeCard>> GetTimecards(CancellationToken cancellationToken)
        {
            if(cancellationToken.IsCancellationRequested) return Enumerable.Empty<TimeCard>();

            var timecards = new List<TimeCard>
            {
                new TimeCard
                {
                    TimeEntries = new List<TimeEntry>
                    {
                        new TimeEntry
                        {
                            Start = DateTime.Now.AddMinutes(-100),
                            Stop = DateTime.Now.AddMinutes(-50)
                        },
                    },
                    Employee = new Employee
                    {
                        Name = "John Cena",
                        DateOfBirth = DateTime.Today.AddYears(-40),
                        EmployeeId = Guid.NewGuid()
                    }

                }
            };
            return await Task.FromResult(timecards);
        }
    }

    public interface ITimeTrackingService
    {
        TimeSpan CalculateTime(DateTime startTime, DateTime endTime);
        Task<IEnumerable<TimeCard>> GetTimecards(CancellationToken cancellationToken);
    }
}
