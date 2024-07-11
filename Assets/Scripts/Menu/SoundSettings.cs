using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class SoundSettings : MonoBehaviour
{

    public Button _audioB;
    public Slider _volumeSlider;
    public GameObject _xAudio;

    void OnEnable()
    {
        YandexGame.GetDataEvent += GetLoad;
    }
    void OnDisable()
    {
        YandexGame.GetDataEvent -= GetLoad;
    }

    void Start()
    {
        _volumeSlider.onValueChanged.AddListener(call: ((float value) => { VolumeOnValueChanged(value); }));
        _audioB.onClick.AddListener(call: (() => { MuteUnmute(); }));
        GetLoad();
    }

    void GetLoad()
    {
        LoadVolume();
    }

    public void LoadVolume()
    {
        _volumeSlider.value = YandexGame.savesData.volume;
        VolumeOnValueChanged(_volumeSlider.value);
    }

    public void VolumeOnValueChanged(float value)
    {
        if (value != YandexGame.savesData.volume) YandexPlugin.instance.SetVolume(value);
        Sounds.instance.SetVolume(value);
        _xAudio.SetActive(value == 0f);
    }



    public void MuteUnmute()
    {
        float volume = 0.25f;

        if (_volumeSlider.value > 0)
        {
            volume = 0f;
        }

        _volumeSlider.value = volume;
        VolumeOnValueChanged(volume);
    }

}
