using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PhotonView))]
public class NetworkedAnimation : MonoBehaviourPunCallbacks
{
    #region private fields
    Animator anim;
    PhotonView pv;
    #endregion

    #region monobehaviours
    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
    }

    #endregion

    #region private methods
    private void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PhotonManager.PlayAnimationEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int targetPhotonView = (int)data[0];
            if (targetPhotonView == this.photonView.ViewID)
            {
                string animatorParameter = (string)data[1];
                string parameterType = (string)data[2];
                object parameterValue = (object)data[3];
                switch (parameterType)
                {
                    case "Trigger":
                        anim.SetTrigger(animatorParameter);
                        break;
                    case "Bool":
                        anim.SetBool(animatorParameter, (bool)parameterValue);
                        break;
                    case "Float":
                        anim.SetFloat(animatorParameter, (float)parameterValue);
                        break;
                    case "Int":
                        anim.SetInteger(animatorParameter, (int)parameterValue);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    #endregion

    #region public methods

    

    public void SetBool(string animatorParameter, bool value)
    {
        anim.SetBool(animatorParameter, value);
        PhotonManager.SendPlayAnimationEvent(pv.ViewID, animatorParameter, "Bool", value);
    }

    public void SetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
        PhotonManager.SendPlayAnimationEvent(pv.ViewID, trigger, "Trigger");
    }

    public void SetFloat(string animatorParameter, float value)
    {
        anim.SetFloat(animatorParameter, value);
        PhotonManager.SendPlayAnimationEvent(pv.ViewID, animatorParameter, "Float", value);
    }

    public void SetInt(string animatorParameter, int value)
    {
        anim.SetInteger(animatorParameter, value);
        PhotonManager.SendPlayAnimationEvent(pv.ViewID, animatorParameter, "Int", value);
    }

    #endregion
}
