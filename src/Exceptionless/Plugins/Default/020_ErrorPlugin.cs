﻿#if !PORTABLE && !NETSTANDARD1_2
using System;
using Exceptionless.Extensions;
using Exceptionless.Models;

namespace Exceptionless.Plugins.Default {
    [Priority(20)]
    public class ErrorPlugin : IEventPlugin {
        public void Run(EventPluginContext context) {
            var exception = context.ContextData.GetException();
            if (exception == null)
                return;

            if (exception.IsProcessed()) {
                context.Cancel = true;
                return;
            }
            
            context.Event.Type = Event.KnownTypes.Error;
            context.Event.Data[Event.KnownDataKeys.Error] = exception.ToErrorModel(context.Client);
            exception.MarkProcessed();
        }
    }
}
#endif