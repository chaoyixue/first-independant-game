using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography;
using UnityEngine;

public class skeleton_AI : MonoBehaviour
{
    /* Definition and initialisation of the variables  */

    // seuil platform to check if the player and the skeleton is at the same platform
    float seuilPlatform = 5.0f;

    // latent delay to control the checkpoint of the attack in second
    private float attackDelay = 0.8f;
    private float lastAttack = -900;

    // attack Power
    [SerializeField] float attackPower = 2.0f;

    // playerLayer
    public LayerMask LayerPlayer;

    // Attack point of the skeleton
    public Transform AttackPoint;
    // Attack distance of the skeleton
    [SerializeField] float AttackRange = 0.5f;

    // the max hp of skeleton
    public float maxHealth = 100;
    // the current hp of skeleton
    private float currentHealth;

    // the coordinates z for  start and end position 
    [SerializeField] float startPosition = 1f;
    [SerializeField] float endPosition = 6f;

    private float positionZ;    
    [SerializeField] float moveSpeed = 2.0f;    // control the speed
    private bool faceOrientation = true; // check if the direction faced by the skeleton is z positif 
    private Transform mySkeltonTransform;
    // get the transform of the player
    private Transform myplayerTransform;

    // a seuil to check if the player and the skeleton are at the same height
    float seuilAxisY = 0.1f;

    // a seuil to check if the player is nearby on the axis z
    float seuilAxisZ = 2f;

    // variable used to get the animator of the skeleton
    private Animator mySkeltonAnimator;



    // the state of action
    private int state_of_action = 0;

    /* Definition and initialisation of the functions */

    // draw in the editor
    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }

    // AttackFuntion damage the player in the attackRange
    public void Attack()
    {
        // Debug.Log("attack!");
        Collider[] playerCollided = Physics.OverlapSphere(AttackPoint.position, AttackRange, LayerPlayer);
        foreach (Collider playerInRange in playerCollided)
        {
            // damage the player
            playerInRange.GetComponent<Player>().TakeDamage(attackPower);
        }
    }

    // die function
    void Die()
    {
        // animation of death

        Destroy(this.gameObject);

    }

    // function triggered when attacked
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        // under attack animation

        // if the hp is 0 call the die function
        if (currentHealth <= 0)
        {
            Die();
        }

    }


    // action mode 1 : move following the axis z between startposition and endposition
    private void MoveAutomatic(float startPositionZ, float endPositionZ)
    {
        var rotationVector = mySkeltonTransform.rotation.eulerAngles;
        if (transform.position.z > endPositionZ || transform.position.z < startPositionZ)
        {
            if (rotationVector.y == 0)
            {
                rotationVector.y = 180;
            }
            else
            {
                rotationVector.y = 0;
            }
            // rotate to the orientation
            transform.rotation = Quaternion.Euler(rotationVector);
        }
        // move forward
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        mySkeltonAnimator.SetBool("WalkingOntheStage", true);
    }

    // action mode 2 : if the player is on the same platform then the skeleton will follow him
    private void FollowPlayerSameHeight()
    {
        // look at the direction of the player 
        transform.LookAt(new Vector3(0 , mySkeltonTransform.position.y, myplayerTransform.position.z));
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        mySkeltonAnimator.SetBool("WalkingOntheStage", true);

    }

    // action mode 3 : if the player is nearby, the skeleton will attack
    private void AttackInShortDistance()
    {
        float distanceY = System.Math.Abs(myplayerTransform.position.y - mySkeltonTransform.position.y);
        float distanceZ = System.Math.Abs(myplayerTransform.position.z - mySkeltonTransform.position.z);
        if (distanceY < seuilAxisY && distanceZ < seuilAxisZ)
        {
            // play the attack animation
            mySkeltonAnimator.SetBool("PlayerIntheAttackDistance", true);
            mySkeltonAnimator.SetBool("WalkingOntheStage", false);
            


        }
        else
        {
            mySkeltonAnimator.SetBool("PlayerIntheAttackDistance", false);
        }
    }




    // Start is called before the first frame update
    void Start()
    {
        // the current hp set to th max hp
        currentHealth = maxHealth;

        // get the transform of the skeleton
        mySkeltonTransform = GetComponent<Transform>();
        GameObject thePlayer = GameObject.Find("DogPBR");
        // get the transform of the player
        myplayerTransform = thePlayer.GetComponent<Transform>();
        // get the animator of the skeleton
        mySkeltonAnimator = this.GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > lastAttack + attackDelay)
        {
            AttackInShortDistance();
            lastAttack = Time.time;
        }
        
        // Debug.Log(mySkeltonAnimator.GetBool("PlayerIntheAttackDistance"));

        if (!mySkeltonAnimator.GetBool("PlayerIntheAttackDistance")) // if not in the attack mode
        {
            // if the skeleton is at the same height of the player or aka on the same platform
            if (System.Math.Abs(myplayerTransform.position.y - mySkeltonTransform.position.y) < seuilAxisY && System.Math.Abs(myplayerTransform.position.z - mySkeltonTransform.position.z) < seuilPlatform)


            {
                // follow the player
                FollowPlayerSameHeight();
            }

            else // if not walking automatically
            {
                
                MoveAutomatic(startPosition, endPosition);
            }
        }
    }

    private void FixedUpdate()
    {


    }

}
