using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public BossScript activeBoss;
    public float curHealth;
    public float maxHealth;
    public Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        maxHealth = activeBoss.maxHealth;
        curHealth = activeBoss.curHealth;

        healthBar.fillAmount = curHealth / maxHealth;
    }
}
