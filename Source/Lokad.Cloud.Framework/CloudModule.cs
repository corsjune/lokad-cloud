﻿#region Copyright (c) Lokad 2009-2011
// This code is released under the terms of the new BSD licence.
// URL: http://www.lokad.com/
#endregion

using Autofac;
using Lokad.Cloud.Diagnostics;
using Lokad.Cloud.Provisioning.Instrumentation;
using Lokad.Cloud.Storage;
using Lokad.Cloud.Storage.Azure;

namespace Lokad.Cloud
{
    /// <summary>
    /// IoC module that registers all usually required components, including
    /// storage providers, management and provisioning and diagnostics/logging.
    /// It is recommended to load this module even when only using the storage (O/C mapping) providers.
    /// Expects a <see cref="CloudConfigurationSettings"/> instance to be registered as well.
    /// </summary>
    /// <remarks>
    /// When only using the storage (O/C mapping) toolkit standalone it is easier
    /// to let the <see cref="CloudStorage"/> factory create the storage providers on demand.
    /// </remarks>
    /// <seealso cref="CloudConfigurationSettings"/>
    /// <seealso cref="CloudStorage"/>
    public sealed class CloudModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new StorageModule());
            builder.RegisterModule(new DiagnosticsModule());

            builder.Register(
                c => new EnvironmentAdapter(
                    c.Resolve<CloudConfigurationSettings>(),
                    c.Resolve<ILog>(),
                    c.ResolveOptional<IProvisioningObserver>()))
                .As<IEnvironment>().SingleInstance();

            builder.RegisterType<Jobs.JobManager>();
        }
    }
}
