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
    public Button _lobbyB;
    public Button _backB;
    public Button _audioB;
    public Button _telegramb;

    public TMP_InputField _roomNameIF;
    public ListItem _roomItemPref;
    public Transform _content;
    public TextMeshProUGUI _connectingText;
    public TextMeshProUGUI _connectedText;
    public Slider _sizeSlider;
    public Slider _volumeSlider;
    public Transform _player;
    public Transform _camera;
    public GameObject _mainMenuPanel;
    public GameObject _lobbyPanel;
    public GameObject _xAudio;


    private bool isConnected;
    private string connectText;
    private Vector3 start_player_scale;
    private Vector3 start_camera_pos;
    private Dictionary<string, ListItem> _rooms = new Dictionary<string, ListItem>();

    private void Start()
    {
        
        _fastGameB.onClick.AddListener(call: (() => { PhotonManager.instance.JoinRandomRoom(); }));
        _roomNameIF.onValueChanged.AddListener(call: ((string value) => { RoomNameOnValueChanged(value); }));
        _connectB.onClick.AddListener(call: (() => { TryConnect(); }));
        _createB.onClick.AddListener(call: (() => { TryCreate(); }));
        _telegramb.onClick.AddListener(call: (() => { GoToTelegram(); }));
        _lobbyB.onClick.AddListener(call: (() => { SetActivePanel(_lobbyPanel); }));
        _backB.onClick.AddListener(call: (() => { SetActivePanel(_mainMenuPanel); }));
        _sizeSlider.onValueChanged.AddListener(call: ((float value) => { SizeOnValueChanged(value); }));
        _volumeSlider.onValueChanged.AddListener(call: ((float value) => { VolumeOnValueChanged(value); }));
        _audioB.onClick.AddListener(call: (() => { MuteUnmute(); }));

        PhotonManager._OnRoomListUpdate.AddListener(OnRoomListUpdate);
        connectText = _connectingText.text;
        start_player_scale = _player.localScale;
        start_camera_pos = _camera.localPosition;

        StartCoroutine(Reconnector());
        StartCoroutine(ConnectingTextUpdate());
    }

    public void SetActivePanel(GameObject activePanel)
    {
        _lobbyPanel.SetActive(false);
        _mainMenuPanel.SetActive(false);

        activePanel.SetActive(true);
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

    public void VolumeOnValueChanged(float value)
    {
        Sounds.instance.SetVolume(value);
        _xAudio.SetActive(value == 0f);
    }

    public void MuteUnmute()
    {
        float volume = 0.25f;

        if (_volumeSlider.value > 0)
        {
            volume = 0f;
        }

        _volumeSlider.value = volume;
        VolumeOnValueChanged(volume);
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
            if (item.RemovedFromList)
            {
                if (_rooms.ContainsKey(item.Name))
                {
                    Destroy(_rooms[item.Name].gameObject);
                    _rooms.Remove(item.Name);
                }
            } else
            {
                ListItem roomItem = Instantiate(_roomItemPref, _content);
                if (roomItem) {
                    roomItem.SetInfo(item);
                    _rooms.Add(item.Name, roomItem);
                }          
            }
        }

    }

    private void SwapConnectionStatus()
    {
        isConnected = !isConnected;

        _connectingText.enabled = !isConnected;
        _connectedText.enabled = isConnected;
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

    public void GoToTelegram()
    {
        Application.OpenURL("https://t.me/smyyya_project");
    }

}
