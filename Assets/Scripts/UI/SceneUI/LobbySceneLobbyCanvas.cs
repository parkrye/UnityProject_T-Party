using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbySceneLobbyCanvas : SceneUI
{    protected override void Awake()
    {
        base.Awake();

        texts["NameText"].text = GameData.PLAYER_NAME;
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
        GameData.PLAYER_AVATAR_NUM--;
        if (GameData.PLAYER_AVATAR_NUM < 0)
            GameData.PLAYER_AVATAR_NUM = 4;
        ChangeAvatarImage();
    }

    void OnDownAvatarButtonTouched()
    {
        GameData.PLAYER_AVATAR_NUM++;
        if (GameData.PLAYER_AVATAR_NUM > 4)
            GameData.PLAYER_AVATAR_NUM = 0;
        ChangeAvatarImage();
    }

    void ChangeAvatarImage()
    {
        images["AvatarImage"].sprite = GameData.AVATAR[GameData.PLAYER_AVATAR_NUM];
        PhotonNetwork.LocalPlayer.SetAvatar(GameData.PLAYER_AVATAR_NUM);
    }
}
