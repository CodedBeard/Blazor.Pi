using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Blazor.Pi.Server.Hubs;
using System;
using System.Device.Gpio;
using System.Linq;

namespace Blazor.Pi.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LEDController : ControllerBase
    {
        private readonly int ledPin = 17;
        private readonly ILogger<LEDController> logger;
        private readonly IHubContext<GPIOHub> _hubContext;
        private readonly GpioController _controller;

        public LEDController(
            ILogger<LEDController> logger,
            GpioController controller,
            IHubContext<GPIOHub> hubContext
            )
        {
            _hubContext = hubContext;
            this.logger = logger;

            _controller = controller;
            if (!controller.IsPinOpen(ledPin))
            {
                controller.OpenPin(ledPin, PinMode.Output);
            }
        }

        [HttpPost]
        public void Post([FromBody] bool toggle)
        {
            _hubContext.Clients.All.SendAsync("ReceiveSwitchStatus", toggle);
            if (toggle)
            {
                _controller.Write(ledPin, PinValue.High);
            }
            else
            {
                _controller.Write(ledPin, PinValue.Low);
            }
        }
    }
}