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

    [Header("Input")]
    public KeyCode _MenuButton;

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
        MyCursor.ShowCursor();
        if (PlayerControl.instance) PlayerControl.instance.enabled = false;
    }

    public void Hide()
    {
        _leaveB.onClick.RemoveAllListeners();
        _respawnB.onClick.RemoveAllListeners();
        _canvas.enabled = false;
        MyCursor.HideCursor();
        if (PlayerControl.instance) PlayerControl.instance.enabled = true;
    }

    private void Update()
    {
        if (InputButtonCheck(_MenuButton)) { ShowHide(); }
    }

    private bool InputButtonCheck(KeyCode kc)
    {
        return Input.GetKeyDown(kc);
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
