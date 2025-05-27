using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class BattleUIManager : MonoBehaviour
{
    public BattleMenuUI battleMenuUI;
    public TargetSelectionUI targetSelectionUI;
    private Character jogadorAtual;

    public static BattleUIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void ShowActionSelection(Character personagem)
    {
        jogadorAtual = personagem;

        battleMenuUI.Show(index =>
        {
            switch (index)
            {
                case 0: // Lutar
                    List<string> inimigos = BattleManager.Instance.GetEnemyNames();
                    TargetSelectionUI.Instance.Show(inimigos, OnInimigoSelecionado);
                    break;


                case 1: // Curar
                    if (jogadorAtual.currentMP >= 6)
                    {
                        List<Character> aliados = BattleManager.Instance.aliados;
                        AllySelectionUI.Instance.InicializarAliados(aliados);
                        AllySelectionUI.Instance.Show(aliados.Select(a => $"{a.characterName}  {a.currentHP}/{a.maxHP} HP  {a.currentMP} MP").ToList(), OnAliadoSelecionado);
                    }
                    else
                    {
                        Debug.Log("Não tem MP suficiente.");
                    }
                    break;

                case 2: // Defender
                    BattleManager.Instance.RegistrarComando(jogadorAtual, jogadorAtual, PlayerActionType.Defender);
                    break;
            }
        });
    }

    public void CancelarSelecao()
    {
        battleMenuUI.Activate();
        ShowActionSelection(jogadorAtual);
    }

    List<Character> GetInimigosVivos()
    {
        return BattleManager.Instance.inimigos.Where(i => !i.isDead && i.isEnemy).ToList();
    }

    List<Character> GetAliadosVivos()
    {
        return BattleManager.Instance.aliados.Where(a => !a.isDead && !a.isEnemy).ToList();
    }
    private void OnInimigoSelecionado(int index)
    {
        var inimigos = BattleManager.Instance.GetEnemyNames();
        var inimigo = BattleManager.Instance.inimigos.FirstOrDefault(e => e.characterName == inimigos[index]);
        if (inimigo != null)
            BattleManager.Instance.RegistrarComando(BattleManager.Instance.aliados.FirstOrDefault(), inimigo, PlayerActionType.Lutar);
    }

    private void OnAliadoSelecionado(int index)
    {
        var aliados = BattleManager.Instance.aliados;
        var executor = BattleManager.Instance.aliados.FirstOrDefault();
        var alvo = aliados[index];
        BattleManager.Instance.RegistrarComando(executor, alvo, PlayerActionType.Curar);
        }
    }


