using Microsoft.ApplicationInsights;
using Microsoft.Win32;
using System;
using System.Globalization;

namespace AspNetMvcScaffolder.Telemetry
{
    internal class ScaffoldingTelemetry
    {
        private TelemetryClient telemetryClient;

        public static bool IsParticipateVsExperienceImprovementProgram = GetIsParticipateVsExperienceImprovementProgram();

        public ScaffoldingTelemetry()
        {
            telemetryClient = new TelemetryClient();
            telemetryClient.InstrumentationKey = "0eac5cec-f2bf-467a-8682-a121ffe68a97"; 
            telemetryClient.Context.Session.Id = Guid.NewGuid().ToString();
            telemetryClient.Context.Device.OperatingSystem = Environment.OSVersion.ToString();
        }

        public void TrackEvent(string eventName)
        {
            if (telemetryClient != null && IsParticipateVsExperienceImprovementProgram == true)
            {
                telemetryClient.TrackEvent(eventName);
            }
        }

        public void TrackException(Exception exception)
        {
            if (telemetryClient != null && IsParticipateVsExperienceImprovementProgram == true)
            {
                telemetryClient.TrackException(exception);
            }
        }

        public void Flush()
        {
            if (telemetryClient != null && IsParticipateVsExperienceImprovementProgram == true)
            {
                telemetryClient.Flush();
            }
        }

        private static bool GetIsParticipateVsExperienceImprovementProgram()
        {
            string path = Environment.Is64BitOperatingSystem? "SOFTWARE\\Wow6432Node\\Microsoft\\VSCommon\\12.0\\SQM": "SOFTWARE\\Microsoft\\VSCommon\\12.0\\SQM";

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path))
            {
                if (key != null)
                {
                    Object o = key.GetValue("Optin");
                    if (o != null)
                    {
                        bool result = Convert.ToBoolean(o, CultureInfo.InvariantCulture);
                        return result;
                    }
                }
            }
            return false;
        }
    }
}
