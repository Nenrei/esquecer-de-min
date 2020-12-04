using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject spritePlayer;
    [SerializeField] Animator anim;
    [SerializeField] float range;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip walkSound;

    private float facing;

    [SerializeField] bool canMove = true;
    private NPCController npcTarget;

    public bool CanMove { get => canMove; set => canMove = value; }

    private void Awake()
    {
        PlayerPrefs.SetInt("correctAnswers", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            if (horizontal != 0)
            {
                Vector3 targetPos = new Vector3(transform.position.x + horizontal, transform.position.y, transform.position.z);

                if (targetPos.x > -15 && targetPos.x < 110)
                {
                    if (!source.isPlaying)
                        source.Play();

                    anim.SetBool("walking", true);
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);

                    if ((horizontal == -1 && transform.localScale.x > 0) || horizontal == 1 && transform.localScale.x < 0)
                    {
                        transform.localScale = new Vector3(transform.localScale.x * -1, 1f, 1f);
                        facing = horizontal;
                    }
                }
                else
                {
                    anim.SetBool("walking", false);
                    source.Stop();
                }

            }
            else
            {
                anim.SetBool("walking", false);
                source.Stop();
            }

            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, range);
            if (colliderArray.Length == 0 && npcTarget != null)
            {
                StartCoroutine(npcTarget.HideInteractUI());
                npcTarget = null;
            }
            else if (npcTarget == null)
            {
                foreach (Collider2D collider2D in colliderArray)
                {
                    if (collider2D.TryGetComponent<NPCController>(out NPCController npc))
                    {
                        if (npc.CanTalk)
                        {
                            npcTarget = npc;
                            npc.ShowIntUI = true;
                        }
                    }
                    if (collider2D.TryGetComponent<EndGame>(out EndGame end))
                    {
                        canMove = false;
                        StartCoroutine(end.StartEndGame());
                    }
                }
            }

        }
        else
        {
            anim.SetBool("walking", false);
            source.Stop();
        }


    }

}
