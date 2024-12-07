using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    [SerializeField] int damageAmount;
    [SerializeField] int speed;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other) // Idea: attach collider trigger to the front of zombie: when zombie is close enough to the player collider, player is attacked and takes damage
    {
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if(dmg != null)
        {
            dmg.takedamage(damageAmount);
        }
    }

}
