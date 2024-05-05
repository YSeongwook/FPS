using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
public class UiManager : Singleton<UiManager>
{

    public TextMeshProUGUI hpText;
    public TextMeshProUGUI bulletText;
    public TextMeshProUGUI grenadeText;
    public Image Hpbar;

    public Component Component { get; set; }

    public int Bullet { get; set; }
    public int Grenade { get; set; }
    public int Hp { get; set; }
    public int MaxBUllet = 30;
    public int MaxGrenade = 2;
    public int Hpbarset;

    public void transBullet(int bullet, int maxbullet)
    {
        //Bullet = bullet;
        //bulletText.text = Bullet + " / "+ MaxBUllet;
        bulletText.text = bullet + " / " + maxbullet;
    }
    public void transGrenade(int grenade, int maxgrenade)
    {
        grenadeText.text = grenade + " / " + maxgrenade;
    }
    //public void transHp(int hp, int maxhp)
    //{
    //    hpText.text = hp + " / " + maxhp;
    //}
    public void transHpbar(float hp, float maxhp)
    {
        //float fillAmount = hp / maxhp;
        Hpbar.fillAmount = hp / maxhp;
        //hpText.text = hp + " / " + maxhp;
        Debug.Log("trasnHp");
    }

}
