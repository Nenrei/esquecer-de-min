using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToPantalla : MonoBehaviour
{
    
    [SerializeField] int index;
    [SerializeField] PantallasController pantallaCtrl;
    [SerializeField] Transform glow;
    [SerializeField] Transform pantalla;
    [SerializeField] GameObject npc;
    [SerializeField] GameObject shadow;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip win0;
    [SerializeField] AudioClip win1;
    [SerializeField] AudioClip win2;

    private Vector3 targetPos;

    bool growGlow;
    bool killGlow;
    bool moveToInventory;

    void Update()
    {
        if (growGlow)
        {
            glow.localScale = new Vector3(glow.localScale.x + Time.deltaTime, glow.localScale.y + Time.deltaTime, 1f);
            if (glow.localScale.x >= 2)
                growGlow = false;
        }

        if (killGlow)
        {
            glow.localScale = new Vector3(glow.localScale.x - Time.deltaTime*4, glow.localScale.y - Time.deltaTime*4, 1f);
            if (glow.localScale.x <= 0)
                killGlow = false;
        }

        if (moveToInventory)
        {
            pantalla.position = Vector3.MoveTowards(pantalla.position, targetPos, Time.deltaTime * 20);

            if(Vector3.Distance(pantalla.position, targetPos) < 0.5)
            {
                moveToInventory = false;
            }
        }
    }

    public void Turn()
    {

        targetPos = Camera.main.ScreenToWorldPoint(pantallaCtrl.gameObject.transform.position);

        StartCoroutine(ShowGlow());
    }

    IEnumerator ShowGlow()
    {
        source.clip = win0;
        source.Play();

        growGlow = true;
        yield return new WaitUntil(() => !growGlow);

        pantalla.gameObject.SetActive(true);
        npc.SetActive(false);
        shadow.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        source.clip = win1;
        source.Play();

        killGlow = true;
        yield return new WaitUntil(() => !killGlow);


        pantallaCtrl.gameObject.GetComponent<Animator>().SetTrigger("inventory");

        yield return new WaitForSeconds(0.5f);

        moveToInventory = true;
        yield return new WaitUntil(() => !moveToInventory);

        source.clip = win2;
        source.Play();
        pantallaCtrl.ShowPantalla(index);

        yield return new WaitForSeconds(1.5f);
        pantallaCtrl.gameObject.GetComponent<Animator>().SetTrigger("inventory");

        PlayerPrefs.SetInt("pantallas", PlayerPrefs.GetInt("pantallas") + 1);


        if (gameObject.name != "Player")
        {
            pantallaCtrl.Player.CanMove = true;
            gameObject.SetActive(false);
        }
        else
            pantalla.gameObject.SetActive(false);

    }

}
