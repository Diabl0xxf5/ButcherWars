using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{

    [SerializeField]
    Image _hpBarImage;

    [SerializeField]
    Image _hpBarPrevImage;

    [SerializeField]
    TextMeshProUGUI _hpText;

    [SerializeField]
    Transform _camera;

    public void UpdateView(int new_hp, int max_hp)
    {
        _hpBarImage.fillAmount = (float)new_hp / max_hp;
        _hpText.text = $"{new_hp} / {max_hp}";
        StartCoroutine(UpdateViewLate());
    }

    IEnumerator UpdateViewLate()
    {
        yield return new WaitForSeconds(1f);
        _hpBarPrevImage.fillAmount = _hpBarImage.fillAmount;
    }

    void FixedUpdate()
    {
        if (_camera)
        {
            transform.LookAt(_camera);
        }
    }

}
