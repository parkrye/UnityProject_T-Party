using UnityEngine;

public class DataManager : BaseManager
{
    public override void Initialize()
    {
        base.Initialize();

        GameData.AVATAR = new Sprite[5];
        for(int i = 0; i < 5; i++)
        {
            GameData.AVATAR[i] = GameManager.Resource.Load<Sprite>($"Materials/ch_{i}");
        }
    }
}