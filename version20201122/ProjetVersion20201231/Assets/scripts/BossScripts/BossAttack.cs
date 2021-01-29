using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : StateMachineBehaviour
{
    // latent delay to control the checkpoint of the attack in second
    public float attackDelay = 1.0f;
    private float lastAttack = -900;
    [SerializeField] int number_float_wait = 16;
    // damage of the boss
    public float damage = 20;

    // get access to the boss weapon
    BossWeapon bossWeapon;
    // define a compteur to count the number of frames passed
    int compteur_frame = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossWeapon = animator.GetComponent<BossWeapon>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if(Time.time > lastAttack + attackDelay)
        {   compteur_frame += 1;
            if(compteur_frame >= number_float_wait)
            {
                Debug.Log("the boss is attacking the player");
                bossWeapon.Attack();
                // reset the counter
                compteur_frame = 0;
                lastAttack = Time.time;
            }
            
            
            
            
        }
            
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
