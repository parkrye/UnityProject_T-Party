using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneRoomCanvas : SceneUI
{
    [SerializeField] RectTransform playerContent;
    [SerializeField] PlayerEntry playerEntryPrefab;

    [SerializeField] List<PlayerEntry> playerEntryList;
    [SerializeField] GameObject roomPanel, lobbyPanel;

    protected override void Awake()
    {
        base.Awake();
        playerEntryList = new List<PlayerEntry>();
    }

    private void OnEnable()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            PlayerEntry entry = Instantiate(playerEntryPrefab, playerContent);
            //entry.Initailize(player, player.ActorNumber, player.NickName, avatarCameras[avatarDictionary[player]], avatarTextures[avatarDictionary[player]], avatarRoots[avatarDictionary[player]], player.ActorNumber);
            playerEntryList.Add(entry);
            entry.playerNameButton.onClick.AddListener(() => { OnSwitchMasterClient(player); });
        }

        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.LocalPlayer.SetLoad(false);

        AllPlayerReadyCheck();
        PhotonNetwork.AutomaticallySyncScene = true;
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);
    }

    private void OnDisable()
    {
        foreach (PlayerEntry entry in playerEntryList)
        {
            Destroy(entry.gameObject);
        }
        playerEntryList.Clear();

        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public void PlayerEnterRoom(Player newPlayer)
    {
        PlayerEntry entry = Instantiate(playerEntryPrefab, playerContent);
        //entry.Initailize(newPlayer, entryNumQueue.Dequeue(), newPlayer.NickName, avatarCameras[avatarDictionary[newPlayer]], avatarTextures[avatarDictionary[newPlayer]], avatarRoots[avatarDictionary[newPlayer]], newPlayer.ActorNumber);
        entry.playerNameButton.onClick.AddListener(() => { OnSwitchMasterClient(newPlayer); });
        playerEntryList.Add(entry);
        AllPlayerReadyCheck();

        UpdateRoomState();
    }

    public void PlayerLeftRoom(Player leftPlayer)
    {
        for (int i = 0; i < playerEntryList.Count; i++)
        {
            if (playerEntryList[i].player.Equals(leftPlayer))
            {
                Destroy(playerEntryList[i].gameObject);
                playerEntryList.RemoveAt(i);
                break;
            }
        }
        AllPlayerReadyCheck();

        UpdateRoomState();
    }

    public void PlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        for (int i = 0; i < playerEntryList.Count; i++)
        {
            if (playerEntryList[i].player.Equals(targetPlayer))
            {

                break;
            }
        }
        AllPlayerReadyCheck();
    }

    public void MasterClientSwitched(Player newMasterClient)
    {
        foreach (PlayerEntry entry in playerEntryList)
        {
            entry.CheckAmIMaster();
        }
        AllPlayerReadyCheck();
    }

    public void UpdateRoomState()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        buttons["StartButton"].gameObject.SetActive(CheckPlayerReady());
    }

    public bool CheckPlayerReady()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            return CustomProperty.GetReady(player);
        }

        return true;
    }

    void AllPlayerReadyCheck()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            buttons["StartButton"].gameObject.SetActive(false);
            return;
        }

        int readyCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetReady())
                readyCount++;
        }

        if (readyCount == PhotonNetwork.PlayerList.Length)
            buttons["StartButton"].gameObject.SetActive(true);
        else
            buttons["StartButton"].gameObject.SetActive(false);
    }

    public void OnSwitchMasterClient(Player clickedPlayer)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.SetMasterClient(clickedPlayer);
    }

    public void OnStartButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("InGameScene");
    }

    public void OnLeaveRoomClicked()
    {
        PhotonNetwork.LeaveRoom();
    }
}
