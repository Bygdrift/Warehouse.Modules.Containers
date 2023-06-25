using Bygdrift.Warehouse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Module;
using Module.AppFunctions;
using Module.AppFunctions.Models;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ModuleTests.AppFunctions
{
    [TestClass]
    public class QueuesTests
    {
        private readonly Mock<ILogger<TimeTrigger>> loggerMock = new();
        private readonly TimeTrigger function;
        private readonly AppBase<Settings> app;

        public QueuesTests()
        {
            function = new TimeTrigger(loggerMock.Object);
            app = new AppBase<Settings>();
        }

        [TestMethod]
        public async Task AddData()
        {
           await function.Run(null);
        }

        //[TestMethod]
        //public async Task GetAndDelete()
        //{
        //    await app.DataLakeQueue.DeleteMessagesAsync();
        //    var res0 = GetBody(await function.QueuesPeek(default));
        //    Assert.IsNull(res0);
        //    await app.DataLakeQueue.AddMessageAsync(JsonConvert.SerializeObject(new { data = "content", date = DateTime.Now }));
        //    var res1 = GetBody(await function.QueuesPeek(default));
        //    Assert.AreEqual(1, res1.Count());
        //    var res2 = GetBody(await function.QueuesGetAndDelete(default));
        //    Assert.AreEqual(1, res2.Count());
        //    var res3 = GetBody(await function.QueuesGetAndDelete(default));
        //    Assert.IsNull(res3);
        //    var errors = function.App.Log.GetErrorsAndCriticals();
        //    Assert.AreEqual(0, errors.Count());
        //}

        //[TestMethod]
        //public async Task GetSortedAndDelete()
        //{
        //    //Add some data - more than 32 items:
        //    await app.DataLakeQueue.DeleteMessagesAsync();
        //    var tasks = new List<string>();
        //    for (int i = 0; i < 40; i++)
        //        tasks.Add("Message " + (i < 10 ? "0"+ i : i));

        //    await app.DataLakeQueue.AddMessagesAsync(tasks.ToArray());

        //    //Request data:
        //    var httpContext = new DefaultHttpContext();
        //    httpContext.Request.ContentType = "application/json";
        //    var stream = new MemoryStream();
        //    var writer = new StreamWriter(stream);
        //    await writer.WriteAsync("[\"EmptyStart\", \"Message 04\", \"Message 02\", \"EmptyEnd\"]");
        //    await writer.FlushAsync();
        //    stream.Position = 0;
        //    httpContext.Request.Body = stream;

        //    var res = GetBody(await function.QueuesGetSortedAndDelete(httpContext.Request));
        //    Assert.AreEqual(2, res.Count());
        //    Assert.AreEqual("Message 04", res.First());
        //    Assert.AreEqual("Message 02", res.Last());

        //    //Cleanup
        //    var errors = function.App.Log.GetErrorsAndCriticals();
        //    Assert.AreEqual(0, errors.Count());
        //    var messages = await app.DataLakeQueue.MessagesCountAsync();
        //    Assert.AreEqual(38, messages);
        //    await app.DataLakeQueue.DeleteMessagesAsync();
        //}


        //private static IEnumerable<string> GetBody(IActionResult res)
        //{
        //    var okResult = res as OkObjectResult;
        //    Assert.IsNotNull(okResult);
        //    return (okResult.Value as IEnumerable<QueueResponse>)?.Select(o => o.Body);
        //}

        //[TestMethod]
        //public async Task AddDataToDataLakeQueue()
        //{
        //    var res = await app.DataLakeQueue.AddMessageAsync(JsonConvert.SerializeObject(new { data = "content", date = DateTime.Now }));
        //}

        //[TestMethod]
        //public async Task PurgeDataToDataLakeQueue()
        //{
        //    await app.DataLakeQueue.DeleteMessagesAsync();
        //}
    }
}
