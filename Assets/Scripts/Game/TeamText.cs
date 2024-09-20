using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamText : MonoBehaviour
{
    [Header("References")]
    public GameObject _teammateText;
    public GameObject _enemyText;

    bool isEnemy;
    public Photon.Pun.UtilityScripts.PhotonTeam pt;

    public void SetPhotonTeam(Photon.Pun.UtilityScripts.PhotonTeam _pt)
    {
        pt = _pt;
        UpdateView();
    }

    // Update is called once per frame
    void UpdateView()
    {
        isEnemy = (pt != PhotonManager._photonTeam);
        _teammateText.SetActive(!isEnemy);
        _enemyText.SetActive(isEnemy);
    }

    private void FixedUpdate()
    {
        if (pt == null) return;
        bool _isEnemy = (pt != PhotonManager._photonTeam);
        if (isEnemy != _isEnemy)
            UpdateView();
    }
}
