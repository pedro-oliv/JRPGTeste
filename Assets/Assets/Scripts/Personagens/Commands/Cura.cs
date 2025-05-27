using System.Collections.Generic;
using UnityEngine;

public class Cura : IClassCommand
{
    public string NomeComando => "Curar";
    public bool AlvoEhAliado => true;

    public void Executar(Character executor, Character alvo, BattleManager manager){
        alvo.TakeHeal(BattleFormulas.CalcularCura(50, executor.intelligence));
    }
}
