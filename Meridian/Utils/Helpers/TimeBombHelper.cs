using Jupiter.Services.Settings;
using System;

namespace Meridian.Utils.Helpers
{
    public class TimeBombHelper
    {
        public static readonly DateTime ExpireDate = new DateTime(2017, 8, 15);

        public static DateTime? StartDate
        {
            get { return SettingsService.Roaming.Get<DateTime?>(); }
            set { SettingsService.Roaming.Set(value); }
        }

        public static void Initialize()
        {
            if (StartDate == null || StartDate.Value == DateTime.MinValue)
                StartDate = DateTime.Now;
        }

        public static bool HasExpired()
        {
            return StartDate?.Date >= ExpireDate;
        }
    }
}