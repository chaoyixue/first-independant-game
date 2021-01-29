using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInstantiation : MonoBehaviour
{
    // declaration and initialisation of variables
    [SerializeField] GameObject myplayer;
    private bool TheBossShowedUp = false; // variable to monitor if the boss is already shown up
    [SerializeField] float ZvalueForBossShownUp; // the position z attend by the player to trigger the instantiation of the boss
    public GameObject myBossPrefab; // the prefab of the boss object

    // declaration and initialisation of functions

    // the boss showed up when the player is nearby
    void BossShowUp(float position_z_player)
    {  
        // if the player is on the right position
        if(position_z_player > ZvalueForBossShownUp )
        {
            Instantiate(myBossPrefab, new Vector3(0, -2.8f, 10), new Quaternion(0,180,0,1));
            TheBossShowedUp = true;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   if(!TheBossShowedUp)
        {
            BossShowUp(myplayer.transform.position.z);
        }
     
    }
}
