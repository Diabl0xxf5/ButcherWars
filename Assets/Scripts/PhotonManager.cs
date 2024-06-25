using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager :MonoBehaviourPunCallbacks
{

    [SerializeField] string region;
    
    public static PhotonManager instance;

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

        transform.parent = null;
        DontDestroyOnLoad(this);

    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log($"Вы подключены к {PhotonNetwork.CloudRegion}");
        //base.OnConnectedToMaster();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Вы отключены от сервера. По причине {cause.ToString()}");
        base.OnDisconnected(cause);
    }

}
