using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Device.Gpio;
using Blazor.Pi.Shared;

namespace Blazor.Pi.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LEDController : ControllerBase
    {
        private readonly int ledPin = 17;
        private readonly ILogger<LEDController> _logger;
        private readonly GpioController _controller;
        
        public LEDController(
            ILogger<LEDController> logger,
            GpioController controller
            )
        {
            _controller = controller;
            _logger = logger;
            
            if (!controller.IsPinOpen(ledPin))
            {
                controller.OpenPin(ledPin, PinMode.Output);
            }
        }

        [HttpPost]
        public void Post([FromBody] bool toggle)
        {            
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