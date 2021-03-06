﻿using Xunit;

namespace Bootstrap.Admin.Controllers.SqlServer
{
    public class HealthCheck : ControllerTest
    {
        public HealthCheck(BAWebHost factory) : base(factory, "") { }

        [Fact]
        public async void View_Ok()
        {
            var content = await Client.GetStringAsync("/Healths");
            Assert.Contains("TotalDuration", content);
        }

        [Fact]
        public async void UI_Ok()
        {
            var content = await Client.GetStringAsync("/Healths-ui");
            Assert.Contains("健康检查", content);
        }
    }
}
