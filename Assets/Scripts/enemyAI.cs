using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int HP;

    [SerializeField] Transform headPos;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int FOV;
    [SerializeField] int damage;

    [SerializeField] float attackCooldown;
    [SerializeField] float attackRange;

    bool isMauling = false;
    bool playerInRange;

    Color colorOrig;
    Vector3 playerDir;
    float angleToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        colorOrig = model.material.color;
        GameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        float agentSpeed = agent.velocity.normalized.magnitude;
        float animSpeed = anim.GetFloat("Speed");

        anim.SetFloat("Speed", Mathf.MoveTowards(animSpeed, agentSpeed, Time.deltaTime * animSpeedTrans));

        if (playerInRange && canSeePlayer())
        {

        }
    }

    bool canSeePlayer()
    {
        playerDir = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        Debug.DrawRay(headPos.position, playerDir);

        //RaycastHit hit;

        //if (Physics.Raycast(headPos.position, playerDir, out hit))
        //{
            //if (hit.collider.CompareTag("Player") && angleToPlayer < FOV)
            //{
       agent.SetDestination(GameManager.instance.player.transform.position);

       if (agent.remainingDistance < agent.stoppingDistance)
       {
           faceTarget();
       }

       if(playerDir.magnitude <= attackRange)
       {
             Debug.Log("In Range!");

             if (!isMauling)
             {
                 StartCoroutine(maul());
             }

       }


        return true;
    }

    IEnumerator maul()
    {
        isMauling = true;
        // AttackAnimation
        anim.SetTrigger("Attack1");
        // Attack Player
        GameManager.instance.playerScript.takeDamage(damage);
        Debug.Log("ATTACK!!!!");
        
        yield return new WaitForSeconds(attackCooldown);
        isMauling = false;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        agent.SetDestination(GameManager.instance.player.transform.position);
        StartCoroutine(flashRed());

        if (HP <= 0)
        {
            // Dead
            GameManager.instance.updateGameGoal(-1);
            GameManager.instance.updatePlayerPoints(100);
            Destroy(gameObject);
        }

    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }
}
