using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    /* declaration and initialisation of variables */

    // audiosource of the player
    private AudioSource myaudioSource;

    // audio clip walking
    public AudioClip walkingPlayerSound;

    // audio clip light attacking
    public AudioClip LightAttackSound;

    // audio clip heavy attacking
    public AudioClip HeavyAttackSound;

    // boss layer for the detection of damaging the boss
    public LayerMask bossLayer;

    // collider
    private Collider colliderPlayer;

    // set a period of immortal frame
    //[SerializeField] float immortal_frame = 0.5f; // secondes
    //private float lastTimeAttacked = -999;

    // the index of the scene active now
    private int sceneIndex;

    // check if we are on pause
    public bool TfPaused = false;

    //  up down speed
    [SerializeField] float upDownSpeed = 0.05f;

    // layermask for ladders
    [SerializeField] LayerMask LayerLadder;
    // the maximum distance for the raycast
    [SerializeField] float DistanceLadder = 2f;
    // check if on the way climbing
    private bool isClimbing;
    // check if on the way climbing down
    private bool isClimbingDown;

    // modes of attack to control between light attack, heavy attck
    private uint modeAttack;

    // set an attack rate
    [SerializeField] float attackRate = 2f;

    // lightAttack Damage
    public float LightAttackDamage = 40f;

    // HeavyAttack Damage
    public float HeavyAttackDamage = 50f;

    // variable to check if we can attack once more
    float nextAttackTime = 0f;

    // object to set the AttackPoint of the player
    public Transform AttackPoint;
    // the attack range of the weapon
    [SerializeField] float attackRange = 1.5f;

    // check the collier with enemies
    public LayerMask enemyLayers;

   
    // the move speed of the player
    [SerializeField] float moveSpeed = 3f;

    // the force of jumping
    [SerializeField] float jumpForce = 5f;

    // check if in the air already
    private bool inTheAir = true;

    private float axis_horizontal = 0f;

    // check if space is pressed 
    private bool spaceKeyDown = false;

    // get the rigid body
    private Rigidbody rigidBodyComponent;

    // get the camera in the scene
    private Camera mainCamera;

    // layer mask for all the other layers except player
    [SerializeField] LayerMask playerMask;

    //private bool isRunningForward = false;
    private Animator myAnimator; // 用来获取player的animator



    // transform beneath the feet
    [SerializeField] private Transform groundCheckTransform = null;

    // the player transform
    private Transform myTransform;

    // the orientation of the player true for the axis z positive 
    private bool faceZpositive = true;

    // mode of weapons
    [SerializeField] private int weaponMode = 0; //0: sword //1: gun

    // change the percentage of hp of the health bar
    public HealthBar healthBar;

    public float HpMax = 5.0f; // maxHp of the player
    public float HpCurrent = 5.0f; // currentHp of the player




    /* Definition and Initialisation of the functions */

    // function to take damage
    public void TakeDamage(float ValueDamage)
    {
            // decrease the current Hp
            HpCurrent -= ValueDamage;
            //change the percentage of the healthBar
            healthBar.SetHealth(HpCurrent);
            // play the underAttack animation
            myAnimator.SetTrigger("UnderAttack");
        
        
        
    }


    // climbing ladders 
    private void ClimbingLadders()
    {
        // ray cast from our player to the direction x, if the ray interacts with the ladder, hit ladder = true
        bool hit_ladder = Physics.Raycast(myTransform.position, new Vector3(1, 0, 0), DistanceLadder, LayerLadder);
        Debug.DrawLine(myTransform.position, new Vector3(myTransform.position.x + DistanceLadder, myTransform.position.y, myTransform.position.z));
        if (hit_ladder)
        {
            if (rigidBodyComponent.useGravity == true)
            {
                rigidBodyComponent.useGravity = false; // disable the gravity of the player
                colliderPlayer.enabled = false; // disabled the collider
                
            }
            
            // Debug.Log(rigidBodyComponent.useGravity);
            // now on the ladder if we tapped w, we can climb the ladder
            if (Input.GetKey(KeyCode.W))
            {
                isClimbing = true;
            }
            else
            {
                isClimbing = false;
            }
            // on the ladder and tapped down can control the player to climb down the ladder
            if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
            {
                isClimbingDown = true;
            }
            else
            {
                isClimbingDown = false;
            }
        }

        else
        {
            if (rigidBodyComponent.useGravity == false)
            {
                rigidBodyComponent.useGravity = true; // renable the gravity of the player
                colliderPlayer.enabled = true;
            }

            
            //Debug.Log(rigidBodyComponent.useGravity);
        }
        // climbing and climbing down realized by translate
        if (isClimbing)
        {


            myTransform.Translate(Vector3.up * upDownSpeed);



        }
        if (isClimbingDown)
        {

            myTransform.Translate(Vector3.down * upDownSpeed);
        }
    }

    // draw in the editor
    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
    }

    // attack
    void Attack(uint mode)
    {
        // light attack mode
        if (mode == 0)
        {
            LightAttack();
        }
        // heavy attack mode
        if (mode == 1)
        {
            HeavyAttack();
        }
        // check the skeletons 
        Collider[] hitEnemies = Physics.OverlapSphere(AttackPoint.position, attackRange, enemyLayers);

        // damage skeletons
        foreach (Collider enemy in hitEnemies)
        {
            if (mode == 0)
            {
                enemy.GetComponent<skeleton_AI>().TakeDamage(LightAttackDamage);
            }
            else if (mode == 1)
            {
                enemy.GetComponent<skeleton_AI>().TakeDamage(HeavyAttackDamage);
            }

        }
        // check the demon boss
        Collider[] hitBoss = Physics.OverlapSphere(AttackPoint.position, attackRange, bossLayer);

        // damage skeletons
        foreach (Collider enemy in hitBoss)
        {
            Debug.Log("entering the collider with the boss");
            if (mode == 0)
            {
                enemy.GetComponent<BossTakeDamage>().take_damage(LightAttackDamage);
            }
            else if (mode == 1)
            {
                enemy.GetComponent<BossTakeDamage>().take_damage(HeavyAttackDamage);
            }

        }
    }

    // light attack
    void LightAttack()
    {
        // animation of light attack
        myAnimator.SetTrigger("IsLightAttack");

    }

    // heavy attack
    void HeavyAttack()
    {
        // 播放重攻击动画
        myAnimator.SetTrigger("IsHeavyAttack");
    }

    // die function triggered when the hp is < 0
    void Die()
    {
        
        Debug.Log(" sceneIndex now :" + sceneIndex);
        // reload the scene
        SceneManager.LoadScene(sceneIndex);
    }

    // get the position of the mouse
    Vector3 GetPointToLook()
    {
        Vector3 pointToLook = new Vector3(0f, 0f, 0f);
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane verticalPlane = new Plane(new Vector3(1f, 0f, 0f), Vector3.zero);
        float rayLength;
        if (verticalPlane.Raycast(cameraRay, out rayLength))
        {
            pointToLook = cameraRay.GetPoint(rayLength);
        }
        return pointToLook;
    }

    

    // get horizontal axis value
    float GetAxisHorizontal()
    {
        return Input.GetAxis("Horizontal");
    }

    //check if space is tapped
    bool TfSpaceDown()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    // jump triggered by key space
    void Jump(bool spaceKeyEtat, float jumpPower, bool inTheAir)
    {
        if (spaceKeyEtat && (!inTheAir))
        {
            rigidBodyComponent.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
        }
        spaceKeyDown = false;

    }

    // move forward and backward
    void Move(float axis_horizontal, bool faceOrientation)
    {
        //Debug.Log(axis_horizontal);

        // 如果面朝z轴正半轴
        if (faceOrientation)
        {
            // 沿着z轴正半轴方向根据 axis的值调整移动的方向，移动的距离为两帧之间的间隔时间乘以移动速度
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed * axis_horizontal);
        }
        else if (!faceOrientation)
        {
            // 沿着z轴正半轴方向根据 axis的值调整移动的方向，移动的距离为两帧之间的间隔时间乘以移动速度
            transform.Translate(Vector3.back * Time.deltaTime * moveSpeed * axis_horizontal);
        }
    }

    // running forward animation
    void RunningAnimationCheck(float axisHorizontal)
    {
        //如果水平方向的变化很大的话，则启用跑步动作
        if (axisHorizontal > 0.95 || axisHorizontal < -0.95)
        {
            myAnimator.SetBool("isRunningForward", true);
        }
        else
        {
            myAnimator.SetBool("isRunningForward", false);
        }
    }

    // walking forward animation
    void WalkingAnimationCheck(float axisHorizontal)
    {
        // 如果水平方向变化较小，则启用走路动作
        if ((axisHorizontal > 0 && axisHorizontal <= 0.95) || (axisHorizontal < 0 && axisHorizontal >= -0.95))
        {
            myAnimator.SetBool("isWalkingForward", true);
        }
        else
        {
            myAnimator.SetBool("isWalkingForward", false);
        }
    }

    // change the orientation of the player
    void ChangeOrientation(float axisHorizontal)
    {
        var rotationVector = myTransform.rotation.eulerAngles;
        if (axisHorizontal > 0)
        {
            // not turing around
            rotationVector.y = 0;
            // turn  to axis z positive
            faceZpositive = true;
        }
        else if (axisHorizontal < 0)
        {
            // turn around 180 degrees
            rotationVector.y = 180;
            // turn to axis z negative
            faceZpositive = false;
        }
        transform.rotation = Quaternion.Euler(rotationVector);
    }

    // check if already in the air
    bool CheckTfInTheAir(Transform groundCheckTransform)
    {
        // a sphere collider on the feet to check if collider with the ground
        return Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0;
    }




    // Start is called before the first frame update
    void Start()
    {
        // 获取rigidbody
        rigidBodyComponent = GetComponent<Rigidbody>();
        // playermask 变更为 011111111
        playerMask = ~playerMask;
        // 获取animator的信息 并同调myAnimator
        myAnimator = GetComponent<Animator>();
        //get the transform of the player
        myTransform = GetComponent<Transform>();
        //get the camera object
        mainCamera = FindObjectOfType<Camera>();
        // get the collider of the player
        colliderPlayer = GetComponent<Collider>();

        HpCurrent = HpMax;
        healthBar.SetMaxHealth(HpMax);

        // get the index of the current scene
        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        myaudioSource = GetComponent<AudioSource>();
    }

    // public function to play the walking clip
    public void PlayWalkingSound()
    {
        // playing the walking sound
        myaudioSource.clip = walkingPlayerSound;

        myaudioSource.Play();
    }

    // public function to play the light attack sound clip
    public void PlayLightAttackSound()
    {
        // playing the light attack clip
        myaudioSource.clip = LightAttackSound;
        myaudioSource.Play();
    }

    // public function to play the heavy attack sound clip
    public void  PlayHeavyAttackSound()
    {
        // playing the heavy attack clip
        myaudioSource.clip = HeavyAttackSound;
        myaudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time >= nextAttackTime)
        {

            // press J to light attack
            if (Input.GetKeyDown(KeyCode.J))
            {
                Attack(0);
                // set the next time to attack
                nextAttackTime = Time.time + 1f / attackRate;
            }

            // press I to heavy attack
            if (Input.GetKeyDown(KeyCode.I))
            {
                Attack(1);
                // set the next time to attack
                nextAttackTime = Time.time + 1f / attackRate;
            }

        }

       
        // get horizontal axis value
        axis_horizontal = GetAxisHorizontal();
        // check if the player is on the ground
        inTheAir = CheckTfInTheAir(groundCheckTransform);
        //Debug.Log(inTheAir);
        // check if space is tapped
        spaceKeyDown = TfSpaceDown();
        // running animation trigger
        RunningAnimationCheck(axis_horizontal);
        // walking animation trigger
        WalkingAnimationCheck(axis_horizontal);
        // change the orientation of the player
        ChangeOrientation(axis_horizontal);

        // move forward and backward trigger
        Move(axis_horizontal, faceZpositive);
        // jump trigger
        Jump(spaceKeyDown, jumpForce, inTheAir);

        // if hp is lower than zero die
        if (HpCurrent <= 0)
        {
            Die();
        }

    }

    private void FixedUpdate()
    {
        // check if the player is climbing and do something for it
        ClimbingLadders();

    }



    //Triggers
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("in trigger");

        if (other.gameObject.name == "Ground") // if fall down to the ground ,then load scene
        {
            // load the current activate scene
            SceneManager.LoadScene(sceneIndex);
        }
        if (other.gameObject.name == "aircraft")
        {
            //Debug.Log("in trigger aircraft");
        }
        if (other.gameObject.tag == "skeleton") // if collider with skeleton, then HP--
        {
            Debug.Log("in trigger skle");
            TakeDamage(1);

            //Debug.Log("hp: " + HpCurrent);
            //Debug.Log("in trigger skle");
        }


    }


}
