using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPunManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject talkCanvas, voteCanvas, nightCanvas;

    int readyPlayerCount;

    Dictionary<int, bool> aliveDictionary;
    List<int> normalStudents;
    List<int> spyStudents;

    void Awake()
    {
        readyPlayerCount = 0;

        aliveDictionary = new Dictionary<int, bool>();
        normalStudents = new List<int>();
        spyStudents = new List<int>();

        talkCanvas.SetActive(true);
        voteCanvas.SetActive(false);
        nightCanvas.SetActive(false);

        PhotonNetwork.LocalPlayer.SetLoad(true);

        if (!PhotonNetwork.IsMasterClient)
            return;

        StartCoroutine(GameSettingRoutine());
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //Debug.Log($"Disconnected : {cause}");
        GameManager.Scene.LoadScene("LobbyScene");
    }

    public override void OnLeftRoom()
    {
        //Debug.Log("Left Room");
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
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            normalStudents.Add(i);
            aliveDictionary.Add(i, true);
        }

        int spyCount = (PhotonNetwork.PlayerList.Length >> 2) > 0 ? (PhotonNetwork.PlayerList.Length >> 2) : 1;

        while(spyCount > 0)
        {
            int nextSpy = Random.Range(0, PhotonNetwork.PlayerList.Length);
            if (spyStudents.Contains(nextSpy))
                continue;
            spyStudents.Add(nextSpy);
            normalStudents.Remove(nextSpy);
        }

        photonView.RPC("RequestSynchronizeData", RpcTarget.AllBufferedViaServer, aliveDictionary, normalStudents, spyStudents);
    }

    [PunRPC]
    void RequestSynchronizeData(Dictionary<int, bool> _alive, List<int> _norm, List<int> _spy)
    {
        aliveDictionary = _alive;
        normalStudents = _norm;
        spyStudents = _spy;
    }
}
