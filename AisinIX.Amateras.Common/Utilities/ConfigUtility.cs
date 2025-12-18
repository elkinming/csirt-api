using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AisinIX.Amateras.Common.Models;

namespace AisinIX.Amateras.Common.Utilities
{
    /// <summary>
    /// 構成設定のユーティリティ
    /// </summary>
    public class ConfigUtility : IConfigUtility
    {
        private readonly ILogger<ConfigUtility> _logger;
        private readonly AppSettings _appSettings;

        private ConfigUtility()
        {
            // 引数無しコンストラクタは無効
        }

        public ConfigUtility(ILogger<ConfigUtility> logger, IOptions<AppSettings> configAccessor)
        {
            if ((logger == null) || (configAccessor == null))
            {
                throw new ArgumentNullException();
            }

            _logger = logger;
            _appSettings = configAccessor.Value;
        }

        /// <summary>
        /// 構成設定から文字列の設定値を取得します。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetStringSetting(string key, string defaultValue = "")
        {
            try
            {
                string value = _appSettings.Configs[key];
                if (value == null)
                {
                    _logger.LogWarning($"構成設定に key=\"{key}\" の設定が見つからない為、既定値 \"{defaultValue}\" を使用します。");

                    return defaultValue;
                }

                return value;
            }
            catch (Exception)
            {
                _logger.LogWarning($"構成設定に key=\"{key}\" の設定が見つからない為、既定値 \"{defaultValue}\" を使用します。");

                return defaultValue;
            }
        }

        /// <summary>
        /// 構成設定から文字列の設定値一覧を取得します。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<string> GetStringListSetting(string key, StringSplitOptions options)
        {
            List<string> list = new List<string>();

            string settingValue = GetStringSetting(key);
            foreach (var value in settingValue.Split(new char[] { ';' }, options))
            {
                list.Add(value);
            }

            return list;
        }
        /// <summary>
        /// 構成設定からInt32の設定値を取得します。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetInt32Setting(string key, int defaultValue)
        {
            try
            {
                string settingValue = GetStringSetting(key);
                int value;
                if (Int32.TryParse(settingValue, out value))
                {
                    return value;
                }
                else
                {
                    _logger.LogWarning($"構成設定の key=\"{key}\" の値が正しい形式でない為、既定値 \"{defaultValue}\" を使用します。");

                    return defaultValue;
                }
            }
            catch (Exception)
            {
                _logger.LogWarning($"構成設定に key=\"{key}\" の設定が見つからない為、既定値 \"{defaultValue}\" を使用します。");

                return defaultValue;
            }
        }
        /// <summary>
        /// 構成設定からDoubleの規定値を取得します。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public double GetDoubleSetting(string key, double defaultValue)
        {
            try
            {
                string settingValue = GetStringSetting(key);
                double value;
                if(double.TryParse(settingValue, out value))
                {
                    return value;
                }
                else
                {
                    _logger.LogWarning($"構成設定に key=\"{key}\" の値が正しい形式でない為、規定値 \"{defaultValue}\" を使用します。");

                    return defaultValue;
                }
            }
            catch(Exception)
            {
                _logger.LogWarning($"構成設定に key=\"{key}\" の設定がいつからない為、規定値 \"{defaultValue}\" を使用します。");

                return defaultValue;
            }
        }
        /// <summary>
        /// 構成設定からBooleanの設定値を取得します。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool GetBooleanSetting(string key, bool defaultValue)
        {
            try
            {
                string settingValue = GetStringSetting(key);
                bool value;
                if (Boolean.TryParse(settingValue, out value))
                {
                    return value;
                }
                else
                {
                    _logger.LogWarning($"構成設定の key=\"{key}\" の値が正しい形式でない為、既定値 \"{defaultValue}\" を使用します。");

                    return defaultValue;
                }
            }
            catch (Exception)
            {
                _logger.LogWarning($"構成設定に key=\"{key}\" の設定が見つからない為、既定値 \"{defaultValue}\" を使用します。");

                return defaultValue;
            }
        }
        /// <summary>
        /// 構成設定からTimeSpanの設定値を取得します。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public TimeSpan GetTimeSpanSetting(string key, TimeSpan defaultValue)
        {
            try
            {
                string settingValue = GetStringSetting(key);
                TimeSpan value;
                if (TimeSpan.TryParse(settingValue, out value))
                {
                    return value;
                }
                else
                {
                    _logger.LogWarning($"構成設定の key=\"{key}\" の値が正しい形式でない為、既定値 \"{defaultValue}\" を使用します。");

                    return defaultValue;
                }
            }
            catch (Exception)
            {
                _logger.LogWarning($"構成設定に key=\"{key}\" の設定が見つからない為、既定値 \"{defaultValue}\" を使用します。");

                return defaultValue;
            }
        }

        /// <summary>
        /// 構成設定からGuidの設定値を取得します。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public Guid GetUniqueIdentifier(string key, Guid defaultValue)
        {
            try
            {
                string settingValue = GetStringSetting(key);
                Guid value;
                if (Guid.TryParse(settingValue, out value))
                {
                    return value;
                }
                else
                {
                    _logger.LogWarning($"構成設定の key=\"{key}\" の値が正しい形式でない為、既定値 \"{defaultValue}\" を使用します。");

                    return defaultValue;
                }
            }
            catch (Exception)
            {
                _logger.LogWarning($"構成設定に key=\"{key}\" の設定が見つからない為、既定値 \"{defaultValue}\" を使用します。");

                return defaultValue;
            }
        }
    }
}
