using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbySceneLobbyCanvas : SceneUI
{
    [SerializeField] GameObject lobbyPanel, roomPanel;

    protected override void Awake()
    {
        base.Awake();
    }

    void OnEnable()
    {
        PhotonNetwork.JoinLobby();
    }

    public void OnCreateRoomButtonClicked()
    {
        roomPanel.gameObject.SetActive(true);
        lobbyPanel.gameObject.SetActive(false);
    }

    public void OnLeaveLobbyClicked()
    {
        GameManager.Scene.LoadScene("StartScene");
    }

    public void OnRandomMatchingButtonClicked()
    {
        string name = string.Format("Room{0}", Random.Range(1000, 10000));
        RoomOptions roomOptions = new()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 8
        };
        PhotonNetwork.JoinRandomOrCreateRoom(roomName: name, roomOptions: roomOptions, expectedCustomRoomProperties: roomOptions.CustomRoomProperties);
    }
}
