using System.Collections;
using UnityEngine;

public class TitleScene : BaseScene
{
    protected override IEnumerator LoadingRoutine()
    {
        yield return null;
        Progress = 1f;
    }
}
