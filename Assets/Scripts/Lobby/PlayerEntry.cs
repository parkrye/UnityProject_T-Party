using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayerEntry : SceneUI
{
    [SerializeField] public Player player;
    [SerializeField] public Button playerNameButton;

    [SerializeField] int avatarNum, avatarColorNum, entryNum;
    [SerializeField] Camera avatarCamera;
    [SerializeField] RenderTexture avatarTexture;
    [SerializeField] RawImage avatarImage;
    [SerializeField] Image crownImage;
    [SerializeField] int myNum;
    public int EntryNum { get { return entryNum; } }

    public void Initailize(Player _player, int id, string name, Camera _avatarCamera, RenderTexture _avatarTexture, GameObject avatarRoot, int _numbering)
    {
        player = _player;
        entryNum = id;
        if (PhotonNetwork.LocalPlayer.ActorNumber != player.ActorNumber)
        {
            buttons["PlayerReadyButton"].gameObject.SetActive(false);
            buttons["LeftAvatarButton"].gameObject.SetActive(false);
            buttons["RightAvatarButton"].gameObject.SetActive(false);
            buttons["LefColorButton"].gameObject.SetActive(false);
            buttons["RightColorButton"].gameObject.SetActive(false);
        }
        texts["PlayerNameText"].text = name;
        texts["ReadyText"].text = "";
        avatarCamera = _avatarCamera;
        avatarTexture = _avatarTexture;
        myNum = _numbering;
        avatarImage.texture = avatarTexture;

        if (CustomProperty.GetReady(player))
        {
            SetPlayerReady(true);
        }

        if (PhotonNetwork.MasterClient.ActorNumber == myNum)
            crownImage.enabled = true;
        else
            crownImage.enabled = false;
    }

    public void SetPlayerReady(bool ready)
    {
        texts["ReadyText"].text = ready ? "Ready" : "";
    }

    public void OnReadyButtonClicked()
    {
        bool isPlayerReady = !CustomProperty.GetReady(player);
        CustomProperty.SetReady(player, isPlayerReady);

        SetPlayerReady(isPlayerReady);
    }

    public void CheckAmIMaster()
    {
        //Debug.LogError($"{myNum}, {PhotonNetwork.LocalPlayer.GetPlayerNumber()}");
        if (PhotonNetwork.MasterClient.ActorNumber == myNum)
            crownImage.enabled = true;
        else
            crownImage.enabled = false;
    }

    void OnF5()
    {
        //Debug.LogError($"{myNum}, {PhotonNetwork.LocalPlayer.GetPlayerNumber()}");
        if (myNum == PhotonNetwork.LocalPlayer.ActorNumber)
            OnReadyButtonClicked();
    }
}