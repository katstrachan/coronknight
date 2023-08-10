using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class gameManagerController : MonoBehaviour
{
    
    float timerLimit = 180f;
    public AudioClip[] aClips;
    public AudioClip buttonNoise;
    int waveQuantity = 5;
    int levelCounter;
    int enemyCapturedInitially;
    int[] waveSpawn;
    float spawnTimerLimit;
    int enemyDefenders;
    int allyDefenders;
    float beaconRespawnInterval;
    int iEnemyLimit;
    int bHEnemyLimit;
    int spawnBoxEnemyLimit;
    float healthChanceThresh;
    UIManager UI;
    bool menuShown = false;
    public GameObject blankscreenObj;
    CanvasGroup blankscreen;
   
    
    float iEnemyProb;
    float bHEnemyProb;
    AudioSource myAudio;
    GameObject[] beacons;
    GameObject[] spawnBoxes;
    
    GameObject player;

    List<bool> beaconCaptureStatus = new List<bool>();
  
    float waveTimer;
    bool waveTimerEnabled = false;

    int waveCounter = 1;

    Vector3 playerSpawnPoint = new Vector3(-46.32f, 1.64f, 50.9f);

    private void Awake()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        iEnemyProb = PlayerPrefs.GetFloat("iEnemyProb");
        bHEnemyProb = PlayerPrefs.GetFloat("bHEnemyProb");
        enemyCapturedInitially = PlayerPrefs.GetInt("enemyCapturedInitially");
        spawnTimerLimit = PlayerPrefs.GetFloat("spawnTimerLimit");
        enemyDefenders = PlayerPrefs.GetInt("enemyDefenders");
        allyDefenders = PlayerPrefs.GetInt("allyDefenders");
        beaconRespawnInterval = PlayerPrefs.GetFloat("beaconRespawnInterval");
        iEnemyLimit = PlayerPrefs.GetInt("iEnemyLimit");
        bHEnemyLimit = PlayerPrefs.GetInt("bHEnemyLimit");
        healthChanceThresh = PlayerPrefs.GetFloat("healthChanceThresh");

    }

    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
       
        blankscreen = blankscreenObj.GetComponent<CanvasGroup>();
        setAudioVolume();
        spawnBoxEnemyLimit = iEnemyLimit + bHEnemyLimit;
        UI = GetComponent<UIManager>();
        waveTimer = timerLimit;


        initialiseLevel();

        showStartCutscene();
        initialiseUI();
        UI.displayStartUI();
        startStopWaveTimer();

    }

    private void Update()
    {
        if(Input.GetButtonDown("Escape"))
        {
            if(menuShown)
            {
                
                removeUIMenu();
            }
            else
            {
                
                showUIMenu();
            }
            
        }
         
            //if wave is completed
            if (waveTimer <= 0f)
        {
          
            startStopWaveTimer();
            endWave();
        }
        //if finalwave and timer != 0, check if player has captured all beacons and won game
        else if (waveCounter == waveQuantity)
        {
          
            bool won = checkWin();
            if (won)
            {
                startStopWaveTimer();
                win();
            }
            else
            {
                timerTick();
            }
        }
        //add time to clock if nothing else applies
        else
        {
            timerTick();
        }
    }

    void setAudioVolume()
    {
        
        float vol = PlayerPrefs.GetFloat("volume");
        
        if(vol == 0f)
        {
            myAudio.volume = 0;
        }
        else if(vol == 0.2f)
        {
            myAudio.volume = 0.05f;
        }
        else
        {
            myAudio.volume = (vol - 0.2f);
        }
        
       
    }

    public AudioClip[] sendAllyNoises()
    {
        AudioClip[] clipsToSend = { aClips[0], aClips[1], aClips[2], aClips[6], aClips[7] }; 
        return clipsToSend;
    }

    public AudioClip[] sendEnemyNoises()
    {
        AudioClip[] clipsToSend = { aClips[3], aClips[4], aClips[5], aClips[8] }; 
        return clipsToSend;
    }

    public float getHealthChanceThreshold()
    {
        return healthChanceThresh;
    }


    //end the current wave
    void endWave()
    {
        bool passedWave = checkLose(); //check if player lost

        //if final wave, check if player has all beacons, if not, lose
        if (waveCounter == waveQuantity)
        {
            bool won = checkWin();
            if (won)
            {
                win();
            }
            else
            {
                lose();
            }

        }
        //if they passed the wave, and next wave is final, set up final wave rules
        else if (passedWave && (waveCounter == (waveQuantity - 1)))
        {
            waveCounter += 1;
            initialiseNextWave(true);
        }
        //if they didnt pass the wave, lose
        else if (!passedWave)
        {
            
            lose();
        }
        //else increase wave and set up
        else
        {
            waveCounter += 1;
            initialiseNextWave(false); // sapwns some health, UI etc
        }
    }


    void initialiseNextWave(bool finalWave)
    {
        resetWaveTimer();   
        updateWaveUI(finalWave);
        startStopWaveTimer();
    }


    //initial set up of the level 
    void initialiseLevel()
    {
        beacons = GameObject.FindGameObjectsWithTag("beacon");
        spawnBoxes = GameObject.FindGameObjectsWithTag("spawnBox");
        setInitialSpawnBoxes();
        setInitialBeaconStatuses();
      
    }

    void setInitialSpawnBoxes()
    {

        for(int i = 0; i< spawnBoxes.Length; i ++)
        {
            boxSpawnManager spawnBox = spawnBoxes[i].GetComponent<boxSpawnManager>();
            spawnBox.setBoxEnemyQuantity(iEnemyLimit, bHEnemyLimit, spawnBoxEnemyLimit);
            spawnBox.setEnemyProbabilities(iEnemyProb, bHEnemyProb);
            spawnBox.setRespawnInterval(spawnTimerLimit);
            spawnBox.initialiseSpawnBox();
        }
    }

    void setInitialBeaconStatuses()
    {
        int numOfEnemyBeaconsChosen = 0;
        bool[] beaconIndicesChosen = { false, false, false, false, false, false };


        while(numOfEnemyBeaconsChosen != enemyCapturedInitially)
        {
            int r = Random.Range(0, 5);
            if(beaconIndicesChosen[r] == false && beacons[r].name != "playerSpawnBeacon")
            {
                beaconIndicesChosen[r] = true;
                numOfEnemyBeaconsChosen += 1;
            }

        }

        for(int i = 0; i < beacons.Length; i++)
        {
            if(beaconIndicesChosen[i] == true)
            {
                beaconStats beaconStatus = beacons[i].GetComponent<beaconStats>();
                setDefenders(beacons[i]);
                beaconStatus.setEnemyCaptured(true);
            }
            else
            {
                beaconStats beaconStatus = beacons[i].GetComponent<beaconStats>();
                setDefenders(beacons[i]);
                beaconStatus.setPlayerCaptured(true);
            }
        }
        
       

    }

    void setDefenders(GameObject beacon)
    {
        beaconSpawnManager beaconSM = beacon.GetComponent<beaconSpawnManager>();
        beaconSM.setDefenderQuantity(allyDefenders, enemyDefenders);
        beaconSM.setRespawnInterval(beaconRespawnInterval);
    }

 

    //win game 
    public void win()
    {
        UI.displayWinUI();
       
        StartCoroutine(fadeOutToWin());
        
    }

    //lose game
    public void lose()
    {
        UI.displayLoseUI();
        StartCoroutine(fadeOutToLose());
    }

    //check for losing conditions
    public bool checkLose()
    {
        for(int i = 0; i < beacons.Length; i++)
        {
            bool playerCaptured = beacons[i].GetComponent<beaconStats>().getIsPlayerCaptured();
            if(playerCaptured)
            {
                return true;
            }
        }
        return false;
    }

    //check for win conditions on final wave
    public bool checkWin()
    {
        for (int i = 0; i < beacons.Length; i++)
        {
            bool playerCaptured = beacons[i].GetComponent<beaconStats>().getIsPlayerCaptured();
            if (!playerCaptured)
            {
                return false;
            }
        }
        return true;
    }

    public void timerTick()
    {
        if(waveTimerEnabled)
        {
            waveTimer -= Time.deltaTime;
            updateTimerUI(waveTimer);
        }
    }

    void resetWaveTimer()
    {
        waveTimer = timerLimit;
        updateTimerUI(waveTimer);
    }

    //start wave timer
    void startStopWaveTimer()
    {
        waveTimerEnabled = !waveTimerEnabled;
    }

    public void updateTimerUI(float timer)
    {
        UI.updateTime(timer);
    }

    //initialise UI for player
    void initialiseUI()
    {
        UI.initialiseLevel(waveQuantity, timerLimit, (6-enemyCapturedInitially));
    }

    void updateWaveUI(bool finalWave)
    {
        UI.updateWave(finalWave);
    }

    void showStartCutscene()
    {
        StartCoroutine(fadeIn());
    }

    public IEnumerator fadeIn()
    {
        float timer = 0;
        float transitionTime = 1;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            blankscreen.alpha = Mathf.Lerp(1, 0, timer / transitionTime);
            yield return null;
        }
        blankscreenObj.SetActive(false);
        yield return null;
    }

    public IEnumerator showCutscene() 
    {
       
        float timer = 0;
        float transitionTime = 1;
        float vol = myAudio.volume;
       

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            blankscreen.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            myAudio.volume = Mathf.Lerp(vol, 0, timer / transitionTime);
            
            yield return null;
        }
        yield return StartCoroutine(changeToWinScene());
    }

    public void showUIMenu()
    {
        menuShown = true;
        playPauseEverything(true);
        UI.showMenu();
    }

    public void removeUIMenu()
    {
        menuShown = false;
        myAudio.PlayOneShot(buttonNoise);
        playPauseEverything(false);
        UI.hideMenu();
    }

    
    public void playPauseEverything(bool pauseGame)
    {
        if(pauseGame)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }

    public IEnumerator fadeOutToLevelSel()
    {
        
        blankscreenObj.SetActive(true);
        float timer = 0;
        float transitionTime = 1;
        float vol = myAudio.volume;
       

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            blankscreen.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            myAudio.volume = Mathf.Lerp(vol, 0, timer / transitionTime);
            
            yield return null;
        }
        yield return StartCoroutine(changeToLevelSelect());
    }

    public IEnumerator fadeOutToLose()
    {
        blankscreenObj.SetActive(true);
        float timer = 0;
        float transitionTime = 1;
        float vol = myAudio.volume;
       
        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            blankscreen.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            myAudio.volume = Mathf.Lerp(vol, 0, timer / transitionTime);
           
            yield return null;
        }
        yield return StartCoroutine(changeToLoseScene());
    }

    public IEnumerator fadeOutToWin()
    {
        blankscreenObj.SetActive(true);
        float timer = 0;
        float transitionTime = 1;
        float vol = myAudio.volume;
        

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            blankscreen.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            myAudio.volume = Mathf.Lerp(vol, 0, timer / transitionTime);
           
            yield return null;
        }
        yield return StartCoroutine(changeToWinScene());
    }

    public IEnumerator changeToWinScene()
    {
        //PlayerPrefs.SetFloat("finaltime", waveTimer);
        SceneManager.LoadScene("Win");
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        yield return null;
    }
    public IEnumerator changeToLoseScene()
    {
        SceneManager.LoadScene("Lose");
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        yield return null;
    }
    
    public IEnumerator changeToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
        
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        yield return null;
    }

    public void returnToMapScene()
    {
        myAudio.PlayOneShot(buttonNoise);
        playPauseEverything(false);
        StartCoroutine(fadeOutToLevelSel());
        
    }


}
