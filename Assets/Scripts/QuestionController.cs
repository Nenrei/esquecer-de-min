using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionController : MonoBehaviour
{
    [SerializeField] Animator inventoryPanel;
    [SerializeField] PlayerMovement player;

    [SerializeField] Question[] questions;

    [Space(20)]
    //[SerializeField] TextMeshProUGUI next;
    //[SerializeField] TextMeshProUGUI feedback;
    [SerializeField] GameObject questionPanel;

    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] TextMeshProUGUI answer0;
    [SerializeField] TextMeshProUGUI answer1;
    [SerializeField] TextMeshProUGUI answer2;

    [Space(20)]
    [SerializeField] GameObject[] answersObjects;
    [SerializeField] Sprite defaultColor;
    [SerializeField] Sprite selectedColor;
    [SerializeField] Sprite correctColor;
    [SerializeField] Sprite incorrectColor;

    [Space(20)]
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip correct;
    [SerializeField] AudioClip wrong;

    NPCController npc;

    int currentQuestion;
    int selectedAnswer;
    int correctAnswers;

    bool checkingAnswer;
    bool canDoSomething;
    bool canAutoNext;

    public Question[] Questions { get => questions; set => questions = value; }
    public NPCController Npc { get => npc; set => npc = value; }

    public void ShowQuestion(int questionIndex)
    {
        player.CanMove = false;

        if (inventoryPanel.GetCurrentAnimatorClipInfo(0)[0].clip.name == "InventoryOpened")
            inventoryPanel.SetTrigger("inventory");

        questionText.fontSize = 35;
        questionText.lineSpacing = 0;

        currentQuestion = questionIndex;
        selectedAnswer = 0;

        string qText = questions[currentQuestion].text;

        if (qText.Contains("(small)"))
        {
            questionText.fontSize = 25;
            questionText.lineSpacing = -50;
            qText = qText.Replace("(small)", "");
        }
        else if (qText.Contains("(mid)"))
        {
            questionText.lineSpacing = -50;
            qText = qText.Replace("(mid)", "");
        }

        questionText.text = qText;
        answer0.text = questions[currentQuestion].answers[0].text;
        answer1.text = questions[currentQuestion].answers[1].text;
        answer2.text = questions[currentQuestion].answers[2].text;

        UpdateSelection();

        canDoSomething = true;
        checkingAnswer = false;

    }


    // Update is called once per frame
    void Update()
    {
        if (!checkingAnswer)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (selectedAnswer == 0)
                    selectedAnswer = 2;
                else
                    selectedAnswer -= 1;

                UpdateSelection();

            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (selectedAnswer == 2)
                    selectedAnswer = 0;
                else
                    selectedAnswer += 1;

                UpdateSelection();
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                checkingAnswer = true;
                CheckAnswer();
            }
        }/*else if (canDoSomething && Input.GetKeyDown(KeyCode.Return))
        {
            if (currentQuestion < 2)
                StartCoroutine(NextQuestion());
            else
                CloseQuestion();
        }*/
    }

    void UpdateSelection()
    {
        foreach(GameObject answerObject in answersObjects)
        {
            answerObject.GetComponent<Image>().sprite = defaultColor;
        }
        answersObjects[selectedAnswer].GetComponent<Image>().sprite = selectedColor;
    }

    void CheckAnswer()
    {
        bool isCorrectAnswer = Questions[currentQuestion].answers[selectedAnswer].isCorrect;


        for(int i = 0; i < Questions[currentQuestion].answers.Length; i++)
        {
            if (Questions[currentQuestion].answers[i].isCorrect)
                answersObjects[i].GetComponent<Image>().sprite = correctColor;
            else
                answersObjects[i].GetComponent<Image>().sprite = incorrectColor;
        }

        //feedback.gameObject.SetActive(true);
        //next.gameObject.SetActive(true);

        if (isCorrectAnswer)
        {
            source.PlayOneShot(correct);

            //feedback.text = "Moi ben!";
            PlayerPrefs.SetInt("correctAnswers", PlayerPrefs.GetInt("correctAnswers") + 1);
            correctAnswers++;

        }
        else
        {
            source.PlayOneShot(wrong);
            //feedback.text = "Ui, que pena";
        }
        canAutoNext = true;
        Invoke("AutoNextQuestion", 2);

    }

    void AutoNextQuestion()
    {
        if (!canAutoNext) return;

        if (currentQuestion < 2)
            StartCoroutine(NextQuestion());
        else
            CloseQuestion();
    }


    IEnumerator NextQuestion()
    {
        canAutoNext = false;
        canDoSomething = false;
        yield return new WaitForSeconds(0.5f);
        //feedback.gameObject.SetActive(false);
        //next.gameObject.SetActive(false);
        checkingAnswer = false;
        int nextQuestion = currentQuestion + 1;
        ShowQuestion(nextQuestion);
    }

    void CloseQuestion()
    {
        canAutoNext = false;

        currentQuestion = 0;
        selectedAnswer = 0;

        questionText.text = "";
        answer0.text = "";
        answer1.text = "";
        answer2.text = "";

        Questions = null;

        player.CanMove = true;

        if (correctAnswers >= 2)
            Npc.gameObject.GetComponent<TurnToPantalla>().Turn();
        else
            Npc.CanTalk = false;

        correctAnswers = 0;

        gameObject.SetActive(false);

    }

}
