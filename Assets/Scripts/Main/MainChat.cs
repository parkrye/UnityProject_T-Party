using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Chat.Demo;
using Photon.Pun;
using System;
using UnityEngine;

public class MainChat : SceneUI, IChatClientListener
{
    string[] channels;
    [SerializeField] int channel;

    [SerializeField] int HistoryLengthToFetch;

    ChatClient chatClient;
    ChatAppSettings chatAppSettings;

    protected override void Awake()
    {
        base.Awake();

        channels = Enum.GetNames(typeof(GameData.PlayerState));
        channel = 0;

        inputFields["ChatInputField"].onSubmit.AddListener(InputChat);
        buttons["EveryoneButton"].onClick.AddListener(() => ChangeChatServer(0));
        buttons["DeadmanButton"].onClick.AddListener(() => ChangeChatServer(1));
        buttons["SpyButton"].onClick.AddListener(() => ChangeChatServer(2));

        chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
    }

    void Start()
    {
        chatClient = new ChatClient(this);
        chatClient.UseBackgroundWorkerForSending = true;
        chatClient.AuthValues = new AuthenticationValues(GameManager.Data.playerName);
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

    public void AddLine(string lineString, string sender = "")
    {
        if (string.IsNullOrEmpty(lineString) || channel < 0 || channel > 2)
        {
            return;
        }

        texts["ChatText"].text += lineString + "\n";

        ChatChannel chatChannel = null;
        bool found = this.chatClient.TryGetChannel(channels[channel], out chatChannel);
        chatChannel.Add(sender, lineString, 0); //TODO: how to use msgID?
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else if (level == DebugLevel.WARNING)
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
        if (channels != null && channels.Length > 0)
        {
            chatClient.Subscribe(channels, HistoryLengthToFetch);
        }

        Debug.Log($"[{GameManager.Data.playerName}] 채팅 서버에 연결되었습니다");

        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void OnDisconnected()
    {
        Debug.Log($"[{GameManager.Data.playerName}] 채팅 서버에 연결이 끊겼습니다");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName.Equals(channels[channel]))
        {
            ShowChannel(channels[channel]);
        }
        for (int i = 0; i < senders.Length; i++)
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
            Debug.Log($"[{GameManager.Data.playerName}] 채팅 서버에 입장되었습니다");
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log($"[{GameManager.Data.playerName}] 채팅 서버에 퇴장되었습니다");
    }

    public void OnUserSubscribed(string channel, string user)
    {
        Debug.Log($"[{GameManager.Data.playerName}] 채팅 서버에 입장되었습니다");
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        Debug.Log($"[{GameManager.Data.playerName}] 채팅 서버에 퇴장되었습니다");
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

        AddLine($"{GameManager.Data.playerName}: {text}", GameManager.Data.playerName);
    }

    public void EnableChatServer()
    {
        if (GameManager.Data.playerState == GameData.PlayerState.Deadman)
            buttons["DeadmanButton"].interactable = true;
        if (GameManager.Data.playerState == GameData.PlayerState.Spy)
            buttons["SpyButton"].interactable = true;
    }

    void ChangeChatServer(int serverNum)
    {
        channel = serverNum;

        ChatChannel chatChannel = null;
        bool found = this.chatClient.TryGetChannel(channels[channel], out chatChannel);
        texts["ChatText"].text = chatChannel.ToStringMessages();

        ShowChannel(channels[channel]);
    }
    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
        {
            return;
        }

        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(channelName, out channel);
        if (!found)
        {
            Debug.Log("ShowChannel failed to find channels: " + channelName);
            return;
        }

    }
}