using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public List<Character> aliados;
    public List<Character> inimigos;

    private List<ActionCommand> comandosPendentes = new List<ActionCommand>();
    public List<AllySpriteController> allySprites;
    public DamageEffect dmgFX;
    public GameObject inimg;

    private Color laranja = new Color(0.8f, 0.4f, 0f);

    public AudioClip enemyhit, heal, allyhit, allycrit, enemycrit;

    public AudioClip fightmsc, win, gameover;

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
        dmgFX = new DamageEffect(this);
    }

    private void Start()
    {
        TargetSelectionUI.Instance.InicializarInimigos(GetEnemyNames());
        AllySelectionUI.Instance.InicializarAliados(aliados);
        allySprites[0].SetIdle();
        MusicManager.Instance.PlayMusic(fightmsc);
        StartTurno();
    }

    public void StartTurno()
    {
        foreach (Character c in aliados.Concat(inimigos))
        {
            c.ResetDef();
        }

        comandosPendentes.Clear();

        foreach (Character aliado in aliados)
        {
            if (!aliado.isDead)
            {
                BattleUIManager.Instance.ShowActionSelection(aliado);
                allySprites[0].SetIdle();
            }
            else
            {
                allySprites[0].SetDead();
            }
        }

        Character inimigo = inimigos.FirstOrDefault(i => !i.isDead);
        Character alvo = aliados.FirstOrDefault(a => !a.isDead);

        if (inimigo != null && alvo != null)
        {
            RegistrarComando(inimigo, alvo, PlayerActionType.Lutar);
        }
    }

    public void RegistrarComando(Character ator, Character alvo, PlayerActionType tipo)
    {
        ActionCommand comando = new ActionCommand(ator, alvo, tipo);
        comandosPendentes.Add(comando);

        if (comandosPendentes.Count >= aliados.Count(a => !a.isDead) + inimigos.Count(i => !i.isDead))
        {
            StartCoroutine(ExecutarTurno());
        }
    }

    IEnumerator ExecutarTurno()
    {
        comandosPendentes = comandosPendentes.OrderByDescending(c => c.actor.speed).ToList();

        foreach (var comando in comandosPendentes)
        {
            if (comando.actor.isDead) continue;

            switch (comando.actionType)
            {
                case PlayerActionType.Lutar:
                    if (BattleFormulas.VerificarEvasao(Mathf.CeilToInt(comando.target.evasion)))
                    {
                        Debug.Log(comando.target.evasion + "desviou");
                        if (!comando.target.isEnemy)
                        {
                            DamageIndicator.Instance.MostrarTextoAlly("Miss", Color.white);
                            yield return new WaitForSeconds(0.7f);
                            DamageIndicator.Instance.OcultarTextoAlly();
                        }
                        else
                        {
                            DamageIndicator.Instance.MostrarTextoEnemy("Miss", Color.white);
                            yield return new WaitForSeconds(0.7f);
                            DamageIndicator.Instance.OcultarTextoEnemy();
                        }
                    }
                    else
                    {
                        bool crit = BattleFormulas.VerificarCritico(Mathf.CeilToInt(comando.actor.critChance));
                        if (crit) Debug.Log("Crítico!");
                        int dano = BattleFormulas.CalcularDanoFisico(comando.actor.weaponDamage, comando.actor.strength, crit);
                        int finalDano = BattleFormulas.CalcularDanoRecebido(dano, comando.target.defense);
                        if (!comando.actor.isEnemy)
                        {
                            allySprites[0].SetAttack();
                            yield return new WaitForSeconds(0.5f);
                            allySprites[0].SetIdle();
                        }
                        else
                        {
                            allySprites[0].SetDefend();
                            yield return new WaitForSeconds(0.5f);
                            allySprites[0].SetIdle();
                        }
                        if (crit)
                        {
                            dmgFX.PlayEffect(comando.target.img, Color.red, 0.6f);
                        }
                        else
                        {
                            dmgFX.PlayEffect(comando.target.img, laranja, 0.6f);
                        }
                        if (!comando.actor.isEnemy)
                        {
                            if (crit) SoundEffectManager.Instance.PlaySFX(allycrit);
                            else SoundEffectManager.Instance.PlaySFX(allyhit);
                        }
                        else
                        {
                            if (crit) SoundEffectManager.Instance.PlaySFX(enemycrit);
                            else SoundEffectManager.Instance.PlaySFX(enemyhit);
                        }
                        if (!comando.target.isEnemy)
                        {
                            DamageIndicator.Instance.MostrarTextoAlly(finalDano.ToString(), Color.white);
                            yield return new WaitForSeconds(0.7f);
                            DamageIndicator.Instance.OcultarTextoAlly();
                        }
                        else
                        {
                            DamageIndicator.Instance.MostrarTextoEnemy(finalDano.ToString(), Color.white);
                            yield return new WaitForSeconds(0.7f);
                            DamageIndicator.Instance.OcultarTextoEnemy();
                        }
                        comando.target.TakeDamage(finalDano);
                        AllySelectionUI.Instance.InicializarAliados(aliados);
                        yield return new WaitForSeconds(0.5f);
                    }
                    break;

                case PlayerActionType.Curar:
                    int cura = BattleFormulas.CalcularCura(50, comando.actor.intelligence);
                    int customp = 6;
                    comando.actor.currentMP -= customp;
                    allySprites[0].SetHeal();
                    yield return new WaitForSeconds(0.5f);
                    allySprites[0].SetIdle();
                    SoundEffectManager.Instance.PlaySFX(heal);
                    comando.target.TakeHeal(cura);
                    dmgFX.PlayEffect(comando.target.img, Color.green, 0.6f);
                    DamageIndicator.Instance.MostrarTextoAlly(cura.ToString(), Color.green);
                    yield return new WaitForSeconds(0.7f);
                    DamageIndicator.Instance.OcultarTextoAlly();
                    AllySelectionUI.Instance.InicializarAliados(aliados);
                    break;

                case PlayerActionType.Defender:
                    allySprites[0].SetDefend();
                    comando.actor.AtivarDefesa(10);
                    break;
            }
            AllySelectionUI.Instance.InicializarAliados(aliados);
            yield return new WaitForSeconds(0.5f);
        }
        comandosPendentes.Clear();
        AllySelectionUI.Instance.InicializarAliados(aliados);
        yield return new WaitForSeconds(0.5f);


        VerificarFimDeBatalha();
    }


    public void VerificarFimDeBatalha()
    {
        bool todosInimigosDerrotados = inimigos.All(i => i.isDead);
        bool todosAliadosDerrotados = aliados.All(a => a.isDead);

        if (todosInimigosDerrotados)
        {
            inimg.SetActive(false);
            MusicManager.Instance.PlayMusic(win);
            Debug.Log("Você venceu a batalha!");
            BattleEndUI.Instance.MostrarResultado(true);
            StartCoroutine(Comemorar());
            StartCoroutine(FecharJogo());
        }
        else if (todosAliadosDerrotados)
        {
            allySprites[0].SetDead();
            MusicManager.Instance.PlayMusic(gameover);
            Debug.Log("Você perdeu a batalha!");
            BattleEndUI.Instance.MostrarResultado(false);
            StartCoroutine(FecharJogo());
        }
        else
        {
            StartTurno();
        }
    }

    public List<string> GetEnemyNames()
    {
        return inimigos.Where(e => !e.isDead).Select(e => e.characterName).ToList();
    }

    public List<string> GetAllyNames()
    {
        return aliados.Where(a => !a.isDead).Select(a => a.characterName).ToList();
    }

    public List<Character> GetAllies()
    {
        return aliados.Where(e => !e.isEnemy && !e.isDead).ToList();
    }

    IEnumerator FecharJogo()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }

        Application.Quit();
        Debug.Log("quitando");
    }

    IEnumerator Comemorar()
    {
        while (true)
        {
            allySprites[0].SetWin();
            yield return new WaitForSeconds(0.5f);
            allySprites[0].SetIdle();
            yield return new WaitForSeconds(0.5f);
        }
    }

}


    