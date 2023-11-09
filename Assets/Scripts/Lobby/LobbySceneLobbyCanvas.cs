using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbySceneLobbyCanvas : SceneUI
{    protected override void Awake()
    {
        base.Awake();

        texts["NameText"].text = GameManager.Data.playerName;
        buttons["JoinButton"].onClick.AddListener(OnRandomMatchingButtonTouched);
        buttons["BackButton"].onClick.AddListener(OnLeaveLobbyButtonTouched);
        buttons["UpAvatarButton"].onClick.AddListener(OnUpAvatarButtonTouched);
        buttons["DownAvatarButton"].onClick.AddListener(OnDownAvatarButtonTouched);
    }

    void OnEnable()
    {
        PhotonNetwork.JoinLobby();
    }

    void OnLeaveLobbyButtonTouched()
    {
        GameManager.Scene.LoadScene("StartScene");
    }

    void OnRandomMatchingButtonTouched()
    {
        RoomOptions roomOptions = new()
        {
            IsOpen = true,
            MaxPlayers = 8
        };
        PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: roomOptions);
    }

    void OnUpAvatarButtonTouched()
    {
        GameManager.Data.playerAvatar--;
        if (GameManager.Data.playerAvatar < 0)
            GameManager.Data.playerAvatar = 4;
        ChangeAvatarImage();
    }

    void OnDownAvatarButtonTouched()
    {
        GameManager.Data.playerAvatar++;
        if (GameManager.Data.playerAvatar > 4)
            GameManager.Data.playerAvatar = 0;
        ChangeAvatarImage();
    }

    void ChangeAvatarImage()
    {
        images["AvatarImage"].sprite = GameManager.Data.AVATAR[GameManager.Data.playerAvatar];
        PhotonNetwork.LocalPlayer.SetAvatar(GameManager.Data.playerAvatar);
    }
}
