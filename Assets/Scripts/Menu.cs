using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Settings")]
    public Button _connectB;
    public Button _createB;
    public Button _fastGameB;
    public TMP_InputField _roomNameIF;

    private void Awake()
    {
        _connectB.onClick.AddListener(call: (() => { PhotonManager.instance.Connect(); }));
    }

}
