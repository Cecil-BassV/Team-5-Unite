using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public AudioSource aud;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin, menuLose;
    [SerializeField] GameObject reloadInd;
    [SerializeField] TMP_Text goalCountText;
    [SerializeField] TMP_Text playerAmmoCount;
    [SerializeField] TMP_Text roundNumberText;
    [SerializeField] TMP_Text pointsText;
    [SerializeField] GameObject spawnObj;

    //[Header("----- Game Settings -----")]
    public int roundNumber = 0;

    public int playerPoints;

    public Image playerHPBar;
    public GameObject playerDamageScreen;
    public GameObject player;
    public playerController playerScript;
    public bool isPaused;
    public bool hasReloadPrompt = false;


    float timeScaleOrig;

    public int goalCount;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        updatePlayerPoints(0);
        timeScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == reloadInd)
                reloadPrompt(false);

            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();

                if (hasReloadPrompt == true)
                {
                    reloadPrompt(true);
                }
            }


        }

    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void spawnObject(GameObject spawnObject, Vector3 pos)
    {
        if (spawnObject != null)
        {
            Instantiate(spawnObject, pos, Quaternion.identity);
        }
        else
        {
            // Error
        }
    }

    public void updateGameGoal(int amount)
    {
        goalCount += amount;
        goalCountText.text = goalCount.ToString("F0");

        //if (goalCount <= 0)
        //{
            //statePause();
            //menuActive = menuWin;
            //menuActive.SetActive(true);
            //roundNumber++;
            //updateRoundCount();
            //spawnObject(new Vector3(-24, 2, 18)); // Temp
            
        //}
    }

    public void updateRoundCount()
    {
        roundNumber++;
        roundNumberText.text = roundNumber.ToString("F0");
        
    }

    public void updatePlayerAmmoUI(int currAmmo, int maxAmmmo)
    {
        if(playerScript != null)
        {
            playerAmmoCount.text = currAmmo.ToString("F0") + " / " + maxAmmmo.ToString("F0");
        }

    }

    public void updatePlayerPoints(int amount)
    {
        playerPoints += amount;
        pointsText.text = playerPoints.ToString("F0");
    }

    public void reloadPrompt(bool toggle)
    {
        menuActive = reloadInd;
        menuActive.SetActive(toggle);

        if (toggle != true)
        {
            menuActive = null;
        }
        
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }
}
