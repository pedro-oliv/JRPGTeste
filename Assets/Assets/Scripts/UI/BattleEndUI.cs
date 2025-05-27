using TMPro;
using UnityEngine;

public class BattleEndUI : MonoBehaviour
{
    public static BattleEndUI Instance;

    public GameObject panel;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI texto;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        panel.SetActive(false);
    }

    public void MostrarResultado(bool venceu)
    {
        panel.SetActive(true);
        texto.text = "Aperte enter para sair";
        resultText.text = venceu ? "Vitoria!" : "Derrota!";
    }
}
