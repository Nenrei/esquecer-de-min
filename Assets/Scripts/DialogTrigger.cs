using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{

    [SerializeField] DialogController controller;

    [SerializeField] List<Dialog> dialogSequence;
    [SerializeField] string npcName;
    [SerializeField] Sprite npcImage;

    [SerializeField] AudioClip voice;
    [SerializeField] float pitch = 1;


    public void StartDialog()
    {
        controller.gameObject.SetActive(true);

        controller.NpcQuestions = GetComponent<QuestionTrigger>();
        controller.DialogSequence = dialogSequence;
        controller.NpcImage = npcImage;
        controller.NpcName = npcName;
        controller.Voice = voice;
        controller.Pitch = pitch;
            
        controller.StartDialog();
    }

}
