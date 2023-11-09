using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbySceneRoomCanvas : SceneUI
{
    [SerializeField] PlayerEntry[] playerEntryList;

    protected override void Awake()
    {
        base.Awake();

        buttons["ReadyButton"].onClick.AddListener(OnReadyButtonTouched);
        buttons["QuitButton"].onClick.AddListener(OnLeaveRoomTouched);

        playerEntryList = GetComponentsInChildren<PlayerEntry>();
    }

    void OnEnable()
    {
        ResetAllEntries();

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerEntryList[i].Initialize(PhotonNetwork.PlayerList[i]);
        }

        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.LocalPlayer.SetLoad(false);

        AllPlayerReadyCheck();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void OnDisable()
    {
        ResetAllEntries();

        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public void PlayerEnterRoom(Player newPlayer)
    {
        for (int i = 0; i < 8; i++)
        {
            if (playerEntryList[i].isUsing)
                continue;

            playerEntryList[i].Initialize(newPlayer);
        }

        AllPlayerReadyCheck();

        UpdateRoomState();
    }

    public void PlayerLeftRoom(Player leftPlayer)
    {
        for (int i = 0; i < 8; i++)
        {
            if (!playerEntryList[i].isUsing)
                continue;

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
        for (int i = 0; i < 8; i++)
        {
            if (!playerEntryList[i].isUsing)
                continue;

            if (playerEntryList[i].player.Equals(targetPlayer))
            {
                playerEntryList[i].SetPlayerReady(targetPlayer.GetReady());
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

    }

    void AllPlayerReadyCheck()
    {
        int readyCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetReady())
                readyCount++;
            else
                break;
        }

        if (readyCount < PhotonNetwork.PlayerList.Length || readyCount == 0)
            return;

        PhotonNetwork.LoadLevel("MainScene");
    }

    public void OnSwitchMasterClient(Player clickedPlayer)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.SetMasterClient(clickedPlayer);
    }

    public void OnReadyButtonTouched()
    {
        for (int i = 0; i < 8; i++)
        {
            if (!playerEntryList[i].isUsing)
                continue;

            if (playerEntryList[i].player.Equals(PhotonNetwork.LocalPlayer))
            {
                playerEntryList[i].Ready();
                break;
            }
        }
    }

    public void OnLeaveRoomTouched()
    {
        PhotonNetwork.LeaveRoom();
    }

    void ResetAllEntries()
    {
        foreach (PlayerEntry entry in playerEntryList)
        {
            entry.ResetEntry();
        }
    }
}
