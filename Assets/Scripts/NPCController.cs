using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] GameObject toInteractPanel;
    [SerializeField] Animator toInteractAnim;
    [SerializeField] Transform interactUITarget;

    [SerializeField] Transform player;

    private bool showIntUI;
    private bool canTalk = true;

    public bool ShowIntUI { get => showIntUI; set => showIntUI = value; }
    public bool CanTalk { get => canTalk; set => canTalk = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ShowIntUI)
        {
            toInteractPanel.SetActive(true);
            toInteractAnim.SetBool("opened", true);
            toInteractPanel.transform.position = RectTransformUtility.WorldToScreenPoint(GameObject.Find("Main Camera").GetComponent<Camera>(), interactUITarget.position);

            if (Input.GetKeyDown(KeyCode.Return))
            {
                GetComponent<DialogTrigger>().StartDialog();
                player.gameObject.GetComponent<PlayerMovement>().CanMove = false;
                StartCoroutine(HideInteractUI());
            }

        }
    }
    
    public IEnumerator HideInteractUI()
    {
        ShowIntUI = false;
        toInteractAnim.SetBool("opened", false);
        yield return new WaitForSeconds(0.15f);
        toInteractPanel.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(player.transform.position.x < transform.position.x && transform.localScale.x == -1)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if(player.transform.position.x > transform.position.x && transform.localScale.x == 1)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }


}
