using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    [Space(10)]
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip playerVoice;
    AudioClip voice;
    float pitch;

    [Space(10)]
    [SerializeField] Animator inventoryPanel;
    [SerializeField] PlayerMovement player;
    [SerializeField] QuestionTrigger npcQuestions;

    [Space(10)]
    [SerializeField] GameObject dialogPanel;

    [Space(10)]
    [SerializeField] Animator playerAnim;
    [SerializeField] GameObject playerDialog;
    [SerializeField] TextMeshProUGUI playerMessage;

    [Space(10)]
    [SerializeField] Animator npcAnim;
    [SerializeField] GameObject npcDialog;
    [SerializeField] TextMeshProUGUI npcMessage;
    [SerializeField] Image npcPicture;
    [SerializeField] TextMeshProUGUI npcNameText;

    [Space(10)]
    [SerializeField] string npcName;
    [SerializeField] Sprite npcImage;

    [Space(10)]
    [SerializeField] List<Dialog> dialogSequence;

    [Space(10)]
    [SerializeField] float charSpeed = 0.1f;

    string currentOwner;
    int currentMessage = 0;
    bool messageCompleted;
    bool canGoNext;
    bool allowDialog;

    public List<Dialog> DialogSequence { get => dialogSequence; set => dialogSequence = value; }
    public string NpcName { get => npcName; set => npcName = value; }
    public Sprite NpcImage { get => npcImage; set => npcImage = value; }
    public QuestionTrigger NpcQuestions { get => npcQuestions; set => npcQuestions = value; }
    public AudioClip Voice { get => voice; set => voice = value; }
    public float Pitch { get => pitch; set => pitch = value; }

    public void StartDialog()
    {
        if (inventoryPanel.GetCurrentAnimatorClipInfo(0)[0].clip.name == "InventoryOpened")
            inventoryPanel.SetTrigger("inventory");

        CloseDialog(false);

        currentOwner = DialogSequence[0].owner.ToString();

        npcNameText.text = NpcName;
        npcPicture.sprite = NpcImage;

        player.CanMove = false;
        dialogPanel.SetActive(true);

        if (currentOwner == "player")
            playerDialog.GetComponent<CanvasGroup>().alpha = 1;
        else
            npcDialog.GetComponent<CanvasGroup>().alpha = 1;

        playerAnim.SetBool("close", false);
        npcAnim.SetBool("close", false);
        playerAnim.SetBool("open", true);
        npcAnim.SetBool("open", true);

        ShowMessage(false);

        allowDialog = true;
    }

    void CloseDialog(bool fullClose)
    {
        playerDialog.GetComponent<CanvasGroup>().alpha = 0;
        npcDialog.GetComponent<CanvasGroup>().alpha = 0;

        currentMessage = 0;
        currentOwner = "";
        messageCompleted = false;
        canGoNext = false;

        playerMessage.text = "";
        npcMessage.text = "";

        if (fullClose)
        {
            npcName = "";
            npcImage = null;
            dialogSequence = null;

            StopAllCoroutines();

            dialogPanel.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (allowDialog)
        {

            if (canGoNext && Input.GetKeyDown(KeyCode.Return) && dialogPanel.activeInHierarchy)
            {
                if (messageCompleted)
                {
                    messageCompleted = false;
                    NextMessage();
                }
                else
                {
                    StopAllCoroutines();
                    ShowMessage(true);
                }
            }
        }
    }

    public void NextMessage()
    {

        if (DialogSequence[currentMessage].isEnd)
        {
            if (DialogSequence[currentMessage].triggerQuestions)
                StartCoroutine(ShowQuestions());
            else
                CloseDialog(true);
            return;
        }
        currentMessage++;
        npcMessage.text = "";
        playerMessage.text = "";
        ShowMessage(false);
    }

    void ShowMessage( bool full )
    {
        Dialog currentDialog = DialogSequence[currentMessage];
        currentOwner = currentDialog.owner.ToString();

        TextMeshProUGUI textMP;


        if (currentOwner == "player")
        {
            textMP = playerMessage;
            npcDialog.GetComponent<CanvasGroup>().alpha = 0;
            playerDialog.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            textMP = npcMessage;
            playerDialog.GetComponent<CanvasGroup>().alpha = 0;
            npcDialog.GetComponent<CanvasGroup>().alpha = 1;
        }

        if (full)
        {
            textMP.text = DialogSequence[currentMessage].message;
            messageCompleted = true;
        }
        else
            StartCoroutine(ShowMessageByCharacters(DialogSequence[currentMessage].message, textMP));

    }

    IEnumerator ShowMessageByCharacters(string message, TextMeshProUGUI messageUI)
    {
        char[] characters = message.ToCharArray();
        yield return new WaitForSeconds(0.1f);
        canGoNext = true;


        for (int i = 0; i < characters.Length; i++)
        {
            yield return new WaitForSeconds(charSpeed);
            messageUI.text = messageUI.text + characters[i].ToString();
            if (i % 3 == 0 && Random.Range(1, 3) == 1)
            {
                PlayVoice();
            }
        }

        messageCompleted = true;
    }

    IEnumerator ShowQuestions()
    {
        allowDialog = false;

        playerAnim.SetBool("open", false);
        npcAnim.SetBool("open", false);
        playerAnim.SetBool("close", true);
        npcAnim.SetBool("close", true);

        yield return new WaitForSeconds(0.7f);

        CloseDialog(false);
        NpcQuestions.StartQuestions();
        CloseDialog(true);
    }

    void PlayVoice()
    {
        if (currentOwner == "player")
        {
            source.PlayOneShot(playerVoice);
        }
        else
        {
            source.PlayOneShot(Voice);
        }
    }

}
