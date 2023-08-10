using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class mainMenuManager : MonoBehaviour
{
    AudioSource myAudio;
    public AudioClip buttonPress;
    public AudioClip bookTurn;
    public AudioClip menuSwitch;
    public AudioClip bark;
    public GameObject[] torches;
    public GameObject sliderObj;
    Slider mySlider;
    
    public GameObject mainMenuObject;
    public GameObject howToPlayMenuObject;
    CanvasGroup mainMenu;
    CanvasGroup howToPlayMenu;
    public Camera cam;
    Vector3 mainMenuCamPos = new Vector3(369.95f, 55.87f, 258.5f);
    Quaternion mainMenuCamRot = Quaternion.Euler(14.495f, 95.619f, 3.851f);
    Vector3 howToPlayMenuCamPos = new Vector3(322.025f,53.056f, 182.753f);
    Quaternion howToPlayMenuCamRot = Quaternion.Euler(-6.339f, 20.77f, 0.04f);
    public GameObject bookImgObj;
    Image bookImg;
    public Sprite[] bookPages;
    int pageCounter = 0;
    public GameObject rightBtnObj;
    Button right;
    Button left;
    public GameObject leftBtnObj;
    public GameObject blankscreenObj;
    CanvasGroup blankscreen;
    bool changeScene = false;
    public GameObject playBtn;
    public GameObject howToPlayBtn;
    public GameObject quitBtn;

    Button quit;
    Button play;
    Button howToPlay;


    // Start is called before the first frame update
    void Start()
    {
        mySlider = sliderObj.GetComponent<Slider>();
        myAudio = GetComponent<AudioSource>();
        PlayerPrefs.SetFloat("volume", 1f);
        myAudio.volume = PlayerPrefs.GetFloat("volume");
        quit = quitBtn.GetComponent<Button>();
        play = playBtn.GetComponent<Button>();
        howToPlay = howToPlayBtn.GetComponent<Button>();
        blankscreen = blankscreenObj.GetComponent<CanvasGroup>();
        blankscreenObj.SetActive(true);
        right = rightBtnObj.GetComponent<Button>();
        left = leftBtnObj.GetComponent<Button>();
        bookImg = bookImgObj.GetComponent<Image>();
        mainMenu = mainMenuObject.GetComponent<CanvasGroup>();
        howToPlayMenu = howToPlayMenuObject.GetComponent<CanvasGroup>();
        howToPlayMenu.alpha = 0;
        howToPlayMenuObject.SetActive(false);
        mainMenuEnableDisableButtons(false, false, false);
        StartCoroutine(fadeIn(0.99f, 0));



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
        yield return StartCoroutine(fadeInInfo(0, 1));
    }

    public IEnumerator fadeInInfo(float start, float end)
    {
        float timer = 0;
        float transitionTime = 2;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            mainMenu.alpha = Mathf.Lerp(start, end, timer / transitionTime);
            yield return null;
        }
        yield return StartCoroutine(enableAfterFade());
    }

    public IEnumerator enableAfterFade()
    {

        blankscreenObj.SetActive(false);
        mainMenuEnableDisableButtons(true, true, true);
        yield return null;
    }
   

    void mainMenuEnableDisableButtons(bool p, bool htp, bool q)
    {
        play.interactable = p;
        howToPlay.interactable = htp;
        quit.interactable = q;
    }



    public void makeActive(CanvasGroup menu, GameObject menuObj, bool active)
    {
        if(active)
        {
            menuObj.SetActive(true);

        }
        else
        {
            menuObj.SetActive(false);
        }
       
    }

    public void fade(CanvasGroup menu)
    {
        float fadeToVal;
        if(menu.alpha == 0)
        {
            fadeToVal = 1;
        }
        else
        {
            fadeToVal = 0;
        }

    }

    public IEnumerator fadeMenu(CanvasGroup menu, float startVal, float endVal)
    {
        float timer = 0;
        float transitionTime = 2;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            menu.alpha = Mathf.Lerp(startVal, endVal, timer / transitionTime);
            yield return null;
        }
        myAudio.PlayOneShot(bark);
        yield return null;
        
    }

    public void changeToHowToPlayMenu()
    {
        
        StartCoroutine(fadeMenu(mainMenu, mainMenu.alpha, 0));
        makeActive(mainMenu, mainMenuObject, false);
        StartCoroutine(transitionCamera(mainMenuCamPos, mainMenuCamRot, howToPlayMenuCamPos, howToPlayMenuCamRot, howToPlayMenu, 1));
        makeActive(howToPlayMenu, howToPlayMenuObject, true);
        myAudio.PlayOneShot(buttonPress);
        myAudio.PlayOneShot(menuSwitch);
        changePage(0);
        pageCounter = 0;
        left.interactable = false;
        right.interactable = true;
        
    }

    public void changeToMainMenu()
    {
        myAudio.PlayOneShot(buttonPress);
        myAudio.PlayOneShot(menuSwitch);
        StartCoroutine(fadeMenu(howToPlayMenu, howToPlayMenu.alpha, 0));
        makeActive(howToPlayMenu, howToPlayMenuObject, false);
        StartCoroutine(transitionCamera(howToPlayMenuCamPos, howToPlayMenuCamRot, mainMenuCamPos, mainMenuCamRot, mainMenu, 1));
        makeActive(mainMenu, mainMenuObject, true);
    }

    public IEnumerator transitionCamera(Vector3 startPos, Quaternion startRot, Vector3 endPos, Quaternion endRot, CanvasGroup menu, float fadeToVal)
    {
        float timer = 0;
        float transitionTime = 3;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            cam.transform.position = Vector3.Lerp(startPos, endPos, timer / transitionTime);
            cam.transform.rotation = Quaternion.Lerp(startRot, endRot, timer / transitionTime);
            yield return null; 
        }
        yield return StartCoroutine(fadeMenu(menu, menu.alpha, fadeToVal));

    }

    public void quitGame()
    {
        myAudio.PlayOneShot(buttonPress);
        Application.Quit();
    }

    public void playGame()
    {

        mainMenuEnableDisableButtons(false, false, false);
        changeScene = true;
        myAudio.PlayOneShot(buttonPress);
        StartCoroutine(fadeOut(0, 1));
    }

    public void toHowToPlay()
    {

        changeToHowToPlayMenu();
    }

    public void backToMain()
    {
        changeToMainMenu();
    }

    public void leftBtnClick()
    {
        if(pageCounter > 0)
        {
            pageCounter -= 1;
            changePage(pageCounter);
            right.interactable = true;
        }
        if(pageCounter == 0)
        {
            left.interactable = false;
        }
    }

    public void rightBtnClick()
    {
        if (pageCounter < 2)
        {
            pageCounter += 1;
            changePage(pageCounter);
            left.interactable = true;
        }
        if(pageCounter == 2)
        {
            right.interactable = false;
        }
        
    }

    void changePage(int page)
    {
        myAudio.PlayOneShot(bookTurn);
        bookImg.sprite = bookPages[page];
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
    private void Update()
    {
        if(blankscreen.alpha == 1)
        {
            if (changeScene)
            {
                toCutscene();

            }
        }
    }
        
    public void changeVol()
    {
        PlayerPrefs.SetFloat("volume", mySlider.value);
        myAudio.volume = mySlider.value;
        for (int i = 0; i < torches.Length; i++)
        {
            torches[i].GetComponent<AudioSource>().volume = mySlider.value;
        }
    }

    void toCutscene()
    {
        SceneManager.LoadScene("StoryCutscene");
    }
}
