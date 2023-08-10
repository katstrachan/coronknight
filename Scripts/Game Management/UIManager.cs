using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    AudioSource myAudio;
    public AudioClip menuAppear;
    public AudioClip menuDisappear;
    public AudioClip buttonNoise;
    public AudioClip startNoise;
    public AudioClip UIAlert;
    public AudioClip fail;
    public AudioClip win;
    Text[] waveDisplay = new Text[2];
    Text beaconDisplay;
    Text waveTimer;
    Slider playerHealthBar;
    GameObject[] beaconsHealthBars = new GameObject[6];
    GameObject[] beaconIcons = new GameObject[6];
    public ParticleSystem enemyCaptureParticles;
    public ParticleSystem playerCaptureParticles;
    public Material playerBeaconMaterial;
    public Material enemyBeaconMaterial;
    public GameObject menuUI;

    public GameObject waveStartObj;
    public GameObject waveEndObj;
    public GameObject finalWaveStartObj;
    public GameObject finalWaveEndObj;
    public GameObject playerCapturedObj;
    public GameObject enemyCapturedObj;
    public GameObject failedObj;
    public GameObject winObj;

    CanvasGroup waveStart;
    CanvasGroup waveEnd;
    CanvasGroup finalWaveEnd;
    CanvasGroup finalWaveStart;
    CanvasGroup playerCaptured;
    CanvasGroup enemyCaptured;
    CanvasGroup failed;
    CanvasGroup won;
    float vol;

    // Start is called before the first frame update
    private void Awake()
    {
        waveStart = waveStartObj.GetComponent<CanvasGroup>();
        waveEnd = waveEndObj.GetComponent<CanvasGroup>();
        finalWaveEnd = finalWaveEndObj.GetComponent<CanvasGroup>();
        finalWaveStart = finalWaveStartObj.GetComponent<CanvasGroup>();
        playerCaptured = playerCapturedObj.GetComponent<CanvasGroup>();
        enemyCaptured = enemyCapturedObj.GetComponent<CanvasGroup>();
        failed = failedObj.GetComponent<CanvasGroup>();
        won = winObj.GetComponent<CanvasGroup>();



        waveDisplay[0] = GameObject.Find("Canvas/left UI/wd_1").GetComponent<Text>();
        waveDisplay[1] = GameObject.Find("Canvas/left UI/wd_2").GetComponent<Text>();
        myAudio = GetComponent<AudioSource>();
        beaconDisplay = GameObject.Find("Canvas/Beacon Info/bd_1").GetComponent<Text>();
        playerHealthBar = GameObject.Find("Canvas/left UI/Health Bar").GetComponent<Slider>();

        waveTimer = GameObject.Find("Canvas/Mid UI/TimerBox/td_1").GetComponent<Text>();
        menuUI = GameObject.Find("Canvas/menu");
        menuUI.SetActive(false);


    }
    private void Start()
    {
        vol = PlayerPrefs.GetFloat("volume");
    }

    public void showMenu()
    {

        myAudio.PlayOneShot(menuAppear, vol);
        menuUI.SetActive(true);

    }

    public void hideMenu()
    {
        myAudio.PlayOneShot(menuDisappear, vol);
        menuUI.SetActive(false);
    }


    public void initialiseLevel(int waveLimit, float timer, int playerControlledBeacons)
    {

        //beacons display
        beaconDisplay.text = playerControlledBeacons.ToString();

        //wave number
        waveDisplay[0].text = "1";
        waveDisplay[1].text = waveLimit.ToString();

        //timer
        updateTime(timer);
       
        
    }

    public void updateWave(bool finalWave)
    {
        if(finalWave)
        {
            StartCoroutine(show(finalWaveStart, UIAlert));
        }
        else
        {
            StartCoroutine(show(waveEnd, UIAlert));
        }
        
        waveDisplay[0].text = increase(waveDisplay[0].text);
    }

    public void initialisePlayerHealthBar(int health)
    {
        playerHealthBar.maxValue = health;
        playerHealthBar.value = health;
    }
    public void setPlayerHealthBar(int health)
    {
        playerHealthBar.value = health;
    }

    public void updateBeaconDisplay(bool increment, bool setup)
    {
        if(increment)
        {
            if (!setup)
            {
                StartCoroutine(show(playerCaptured, UIAlert));
            }
            beaconDisplay.text = increase(beaconDisplay.text);
        }
        else
        {
            if(!setup)
            {
                StartCoroutine(show(enemyCaptured, UIAlert));
            }
            
            beaconDisplay.text = decrease(beaconDisplay.text);
        }
    }

   

    public void updateBeaconIcon(GameObject beacon, bool playerCaptured)
    {
        GameObject beaconIcon = beacon.transform.Find("beaconQuad").gameObject;
        MeshRenderer beaconMesh = beaconIcon.GetComponent<MeshRenderer>();
        if(playerCaptured)
        {
            beaconMesh.material = playerBeaconMaterial;
        }
        else
        {
            beaconMesh.material = enemyBeaconMaterial;
        }
    }


    public void updateTime(float timer)
    {
       
        timer += 1;

        float mins = Mathf.FloorToInt(timer / 60);
        float secs = Mathf.FloorToInt(timer % 60);

        waveTimer.text = string.Format("{0:00}:{1:00}", mins, secs);

    }

    string decrease(string textDigit)
    {
        int intDigit = (int.Parse(textDigit)) - 1;
        return intDigit.ToString();
    }

    string increase(string textDigit)
    {
        int intDigit = (int.Parse(textDigit)) + 1;
        return intDigit.ToString();
    }

    public void displayStartUI()
    {
        StartCoroutine(show(waveStart, startNoise));
    }

    public void displayWinUI()
    {
        StartCoroutine(show(won, win));
    }

    public void displayLoseUI()
    {
        StartCoroutine(show(failed, fail ));
    }

    public IEnumerator show(CanvasGroup element, AudioClip clip)
    {
        
        float timer = 0;
        float transitionTime = 1;
        

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            element.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            yield return null;
        }
        myAudio.PlayOneShot(clip, vol);
        yield return StartCoroutine(UIWait(element));
    }

    public IEnumerator UIWait(CanvasGroup element)
    {

        float timer = 0;
        float transitionTime = 1;
       

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            
            yield return null;
        }
        yield return StartCoroutine(hide(element));
    }
    public IEnumerator hide(CanvasGroup element)
    {
        float timer = 0;
        float transitionTime = 1;
      

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            element.alpha = Mathf.Lerp(1, 0, timer / transitionTime);
            yield return null;
        }
        yield return null;
    }

}
