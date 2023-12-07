using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponChange : MonoBehaviour
{

    public AnimatorOverrideController weaponOverrideController;
    public string weaponName;
    //  is to pick up the weapon
    public void PickUp(GameObject player)
    {
        playerMovements playerScript = player.GetComponent<playerMovements>();

        if (playerScript)
        {
            // If the player already has a weapon, drop it
            if (playerScript.HasWeapon())
            {
                playerScript.DropWeapon();
            }
            playerScript.EquipWeapon(weaponOverrideController);
            
        }

    }
}