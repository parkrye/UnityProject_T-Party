using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Chat.Demo;
using Photon.Pun;
using UnityEditor.VersionControl;
using UnityEngine;

public class MainChat : SceneUI, IChatClientListener
{
    string[] channels;
    int chnnel;

    [SerializeField] int HistoryLengthToFetch;

    ChatClient chatClient;
    ChatAppSettings chatAppSettings;

    protected override void Awake()
    {
        base.Awake();

        inputFields["ChatInputField"].onSubmit.AddListener(InputChat);

        chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
    }

    void Start()
    {
        chatClient = new ChatClient(this);
        chatClient.UseBackgroundWorkerForSending = true;
        chatClient.AuthValues = new AuthenticationValues(GameData.PLAYER_NAME);
        chatClient.ConnectUsingSettings(chatAppSettings);
        
    }

    void OnDestroy()
    {
        chatClient.Disconnect();
    }

    void OnApplicationQuit()
    {
        chatClient.Disconnect();
    }

    public void AddLine(string lineString, int ch = 0)
    {
        texts["ChatText"].text += lineString + "\n";

        chatClient.PublishMessage(channels[ch], lineString);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log($"OnChatStateChange: {state}");
    }

    public void OnConnected()
    {
        if (this.channels != null && this.channels.Length > 0)
        {
            this.chatClient.Subscribe(this.channels, this.HistoryLengthToFetch);
        }

        AddLine($"[{GameData.PLAYER_NAME}] 채팅 서버에 연결되었습니다");

        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void OnDisconnected()
    {
        AddLine($"[{GameData.PLAYER_NAME}] 채팅 서버에 연결이 끊겼습니다");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for(int i = 0; i < senders.Length; i++)
        {
            AddLine($"{senders[i]}: {messages[i]}");
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        AddLine($"[비밀] {sender}: {message}");
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log($"OnStatusUpdate: user:{user}, status:{status}, msg:{message}");
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        foreach (string channel in channels)
        {
            AddLine($"[{GameData.PLAYER_NAME}] 채팅 서버에 입장되었습니다");
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
        AddLine($"[{GameData.PLAYER_NAME}] 채팅 서버에 퇴장되었습니다");
    }

    public void OnUserSubscribed(string channel, string user)
    {
        AddLine($"[{GameData.PLAYER_NAME}] 채팅 서버에 입장되었습니다");
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        AddLine($"[{GameData.PLAYER_NAME}] 채팅 서버에 퇴장되었습니다");
    }

    void Update()
    {
        if (chatClient == null)
            return;

        chatClient.Service();
    }

    public void InputChat(string text)
    {
        inputFields["ChatInputField"].text = "";

        if (chatClient.State != ChatState.ConnectedToFrontEnd)
            return;

        AddLine($"{GameData.PLAYER_NAME}: {text}", chnnel);
    }
}