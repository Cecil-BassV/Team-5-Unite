using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin, menuLose;
    [SerializeField] TMP_Text goalCountText;
    [SerializeField] TMP_Text playerAmmoCount;

    public Image playerHPBar;
    public GameObject playerDamageScreen;
    public GameObject player;
    public playerController playerScript;
    public bool isPaused;

    float timeScaleOrig;

    int goalCount;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }
        UpdateAmmoUI(); // Continuously update ammo UI
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

    public void updateGameGoal(int amount)
    {
        goalCount += amount;
        goalCountText.text = goalCount.ToString("F0");

        if (goalCount <= 0)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
    }

    private void UpdateAmmoUI()
    {
        if (playerAmmoCount != null && playerScript != null)
        {
            playerAmmoCount.text = $"Ammo: {playerScript.GetCurrentAmmo()} / {playerScript.GetMaxAmmo()}";
        }
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }
}
