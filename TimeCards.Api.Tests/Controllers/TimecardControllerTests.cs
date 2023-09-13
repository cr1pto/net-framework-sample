using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using TimeCards.Lib;
using TimeCards.Web.Controllers;

namespace TimeCards.Api.Tests.Controllers
{
    [TestClass]
    public class TimecardControllerTests
    {
        [TestMethod]
        public async Task Get_WithEmptyMock_Returns_Successfully()
        {
            var mockTimetrackingService = new Mock<ITimeTrackingService>();
            var controller = SetupControllerContext(mockTimetrackingService.Object);
            var actionResult = await controller.Get(CancellationToken.None);
            var result = await UnwrapActionResult<IEnumerable<TimeCard>>(actionResult);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Get_WithMock_Returns_Successfully()
        {
            var mockTimetrackingService = new Mock<ITimeTrackingService>();
            mockTimetrackingService.Setup(s => s.GetTimecards(It.IsAny<CancellationToken>())).ReturnsAsync(new List<TimeCard>
            {
                new TimeCard
                {
                    Employee = new Employee
                    {
                        Name = "Test",
                    },
                    TimeEntries = new List<TimeEntry>
                    {
                        new TimeEntry
                        {
                            Start = DateTime.MaxValue,
                            Stop = DateTime.MaxValue
                        }
                    }
                }
            });
            var controller = SetupControllerContext(mockTimetrackingService.Object);
            var actionResult = await controller.Get(CancellationToken.None);
            var result = await UnwrapActionResult<IEnumerable<TimeCard>>(actionResult);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<TimeCard>));
            Assert.AreEqual("Test", result.First().Employee.Name);
        }

        private TimeCardsController SetupControllerContext(ITimeTrackingService timeTrackingService)
        {
            var controller = new TimeCardsController(timeTrackingService);
            controller.ControllerContext = new HttpControllerContext();
            controller.ControllerContext.RequestContext = new HttpRequestContext();
            controller.ControllerContext.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            return controller;
        }

        private async Task<TResponse> UnwrapActionResult<TResponse>(IHttpActionResult httpActionResult)
        {
            var response = await httpActionResult.ExecuteAsync(CancellationToken.None);
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(data);
        }
    }
}
