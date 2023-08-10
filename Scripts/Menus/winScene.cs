using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class winScene : MonoBehaviour
{

    AudioSource myAudio;
    public AudioClip dogBark;
    public AudioClip menuAppear;
    public GameObject blankscrnObj;
    CanvasGroup blankscrn;
    public GameObject titleBtn;
    public GameObject lvlSelBtn;
    public GameObject infoObj;
    CanvasGroup info;
    Button title;
    Button lvlSelect;
    bool titleFlag = false;
    bool lvlSelFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        myAudio.volume = PlayerPrefs.GetFloat("volume");
        info = infoObj.GetComponent<CanvasGroup>();
        title = titleBtn.GetComponent<Button>();
        lvlSelect = lvlSelBtn.GetComponent<Button>();
        blankscrn = blankscrnObj.GetComponent<CanvasGroup>();
        blankscrnObj.SetActive(true);
        enableDisableButtons(false, false);
        StartCoroutine(fadeIn(0.99f, 0));
    }
    private void Update()
    {
        if(blankscrn.alpha == 1)
        {
            if(titleFlag)
            {
                //change scene to title
                SceneManager.LoadScene("Menu");
            }
            else if(lvlSelFlag)
            {
                //changeTo level
                SceneManager.LoadScene("LevelSelect");
            }
        }
    }

    public void killCharacter()
    {

    }

    public void toTitle()
    {
        enableDisableButtons(false, false);
        titleFlag = true;
        StartCoroutine(fadeOut(0, 1));
    }

    public void toLevelSelect()
    {
        enableDisableButtons(false, false);
        lvlSelFlag = true;
        StartCoroutine(fadeOut(0, 1));
    }

    public IEnumerator fadeInInfo (float start, float end)
    {
        float timer = 0;
        float transitionTime = 1;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            info.alpha = Mathf.Lerp(start, end, timer / transitionTime);
            yield return null;
        }
        myAudio.PlayOneShot(menuAppear, 1);
        yield return StartCoroutine(enableAfterFade());
    }

    public IEnumerator fadeIn(float start, float end)
    {
        float timer = 0;
        float transitionTime = 1;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            blankscrn.alpha = Mathf.Lerp(start, end, timer / transitionTime);
            yield return null;
        }
        myAudio.PlayOneShot(dogBark, 1);
        myAudio.PlayOneShot(dogBark, 1);
        yield return StartCoroutine(fadeInInfo(0, 1));
    }

    public IEnumerator fadeOut(float start, float end)
    {
        blankscrnObj.SetActive(true);
        float timer = 0;
        float transitionTime = 1;
        float vol = myAudio.volume;
        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            blankscrn.alpha = Mathf.Lerp(start, end, timer / transitionTime);
            myAudio.volume = Mathf.Lerp(vol, 0, timer / transitionTime);
            yield return null;
        }
    }

    public IEnumerator enableAfterFade()
    {
        blankscrnObj.SetActive(false);
        enableDisableButtons(true, true);
        
        yield return null;
    }

    void enableDisableButtons(bool t, bool ls)
    {
        title.interactable = t;
        lvlSelect.interactable = ls;
    }
}
