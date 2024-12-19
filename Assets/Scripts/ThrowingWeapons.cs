using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingWeapons : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] LayerMask ignoreMask;
    [SerializeField] Transform Camera;
    [SerializeField] Transform attackPos;
    [SerializeField] GameObject objectToThrow;

    [Header("----- Stats -----")]
    [SerializeField] int knifeDamage;
    [SerializeField] int knifeDist;
    [SerializeField] int throwMax;
    [SerializeField] float throwCooldown;

    [Header("----- Throwing -----")]
    public KeyCode throwKey = KeyCode.Q;
    [SerializeField] float throwForce;
    [SerializeField] float throwUpwardForce;

    bool readyToThrow;

    // Start is called before the first frame update
    void Start()
    {      
        readyToThrow = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && readyToThrow && throwMax > 0)
        {
            Throw();
        }

        void Throw()
        {
            readyToThrow = false;

            GameObject projectile = Instantiate(objectToThrow, attackPos.position, Camera.rotation); // Instantiates the projectile

            if(projectile != null)
            {
                Debug.Log("Projectile instantiated successfully.");

                Destroy(projectile, 1f);  // Destroys gameObject after 1 second if there is no hit
            }                             // Adjust the delay as you see fit
            else
            {
                Debug.LogError("Projectile instantiation failed!");
            }

            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>(); // Gets the rigidbody component of the object

            Vector3 forceDirection = Camera.transform.forward;         
                             

            RaycastHit hit; // Checks for raycast hit
            if (Physics.Raycast(Camera.position, Camera.forward, out hit, 500f, ~ignoreMask))
            {
                forceDirection = (hit.point - attackPos.position).normalized;

                if (hit.collider.CompareTag("Enemy")) // Checks if object is hit by an enemy tag
                {
                    IDamage dmg = hit.collider.GetComponent<IDamage>(); // Interacts with the IDamage interface
                    if (dmg != null)
                    {
                        dmg.takeDamage(knifeDamage);
                    }

                    Destroy(hit.collider.gameObject); // Destroys gameObject
                }              
            }
           
         
            Vector3 forceAdd = forceDirection * throwForce + transform.up * throwUpwardForce;
            projectileRb.AddForce(forceAdd, ForceMode.Impulse);

            throwMax--;

            Invoke(nameof(ResetThrow), throwCooldown); // Resets throw after cooldown
        }
    }

    void ResetThrow()
    {
        readyToThrow = true;
    }

    public void takeDamage(int amount)
    {
        throw new System.NotImplementedException();
    }
}
