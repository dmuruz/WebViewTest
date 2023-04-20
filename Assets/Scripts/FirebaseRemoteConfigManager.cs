using System;
using Firebase;
using Firebase.RemoteConfig;
using UnityEngine;

public class FirebaseRemoteConfigManager : MonoBehaviour
{
    private static FirebaseRemoteConfigManager instance;
    private FirebaseRemoteConfig firebaseRemoteConfig;
    private string url;

    public bool isFetched = false;

    public string GetUrl()
    {
        return url;
    }
    public static FirebaseRemoteConfigManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FirebaseRemoteConfigManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("FirebaseRemoteConfigManager");
                    instance = obj.AddComponent<FirebaseRemoteConfigManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            if (app == null)
            {
                FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
                {
                    FirebaseApp app = FirebaseApp.DefaultInstance;
                    if (app == null)
                    {
                        app = FirebaseApp.Create();
                    }
                    firebaseRemoteConfig = FirebaseRemoteConfig.DefaultInstance;
                    FetchRemoteConfig();
                });
            }
            else
            {
                firebaseRemoteConfig = FirebaseRemoteConfig.DefaultInstance;
                FetchRemoteConfig();
            }
        });
    }

    private void FetchRemoteConfig()
    {
        firebaseRemoteConfig.FetchAndActivateAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("FetchRemoteConfig canceled.");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("FetchRemoteConfig encountered an error: " + task.Exception);
            }
            else
            {
                Debug.Log("FetchRemoteConfig completed successfully.");
                url = firebaseRemoteConfig.GetValue("url").StringValue;
                isFetched = true;
            }
        });
    }

}
