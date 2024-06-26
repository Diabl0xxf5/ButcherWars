using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Teams
{
    Green,
    Red
}

public class GameManager : MonoBehaviour
{

    [Header("References")]
    public List<Transform> GreenSpawnPoints;
    public List<Transform> RedSpawnPoints;
    public GameObject _playerPrefab;

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
    }

    private void Start()
    {
        if (PhotonManager.instance)
            PhotonManager.instance.SpawnPlayer(_playerPrefab);
        else
            Instantiate(_playerPrefab);
    }

    public void FillPlayerBehaviour(PlayerBehaviour _pb)
    {
        if (GreenSpawnPoints.Count >= RedSpawnPoints.Count)
        {
            _pb._spawnPoint = GreenSpawnPoints[0];
            _pb._team = Teams.Green;
            GreenSpawnPoints.RemoveAt(0);
        } else
        {
            _pb._spawnPoint = RedSpawnPoints[0];
            _pb._team = Teams.Red;
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

}
