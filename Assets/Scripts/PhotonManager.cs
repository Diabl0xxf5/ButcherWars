using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.Events;
using ExitGames.Client.Photon;

public class PhotonManager : MonoBehaviourPunCallbacks
{

    [SerializeField] string region;

    public static UnityEvent<List<RoomInfo>> _OnRoomListUpdate = new UnityEvent<List<RoomInfo>>();
    public static UnityEvent<string, string> _OnGetMessage = new UnityEvent<string, string>();
    public static UnityEvent _OnJoinedLobby = new UnityEvent();
    public static UnityEvent<int> _OnKill = new UnityEvent<int>();

    public static PhotonManager instance;
    public static PhotonView _pview;

    private GameObject player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _pview = gameObject.AddComponent<PhotonView>();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        transform.parent = null;
        DontDestroyOnLoad(this);
        
    }

    void Start()
    {

    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }

    public void CreateRoom(string roomName)
    {
        CheckConnectionStatus();
        PhotonNetwork.CreateRoom(roomName, DefaultRoomOptions());
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: DefaultRoomOptions());
    }

    public void LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.Destroy(player);
            PhotonNetwork.LeaveRoom();
        }
            
    }

    public void SpawnPlayer(GameObject go)
    {
        player = PhotonNetwork.Instantiate(go.name, Vector3.zero, Quaternion.identity);
    }

// приват

    public bool CheckConnectionStatus()
    {
        if (PhotonNetwork.InLobby) return true;
        if (!PhotonNetwork.IsConnected) Connect();
        else if (!PhotonNetwork.InLobby) PhotonNetwork.JoinLobby(); 
        return false;
    }

    private RoomOptions DefaultRoomOptions()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 6;
        return roomOptions;
    }

//колбэки

    public override void OnConnectedToMaster()
    {
        Debug.Log($"Вы подключены к {PhotonNetwork.CloudRegion}");
        if (!PhotonNetwork.InLobby) PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Вы отключены от сервера. По причине {cause.ToString()}");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"Создана комната ({PhotonNetwork.CurrentRoom.Name})");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Ошибка создания комнаты. По причине {returnCode}:{message}");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _OnRoomListUpdate.Invoke(roomList);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Подключились к комнате ({PhotonNetwork.CurrentRoom.Name})");
        PhotonNetwork.LoadLevel("ButcherWars");
    }

    public override void OnLeftRoom()
    {
        Debug.Log($"Отключились от комнаты");
        PhotonNetwork.LoadLevel("MainMenu");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Ошибка подключения к комнате. По причине {returnCode}:{message}");
    }

    public override void OnJoinedLobby()
    {
        _OnJoinedLobby.Invoke();
    }

    
    public const byte PlayAnimationEventCode = 1;
    public const byte StartGrapplingEventCode = 2;
    public const byte StopGrapplingEventCode = 3;
    public const byte AttackEventCode = 4;
    public const byte HealEventCode = 5;
    public const byte KillEventCode = 6;

    public static void SendStartEvent(int photonViewID, Vector3 _start_grapple_position, Vector3 _grapple_forward)
    {
        object[] content = new object[] { photonViewID, _start_grapple_position, _grapple_forward };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(StartGrapplingEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
    public static void SendStopEvent(int photonViewID)
    {
        object[] content = new object[] { photonViewID };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(StopGrapplingEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }   
    public static void SendPlayAnimationEvent(int photonViewID, string animatorParameter, string parameterType, object parameterValue = null)
    {
        object[] content = new object[] { photonViewID, animatorParameter, parameterType, parameterValue };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(PlayAnimationEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
    public static void SendAttackEvent(int photonViewID, int damage, Vector3 force)
    {
        object[] content = new object[] { photonViewID, damage, force};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(AttackEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
    public static void SendHealEvent(int photonViewID, int value)
    {
        object[] content = new object[] { photonViewID, value };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(HealEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    [PunRPC]
    private void OnKill(int teamid)
    {
        _OnKill.Invoke(teamid);
    }
    public static void SendKillRPC(int teamid)
    {
        _pview.RPC("OnKill", RpcTarget.OthersBuffered, teamid);
    }
    
    [PunRPC]
    private void onGetMessage(string nick, string message)
    {
        _OnGetMessage.Invoke(nick, message);
    }
    public void SendMessageRPC(string nick, string message)
    {
        _pview.RPC("onGetMessage", RpcTarget.All, nick, message);
    }
    
}
