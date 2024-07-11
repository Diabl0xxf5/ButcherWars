using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListItem : MonoBehaviour
{
    [Header("References")]
    public Button _button;
    public TextMeshProUGUI _roomNameTMP;
    public TextMeshProUGUI _roomPlayerCountTMP;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(call: (() => { TryToConnect(); }));
    }

    private void TryToConnect()
    {
        PhotonManager.instance.JoinRoom(_roomNameTMP.text);
    }

    public void SetInfo(RoomInfo rInfo)
    {
        SetInfo(rInfo.Name, rInfo.PlayerCount, rInfo.MaxPlayers);
    }
    public void SetInfo(string rName, int pCount, int maxPCount)
    {
        SetInfo(rName, $"{pCount}/{maxPCount}");
    }
    public void SetInfo(string rName, string pCount)
    {
        _roomNameTMP.text = rName;
        _roomPlayerCountTMP.text = pCount;
    }
}
