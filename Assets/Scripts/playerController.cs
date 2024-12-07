using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreMask;

    [Header("----- Stats -----")]
    [SerializeField] [Range(1, 5)] int speed;
    [SerializeField] [Range(2, 5)] int sprintMod;
    [SerializeField] [Range(1, 3)] int jumpMax;
    [SerializeField] [Range(5, 20)] int jumpSpeed;
    [SerializeField] [Range(15, 40)] int gravity;


    Vector3 moveDir;
    Vector3 playerVel;

    int jumpCount;

    bool isSprinting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement();

        sprint();
    }

    void movement()
    {
        moveDir = (transform.forward * Input.GetAxis("Vertical")) + 
                  (transform.right * Input.GetAxis("Horizontal"));

        controller.Move(moveDir * speed * Time.deltaTime);

        jump();

        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;

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
        if (Input.GetButtonDown("Sprint"))
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
}
