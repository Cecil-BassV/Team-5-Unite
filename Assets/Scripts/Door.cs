using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] GameObject UI;
    [SerializeField] int doorTax;

    bool playerInTrigger;

    private void Update()
    {
        if (playerInTrigger)
        {
            if (Input.GetButtonDown("Interact") && GameManager.instance.playerPoints >= doorTax)
            {
                door.SetActive(false);
                GameManager.instance.updatePlayerPoints(GameManager.instance.playerPoints -= doorTax);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        playerInTrigger = true;

        IOpen open = other.GetComponent<IOpen>();

        if (open != null)
        {
            UI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
            return;

        playerInTrigger = true;

        IOpen open = other.GetComponent<IOpen>();

        if (open != null)
        {
            UI.SetActive(false);
        }
    }
}
