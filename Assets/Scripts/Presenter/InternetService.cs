using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class InternetService : MonoBehaviour
{
    public GameObject[] servicesToEnable;
    public GameObject noInternetPanel;
    public GameObject loadingPanel;
    private bool firstOpen;
    public bool isChecking;
    public bool isConnected;
    void Start()
    {
        firstOpen = true;

        try
        {
            if (File.Exists(Path.Combine(Application.persistentDataPath, "Files", "places.json")))
            {
                loadingPanel.SetActive(false);
                EnableServices();
                return;
            }
        }
        catch
        {
            Debug.Log("Первый запуск");
        }
        
        StartCoroutine(CheckInternetConnection((hasInternet) =>
        {
            if (hasInternet)
                EnableServices();
            else
            {
                noInternetPanel.SetActive(true);
                noInternetPanel.GetComponent<NoInternetPanelController>().InitializePanel();
            }
        }));
    }
    public IEnumerator CheckInternetConnection(Action<bool> result)
    {
        isChecking = true;
        if (firstOpen)
        {
            yield return new WaitForSeconds(2);
            firstOpen = false;
        }

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            loadingPanel.SetActive(false);
            result?.Invoke(false);
            isChecking = false;
            yield break;
        }

        using (UnityWebRequest request = UnityWebRequest.Get("https://clients3.google.com/generate_204"))
        {
            request.timeout = 5;
            yield return request.SendWebRequest();

            bool hasInternet =
                request.result == UnityWebRequest.Result.Success &&
                request.responseCode == 204;

            loadingPanel.SetActive(false);
            result?.Invoke(hasInternet);
        }

        isChecking = false;
    }
    private void EnableServices()
    {
        foreach (var service in servicesToEnable)
            service.SetActive(true);
        noInternetPanel.SetActive(false);
    }
    public void CheckInternet()
    {
        if (!isChecking)
            StartCoroutine(CheckInternetConnection((hasInternet) =>
            {
                if (hasInternet)
                    EnableServices();
                else
                {
                    noInternetPanel.SetActive(true);
                    noInternetPanel.GetComponent<NoInternetPanelController>().InitializePanel();
                }
            }));
    }
}
