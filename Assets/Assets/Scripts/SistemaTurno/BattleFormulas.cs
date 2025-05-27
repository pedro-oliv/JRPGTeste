using UnityEngine;

public static class BattleFormulas
{
    public static int CalcularDanoFisico(int baseArma, int forca, bool critico){
        float multiplicador = Random.Range(2.5f, 4f);
        float dano = baseArma + (forca*multiplicador);
        if(critico){
            dano*=1.5f;
        }
        return Mathf.CeilToInt(dano);
    }

    public static int CalcularDanoMagico(int baseMagia, int inteligencia, bool fraqueza){
        float multiplicador = Random.Range(2.5f, 3.5f);
        float dano = baseMagia + (inteligencia * multiplicador);
        if(fraqueza){
            dano*=1.5f;
        }
        return Mathf.CeilToInt(dano);
    }

    public static int CalcularDanoRecebido(int danoBruto, int defesa){
        float danoFinal = danoBruto * (100f / (100f + defesa));
        return Mathf.CeilToInt(danoFinal);
    }

    public static int CalcularCura(int baseCura, int inteligencia){
        float variacao = Random.Range(-0.05f, 0.14f);
        float cura = baseCura + (inteligencia * Random.Range(2.5f, 3f));
        cura *= 1f + variacao;
        return Mathf.CeilToInt(cura);
    }

    public static bool VerificarEvasao(int evasao){
        evasao = Mathf.Clamp(evasao, 5, 60);
        int aleatorio = Random.Range(0, 101);
        return aleatorio < evasao;
    }

    public static bool VerificarCritico(int critChance){
        critChance = Mathf.Clamp(critChance, 10, 15);
        int aleatorio = Random.Range(0, 101);
        return aleatorio < critChance;
    }
}
