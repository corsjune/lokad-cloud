﻿#region Copyright (c) Lokad 2009-2011
// This code is released under the terms of the new BSD licence.
// URL: http://www.lokad.com/
#endregion

using System;

namespace Lokad.Cloud.Services.Framework.Logging
{
    public class NullLogWriter : ILogWriter
    {
        public void Log(LogLevel level, Exception ex, object message)
        {
            //do nothing
        }

        public void Log(LogLevel level, object message)
        {
            Log(level, null, message);
        }
    }
}
