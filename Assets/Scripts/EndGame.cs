using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] TurnToPantalla player;
    [SerializeField] GameObject panel;

    [SerializeField] TextMeshProUGUI text1;

    [SerializeField] TextMeshProUGUI text2;

    [SerializeField] TextMeshProUGUI text3;

    [SerializeField] TextMeshProUGUI text4;
    [SerializeField] TextMeshProUGUI text5;

    [SerializeField] TextMeshProUGUI text6;
    [SerializeField] TextMeshProUGUI text7;

    [SerializeField] float charSpeed;

    TextMeshProUGUI prevText;

    bool messageCompleted;
    bool gameCompleted;


    // Update is called once per frame
    void Update()
    {
        if (gameCompleted && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Main");
        }
    }

    public IEnumerator StartEndGame()
    {
        panel.SetActive(true);

        messageCompleted = false;
        StartCoroutine(ShowMessageByCharacters(text1, prevText));
        yield return new WaitUntil(() => messageCompleted);


        yield return new WaitForSeconds(2f);
        messageCompleted = false;
        StartCoroutine(ShowMessageByCharacters(text2, prevText));
        yield return new WaitUntil(() => messageCompleted);
        

        if (PlayerPrefs.GetInt("pantallas") == 5)
        {
            yield return new WaitForSeconds(2f);
            messageCompleted = false;
            StartCoroutine(ShowMessageByCharacters(text3, prevText));
            yield return new WaitUntil(() => messageCompleted);

            player.Turn();

            yield return new WaitUntil(() => PlayerPrefs.GetInt("pantallas") == 6);
            yield return new WaitForSeconds(0.5f);

        }
        else
        {
            yield return new WaitForSeconds(2f);
        }


        messageCompleted = false;
        StartCoroutine(ShowMessageByCharacters(text5, prevText));
        yield return new WaitUntil(() => messageCompleted);

        yield return new WaitForSeconds(1f);
        var score = text4.text;
        score = score.Replace("p", PlayerPrefs.GetInt("pantallas").ToString());
        score = score.Replace("r", PlayerPrefs.GetInt("correctAnswers").ToString());
        text4.text = score;
        text4.gameObject.SetActive(true);


        yield return new WaitForSeconds(3.5f);
        text4.gameObject.SetActive(false);
        text5.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        text6.gameObject.SetActive(true);
        text7.gameObject.SetActive(true);

        gameCompleted = true;
    }


    IEnumerator ShowMessageByCharacters(TextMeshProUGUI messageUI, TextMeshProUGUI prevMessageUI)
    {
        if (prevMessageUI)
        {
            prevMessageUI.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }



        string message = messageUI.text;
        messageUI.text = "";
        messageUI.gameObject.SetActive(true);
        char[] characters = message.ToCharArray();
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < characters.Length; i++)
        {
            messageUI.text = messageUI.text + characters[i].ToString();
            yield return new WaitForSeconds(charSpeed);
        }

        prevText = messageUI;
        messageCompleted = true;
    }
}
