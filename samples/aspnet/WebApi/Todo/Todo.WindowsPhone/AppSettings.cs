using Account.Client;
using System;
using System.IO.IsolatedStorage;

namespace Todo.WindowsPhone
{
    public class AppSettings : IAccessTokenStore
    {
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        // Setting keys
        const string AccessTokenKey = "TodoAccessToken";
        const string LoginProviderKey = "TodoLoginProvider";
        const string AuthorizationRequestUriKey = "TodoAuthorizationRequestUri";

        public void AddOrUpdateValue(string key, Object value)
        {
            if (settings.Contains(key))
            {
                if (settings[key] != value)
                {
                    settings[key] = value;
                }
            }
            else
            {
                settings.Add(key, value);
            }
            Save();
        }

        public T GetValueOrDefault<T>(string key)
        {
            return settings.Contains(key) ? (T)settings[key] : default(T);
        }

        public void Save()
        {
            settings.Save();
        }

        public string AccessToken
        {
            get
            {
                return GetValueOrDefault<string>(AccessTokenKey);
            }
            set
            {
                AddOrUpdateValue(AccessTokenKey, value);
            }
        }
    }
}
