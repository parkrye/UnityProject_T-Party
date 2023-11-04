using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbySceneRoomCanvas : SceneUI
{
    [SerializeField] PlayerEntry[] playerEntryList;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        int index = 0;
        for(index = 0; index < PhotonNetwork.PlayerList.Length; index++)
        {
            playerEntryList[index].Initialize(PhotonNetwork.PlayerList[index]);
        }

        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.LocalPlayer.SetLoad(false);

        AllPlayerReadyCheck();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void OnDisable()
    {
        foreach (PlayerEntry entry in playerEntryList)
        {
            entry.ResetEntry();
        }

        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public void PlayerEnterRoom(Player newPlayer)
    {
        int index = PhotonNetwork.PlayerList.Length - 1;
        playerEntryList[index].Initialize(PhotonNetwork.PlayerList[index]);

        AllPlayerReadyCheck();

        UpdateRoomState();
    }

    public void PlayerLeftRoom(Player leftPlayer)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (playerEntryList[i].player.Equals(leftPlayer))
            {
                playerEntryList[i].ResetEntry();
                break;
            }
        }
        AllPlayerReadyCheck();

        UpdateRoomState();
    }

    public void PlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
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
        AllPlayerReadyCheck();
    }

    public void UpdateRoomState()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        buttons["ReadyButton"].gameObject.SetActive(CheckPlayerReady());
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
            buttons["ReadyButton"].gameObject.SetActive(false);
            return;
        }

        int readyCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetReady())
                readyCount++;
        }

        if (readyCount == PhotonNetwork.PlayerList.Length)
            buttons["ReadyButton"].gameObject.SetActive(true);
        else
            buttons["ReadyButton"].gameObject.SetActive(false);
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

        //PhotonNetwork.LoadLevel("InGameScene");
    }

    public void OnLeaveRoomClicked()
    {
        PhotonNetwork.LeaveRoom();
    }
}
