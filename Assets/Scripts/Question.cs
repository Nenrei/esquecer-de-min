using UnityEngine;

[System.Serializable]
public class Question
{
    [TextArea] public string text;
    public Answer[] answers;
}
