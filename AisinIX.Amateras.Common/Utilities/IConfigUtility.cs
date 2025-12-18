using System;
using System.Collections.Generic;

namespace AisinIX.Amateras.Common.Utilities
{
    public interface IConfigUtility
    {
        string GetStringSetting(string key, string defaultValue = "");
        List<string> GetStringListSetting(string key, StringSplitOptions options);
        int GetInt32Setting(string key, int defaultValue);
        double GetDoubleSetting(string key, double defaultValue);
        bool GetBooleanSetting(string key, bool defaultValue);
        TimeSpan GetTimeSpanSetting(string key, TimeSpan defaultValue);
        Guid GetUniqueIdentifier(string key, Guid defaultValue);

    }
}