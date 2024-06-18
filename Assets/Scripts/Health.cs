using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField]
    HPBar _hpBar;

    [SerializeField]
    public int _hp = 100;

    private int _max;

    public void Damage(int value)
    {
        _hp -= Mathf.Min(_hp, value);
        _hpBar.UpdateView(_hp, _max);
    }

    public void Heal(int value)
    {
        _hp = Mathf.Min(_hp + value, _max);
        _hpBar.UpdateView(_hp, _max);
    }

    void Start()
    {
        _max = _hp;
    }

}
