using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public BossScript activeBoss;
    public float curHealth;
    public float maxHealth;
    public float shownHealth;
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

        if (shownHealth > curHealth)
        {
            shownHealth -= 30f * Time.deltaTime;
        }
        else shownHealth = curHealth;

        healthBar.fillAmount = shownHealth / maxHealth;

        if (activeBoss.GetComponentInParent<BossActions>().dying && activeBoss.isPuppet)
        {
            gameObject.SetActive(false);
        }
    }
}
