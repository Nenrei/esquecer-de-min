using UnityEngine;

[System.Serializable]
public class Dialog
{
    public enum Owner { player, npc }

    public Owner owner;

    
    [TextArea]public string message;

    public bool isEnd;
    public bool triggerQuestions;
}
