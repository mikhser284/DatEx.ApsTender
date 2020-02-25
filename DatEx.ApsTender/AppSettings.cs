using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace DatEx.ApsTender
{
    public class AppSettings
    {
        public const String ApplicationId = "DatEx.ApsTender";

        public static AppSettings Load()
        {
            String settingsFilePath = GetSettingsFilePath();
            String dirName = Path.GetDirectoryName(settingsFilePath);
            if(!Directory.Exists(dirName)) Directory.CreateDirectory(dirName);
            AppSettings settings = null;
            if(!File.Exists(settingsFilePath))
            {
                using(StreamWriter stream = File.CreateText(settingsFilePath))
                {
                    settings = ByDefault();
                    JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings() { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore });
                    serializer.Serialize(stream, settings);
                }
            }
            else
            {
                using(StreamReader file = File.OpenText(settingsFilePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    settings = (AppSettings)serializer.Deserialize(file, typeof(AppSettings));
                }
                IfNeedUpdateApsConnectorMD5HashAuthInfo(ref settings);
            }
            return settings;
        }

        private static void IfNeedUpdateApsConnectorMD5HashAuthInfo(ref AppSettings settings)
        {
            if(settings == null || String.IsNullOrEmpty(settings.ApsConnectorLogin) || String.IsNullOrEmpty(settings.ApsConnectorPassword)) return;
            settings.ApsConnectorAuthInfoInBase64String = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{settings.ApsConnectorLogin}:{GetMD5Hash(settings.ApsConnectorPassword)}"));
            //
            settings.ApsConnectorLogin = String.Empty;
            settings.ApsConnectorPassword = String.Empty;
            String settingsFilePath = GetSettingsFilePath();
            if(!File.Exists(settingsFilePath)) throw new FileNotFoundException("App settings file not exist");
            using(StreamWriter stream = File.CreateText(settingsFilePath))
            {
                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings() { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore });
                serializer.Serialize(stream, settings);
            }

            static String GetMD5Hash(String sourceString)
            {
                using(System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                {
                    byte[] inputBytes = Encoding.ASCII.GetBytes(sourceString);
                    byte[] hashBytes = md5.ComputeHash(inputBytes);

                    // Convert the byte array to hexadecimal string
                    StringBuilder sb = new StringBuilder();
                    for(int i = 0; i < hashBytes.Length; i++)
                        sb.Append(hashBytes[i].ToString("X2"));
                    return sb.ToString();
                }
            }
        }

        private static String GetSettingsFilePath()
        {
            String SettingsFileDefaultPath = @$"C:\_AppConfigs\{ApplicationId}\Default\{ApplicationId} - Main.config";
            String sysVarName = $"AppConfigFilePath_{ApplicationId}";
            String settingsFilePath = Environment.GetEnvironmentVariable(sysVarName, EnvironmentVariableTarget.Machine);
            if(String.IsNullOrEmpty(settingsFilePath))
            {
                try
                {
                    Environment.SetEnvironmentVariable(sysVarName, SettingsFileDefaultPath, EnvironmentVariableTarget.Machine);
                    settingsFilePath = Environment.GetEnvironmentVariable(sysVarName, EnvironmentVariableTarget.Machine);
                }
                catch(Exception) { }
            }
            if(String.IsNullOrEmpty(settingsFilePath))
                settingsFilePath = Environment.GetEnvironmentVariable(sysVarName, EnvironmentVariableTarget.User);
            if(String.IsNullOrEmpty(settingsFilePath))
            {
                Environment.SetEnvironmentVariable(sysVarName, SettingsFileDefaultPath, EnvironmentVariableTarget.User);
                settingsFilePath = Environment.GetEnvironmentVariable(sysVarName, EnvironmentVariableTarget.User);
            }
            if(String.IsNullOrEmpty(settingsFilePath)) throw new KeyNotFoundException($"System variable \"{sysVarName}\" not found");
            return settingsFilePath;
        }

        private AppSettings() { }

        public static AppSettings ByDefault()
        {
            AppSettings defaultSettings = new AppSettings();
            defaultSettings.HttpAddressOf = Settings_HttpAddress.ByDefault();
            defaultSettings.ApsConnectorLogin = "UserLogin";
            defaultSettings.ApsConnectorPassword = "UserPassword";
            defaultSettings.ApsConnectorAuthInfoInBase64String = "";
            return defaultSettings;
        }

        [JsonProperty(nameof(HttpAddressOf))]
        public Settings_HttpAddress HttpAddressOf { get; set; }

        [JsonProperty(nameof(ApsConnectorLogin))]
        private String ApsConnectorLogin { get; set; }

        [JsonProperty(nameof(ApsConnectorPassword))]
        private String ApsConnectorPassword { get; set; }

        [JsonProperty(nameof(ApsConnectorAuthInfoInBase64String))]
        public String ApsConnectorAuthInfoInBase64String { get; private set; }
    }

    public class Settings_HttpAddress
    {
        [JsonProperty(nameof(ApsRestService))]
        public String ApsRestService { get; private set; }

        [JsonProperty(nameof(ApsSoapService))]
        public String ApsSoapService { get; private set; }

        public static Settings_HttpAddress ByDefault()
        {
            Settings_HttpAddress defaultSettings = new Settings_HttpAddress();
            defaultSettings.ApsRestService = "https://ApsRestServiceBaseAddress.com:80/rest/rest.dll/";
            defaultSettings.ApsSoapService = "https://ApsRestServiceBaseAddress.com:80/ws/remote.dll/";
            return defaultSettings;
        }

        private Settings_HttpAddress() { }
    }
}
