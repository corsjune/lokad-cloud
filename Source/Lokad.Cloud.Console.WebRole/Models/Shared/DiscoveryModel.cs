﻿#region Copyright (c) Lokad 2009-2011
// This code is released under the terms of the new BSD licence.
// URL: http://www.lokad.com/
#endregion

namespace Lokad.Cloud.Console.WebRole.Models.Shared
{
    public class DiscoveryModel
    {
        public bool IsAvailable { get; set; }

        public bool ShowLastDiscoveryUpdate { get; set; }
        public string LastDiscoveryUpdate { get; set; }
    }
}