using Photon.Pun;
using UnityEngine;

public class LoginPanel : SceneUI
{
    static string playerID = null;
    [SerializeField] GameObject LobbyPanel;

    protected override void Awake()
    {
        base.Awake();
    }

    public void OnLoginButtonClicked()
    {
        playerID = inputFields["NameInputField"].text;

        if (playerID == "")
        {
            GameData.PLAYER_NAME = $"Student{Random.Range(1000, 5000)}";
        }
        else
        {
            GameData.PLAYER_NAME = playerID;
        }

        ExitGames.Client.Photon.Hashtable props = new()
        {
            { GameData.PLAYER_READY, false },
            { GameData.PLAYER_LOAD, false },
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        inputFields["NameInputField"].text = "";
        PhotonNetwork.LocalPlayer.NickName = playerID;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    void OnEnter()
    {
        OnLoginButtonClicked();
    }
}