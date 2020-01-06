using Microsoft.AspNet.Scaffolding;
using System;
using System.Collections.Generic;

namespace AspNetMvcScaffolder.Telemetry
{
    internal static class CodeGenerationContextExtensions
    {
        public static void AddTelemetryData(this CodeGenerationContext context, string key, object value)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            Dictionary<string, object> mvcTelemetryItems;

            if (!context.Items.TryGetProperty<Dictionary<string, object>>(TelemetrySharedKeys.MvcTelemetryItems, out mvcTelemetryItems))
            {
                mvcTelemetryItems = new Dictionary<string, object>();
                context.Items.AddProperty(TelemetrySharedKeys.MvcTelemetryItems, mvcTelemetryItems);
            }

            mvcTelemetryItems[key] = value;
        }
    }
}
