using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public string characterName;
    public CharacterClasses characterClass;
    public int maxHP;
    public int currentHP;
    public int maxMP;
    public int currentMP;
    public int strength;
    public int weaponDamage;
    public int speed;
    public int intelligence;
    public int defense;
    [Range(0.10f, 0.15f)] public float critChance;
    [Range(0.5f, 0.60f)] public float evasion;
    public bool isEnemy;
    private Character protectingAlly = null;
    private bool isProtected = false;
    private int baseDefense;
    private bool isDefending = false;
    public bool isDead = false;
    public List<IClassCommand> classCommands;
    public Image img;
    public GameObject teste;

    void Start()
    {
        if (currentHP == 0)
        {
            currentHP = maxHP;
        }
        if (currentMP == 0)
        {
            currentMP = maxMP;
        }
        classCommands = ClassCommandCreator.GetClassCommands(characterClass);
        baseDefense = defense;
    }

    public bool usarMP(int quantidade)
    {
        if (currentMP >= quantidade)
        {
            currentMP -= quantidade;
            return true;
        }
        else
        {
            Debug.Log(characterName + " não tem MP suficiente!");
            return false;
        }
    }

    public void AtivarDefesa(int valor)
    {
        if (!isDefending)
        {
            isDefending = true;
            defense += valor;
        }
    }
    public void ResetDef()
    {
        if (isDefending)
        {
            defense = baseDefense;
            isDefending = false;
        }
    }

    public void SetProtectingAlly(Character ally)
    {
        protectingAlly = ally;
    }
    public Character GetProtectingAlly()
    {
        return protectingAlly;
    }

    public void SetIsProtected(bool b)
    {
        isProtected = b;
    }

    public bool GetIsProtected()
    {
        return isProtected;
    }

    public void TakeDamage(int damage)
    {

        currentHP -= damage;

        Debug.Log(characterName + " tomou " + damage + "de dano. HP restante: " + currentHP);



        if (currentHP <= 0)
        {
            currentHP = 0;
            isDead = true;
            Debug.Log(characterName + " foi derrotado!");
            if (!isEnemy)
            {
                BattleManager.Instance.allySprites[0].SetDead();
            }
        }
    }

    public void TakeHeal(int hp)
    {
        if (currentHP <= 0)
        {
            Debug.Log("Não é possivel curar mortos!");
        }
        else
        {
            currentHP += hp;

            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }

            Debug.Log(characterName + " curado por " + hp + " HP.");
        }
    }

    private IEnumerator DefesaCoroutine()
    {
        BattleManager.Instance.allySprites[0].SetDefend();
        yield return new WaitForSeconds(0.5f);
        if (currentHP <= 0)
        {
            BattleManager.Instance.allySprites[0].SetDead();
        }
        else
        {
            BattleManager.Instance.allySprites[0].SetIdle();
        }
    }

    public void MostrarDefesa()
    {
        StartCoroutine(DefesaCoroutine());
    }


}
