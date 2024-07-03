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
    public Slider _sizeSlider;
    public Transform _player;
    public Transform _camera;

    private bool isConnected;
    private string connectText;
    private Vector3 start_player_scale;
    private Vector3 start_camera_pos;

    private void Start()
    {
        
        _fastGameB.onClick.AddListener(call: (() => { PhotonManager.instance.JoinRandomRoom(); }));
        _roomNameIF.onValueChanged.AddListener(call: ((string value) => { RoomNameOnValueChanged(value); }));
        _connectB.onClick.AddListener(call: (() => { TryConnect(); }));
        _createB.onClick.AddListener(call: (() => { TryCreate(); }));
        _sizeSlider.onValueChanged.AddListener(call: ((float value) => { SizeOnValueChanged(value); }));

        PhotonManager._OnRoomListUpdate.AddListener(OnRoomListUpdate);
        connectText = _connectingText.text;
        start_player_scale = _player.localScale;
        start_camera_pos = _camera.localPosition;

        StartCoroutine(Reconnector());
        StartCoroutine(ConnectingTextUpdate());
    }

    public void RoomNameOnValueChanged(string value)
    {
        if (value.Equals(" "))
            _roomNameIF.text = "";
    }

    public void SizeOnValueChanged(float value)
    {
        if(value <= 5)
        {
            _player.localScale = start_player_scale * value / 5;
            _camera.localPosition = start_camera_pos * value / 5;
        }      
        else
        {
            _player.localScale = start_player_scale * value / 4;
            _camera.localPosition = start_camera_pos * value / 4;
        }
            
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
