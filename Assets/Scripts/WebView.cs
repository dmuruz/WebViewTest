using UnityEngine;

public class WebView : MonoBehaviour
{
    [SerializeField] private GameObject inrernetConnectionWindow;
    [SerializeField] private GameObject serviceWindow;
    [SerializeField] private UniWebView webView;
    private string url = "";
    private bool isStarted = false;

    public void Start()
    {
        if (PlayerPrefs.HasKey("url"))
        {
            url = PlayerPrefs.GetString("url", "");
        }
        bool isLinkSaved = url != "";
        bool hasInternetConnection = Application.internetReachability != NetworkReachability.NotReachable;
        if (isLinkSaved)
        {
            if (!hasInternetConnection)
            {

                ShowInternetWindow();
            }
            else
            {
                OpenWebView(url);
            }
        }
        else
        {
            if (!hasInternetConnection)
            {
                ShowInternetWindow();
            }
            else
            {
                try
                {
                    FirebaseRemoteConfigManager.Instance.FetchDataAsync();
                }
                catch
                {
                    ShowInternetWindow();
                }
            }
        }
    }

    private void SetUrl()
    {
        url = FirebaseRemoteConfigManager.Instance.GetUrl();
        if (SystemInfo.deviceModel.ToLower().Contains("google") ||
                SystemInfo.deviceName.ToLower().Contains("google") || url == "")
        {
            OpenService();
        }
        else
        {
            PlayerPrefs.SetString("url", url);
            PlayerPrefs.Save();
            OpenWebView(url);
        }
    }
    private void Update()
    {
        if (!isStarted && FirebaseRemoteConfigManager.Instance.isFetched)
        {
            SetUrl();
            isStarted = true;
        }
        webView.OnOrientationChanged += (view, orientation) =>
        {
            webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
        };
        webView.OnShouldClose += (view) =>
        {
            return false;
        };
    }

    public void ShowInternetWindow()
    {
        inrernetConnectionWindow.SetActive(true);
    }

    private void OpenWebView(string url)
    {
        webView.gameObject.SetActive(true);
        webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
        webView.Load(url);
        webView.Show();
    }

    private void OpenService()
    {
        serviceWindow.SetActive(true);
    }
}