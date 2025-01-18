using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartClient : MonoBehaviour
{
    [SerializeField] private InputField userNameInput;
    [SerializeField] private Button confirmButton;
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
            ConnectToServer();
        }
        else
        {
            PlayerPrefs.SetString(UserNameKey, defaultUserName);
            PlayerPrefs.Save();
            ConnectToServer();
        }
    }

    public void ConnectToServer()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.StartClient();
    }
}

