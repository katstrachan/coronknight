using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class storyCutscene : MonoBehaviour
{
    
    public Sprite[] bookPages;
    AudioSource myAudio;
    public AudioClip bookFlick;
    public AudioClip closeBook;
    float volume = 1f;
    
    int pageIndex = 0;
  
    public GameObject bookObj;

    CanvasGroup book;
    int numOfPages = 9;
    public GameObject rightBtnObj;
    Button right;
    Button left;
    public GameObject leftBtnObj;
    public GameObject bookImgObj;
    Image bookImg;
    
 
    bool changeSceneFlag = false;

    public GameObject finalTextAObj;
    public GameObject finalTextBObj;
    public GameObject finalTextCObj;

    CanvasGroup finalTextA;
    CanvasGroup finalTextB;
    CanvasGroup finalTextC;

    public GameObject introTextAObj;
    public GameObject introTextBObj;
    public GameObject introTextCObj;
    public GameObject introTextDObj;
    public GameObject introTextEObj;
    public GameObject introTextFObj;

    CanvasGroup introTextA;
    CanvasGroup introTextB;
    CanvasGroup introTextC;
    CanvasGroup introTextD;
    CanvasGroup introTextE;
    CanvasGroup introTextF;

    public GameObject blankscrnObj;
    CanvasGroup blankscrn;

    public GameObject skipBtn;
    Button skipp;

    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        myAudio.volume = PlayerPrefs.GetFloat("volume");
        skipp = skipBtn.GetComponent<Button>();
        skipp.interactable = true;
        blankscrn = blankscrnObj.GetComponent <CanvasGroup>();
        introTextA = introTextAObj.GetComponent<CanvasGroup>();
        introTextB = introTextBObj.GetComponent<CanvasGroup>();
        introTextC = introTextCObj.GetComponent<CanvasGroup>();
        introTextD = introTextDObj.GetComponent<CanvasGroup>();
        introTextE = introTextEObj.GetComponent<CanvasGroup>();
        introTextF = introTextFObj.GetComponent<CanvasGroup>();


        right = rightBtnObj.GetComponent<Button>();
        left = leftBtnObj.GetComponent<Button>();
        finalTextA = finalTextAObj.GetComponent<CanvasGroup>();
        finalTextB = finalTextBObj.GetComponent<CanvasGroup>();
        finalTextC = finalTextCObj.GetComponent<CanvasGroup>();

        book = bookObj.GetComponent<CanvasGroup>();
        bookImg = bookImgObj.GetComponent<Image>();
        bookObj.SetActive(false);

        
        StartCoroutine(fadeInTextA());
    }

    public IEnumerator fadeInTextA()
    {

        float timer = 0;
        float transitionTime = 3;
        

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            introTextA.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            yield return null;
        }
        yield return StartCoroutine(fadeInTextB());

    }

    public IEnumerator fadeInTextB()
    {

        float timer = 0;
        float transitionTime = 3;


        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            introTextB.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            yield return null;
        }
        yield return StartCoroutine(fadeInTextC());

    }

    public IEnumerator fadeInTextC()
    {

        float timer = 0;
        float transitionTime = 3;


        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            introTextC.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            yield return null;
        }
        yield return StartCoroutine(fadeInTextD());

    }

    public IEnumerator fadeInTextD()
    {

        float timer = 0;
        float transitionTime = 4;


        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            introTextD.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            yield return null;
        }
        yield return StartCoroutine(fadeInTextE());

    }

    public IEnumerator fadeInTextE()
    {

        float timer = 0;
        float transitionTime = 3;


        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            introTextE.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            yield return null;
        }
        yield return StartCoroutine(fadeInTextF());

    }

    public IEnumerator fadeInTextF()
    {

        float timer = 0;
        float transitionTime = 3;


        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            introTextF.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            yield return null;
        }
        yield return StartCoroutine(waitIntro());

    }

    public IEnumerator waitIntro()
    {

        float timer = 0;
        float transitionTime = 5f;


        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            
            yield return null;
        }
        yield return StartCoroutine(fadeOutText());

    }

   

    public IEnumerator fadeOutText()
    {
        float timer = 0;
        float transitionTime = 5;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            introTextA.alpha = Mathf.Lerp(1, 0, timer / transitionTime);
            introTextB.alpha = Mathf.Lerp(1, 0, timer / transitionTime);
            introTextC.alpha = Mathf.Lerp(1, 0, timer / transitionTime);
            introTextD.alpha = Mathf.Lerp(1, 0, timer / transitionTime);
            introTextE.alpha = Mathf.Lerp(1, 0, timer / transitionTime);
            introTextF.alpha = Mathf.Lerp(1, 0, timer / transitionTime);
            yield return null;
        }
        yield return StartCoroutine(fadeInBook(0, 1));
    }

    public IEnumerator fadeInBook(float start, float end)
    {
        float timer = 0;
        float transitionTime = 5;
        bookObj.SetActive(true);
        left.interactable = false;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            book.alpha = Mathf.Lerp(start, end, timer / transitionTime);
            yield return null;
        }
        
            
        yield return null;
        
    }

    public IEnumerator fadeOutBook(float start, float end)
    {
        float timer = 0;
        float transitionTime = 5;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            book.alpha = Mathf.Lerp(start, end, timer / transitionTime);
            yield return null;
        }
        
            yield return StartCoroutine(fadeInFinalA());
    }
    public IEnumerator fadeInFinalA()
    {
        float timer = 0;
        float transitionTime = 5;
        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            finalTextA.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            yield return null;
        }

        yield return StartCoroutine(fadeInFinalB());
    }

    public IEnumerator fadeInFinalB()
    {
        float timer = 0;
        float transitionTime = 5;
        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            finalTextB.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            yield return null;
        }

        yield return StartCoroutine(fadeInFinalC());
    }


    public IEnumerator fadeInFinalC()
    {
        float timer = 0;
        float transitionTime = 5;
        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            finalTextC.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            yield return null;
        }

        yield return StartCoroutine(finalWait());
    }
    public IEnumerator finalWait()
    {
        float timer = 0;
        float transitionTime = 5;
        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(fadeOutFinal());
    }
    public IEnumerator fadeOutFinal()
    {
        float timer = 0;
        float transitionTime = 5;
        float vol = myAudio.volume;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            finalTextA.alpha = Mathf.Lerp(1, 0, timer / transitionTime);
            finalTextB.alpha = Mathf.Lerp(1, 0, timer / transitionTime);
            finalTextC.alpha = Mathf.Lerp(1, 0, timer / transitionTime);
            myAudio.volume = Mathf.Lerp(vol, 0, timer / transitionTime);
            yield return null;
        }
        changeSceneFlag = true;
        yield return null;
    }

 
    public void nextPage()
    {
        
        
        if (pageIndex < numOfPages)
        {
            
            pageIndex += 1;
            changePage(pageIndex);
            left.interactable = true;
        }
        else if (pageIndex == numOfPages)
        {
            right.interactable = false;
            left.interactable = false;
            StartCoroutine(fadeOutBook(1, 0));
        }
    
    }

    public void prevPage()
    {
        
        if (pageIndex > 0)
        {
            
            pageIndex -= 1;
            changePage(pageIndex);
        }
        if (pageIndex == 0)
        {
            left.interactable = false;
        }
        
    }

    void changePage(int page)
    {
        if(pageIndex == 9)
        {
            myAudio.PlayOneShot(closeBook, volume);
            
        }
        else
        {
            myAudio.PlayOneShot(bookFlick, volume);
            
        }
        bookImg.sprite = bookPages[page];

    }

    public void changeScene()
    {

        SceneManager.LoadScene("LevelSelect");
    }

    private void Update()
    {
        if(changeSceneFlag)
        {
            changeScene();
        }
    }

   public void skip()
    {
        skipp.interactable = false;
        StartCoroutine(fadeOutSkip());
    }

    public IEnumerator fadeOutSkip()
    {
        float timer = 0;
        float transitionTime = 2;
        float vol = myAudio.volume;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            blankscrn.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            myAudio.volume = Mathf.Lerp(vol, 0, timer / transitionTime);
            yield return null;
        }
        changeSceneFlag = true;
        yield return null;
    }

}
