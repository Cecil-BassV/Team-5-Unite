using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingWeapons : MonoBehaviour
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

            GameObject projectile = Instantiate(objectToThrow, attackPos.position, Camera.rotation);

            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            if(projectileRb == null)
            {
                Debug.LogError("Projectile prefab doesn't have a Rigidbody component!");
                return;
            }
         
            Vector3 forceDirection = Camera.transform.forward;

            RaycastHit hit;

            if (Physics.Raycast(Camera.position, Camera.forward, out hit, 500f, ~ignoreMask))
            {
                forceDirection = (hit.point - attackPos.position).normalized;
            }

            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {              
                    dmg.takeDamage(knifeDamage);                    
            }
            else
            {
                Debug.Log($"Object {hit.collider.gameObject.name} does not implement IDamage");
            }

            Vector3 forceAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

            projectileRb.AddForce(forceAdd, ForceMode.Impulse);

            throwMax--;

            Invoke(nameof(ResetThrow), throwCooldown);
        }
    }

    void ResetThrow()
    {
        readyToThrow = true;
    }
}
