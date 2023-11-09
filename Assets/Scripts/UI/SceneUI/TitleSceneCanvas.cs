using Photon.Pun;
using System.Collections;
using UnityEngine;

public class TitleSceneCanvas : SceneUI
{
    bool quitButtonClick;

    public override void Initialize()
    {
        base.Initialize();

        quitButtonClick = false;

        buttons["StartButton"].onClick.AddListener(OnStartButtonTouched);
        buttons["QuitButton"].onClick.AddListener(OnQuitButtonTouched);

        images["QuitImage"].gameObject.SetActive(false);
    }

    void OnStartButtonTouched()
    {
        string playerName = inputFields["NameInputField"].text;

        if (playerName == "")
        {
            playerName = $"Mob {Random.Range(1000, 5000)}";
        }

        GameManager.Data.playerName = playerName;
        GameManager.Data.playerAvatar = 0;

        ExitGames.Client.Photon.Hashtable props = new()
        {
            { GameData.PLAYER_NAME, playerName },
            { GameData.PLAYER_AVATAR, 0 },
            { GameData.PLAYER_READY, false },
            { GameData.PLAYER_LOAD, false },
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        inputFields["NameInputField"].text = "";
        PhotonNetwork.LocalPlayer.NickName = playerName;
        PhotonNetwork.ConnectUsingSettings();
        GameManager.Scene.LoadScene("LobbyScene");
    }

    void OnQuitButtonTouched()
    {
        if (quitButtonClick)
        {
            Application.Quit();
        }
        else
        {
            StartCoroutine(QuitButtonRoutine());
        }
    }

    IEnumerator QuitButtonRoutine()
    {
        quitButtonClick = true;
        images["QuitImage"].gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        quitButtonClick = false;
        images["QuitImage"].gameObject.SetActive(false);
    }
}
