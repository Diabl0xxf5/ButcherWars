using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SoundType
{
    Attack,
    Grapple
}


public class Sounds : MonoBehaviour
{
    public static Sounds instance;

    [Header("Settings")]
    public float startVolume;

    [Header("References")]
    public AudioClip _hitSound;
    public AudioClip _chainSound;
    public AudioSource _otherSource;

    private AudioSource _audioSource;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        _audioSource = GetComponent<AudioSource>();
        SetVolume(startVolume);
        transform.parent = null;
        DontDestroyOnLoad(this);
    }

    public void SetVolume(float value)
    {
        _audioSource.volume = value;
    }

    public void PlaySound(SoundType stype, float min_p = 0.7f, float max_p = 1f)
    {

        _otherSource.pitch = Random.Range(min_p, max_p);

        AudioClip TargetClip = null;

        if (stype == SoundType.Attack)
            TargetClip = _hitSound;
        else if (stype == SoundType.Grapple)
            TargetClip = _chainSound;

        _otherSource.PlayOneShot(TargetClip, _audioSource.volume);
    }

    public void StopSound()
    {
        _otherSource.Stop();
    }

}
