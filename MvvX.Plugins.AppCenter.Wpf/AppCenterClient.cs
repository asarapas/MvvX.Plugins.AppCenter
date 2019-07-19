﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace MvvX.Plugins.AppCenter.Wpf
{
    public class AppCenterClient : IAppCenterClient
    {
        public void Configure(string identifier, 
                                string version, 
                                bool activateTelemetry, 
                                bool activateMetrics, 
                                bool activateCrashReports,
                                string automaticAttachedFilePathOnCrash)
        {
            Microsoft.AppCenter.AppCenter.Start(identifier, typeof(Analytics), typeof(Crashes));

            Analytics.SetEnabledAsync(activateTelemetry || activateMetrics);

            Crashes.SetEnabledAsync(activateCrashReports);
            if (activateCrashReports && !string.IsNullOrWhiteSpace(automaticAttachedFilePathOnCrash))
            {
                Crashes.GetErrorAttachments = (ErrorReport report) =>
                {
                        // Your code goes here.
                        return new ErrorAttachmentLog[]
                    {
                            ErrorAttachmentLog.AttachmentWithBinary(File.ReadAllBytes(automaticAttachedFilePathOnCrash), "logfile.log", "image/jpeg")
                    };
                };
            }
        }

        public void TrackEvent(string eventName)
        {
            Analytics.TrackEvent(eventName);
        }

        public void TrackEvent(string eventName, IDictionary<string, string> properties)
        {
            Analytics.TrackEvent(eventName, properties);
        }

        public void TrackException(Exception ex, IDictionary<string, string> properties)
        {
            Crashes.TrackError(ex, properties);
        }
    }
}