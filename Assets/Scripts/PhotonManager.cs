using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.Events;


public class PhotonManager : MonoBehaviourPunCallbacks
{

    [SerializeField] string region;

    public static UnityEvent<List<RoomInfo>> _OnRoomListUpdate = new UnityEvent<List<RoomInfo>>();
    public static UnityEvent<string, string> _OnGetMessage = new UnityEvent<string, string>();

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
            PhotonNetwork.LeaveRoom();
    }

    public void SendMessage(string nick, string message)
    {
        _pview.RPC("onGetMessage", RpcTarget.All, nick, message);
    }

    public void SpawnPlayer(GameObject go)
    {
        player = PhotonNetwork.Instantiate(go.name, Vector3.zero, Quaternion.identity);
    }

// ������

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

//�������

    public override void OnConnectedToMaster()
    {
        Debug.Log($"�� ���������� � {PhotonNetwork.CloudRegion}");
        if (!PhotonNetwork.InLobby) PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"�� ��������� �� �������. �� ������� {cause.ToString()}");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"������� ������� ({PhotonNetwork.CurrentRoom.Name})");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"������ �������� �������. �� ������� {returnCode}:{message}");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _OnRoomListUpdate.Invoke(roomList);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"������������ � ������� ({PhotonNetwork.CurrentRoom.Name})");
        PhotonNetwork.LoadLevel("ButcherWars");
    }

    public override void OnLeftRoom()
    {
        Debug.Log($"����������� �� �������");
        PhotonNetwork.Destroy(player);
        PhotonNetwork.LoadLevel("MainMenu");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"������ ����������� � �������. �� ������� {returnCode}:{message}");
    }

    [PunRPC]
    private void onGetMessage(string nick, string message)
    {
        _OnGetMessage.Invoke(nick, message);
    }

}
