using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebView : MonoBehaviour
{
    void Start()
    {
        var webView = gameObject.AddComponent<UniWebView>();
        webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
        bool isLinkSaved = PlayerPrefs.GetString("url", "") != "";
        bool isInternetDisabled = Application.internetReachability == NetworkReachability.NotReachable;
        if(isLinkSaved){
            if(isInternetDisabled){
                
            }

        }

        webView.Load("https://uniwebview.com");
        webView.Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
