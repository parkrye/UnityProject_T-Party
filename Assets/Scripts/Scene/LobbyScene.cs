using Photon.Pun;
using System.Collections;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override IEnumerator LoadingRoutine()
    {
        yield return null;
        Progress = 1f;
    }
}

