using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float dir;
    public PlayerController2D controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //saut
        if (Input.GetKeyDown(KeyCode.Space))
        {
            controller.Jump();
        }

        //déplacement horizontal
        dir = Input.GetAxisRaw("Horizontal"); //-1 gauche     1 droite     0 bouge pas

    }

    private void FixedUpdate()
    {
        controller.HorizontalMove(dir);
    }
}
