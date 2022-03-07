using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator myAnimator;
    public LayerMask whatIsPlayer;
    public Transform player;
    public float openRange;
    private bool playerInOpenRange;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        player = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        playerInOpenRange = Physics.CheckSphere(transform.position, openRange, whatIsPlayer);

        if (playerInOpenRange)
            myAnimator.SetBool("character_nearby", true);
        else
            myAnimator.SetBool("character_nearby", false);
    }
}
