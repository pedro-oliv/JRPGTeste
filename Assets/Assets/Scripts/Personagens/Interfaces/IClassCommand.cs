using System.Collections.Generic;
using UnityEngine;

public interface IClassCommand
{
    string NomeComando { get; }
    bool AlvoEhAliado { get; }
    void Executar(Character executor, Character alvo, BattleManager manager);
}
