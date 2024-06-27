using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    [SerializeField]
    public HPBar _InGameHood;
    [SerializeField]
    public HPBar _InWorld;

    [SerializeField]
    public int _hp = 100;

    [SerializeField]
    public PlayerBehaviour _pb;
    public Animation _anim;
    public GameObject _damageImage;

    private int _max;

    public void Kill()
    {
        Damage(_hp);
    }

    public void Damage(int value)
    {
        if (_hp == 0) return;

        _hp -= Mathf.Min(_hp, value);
        _InGameHood.UpdateView(_hp, _max);
        _InWorld.UpdateView(_hp, _max);

        if (_anim)
        {
            _anim.Play();
        }

        if (_damageImage)
        {
            _damageImage.SetActive(true);
            StartCoroutine(offDamageImage());
        }

        if (_hp == 0)
        {
            _pb.Die();
        } else
        {
            _pb.TakeDamage();
        }
    }

    IEnumerator offDamageImage()
    {
        yield return new WaitForSeconds(0.15f);
        _damageImage.SetActive(false);
    }

    public void Heal(int value)
    {
        _hp = Mathf.Min(_hp + value, _max);
        _InGameHood.UpdateView(_hp, _max);
        _InWorld.UpdateView(_hp, _max);
    }

    void Start()
    {
        _max = _hp;
    }

}
