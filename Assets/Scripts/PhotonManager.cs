using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class PhotonManager :MonoBehaviourPunCallbacks
{

    [SerializeField] string region;
    
    public static PhotonManager instance;
    public Menu _menu;

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

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }

    public void CreateRoom(string roomName)
    {
        CheckConnectionStatus();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 6;
        if (PhotonNetwork.CreateRoom(roomName, roomOptions))
        {
            PhotonNetwork.LoadLevel("ButcherWars");
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    private void CheckConnectionStatus()
    {
        if (PhotonNetwork.IsConnected) return;
        Connect();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log($"�� ���������� � {PhotonNetwork.CloudRegion}");
        PhotonNetwork.JoinLobby();
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
        _menu.OnRoomListUpdate(roomList);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"������������ � ������� ({PhotonNetwork.CurrentRoom.Name})");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"������ ����������� � �������. �� ������� {returnCode}:{message}");
    }

}
