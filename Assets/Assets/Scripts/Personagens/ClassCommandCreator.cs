using System.Collections.Generic;
using UnityEngine;

public static class ClassCommandCreator
{
    public static List<IClassCommand> GetClassCommands(CharacterClasses characterClass){
        switch(characterClass){
            case CharacterClasses.None:
                return new List<IClassCommand> { new Cura() };
            default: return new List<IClassCommand> { new Cura()};
        }
    }
}
