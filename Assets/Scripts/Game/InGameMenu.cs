using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [Header("References")]
    public Canvas _canvas;
    public PlayerBehaviour _pb;

    [Header("Main menu")]
    public Button _leaveB;
    public Button _respawnB;

    public void ShowHide()
    {
        if (_canvas.enabled)
            Hide();
        else
            Show();
    }

    public void Show()
    {
        _leaveB.onClick.AddListener(call: (() => { LeaveRoom(); }));
        _respawnB.onClick.AddListener(call: (() => { Respawn(); }));
        _respawnB.gameObject.SetActive(_pb.died);
        _canvas.enabled = true;
    }

    public void Hide()
    {
        _leaveB.onClick.RemoveAllListeners();
        _respawnB.onClick.RemoveAllListeners();
        _canvas.enabled = false;
    }

    private void Awake()
    {
        
    }

    void LeaveRoom()
    {
        PhotonManager.instance.LeaveRoom();
    }

    void Respawn()
    {
        _pb.Respawn();
        Hide();
    }
}
