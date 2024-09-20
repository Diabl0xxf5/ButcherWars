using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [Header("References")]
    public Image _hpBarImage;
    public Image _hpBarPrevImage;
    public TextMeshProUGUI _hpText;
    

    public void UpdateView(int new_hp, int max_hp)
    {
        if (!gameObject.activeInHierarchy) return;

        _hpBarImage.fillAmount = (float)new_hp / max_hp;
        _hpText.text = $"{new_hp} / {max_hp}";
        StartCoroutine(UpdateViewLate());
    }

    IEnumerator UpdateViewLate()
    {
        yield return new WaitForSeconds(1f);
        _hpBarPrevImage.fillAmount = _hpBarImage.fillAmount;
    }

}
