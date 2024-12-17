using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingWeapons : MonoBehaviour
{
    [Header("----- Components -----")]
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
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Throw();
        }

        void Throw()
        {
            readyToThrow = false;

            GameObject projectile = Instantiate(objectToThrow, attackPos.position, Camera.rotation);

            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            Vector3 forceDirection = Camera.transform.forward;

            RaycastHit hit;

            if(Physics.Raycast(Camera.position, Camera.forward, out hit, 500f))
            {
                forceDirection = (hit.point - attackPos.position).normalized;
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
