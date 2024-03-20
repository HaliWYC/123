using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Execute_Sign : MonoBehaviour
{
    private Animator anim;
    public GameObject signSprite;
    public bool canExecute;
    public Transform characterTransform;

    private void Awake()
    {
        anim = signSprite.GetComponent<Animator>(); 
    }

    private void Update()
    {
        if (GetComponentInParent<EnemyController>().dizzytime > 0)
            canExecute = true;
        else
            canExecute = false;
        signSprite.GetComponent<SpriteRenderer>().enabled = canExecute;
        signSprite.transform.localScale = characterTransform.localScale;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().canExecute = canExecute;
        }
    }


}
