using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hpmanager : MonoBehaviour
{
    public int MaxHp = 100;
    Collider Col;
    public int Hp;
    void Awake()
    {
        Hp = MaxHp;
    }
    public void hpChange(int Damage)
    {
        Hp -=Damage;
        UiManager.Instance.transHpbar(Hp, MaxHp);
    }

}
