using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPunManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject morningCanvas, eveningCanvas, nightCanvas;

    MainChat chat;

    enum Time { Morning, Evening, Night}
    Time time;

    Dictionary<int, bool> aliveDictionary;
    List<int> normalStudents;
    List<int> spyStudents;

    void Awake()
    {
        aliveDictionary = new Dictionary<int, bool>();
        normalStudents = new List<int>();
        spyStudents = new List<int>();

        chat = morningCanvas.GetComponent<MainChat>();

        PhotonNetwork.LocalPlayer.SetLoad(true);

        if (!PhotonNetwork.IsMasterClient)
            return;

        StartCoroutine(GameSettingRoutine());
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected : {cause}");
        GameManager.Scene.LoadScene("LobbyScene");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // 방장 작업 대신 수행
        if (newMasterClient.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {

        }
    }

    int PlayerLoadCount()
    {
        int loadCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetLoad())
                loadCount++;
        }
        return loadCount;
    }

    IEnumerator GameSettingRoutine()
    {
        var waitCondition = new WaitUntil(() => PlayerLoadCount() < PhotonNetwork.PlayerList.Length);
        yield return waitCondition;

        GameSetting();
    }

    void GameSetting()
    {
        bool[] spyArray = new bool[PhotonNetwork.PlayerList.Length];

        int spyCount = (PhotonNetwork.PlayerList.Length >> 2) > 0 ? (PhotonNetwork.PlayerList.Length >> 2) : 1;

        while(spyCount > 0)
        {
            int nextSpy = Random.Range(0, PhotonNetwork.PlayerList.Length);
            if (spyArray[nextSpy])
                continue;
            spyArray[nextSpy] = true;
            spyCount--;
        }

        photonView.RPC("RequestSynchronizeData", RpcTarget.AllBufferedViaServer, spyArray);
    }

    [PunRPC]
    void RequestSynchronizeData(bool[] spyArray)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            aliveDictionary.Add(i, true);
            if (spyArray[i])
                spyStudents.Add(i);
            else
                normalStudents.Add(i);
        }

        if (spyStudents.Contains(PhotonNetwork.LocalPlayer.ActorNumber))
            GameManager.Data.playerState = GameData.PlayerState.Spy;

        chat.EnableChatServer();
        TimeFlow(Time.Morning);
    }

    [PunRPC]
    void TimeFlow(Time _time)
    {
        time = _time;

        switch (time)
        {
            default:
            case Time.Morning:
                morningCanvas.SetActive(true);
                eveningCanvas.SetActive(false);
                nightCanvas.SetActive(false);
                break;
            case Time.Evening:
                morningCanvas.SetActive(false);
                eveningCanvas.SetActive(true);
                nightCanvas.SetActive(false);
                break;
            case Time.Night:
                morningCanvas.SetActive(false);
                eveningCanvas.SetActive(false);
                nightCanvas.SetActive(true);
                break;
        }
    }
}
