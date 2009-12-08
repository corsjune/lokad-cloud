﻿#region Copyright (c) Lokad 2009
// This code is released under the terms of the new BSD licence.
// URL: http://www.lokad.com/
#endregion

using Autofac.Builder;
using Autofac.Configuration;
using Lokad.Cloud.Diagnostics;

namespace Lokad.Cloud.Samples.MapReduce
{
	public static class Setup
	{
		public static Autofac.IContainer Container { get; private set; }

		static Setup()
		{
			Container = SetupContainer();
		}

		private static Autofac.IContainer SetupContainer()
		{
			var builder = new ContainerBuilder();
			builder.RegisterModule(new ConfigurationSettingsReader("autofac"));

			// Diagnostics
			builder.RegisterModule(new DiagnosticsModule());

			return builder.Build();
		}

	}
}
