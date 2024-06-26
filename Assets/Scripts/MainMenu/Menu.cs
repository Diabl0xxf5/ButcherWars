using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("References")]
    public GameObject _lobbyPanel;
    public GameObject _mainMenuPanel;

    [Header("Main menu")]
    public Button _lobbyB;
    public Button _fastGameB;

    [Header("Lobby")]
    public Button _connectB;
    public Button _createB;
    public TMP_InputField _roomNameIF;
    public ListItem _roomItemPref;
    public Transform _content;

    private void Start()
    {
        _lobbyB.onClick.AddListener(call: (() => { OpenLobby(); }));
        _fastGameB.onClick.AddListener(call: (() => { PhotonManager.instance.JoinRandomRoom(); }));

        _connectB.onClick.AddListener(call: (() => { PhotonManager.instance.JoinRoom(_roomNameIF.text); }));
        _createB.onClick.AddListener(call: (() => { PhotonManager.instance.CreateRoom(_roomNameIF.text); }));

        PhotonManager._OnRoomListUpdate.AddListener(OnRoomListUpdate);
    }

    private void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo item in roomList)
        {
            ListItem roomItem = Instantiate(_roomItemPref, _content);
            if (roomItem) roomItem.SetInfo(item);
        }
    }

    private void OpenLobby()
    {
        _mainMenuPanel.SetActive(false);
        _lobbyPanel.SetActive(true);
        PhotonManager.instance.Connect();
    }

}
