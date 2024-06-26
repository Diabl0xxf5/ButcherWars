using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.Events;


public class PhotonManager :MonoBehaviourPunCallbacks
{

    [SerializeField] string region;

    public static UnityEvent<List<RoomInfo>> _OnRoomListUpdate = new UnityEvent<List<RoomInfo>>();

    public static PhotonManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        CheckConnectionStatus();
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }

    public void CreateRoom(string roomName)
    {
        CheckConnectionStatus();
        
        if (PhotonNetwork.CreateRoom(roomName, DefaultRoomOptions()))
        {
            PhotonNetwork.LoadLevel("ButcherWars");
        }
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
            PhotonNetwork.LeaveRoom();

        PhotonNetwork.LoadLevel("MainMenu");
    }

    private void CheckConnectionStatus()
    {
        if (PhotonNetwork.IsConnected) return;
        Connect();
    }

    private RoomOptions DefaultRoomOptions()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 6;
        return roomOptions;
    }

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

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Ошибка подключения к комнате. По причине {returnCode}:{message}");
    }

}
