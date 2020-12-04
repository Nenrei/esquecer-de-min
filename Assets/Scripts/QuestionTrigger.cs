using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionTrigger : MonoBehaviour
{
    [SerializeField] QuestionController controller;
    [SerializeField] Question[] questions;


    public void StartQuestions()
    {
        controller.gameObject.SetActive(true);

        controller.Questions = questions;

        controller.ShowQuestion(0);
        controller.Npc = GetComponent<NPCController>();
    }

}
