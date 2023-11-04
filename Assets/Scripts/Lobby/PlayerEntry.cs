using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class PlayerEntry : SceneUI
{
    public Player player;
    public bool isUsing;

    public void Initialize(Player _player)
    {
        base.Initialize();

        player = _player;

        texts["PlayerName"].text = player.NickName;
        images["PlayerImage"].sprite = GameData.AVATAR[player.GetAvatar()];

        SetPlayerReady(CustomProperty.GetReady(player));

        isUsing = true;
    }

    public void ResetEntry()
    {
        player = null;
        texts["PlayerName"].text = "";
        images["PlayerImage"].sprite = null;
        SetPlayerReady(false);

        isUsing = false;
    }

    public void SetPlayerReady(bool ready)
    {
        images["ReadyImage"].gameObject.SetActive(ready);
    }

    public void Ready()
    {
        bool isPlayerReady = !CustomProperty.GetReady(player);
        CustomProperty.SetReady(player, isPlayerReady);

        SetPlayerReady(isPlayerReady);
    }
}