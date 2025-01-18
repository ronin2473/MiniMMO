using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartClient : MonoBehaviour
{
    [SerializeField] private TMP_InputField userNameInput;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Canvas canvas;
    [SerializeField] private string defaultUserName;
    private const string UserNameKey = "Username";

    private void Start()
    {
        if (PlayerPrefs.HasKey(UserNameKey))
        {
            userNameInput.text = PlayerPrefs.GetString(UserNameKey);
        }

        confirmButton.onClick.AddListener(SaveUserName);
    }

    private void SaveUserName()
    {
        string userName = userNameInput.text.Trim();

        if (!string.IsNullOrEmpty(userName))
        {
            PlayerPrefs.SetString(UserNameKey, userName);
            PlayerPrefs.Save();
            canvas.enabled = false;
            ConnectToServer();
        }
        else
        {
            PlayerPrefs.SetString(UserNameKey, defaultUserName);
            PlayerPrefs.Save();
            canvas.enabled = false;
            ConnectToServer();
        }
    }

    public void ConnectToServer()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.StartClient();
    }
}

