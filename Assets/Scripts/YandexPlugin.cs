using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class YandexPlugin : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _textAction;
    public static YandexPlugin instance;

    int money;

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
    void Start()
    {
        if (YandexGame.SDKEnabled == true)
        {
            GetLoad();
        }
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
        // Если ID = 1, то выдаём "+100 монет"
        if (id == 1)
        {
            money += 100;
            SetAction($"+Монетки. Деньги: {money}");
        }
        // Если ID = 2, то выдаём "+оружие".
        else if (id == 2)
        {
            SetAction("+Оружие");
        }

        SaveData();

    }


/* <-- Сохранения --> */

    public void SaveData()
    {
        YandexGame.savesData.money = money;
        YandexGame.SaveProgress();
    }

    public void ResetData()
    {
        YandexGame.ResetSaveProgress();
        SetAction($"+Сброс. Деньги: {money}");
    }

    void GetLoad()
    {
        money = YandexGame.savesData.money;
        SetAction($"Сохранение загружено. Деньги: {money}");
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
        if (id == "50")
            YandexGame.savesData.money += 50;
        else if (id == "250")
            YandexGame.savesData.money += 250;
        else if (id == "1500")
            YandexGame.savesData.money += 1500;

        YandexGame.SaveProgress();
    }

/* <-- Deep Linking --> */

    //TODO обработка перехода по ссылкам

/* <-- Тест --> */

    void SetAction(string strAction)
    {
        _textAction.text = $"Action: {strAction}";
    }


}
