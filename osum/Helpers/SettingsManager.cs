using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace osum.Helpers
{
    public static class SettingsManager
    {
        private static Dictionary<string, object> settings = new Dictionary<string, object>();

        public static void SaveSetting<T>(string key, T value)
        {
            if (settings.ContainsKey(key))
                settings[key] = value;
            else
                settings.Add(key, value);

            // im gay and i kiss boys (  i need to add a way to save settings on restart)
        }

        public static T GetSetting<T>(string key)
        {
            if (settings.ContainsKey(key))
                return (T)settings[key];

            return default(T);
        }
    }
}
