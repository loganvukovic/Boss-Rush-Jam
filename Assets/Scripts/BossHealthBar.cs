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
    public bool flashState;
    public float flickerTimer;
    public float flickerTime;
    public float hue;
    public float s;
    public float v;

    // Start is called before the first frame update
    void Start()
    {
        flickerTime = 0.2f;
        Color.RGBToHSV(new Color(healthBar.color.r, healthBar.color.g, healthBar.color.b), out hue, out s, out v);
    }

    // Update is called once per frame
    void Update()
    {
        maxHealth = activeBoss.maxHealth;
        curHealth = activeBoss.curHealth;

        if (shownHealth > curHealth)
        {
            shownHealth -= 30f * Time.deltaTime;
            flickerTimer += Time.deltaTime;
        }
        else
        {
            shownHealth = curHealth;
            flickerTimer = 0;
            healthBar.color = Color.HSVToRGB(hue, 1f, 1f);
        }

        healthBar.fillAmount = shownHealth / maxHealth;

        if (flickerTimer > flickerTime)
        {
            if (!flashState)
            {
                flashState = true;
                healthBar.color = Color.HSVToRGB(hue, 0.7f, 1f);
            }
            else
            {
                flashState = false;
                healthBar.color = Color.HSVToRGB(hue, 1f, 1f);
            }
        }

        if (activeBoss.GetComponentInParent<BossActions>().dying && activeBoss.isPuppet)
        {
            gameObject.SetActive(false);
        }
    }
}
