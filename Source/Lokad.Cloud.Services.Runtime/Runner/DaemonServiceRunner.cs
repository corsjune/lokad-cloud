﻿#region Copyright (c) Lokad 2009-2011
// This code is released under the terms of the new BSD licence.
// URL: http://www.lokad.com/
#endregion

using System.Collections.Generic;
using System.Linq;
using Lokad.Cloud.Services.Framework;

namespace Lokad.Cloud.Services.Runtime.Runner
{
    internal class DaemonServiceRunner : CommonServiceRunner
    {
        public DaemonServiceRunner(IEnumerable<ServiceWithSettings<DaemonService>> services)
            : base(services.Select(s => s.Service))
        {
        }
    }
}
