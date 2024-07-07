using ExitGames.Client.Photon;
using Photon.Pun;
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
    public List<Transform> GreenSpawnPoints;
    public List<Transform> RedSpawnPoints;
    public GameObject _playerPrefab;

    public int GreenKills;
    public int RedKills;
    public Transform playerCameraTransform;

    public static GameManager instance;

    public Teams _team;

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
    }

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        if (PhotonManager.instance)
            PhotonManager.instance.SpawnPlayer(_playerPrefab);
        else
            Instantiate(_playerPrefab);
    }

    public void FillPlayerBehaviour(PlayerBehaviour _pb)
    {
        int pc = PhotonNetwork.CurrentRoom.PlayerCount;
        
        if (pc % 2 == 0)
        {
            _pb._spawnPoint = GreenSpawnPoints[0];
            _pb._team = Teams.Green;
            _team = Teams.Green;
            GreenSpawnPoints.RemoveAt(0);
        } else
        {
            _pb._spawnPoint = RedSpawnPoints[0];
            _pb._team = Teams.Red;
            _team = Teams.Red;
            RedSpawnPoints.RemoveAt(0);
        }      
    }

    public void ResetSlot(PlayerBehaviour _pb)
    {
        if (_pb._team == Teams.Green)
        {
            GreenSpawnPoints.Add(_pb._spawnPoint);
        } else if (_pb._team == Teams.Red)
        {
            RedSpawnPoints.Add(_pb._spawnPoint);
        }
    }

    public void PlayerKill(Teams t)
    {
        if (t == Teams.Green)
            GreenKills++;
        else if (t == Teams.Red)
            RedKills++;

        if (GreenKills >= 30)
        {
            TeamWin(Teams.Green);
        }
        else if (RedKills >= 30)
        {
            TeamWin(Teams.Red);
        }
    }

    public void PlayerKill(int t_id)
    {
        PlayerKill((Teams)t_id);
    }

    public void TeamWin(Teams t)
    {
        if (t == _team)
        {
            //win
            YandexPlugin.instance.AddWin();
        }
        else
        {
            //lose
        }
    }

}
