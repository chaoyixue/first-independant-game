using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTakeDamage : MonoBehaviour
{
    // max hp
    public float maxHp = 500;
    public float currentHp;

    // let the boss not get attacked when transforming to enraged mode
    public bool isImmortal = false;

    // get access to the healthbar of the boss
    public HealthBar mybossHealthBar;

    // get access to the boss died text
    public GameObject TextBossDie;
    private float durationText = 3.0f;

    // get access to the door
    public GameObject door;


    // function to die
    private void Die()
    {
        this.GetComponent<Animator>().SetTrigger("Die");
    }

    // function to take damage
    public void take_damage(float damage)
    {
        Debug.Log("the boss is attacked");
        // if the boss is not in the immortal mode
        if(!isImmortal)
        {
            currentHp -= damage;
        }
        
        // change the hp showing on the health bar
        mybossHealthBar.SetHealth(currentHp);
        // if the boss hp is lower than 1/4
        if (currentHp <= maxHp * 1/4)
        {
            // the boss is enraged
            this.GetComponent<Animator>().SetBool("IsEnraged", true);
        }

        if(currentHp<=0)
        {
            Die();
            // show the text that boss has been eliminated
            TextBossDie.SetActive(true);
            StartCoroutine(waitForSec());
            // turn the door accessible
            door.SetActive(true);

        }
    }

    IEnumerator waitForSec()
    {
        yield return new WaitForSeconds(durationText);
        Debug.Log("we will destroy the object");
        GameObject.Destroy(TextBossDie);
    }


    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
        // fill the health bar
        mybossHealthBar.SetMaxHealth(maxHp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
