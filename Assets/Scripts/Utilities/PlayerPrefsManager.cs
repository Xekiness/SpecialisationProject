using UnityEngine;

public static class PlayerPrefsManager
{
    public static void Save(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }
    //public static float Load(string key)
    //{
    //    return PlayerPrefs.GetFloat(key);
    //}
    public static float Load(string key, float defaultValue = 100f)
    {
        return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetFloat(key) : defaultValue;
    }
}
