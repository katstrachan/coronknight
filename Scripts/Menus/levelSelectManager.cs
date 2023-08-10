using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class levelSelectManager : MonoBehaviour
{
    public GameObject levelInfoObj;
    int levelSelect = 0;
    public Sprite[] levelImgs;
    string[] levelTexts = { "E a s y", "N o r m a l", "H a r d" };
    public GameObject levelImgObj;
    public GameObject levelTextObj;
    public GameObject blankscreenObj;
    Image levelImg;
    Text levelText;

    AudioSource myAudio;
    public AudioClip buttonClick;
    float volume = 1f;

    CanvasGroup levelInfo;
    CanvasGroup blankscreen;

    public GameObject rightBtn;
    public GameObject leftBtn;
    public GameObject playBtn;
    public GameObject titleBtn;

    Button right;
    Button left;
    Button play;
    Button title;

    bool menuFlag = false;
    bool playFlag = false;

    int pageIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        myAudio.volume = PlayerPrefs.GetFloat("volume");
        levelText = levelTextObj.GetComponent<Text>();
        levelImg = levelImgObj.GetComponent<Image>();
        levelInfo = levelInfoObj.GetComponent<CanvasGroup>();
        blankscreen = blankscreenObj.GetComponent<CanvasGroup>();

        right = rightBtn.GetComponent<Button>();
        left = leftBtn.GetComponent<Button>();
        play = playBtn.GetComponent<Button>();
        title = titleBtn.GetComponent<Button>();
        enableDisableButtons(false, false, false, false);
        setLevelInfo(0);
        StartCoroutine(fadeIn(0.99f, 0));
    }


    public void leftBtnClick()
    {
        right.interactable = true;
        if (pageIndex > 0)
        {
            pageIndex -= 1;
            changePage(pageIndex);
        }
        if (pageIndex == 0)
        {
            left.interactable = false;
        }
        else
        {
            left.interactable = true;
        }
    }

    public void rightBtnClick()
    {
        left.interactable = true;
        if (pageIndex < 2)
        {
            pageIndex += 1;
            changePage(pageIndex);
        }
        if (pageIndex == 2)
        {
            right.interactable = false;
        }
        else
        {
            right.interactable = true;
        }
    }

    void setLevelInfo(int level)
    {
        levelSelect = level;
        levelImg.sprite = levelImgs[level];
        levelText.text = levelTexts[level];

    }

    void changePage(int i)
    {
        myAudio.PlayOneShot(buttonClick, volume);
        setLevelInfo(i);

    }

    public void playLevel()
    {

        enableDisableButtons(false, false, false, false);
        myAudio.PlayOneShot(buttonClick, volume);
        StartCoroutine(fadeOut(0, 1));
        playFlag = true;
    }

    void enableDisableButtons(bool r, bool l, bool p, bool t)
    {
        right.interactable = r;
        left.interactable = l;
        play.interactable = p;
        title.interactable = t;
    }

    public void backToMainMenu()
    {
        myAudio.PlayOneShot(buttonClick, volume);
        enableDisableButtons(false, false, false, false);
        StartCoroutine(fadeOut(0, 1));
        menuFlag = true;
    }

    public IEnumerator fadeIn(float start, float end)
    {
        float timer = 0;
        float transitionTime = 1;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            blankscreen.alpha = Mathf.Lerp(start, end, timer / transitionTime);
            yield return null;
        }
        yield return StartCoroutine(enableAfterFade());
    }

    public IEnumerator fadeOut(float start, float end)
    {
        blankscreenObj.SetActive(true);
        float timer = 0;
        float transitionTime = 1;
        float vol = myAudio.volume;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            blankscreen.alpha = Mathf.Lerp(start, end, timer / transitionTime);
            myAudio.volume = Mathf.Lerp(vol, 0, timer / transitionTime);
            yield return null;
        }
    }

    public IEnumerator enableAfterFade()
    {
        blankscreenObj.SetActive(false);
        enableDisableButtons(true, false, true, true);
        yield return null;
    }

    private void Update()
    {
        if (blankscreen.alpha == 1)
        {
            if (menuFlag == true)
            {
                SceneManager.LoadScene("Menu");
            }
            else if (playFlag == true)
            {
                setLevelVariables();
                SceneManager.LoadScene("pre-levelcutscene");

            }
        }

    }

    void setLevelVariables()
    {
        if (levelSelect == 0)
        {
            PlayerPrefs.SetInt("enemyCapturedInitially", 2);
            PlayerPrefs.SetFloat("spawnTimerLimit", 5f);
            PlayerPrefs.SetInt("enemyDefenders", 2);
            PlayerPrefs.SetInt("allyDefenders", 5);
            PlayerPrefs.SetFloat("beaconRespawnInterval", 10f);
            PlayerPrefs.SetInt("iEnemyLimit", 1);
            PlayerPrefs.SetInt("bHEnemyLimit", 1);
            PlayerPrefs.SetFloat("healthChanceThresh", 5f);
            PlayerPrefs.SetFloat("iEnemyProb", 5f);
            PlayerPrefs.SetFloat("bHEnemyProb", 5f);




        }
        else if (levelSelect == 1)
        {
            PlayerPrefs.SetInt("enemyCapturedInitially", 3);
            PlayerPrefs.SetFloat("spawnTimerLimit", 5f);
            PlayerPrefs.SetInt("enemyDefenders", 2);
            PlayerPrefs.SetInt("allyDefenders", 4);
            PlayerPrefs.SetFloat("beaconRespawnInterval", 10f);
            PlayerPrefs.SetInt("iEnemyLimit", 2);
            PlayerPrefs.SetInt("bHEnemyLimit", 2);
            PlayerPrefs.SetFloat("healthChanceThresh", 5f);
            PlayerPrefs.SetFloat("iEnemyProb", 5f);
            PlayerPrefs.SetFloat("bHEnemyProb", 5f);
        }
        else if(levelSelect == 2)
        {
            PlayerPrefs.SetInt("enemyCapturedInitially", 3);
            PlayerPrefs.SetFloat("spawnTimerLimit", 4f);
            PlayerPrefs.SetInt("enemyDefenders", 3);
            PlayerPrefs.SetInt("allyDefenders", 4);
            PlayerPrefs.SetFloat("beaconRespawnInterval", 10f);
            PlayerPrefs.SetInt("iEnemyLimit", 2);
            PlayerPrefs.SetInt("bHEnemyLimit", 2);
            PlayerPrefs.SetFloat("healthChanceThresh", 5f);
            PlayerPrefs.SetFloat("iEnemyProb", 5f);
            PlayerPrefs.SetFloat("bHEnemyProb", 5f);
        }

    }
    
}


