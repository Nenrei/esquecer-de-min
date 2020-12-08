using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantallasController : MonoBehaviour
{

    [SerializeField] PlayerMovement player;
    [SerializeField] Animator anim;
    [SerializeField] Image[] pantallas;
    [SerializeField] AudioSource source;

    public PlayerMovement Player { get => player; set => player = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.CanMove)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                anim.SetTrigger("inventory");
            }
        }
    }

    public void ShowPantalla(int index)
    {
        Color tempColor = pantallas[index].color;
        tempColor.r = 1;
        tempColor.g = 1;
        tempColor.b = 1;
        tempColor.a = 1;
        pantallas[index].color = tempColor;
    }
}
