using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PoppingUpDamageText : MonoBehaviour
{
    TextMeshPro damageText;
    Color textColor;
    public Color normalAttack;
    public Color criticalAttack;
    public Color conDamage;
    public Color conAttack;
    public Color Skill;
    public Color undefeated;
    public Color dodged;
    public float moveUpSpeed;
    public Vector3 moveUpDir;
    public Vector3 moveDownDir = new Vector3(-0.7f, -1, 0);
    public float disappearTimer;

    private void Awake()
    {
        damageText = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        if (disappearTimer > Settings.popDamageTextDisappearTime * 0.5f)
        {
            //Move Up
            transform.position += moveUpDir * Time.deltaTime;
            moveUpDir += moveUpDir * (Time.deltaTime * moveUpSpeed);
            transform.localScale += Vector3.one * (Time.deltaTime * Settings.popDamageTextScaleSpeed);
        }
        else
        {
            //Move Down
            transform.position -= moveDownDir * Time.deltaTime;
            transform.localScale -= Vector3.one * (Time.deltaTime * Settings.popDamageTextScaleSpeed);
        }
        //Disappear
        disappearTimer -= Time.deltaTime;
        if (disappearTimer <= 0)
        {
            textColor.a -= Time.deltaTime * Settings.popDamageTextAlphaSpeed;
            damageText.color = textColor;
            if (textColor.a < 0)
                Destroy(gameObject);
        }
    }

    public void setUp(int damage, AttackEffectType attackEffect)
    {
        

        switch (attackEffect)
        {
            case AttackEffectType.Normal:
                damageText.SetText("- " + damage.ToString());
                textColor = normalAttack;
                damageText.fontSize = 5;
                moveUpDir = new Vector3(0, 2, 0);
                moveUpSpeed = 5f;
                break;
            case AttackEffectType.Critical:
                damageText.SetText("Critical" + "  " + damage.ToString());
                textColor = criticalAttack;
                damageText.fontSize = 5;
                moveUpDir = new Vector3(0.5f, 2, 0);
                moveUpSpeed = 8f;
                break;
            case AttackEffectType.continuousDamage:
                damageText.SetText("CD" + "  " + damage.ToString());
                damageText.fontSize = 5;
                textColor = conDamage;
                moveUpDir = new Vector3(0.25f, 2, 0);
                moveUpSpeed = 5.5f;
                break;
            case AttackEffectType.continuouseAttack:
                damageText.SetText("CA" + "  " + damage.ToString());
                damageText.fontSize = 5;
                textColor = conAttack;
                moveUpSpeed = 4f;
                break;

            case AttackEffectType.Skill:
                damageText.SetText("Skill" + " " + damage.ToString());
                textColor = Skill;
                damageText.fontSize = 5;
                moveUpDir = new Vector3(0.75f, 2, 0);
                moveUpSpeed = 5f;
                break;
            case AttackEffectType.Undefeated:
                damageText.SetText("Undefeated");
                textColor = undefeated;
                damageText.fontSize = 5;
                moveUpDir = new Vector3(0, 2, 0);
                moveUpSpeed = 5f;
                break;
            case AttackEffectType.Dodged:
                damageText.SetText("Dodged");
                textColor = dodged;
                damageText.fontSize = 5;
                moveUpDir = new Vector3(0, 2, 0);
                moveUpSpeed = 5f;
                break;

        }
        damageText.color = textColor;
        disappearTimer = Settings.popDamageTextDisappearTime;
    }

}
