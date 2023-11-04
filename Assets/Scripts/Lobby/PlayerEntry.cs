using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class PlayerEntry : SceneUI
{
    public Player player;

    public void Initialize(Player _player)
    {
        base.Initialize();

        player = _player;

        texts["PlayerName"].text = player.NickName;
        int avatarNum = 0;
        player.CustomProperties.TryGetValue(GameData.PLAYER_AVATAR, out avatarNum);
        images["PlayerImage"].sprite = GameData.AVATAR[avatarNum];

        if (CustomProperty.GetReady(player))
        {
            SetPlayerReady(true);
        }
    }

    public void ResetEntry()
    {
        player = null;
        texts["PlayerName"].text = "";
        images["PlayerImage"].sprite = null;
    }

    public void SetPlayerReady(bool ready)
    {
        images["ReadyImage"].gameObject.SetActive(ready);
    }

    public void OnReadyButtonClicked()
    {
        bool isPlayerReady = !CustomProperty.GetReady(player);
        CustomProperty.SetReady(player, isPlayerReady);

        SetPlayerReady(isPlayerReady);
    }
}