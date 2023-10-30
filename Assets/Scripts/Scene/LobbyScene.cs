using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override IEnumerator LoadingRoutine()
    {
        yield return null;
        Progress = 1f;
    }
}

