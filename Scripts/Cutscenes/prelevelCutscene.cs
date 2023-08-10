using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class prelevelCutscene : MonoBehaviour
{
    public GameObject blankscreenObj;
    public Camera myCam;
    CanvasGroup blankscreen;
    public GameObject infoAObj;
    public GameObject infoBObj;
    public GameObject infoCObj;
    public GameObject infoDObj;
    CanvasGroup infoA;
    CanvasGroup infoB;
    CanvasGroup infoC;
    CanvasGroup infoD;
    AudioSource myAudio;
    public AudioClip displayMessageNoise;
    public AudioClip buttonPress;
    public GameObject ambience;
    public GameObject[] torches;
    Vector3 startEndpos = new Vector3(294.74f, 56.85f, 213.14f);
    Quaternion startEndrot = Quaternion.Euler(24.565f, 100.84f, 0f);
    Vector3 Apos = new Vector3(338.1f, 50.57f, 201.7f);
    Quaternion Arot = Quaternion.Euler(4.11f, 107.63f, 0.489f);
    Vector3 Bpos = new Vector3(417.1f, 72.31f, 216.9f);
    Quaternion Brot = Quaternion.Euler(27.442f, -43.4f, 6.668f);
    Vector3 Cpos = new Vector3(367.2f, 59.76f, 257.26f);
    Quaternion Crot = Quaternion.Euler(19.483f, -80.19f, 2.472f);
    Vector3 Dpos = new Vector3(170.73f, 76.37f, 224.8f);
    Quaternion Drot = Quaternion.Euler(27.378f, 24.475f, 6.604f);
    Vector3 Epos = new Vector3(293.86f, 80.13f, 152.49f);
    Quaternion Erot = Quaternion.Euler(27.378f, 24.475f, 6.604f);
    Vector3 Fpos = new Vector3(295.3f, 84.44f, 84.96f);
    Quaternion Frot = Quaternion.Euler(27.723f, 10.828f, 5.984f);
    Vector3 Ipos = new Vector3(465.9f, 79.1f, 85.7f);
    Quaternion Irot = Quaternion.Euler(25.831f, -39.4f, 2.986f);
    // Start is called before the first frame update
    void Start()
    {
        blankscreen = blankscreenObj.GetComponent<CanvasGroup>();
        infoA = infoAObj.GetComponent<CanvasGroup>();
        infoB = infoBObj.GetComponent<CanvasGroup>();
        infoC = infoCObj.GetComponent<CanvasGroup>();
        infoD = infoDObj.GetComponent<CanvasGroup>();
        myAudio = GetComponent<AudioSource>();
        setVolume();

        StartCoroutine(fadeIn());
    }

    void setVolume()
    {
        float vol = PlayerPrefs.GetFloat("volume");
        if (vol == 0f)
        {
            myAudio.volume = 0f;
        }
        else if (vol == 0.15f)
        {
            myAudio.volume = 0.15f;
        }
        else
        {
            myAudio.volume = (vol - 0.15f);
        }
        for (int i = 0; i < torches.Length; i++)
        {
            torches[i].GetComponent<AudioSource>().volume = vol;
        }
        ambience.GetComponent<AudioSource>().volume = vol;

    }

    public void buttonClick()
    {
        StartCoroutine(fadeOutSkip());
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
        yield return StartCoroutine(moveToA());
    }

    public IEnumerator moveToA()
    {
        float timer = 0;
        float transitionTime = 4;
        Vector3 myPos = myCam.transform.position;
        Quaternion myRot = myCam.transform.rotation;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            myCam.transform.position = Vector3.Lerp(myPos, Apos, timer / transitionTime);
            myCam.transform.rotation = Quaternion.Lerp(myRot, Arot, timer / transitionTime);
            yield return null;
        }
        yield return StartCoroutine(moveToB());

    }
    public IEnumerator moveToB()
    {
        float timer = 0;
        float transitionTime = 4;
        Vector3 myPos = myCam.transform.position;
        Quaternion myRot = myCam.transform.rotation;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            myCam.transform.position = Vector3.Lerp(myPos, Bpos, timer / transitionTime);
            myCam.transform.rotation = Quaternion.Lerp(myRot, Brot, timer / transitionTime);
            infoA.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            yield return null;
        }
        myAudio.PlayOneShot(displayMessageNoise);
        yield return StartCoroutine(moveToC());

    }
    public IEnumerator moveToC()
    {
        float timer = 0;
        float transitionTime = 4;
        Vector3 myPos = myCam.transform.position;
        Quaternion myRot = myCam.transform.rotation;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            myCam.transform.position = Vector3.Lerp(myPos, Cpos, timer / transitionTime);
            myCam.transform.rotation = Quaternion.Lerp(myRot, Crot, timer / transitionTime);
            infoA.alpha = Mathf.Lerp(1, 0, timer / transitionTime);
            yield return null;
        }
        yield return StartCoroutine(moveToD());

    }

    public IEnumerator moveToD()
    {
        float timer = 0;
        float transitionTime = 6;
        Vector3 myPos = myCam.transform.position;
        Quaternion myRot = myCam.transform.rotation;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            myCam.transform.position = Vector3.Lerp(myPos, Dpos, timer / transitionTime);
            myCam.transform.rotation = Quaternion.Lerp(myRot, Drot, timer / transitionTime);
            infoB.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            yield return null;
        }
        myAudio.PlayOneShot(displayMessageNoise);
        yield return StartCoroutine(moveToE());

    }

    public IEnumerator moveToE()
    {
        float timer = 0;
        float transitionTime = 5;
        Vector3 myPos = myCam.transform.position;
        Quaternion myRot = myCam.transform.rotation;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            myCam.transform.position = Vector3.Lerp(myPos, Epos, timer / transitionTime);
            myCam.transform.rotation = Quaternion.Lerp(myRot, Erot, timer / transitionTime);
            infoB.alpha = Mathf.Lerp(1, 0, timer / transitionTime);
            yield return null;
        }
        yield return StartCoroutine(moveToF());

    }
    public IEnumerator moveToF()
    {
        float timer = 0;
        float transitionTime = 5;
        Vector3 myPos = myCam.transform.position;
        Quaternion myRot = myCam.transform.rotation;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            myCam.transform.position = Vector3.Lerp(myPos, Fpos, timer / transitionTime);
            myCam.transform.rotation = Quaternion.Lerp(myRot, Frot, timer / transitionTime);
            infoC.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            yield return null;
        }
        myAudio.PlayOneShot(displayMessageNoise);
        yield return StartCoroutine(moveToI());

    }

    public IEnumerator moveToI()
    {
        float timer = 0;
        float transitionTime = 5;
        Vector3 myPos = myCam.transform.position;
        Quaternion myRot = myCam.transform.rotation;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            myCam.transform.position = Vector3.Lerp(myPos, Ipos, timer / transitionTime);
            myCam.transform.rotation = Quaternion.Lerp(myRot, Irot, timer / transitionTime);
            infoC.alpha = Mathf.Lerp(1, 0, timer / transitionTime);
            yield return null;
        }
        yield return StartCoroutine(moveToEnd());

    }
    public IEnumerator moveToEnd()
    {
        float timer = 0;
        float transitionTime = 5;
        Vector3 myPos = myCam.transform.position;
        Quaternion myRot = myCam.transform.rotation;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            myCam.transform.position = Vector3.Lerp(myPos, startEndpos, timer / transitionTime);
            myCam.transform.rotation = Quaternion.Lerp(myRot, startEndrot, timer / transitionTime);
            yield return null;
        }
        yield return StartCoroutine(fadeOut());


    }

    public IEnumerator fadeOut()
    {
        float timer = 0;
        float transitionTime = 4;
        float start = blankscreen.alpha;
        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            blankscreen.alpha = Mathf.Lerp(start, 1f, timer / transitionTime);
            infoD.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            yield return null;
        }
        yield return StartCoroutine(fadeOutInfoD());
    }

    public IEnumerator fadeOutInfoD()
    {
        float timer = 0;
        float transitionTime = 4;
        float vol = myAudio.volume;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            myAudio.volume = Mathf.Lerp(vol, 0f, timer / transitionTime);
            infoD.alpha = Mathf.Lerp(1, 0, timer / transitionTime);
            yield return null;
        }
        yield return StartCoroutine(toBattle());
    }

    public IEnumerator toBattle()
    {
        SceneManager.LoadScene("Battleground1");
        yield return null;
    }
    public IEnumerator fadeOutSkip()
    {
        float timer = 0;
        float transitionTime = 4;
        float vol = myAudio.volume;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            blankscreen.alpha = Mathf.Lerp(0, 1, timer / transitionTime);
            myAudio.volume = Mathf.Lerp(vol, 0f, timer / transitionTime);
            yield return null;
        }
        yield return StartCoroutine(toBattle());
    }
}
