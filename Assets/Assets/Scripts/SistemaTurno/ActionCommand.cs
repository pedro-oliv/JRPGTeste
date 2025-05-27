using UnityEngine;

public class ActionCommand
{
    public Character actor;
    public Character target;
    public int damage;
    public PlayerActionType actionType;

    public ActionCommand(Character actor, Character target, PlayerActionType type = PlayerActionType.Lutar){
        this.actor = actor;
        this.target = target;
        this.actionType = type;
    }
}
