using UnityEngine;
using UnityEngine.U2D;

public class DataManager : BaseManager
{
    public string playerName = "";
    public int playerAvatar = 0;

    public Sprite[] AVATAR;
    public GameData.PlayerState playerState;

    public override void Initialize()
    {
        base.Initialize();

        Application.runInBackground = true;

        AVATAR = new Sprite[5];
        AVATAR = GameManager.Resource.LoadAll<Sprite>($"Materials/ch");
    }
}