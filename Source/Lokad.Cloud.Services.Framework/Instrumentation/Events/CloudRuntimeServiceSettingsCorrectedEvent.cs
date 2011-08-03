﻿#region Copyright (c) Lokad 2011
// This code is released under the terms of the new BSD licence.
// URL: http://www.lokad.com/
#endregion

namespace Lokad.Cloud.Services.Framework.Instrumentation.Events
{
    /// <summary>
    /// Raised whenever the runtime itself updated the service settings to apply local corrections.
    /// </summary>
    public class CloudRuntimeServiceSettingsCorrectedEvent : ICloudRuntimeEvent
    {
        public CloudRuntimeServiceSettingsCorrectedEvent()
        {
        }
    }
}