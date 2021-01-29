using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_run : StateMachineBehaviour
{
    // locate the player
    Transform playerTransform;
    // Rigidbody of the boss
    Rigidbody RbBoss;
    // speed of the boss
    [SerializeField] float speedBoss = 1;
    // Boss script
    Boss boss;

    // check distance seuil for the player and the boss
    public float attackRangeBoss = 2.0f;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // locate the player
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        // get the rigidbody of the boss
        RbBoss = animator.GetComponent<Rigidbody>();

        boss = animator.GetComponent<Boss>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // find the target position of the player move towards the position z of our player
        Vector3 target_player = new Vector3(0, RbBoss.position.y, playerTransform.position.z);
        // look at the player
        boss.LookAtPlayer();
        // Move to the player
        Vector3 newPos = Vector3.MoveTowards(RbBoss.position, target_player, speedBoss * Time.fixedDeltaTime);
        RbBoss.MovePosition(newPos);

        // check the distance between the player and the boss
        if (Vector3.Distance(playerTransform.position, RbBoss.position) <= attackRangeBoss)
        {
            // trigger to the animation of attacking
            animator.SetTrigger("Attack");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // when leave this walking state, we reset the attack trigger
        animator.ResetTrigger("Attack");
    }

}
