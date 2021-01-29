using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    // declaration and initialisation of variables
    public float AttackDamage = 20;
    public float enrageAttackDamage = 30;

    // Attack point of the boss
    public Transform AttackPoint;
    // the attack range of the boss
    public float AttackRange = 1.0f;
    public LayerMask LayerPlayer;

    // attack function of the boss
    public void Attack()
    {
        Collider[] playerCollided = Physics.OverlapSphere(AttackPoint.position, AttackRange, LayerPlayer);
        foreach (Collider playerInRange in playerCollided)
        {
            // debug
            Debug.Log("the player is damaged");

            // damage the player
            playerInRange.GetComponent<Player>().TakeDamage(AttackDamage);
            
        }

    }

    // attack function of the boss when enraged
    public void EnragedAttack()
    {
        Collider[] playerCollided = Physics.OverlapSphere(AttackPoint.position, AttackRange, LayerPlayer);
        foreach (Collider playerInRange in playerCollided)
        {
            // debug
            Debug.Log("the player is damaged");

            // damage the player
            playerInRange.GetComponent<Player>().TakeDamage(enrageAttackDamage);

        }
    }

    // draw in the editor
    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
