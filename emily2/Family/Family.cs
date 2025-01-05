using emily2.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emily2.Family
{
    internal class Family(ApplicationOptions appSettings, ILogger<Family> logger)
    {
        private ApplicationOptions _appSettings = appSettings;
        private readonly ILogger<Family> _logger = logger;
    }
}
