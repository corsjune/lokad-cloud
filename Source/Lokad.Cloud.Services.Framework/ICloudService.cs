﻿#region Copyright (c) Lokad 2009-2011
// This code is released under the terms of the new BSD licence.
// URL: http://www.lokad.com/
#endregion

using System.Xml.Linq;
using Lokad.Cloud.Services.Framework.Logging;
using Lokad.Cloud.Storage;

namespace Lokad.Cloud.Services.Framework
{
    public interface ICloudService
    {
        CloudServiceType ServiceType { get; }
        void Initialize(XElement userSettings);
        void OnStart();
        void OnStop();

        // Injected by Runtime
        ICloudEnvironment CloudEnvironment { get; set; }
        ILogWriter Log { get; set; }
        IBlobStorageProvider Blobs { get; set; }
        IQueueStorageProvider Queues { get; set; }
        ITableStorageProvider Tables { get; set; }
    }
}