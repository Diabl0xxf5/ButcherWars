using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Teams
{
    Green,
    Red
}

public class GameManager : MonoBehaviourPunCallbacks
{

    [Header("References")]
    public List<Transform> BlueSpawnPoints;
    public List<Transform> RedSpawnPoints;
    public GameObject _playerPrefab;
    public int KillCup;

    public int BlueKills;
    public int RedKills;
    public Transform playerCameraTransform;
    public GameObject playerGO;

    public static GameManager instance;

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

        PhotonManager._OnKill.AddListener(PlayerKill);
        MyCursor.HideCursor();
        StartCoroutine(LoadPlayer());
    }

    IEnumerator LoadPlayer()
    {
        while (!PhotonManager.instance.JoinTeam())
        {
            yield return new WaitForSeconds(0.5f);
        }

        playerGO = PhotonManager.instance.SpawnPlayer(_playerPrefab);
    }

    public Transform GetSpawnPoint()
    {
        if (PhotonManager._photonTeam.Name == "Blue")
            return BlueSpawnPoints[Random.Range(0, 3)];
        else if (PhotonManager._photonTeam.Name == "Red")
            return RedSpawnPoints[Random.Range(0, 3)];

        return null;
    }


    public void PlayerKill(PhotonTeam t)
    {
        if (t.Name == "Blue")
            BlueKills++;
        else if (t.Name == "Red")
            RedKills++;

        if (BlueKills >= KillCup || RedKills >= KillCup)
        {
            TeamWin(t);
        }
    }

    public void PlayerKill(byte t_id)
    {
        PhotonTeam out_t;
        PhotonTeamsManager.Instance.TryGetTeamByCode(t_id, out out_t);
        PlayerKill(out_t);
    }

    public void TeamWin(PhotonTeam t)
    {
        if (PhotonManager._photonTeam == t)
        {
            YandexPlugin.instance.AddWin();
        }
        else
        {
            YandexPlugin.instance.AddLose();
        }

        PhotonManager.instance.LeaveRoom();
    }

}
