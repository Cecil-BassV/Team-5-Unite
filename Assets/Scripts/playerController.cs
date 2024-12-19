using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreMask;

    [Header("----- Stats -----")]
    [SerializeField][Range(1, 10)] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;

    [Header("----- Gun Stats -----")]
    [SerializeField] GameObject gunModel;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField] int maxAmmo; // Maximum ammo capacity
    [SerializeField] int playerAmmoAmount; // UI element for ammo display
    [SerializeField] float reloadTime = 2f; // Time required to reload

    Vector3 moveDir;
    Vector3 playerVel;

    int jumpCount;
    int HPOrig;

    int goalCount;

    bool isShooting;
    bool isSprinting;
    bool isReloading;

    void Start()
    {
        HPOrig = HP;
        playerAmmoAmount = maxAmmo;
        updatePlayerUI();
        updateAmmoUI();
    }

    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

        if(!GameManager.instance.isPaused)
        {
            movement();
            // Select gun will go here.
        }

        sprint();
    }

    void movement()
    {
        if(controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }


        //moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //transform.position += moveDir * speed * Time.deltaTime;

        moveDir = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        controller.Move(moveDir * speed * Time.deltaTime);

        jump();

        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;

        if(Input.GetButton("Fire1") && !isShooting && playerAmmoAmount > 0 && !isReloading)
        {
            StartCoroutine(shoot());
        }

        if((Input.GetKeyDown(KeyCode.R)) && playerAmmoAmount < maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    void jump()
    {
        if(Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
    }

    void sprint()
    {
        if(Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }

    IEnumerator shoot()
    {
         isShooting = true;

         RaycastHit hit;
         if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreMask))
         {
                Debug.Log(hit.collider.name);

                IDamage dmg = hit.collider.GetComponent<IDamage>();
                if (dmg != null)
                {
                    dmg.takeDamage(shootDamage);
                }
         }

         playerAmmoAmount--;
         updateAmmoUI();

         yield return new WaitForSeconds(shootRate);

         isShooting = false;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        GameManager.instance.reloadPrompt(false);
        GameManager.instance.hasReloadPrompt = false;

        yield return new WaitForSeconds(reloadTime);
        playerAmmoAmount = maxAmmo;

        updateAmmoUI();

        isReloading = false;

        Debug.Log("Reloaded!");
    }


    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();
        StartCoroutine(flashScreenDamage());

        if(HP <= 0)
        {
            //Hey I'm dead!
            GameManager.instance.youLose();
        }
    }

    IEnumerator flashScreenDamage()
    {
        GameManager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.playerDamageScreen.SetActive(false);

    }

    public void updatePlayerUI()
    {
        GameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
    }

    public void updateAmmoUI()
    {
        GameManager.instance.updatePlayerAmmoUI(playerAmmoAmount, maxAmmo);

        if (playerAmmoAmount == 0)
        {
            GameManager.instance.reloadPrompt(true);
            GameManager.instance.hasReloadPrompt = true;
        }
    }

    public int GetCurrentAmmo()
    {
        return playerAmmoAmount;
    }

    public int GetMaxAmmo()
    {
        return maxAmmo;
    }

    public void getGunStats(gunStats gun)
    {
        shootDamage = gun.shootDamage;
        shootDist = gun.shootDist;
        shootRate = gun.shootRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.model.GetComponent<MeshRenderer>().sharedMaterial;
    }
}