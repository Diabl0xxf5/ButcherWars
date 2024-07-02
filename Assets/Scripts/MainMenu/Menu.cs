using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    [Header("Refferences")]
    public Button _fastGameB;
    public Button _connectB;
    public Button _createB;
    public TMP_InputField _roomNameIF;
    public ListItem _roomItemPref;
    public Transform _content;
    public TextMeshProUGUI _connectingText;
    public TextMeshProUGUI _roomListText;

    private bool isConnected;
    private string connectText;

    private void Start()
    {
        
        _fastGameB.onClick.AddListener(call: (() => { PhotonManager.instance.JoinRandomRoom(); }));
        _roomNameIF.onValueChanged.AddListener(call: ((string value) => { RoomNameOnValueChanged(value); }));
        _connectB.onClick.AddListener(call: (() => { TryConnect(); }));
        _createB.onClick.AddListener(call: (() => { TryCreate(); }));

        PhotonManager._OnRoomListUpdate.AddListener(OnRoomListUpdate);
        connectText = _connectingText.text;

        StartCoroutine(Reconnector());
        StartCoroutine(ConnectingTextUpdate());
    }

    public void RoomNameOnValueChanged(string value)
    {
        if (value.Equals(" "))
            _roomNameIF.text = "";
    }

    public void TryCreate()
    {
        if (CheckRoomName()) 
            PhotonManager.instance.CreateRoom(_roomNameIF.text);
    }

    public void TryConnect()
    {
        if (CheckRoomName())
            PhotonManager.instance.JoinRoom(_roomNameIF.text);
    }

    public bool CheckRoomName()
    {
        return !_roomNameIF.text.Equals("");
    }

    private void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo item in roomList)
        {
            ListItem roomItem = Instantiate(_roomItemPref, _content);
            if (roomItem) roomItem.SetInfo(item);
        }
    }

    private void SwapConnectionStatus()
    {
        isConnected = !isConnected;

        _connectingText.enabled = !isConnected;
        _roomListText.enabled = isConnected;
    }

    IEnumerator Reconnector()
    {
        while (true)
        {
            if (PhotonManager.instance.CheckConnectionStatus() != isConnected) {
                SwapConnectionStatus();
            }; 
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator ConnectingTextUpdate()
    {
        string dots = ".";

        while (true)
        {
            if (dots.Length > 3) dots = ".";
            _connectingText.text = connectText + dots;
            yield return new WaitForSeconds(0.25f);
            dots += ".";
        }
    }

}
