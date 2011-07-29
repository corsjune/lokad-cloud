﻿using System;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Lokad.Cloud.AppHost.Framework;

namespace Lokad.Cloud.Services.Framework.Runner
{
    public class CellRunner : ICellRunner
    {
        public void Run(XElement settings, IDeploymentReader deploymentReader, IApplicationEnvironment environment, CancellationToken cancellationToken)
        {
            // TODO: For now included directly in the settings xml -> move out to separate hash-named blobs (read using deploymentReader)

            var config = Convert.FromBase64String(settings.SettingsValue("Config"));

            // Build IoC container, resolve all cloud services and run them.
            var servicesSettings = settings.SettingsElements("ServiceSettings", "Service").ToLookup(service => service.AttributeValue("type"));
            using (var container = new ServiceContainer(config, environment))
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var runner = new ServiceRunner();
                    runner.Run(
                        container.ResolveServices<UntypedQueuedCloudService>(servicesSettings["QueuedCloudService"]),
                        container.ResolveServices<ScheduledCloudService>(servicesSettings["ScheduledCloudService"]),
                        container.ResolveServices<ScheduledWorkerService>(servicesSettings["ScheduledWorkerService"]),
                        container.ResolveServices<DaemonService>(servicesSettings["DaemonService"]),
                        cancellationToken);
                }
            }
        }

        public void ApplyChangedSettings(XElement newSettings)
        {
            
        }
    }
}
