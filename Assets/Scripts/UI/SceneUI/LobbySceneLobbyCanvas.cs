using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneLobbyCanvas : SceneUI
{
    [SerializeField] RoomEntry roomEntryPrefab;
    [SerializeField] RectTransform roomContent;
    [SerializeField] List<RoomEntry> roomEntries;

    [SerializeField] GameObject lobbyPanel;
    [SerializeField] GameObject createRoomPanel;

    [SerializeField] EnterPrivateRoomPanel enterPrivateRoomPanel;

    protected override void Awake()
    {
        base.Awake();
        roomEntries = new List<RoomEntry>();
    }

    void OnEnable()
    {
        PhotonNetwork.JoinLobby();
    }

    public void OnCreateRoomButtonClicked()
    {
        createRoomPanel.gameObject.SetActive(true);
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
            MaxPlayers = 8,
            CustomRoomProperties = new()
            {
                { GameData.ROOMTYPE, GameData.PUBLIC },
            },
            CustomRoomPropertiesForLobby = new[]
            {
                GameData.ROOMTYPE,
            }
        };
        PhotonNetwork.JoinRandomOrCreateRoom(roomName: name, roomOptions: roomOptions, expectedCustomRoomProperties: roomOptions.CustomRoomProperties);
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        ClearRoomList();

        foreach (RoomInfo room in roomList)
        {
            RoomEntry entry = Instantiate(roomEntryPrefab, roomContent);

            if (room.RemovedFromList || !room.IsOpen)
                continue;
            if (entry.Equals(null))
                continue;
            entry.Initialize(room.Name, room.PlayerCount, (byte)room.MaxPlayers, room, enterPrivateRoomPanel);
            roomEntries.Add(entry);
        }
    }

    void ClearRoomList()
    {
        foreach (RoomEntry room in roomEntries)
        {
            Destroy(room.gameObject);
        }
        roomEntries.Clear();
    }
}
