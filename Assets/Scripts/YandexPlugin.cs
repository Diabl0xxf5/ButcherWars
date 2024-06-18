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

/* <-- ��������� ������� --> */

    public void ShowAd()
    {
        YandexGame.FullscreenShow();
    }

    public void OpenRewardAd(int id)
    {
        // �������� ����� �������� ����� �������
        YandexGame.RewVideoShow(id);
    }

    void Rewarded(int id)
    {
        // ���� ID = 1, �� ����� "+100 �����"
        if (id == 1)
        {
            money += 100;
            SetAction($"+�������. ������: {money}");
        }
        // ���� ID = 2, �� ����� "+������".
        else if (id == 2)
        {
            SetAction("+������");
        }

        SaveData();

    }


/* <-- ���������� --> */

    public void SaveData()
    {
        YandexGame.savesData.money = money;
        YandexGame.SaveProgress();
    }

    public void ResetData()
    {
        YandexGame.ResetSaveProgress();
        SetAction($"+�����. ������: {money}");
    }

    void GetLoad()
    {
        money = YandexGame.savesData.money;
        SetAction($"���������� ���������. ������: {money}");
    }

/* <-- ���������� --> */

    public void LeaderBoardSaveScore(string tableName, int score)
    {
        //TODO �������� ������ ������� �������
        YandexGame.NewLeaderboardScores(tableName, score);
    }

/* <-- ������������� ������� --> */

    //TODO ��������� ���������������� �������

    void BuyPayments(string id) {
        YandexGame.BuyPayments(id);
    } 

    void SuccessPurchased(string id)
    {
        // ��� ��� ��� ��������� �������. ��������:
        if (id == "50")
            YandexGame.savesData.money += 50;
        else if (id == "250")
            YandexGame.savesData.money += 250;
        else if (id == "1500")
            YandexGame.savesData.money += 1500;

        YandexGame.SaveProgress();
    }

/* <-- Deep Linking --> */

    //TODO ��������� �������� �� �������

/* <-- ���� --> */

    void SetAction(string strAction)
    {
        _textAction.text = $"Action: {strAction}";
    }


}
