﻿using System;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace AspNetMvcScaffolder.VisualStudio
{
    internal static class ProjectSettingsExtensions
    {
        public static void SetBool(this IProjectSettings settings, string key, bool value)
        {
            Contract.Assert(settings != null);
            Contract.Assert(key != null);

            settings[key] = value.ToString(CultureInfo.InvariantCulture);
        }

        public static bool TryGetBool(this IProjectSettings settings, string key, out bool value)
        {
            Contract.Assert(settings != null);
            Contract.Assert(key != null);

            string storedValue = settings[key];
            bool parsedValue;
            if (storedValue != null && Boolean.TryParse(storedValue, out parsedValue))
            {
                value = parsedValue;
                return true;
            }
            else
            {
                value = false;
                return false;
            }
        }

        public static bool TryGetString(this IProjectSettings settings, string key, out string value)
        {
            Contract.Assert(settings != null);
            Contract.Assert(key != null);

            value = settings[key];
            return value != null;
        }

        public static bool TryGetDouble(this IProjectSettings settings, string key, out double value)
        {
            Contract.Assert(settings != null);
            Contract.Assert(key != null);

            string storedValue = settings[key];
            double parsedValue;
            if (storedValue != null && Double.TryParse(storedValue, out parsedValue))
            {
                value = parsedValue;
                return true;
            }
            else
            {
                value = default(double);
                return false;
            }
        }
    }
}
