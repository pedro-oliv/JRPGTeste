using System.Collections;
using TMPro;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    public GameObject textoAlly;
    public TextMeshProUGUI textoAllyTexto;

    public GameObject textoEnemy;
    public TextMeshProUGUI textoEnemyTexto;

    public static DamageIndicator Instance;

    void Awake()
    {
        Instance = this;
    }

    public void MostrarTextoAlly(string mensagem, Color cor)
    {
        textoAlly.SetActive(true);
        textoAllyTexto.text = mensagem;
        textoAllyTexto.color = cor;
    }

    public void OcultarTextoAlly()
    {
        textoAlly.SetActive(false);
    }

    public void MostrarTextoEnemy(string mensagem, Color cor)
    {
        textoEnemy.SetActive(true);
        textoEnemyTexto.text = mensagem;
        textoEnemyTexto.color = cor;
    }

    public void OcultarTextoEnemy()
    {
        textoEnemy.SetActive(false);
    }

    public IEnumerator MostrarAliado(string msg, Color cor)
    {
        MostrarTextoAlly(msg, cor);
        yield return new WaitForSeconds(0.5f);
        OcultarTextoAlly();
    }

    public IEnumerator MostrarEnemy(string msg, Color cor)
    {
        MostrarTextoEnemy(msg, cor);
        yield return new WaitForSeconds(0.5f);
        OcultarTextoEnemy();
    }
}
