using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoInternetPanelController : MonoBehaviour
{
    public GameObject cancelButton;
    public InternetService internetService;
    public MenuUIController menuUIController;
    public TMP_Text description;
    public void InitializePanel(bool isFirstPlay = true)
    {
        if (isFirstPlay)
        {
            description.text = "Для первого запуска приложения " +
                "необходимо загрузить карту и заведения.\r\n" +
                "Пожалуйста, подключитесь к сети";
            cancelButton.SetActive(false);
        }
        else
        {
            description.text = "Для поиска заведений необходимо подключение к сети.";
            cancelButton.SetActive(true);
        }
    }
    public void OnCancelClick()
    {
        gameObject.SetActive(false);
    }
    public void OnRetryClick()
    {
        if (!internetService.isChecking)
            StartCoroutine(internetService.CheckInternetConnection((hasInternet) =>
            {
                if (hasInternet)
                {
                    gameObject.SetActive(false);
                    menuUIController.ExpandSearchArea();
                }
            }));
    }
}
