using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    public Player player;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        setAnimation();
    }

    public void setAnimation()
    {
        if (rb.velocity.x != 0 && rb.velocity.y != 0)
        {
            anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
            
        }
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("velocityY", Mathf.Abs(rb.velocity.y));
        anim.SetBool("isRun", (player.speed>500));
        anim.SetBool("isWalk", (player.movementInput.x!=0 || player.movementInput.y!=0));
    }
}
