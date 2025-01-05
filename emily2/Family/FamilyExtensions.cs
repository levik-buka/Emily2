﻿using emily2.Logger;
using emily2.Options;
using emily2.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emily2.Family
{
    internal static class FamilyExtensions
    {
        internal static IFamilyRepository CreateFamilyRepository(this ApplicationOptions appSettings, ILoggerFactory logFactory)
        {
            ArgumentNullException.ThrowIfNull(appSettings);

            // here can be created any FamilyRepository based on application options
            // but currently only drive repository is supported
            return new DriveRepository(appSettings, logFactory.CreateLogger<DriveRepository>());
        }
    }
}
