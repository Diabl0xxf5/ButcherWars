using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class SensetiveSettings : MonoBehaviour
{
    public Slider _sensetiveSlider;

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
        _sensetiveSlider.onValueChanged.AddListener(call: ((float value) => { SensetiveOnValueChanged(value); }));
        GetLoad();
    }

    void GetLoad()
    {
        LoadSensetive();
    }

    public void LoadSensetive()
    {
        _sensetiveSlider.value = YandexGame.savesData.sensetive;
        SensetiveOnValueChanged(_sensetiveSlider.value);
    }

    public void SensetiveOnValueChanged(float value)
    {
        if (value != YandexGame.savesData.sensetive) YandexPlugin.instance.SetSensetive(value);
        if (PlayerControl.instance) PlayerControl.instance._mouseSensetive = value;
    }

}
