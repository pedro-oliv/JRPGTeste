using UnityEngine;

public class TargetSelector : MonoBehaviour
{
    public static TargetSelector Instance { get; private set; }

    private Character alvoSelecionado;
    public bool TemAlvoSelecionado => alvoSelecionado != null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SelecionarAlvo(Character alvo)
    {
        alvoSelecionado = alvo;
    }

    public Character ObterAlvoSelecionado()
    {
        var temp = alvoSelecionado;
        alvoSelecionado = null;
        return temp;
    }
}
