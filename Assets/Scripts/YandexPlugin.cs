using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class YandexPlugin : MonoBehaviour
{

    public static YandexPlugin instance;

    void OnEnable() {
        YandexGame.RewardVideoEvent += Rewarded;
        YandexGame.GetDataEvent += GetLoad;
        YandexGame.PurchaseSuccessEvent += SuccessPurchased;
    }
    void OnDisable() {
        YandexGame.RewardVideoEvent -= Rewarded;
        YandexGame.GetDataEvent -= GetLoad;
        YandexGame.PurchaseSuccessEvent -= SuccessPurchased;
    }
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

/* <-- Рекламная движуха --> */

    public void ShowAd()
    {
        YandexGame.FullscreenShow();
    }

    public void OpenRewardAd(int id)
    {
        // Вызываем метод открытия видео рекламы
        YandexGame.RewVideoShow(id);
    }

    void Rewarded(int id)
    {
        
        if (id == 1) ;
        else if (id == 2) ;

        SaveData();

    }


/* <-- Сохранения --> */

    public void SaveData()
    {
        try
        {
            YandexGame.SaveProgress();
        }
        catch(Exception ex) {

        }
    }

    public void ResetData()
    {
        YandexGame.ResetSaveProgress();
        Debug.Log($"Сохранение сброшено");
    }

    void GetLoad()
    {
        Debug.Log($"Сохранение загружено");
    }

/* <-- Лидерборды --> */

    public void LeaderBoardSaveScore(string tableName, int score)
    {
        //TODO контроль записи лучшего рекорда
        YandexGame.NewLeaderboardScores(tableName, score);
    }

/* <-- Внутриигровые покупки --> */

    //TODO обработка неиспользованных покупок

    void BuyPayments(string id) {
        YandexGame.BuyPayments(id);
    } 

    void SuccessPurchased(string id)
    {
        // Ваш код для обработки покупки. Например:
        if (id == "50");
        else if (id == "250");
        else if (id == "1500");
        
        YandexGame.SaveProgress();
    }

/* <-- Deep Linking --> */

    //TODO обработка перехода по ссылкам

/* <-- Тест --> */

    void SetAction(string strAction)
    {
        
    }

    public void AddKill()
    {
        YandexGame.savesData.kills++;
        LeaderBoardSaveScore("Kills", YandexGame.savesData.kills);
        SaveData();
    }

    public void AddWin()
    {
        YandexGame.savesData.wins++;
        LeaderBoardSaveScore("Wins", YandexGame.savesData.wins);
        SaveData();
    }

    public void AddLose()
    {
        YandexGame.savesData.loses++;
        LeaderBoardSaveScore("Loses", YandexGame.savesData.loses);
        SaveData();
    }

    public void SetVolume(float v)
    {
        YandexGame.savesData.volume = v;
        SaveData();
    }

    public void SetSensetive(float s)
    {
        YandexGame.savesData.sensetive = s;
        SaveData();
    }

}
