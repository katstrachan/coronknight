using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beaconStats : MonoBehaviour
{
    beaconAudio audioScript;
    List<GameObject> defenders = new List<GameObject>();
    beaconSpawnManager mySpawner;
    private bool playerCaptured = true;
    int captureStatus = 100;
    Animator artist;
    MeshRenderer myMesh;
    public Material playerMaterial;
    public Material enemyMaterial;
    public ParticleSystem playerParticles;
    public ParticleSystem enemyParticles;
    public ParticleSystem changeCaptureParticles;
    beaconHealthBarBehaviour myHealthBar;
    UIManager myUI;

    private void Awake()
    {
        audioScript = GetComponent<beaconAudio>();
        myMesh = GetComponent<MeshRenderer>();
        mySpawner = GetComponent<beaconSpawnManager>();
        myUI = GameObject.FindGameObjectWithTag("gameManager").GetComponent<UIManager>();
        myHealthBar = gameObject.transform.Find("BeaconHealthbar").GetComponent<beaconHealthBarBehaviour>();
    }

    void Start()
    {
        
        artist = GetComponent<Animator>();


    }

    public void setPlayerCaptured(bool setup)
    {
        audioScript.makeAllyCaptureNoise();
        playerCaptured = true;
        myUI.updateBeaconDisplay(true, setup);
        myHealthBar.initialiseHealthBar(200, 0, true);
        captureStatus = 100;
        updateBeacon(playerMaterial, playerParticles);

    }

    public void setEnemyCaptured(bool setup)
    {
        audioScript.makeEnemyCaptureNoise();
        playerCaptured = false;
        myUI.updateBeaconDisplay(false, setup);
        myHealthBar.initialiseHealthBar(200, 0, false);
        
        captureStatus = -100;
        updateBeacon(enemyMaterial, enemyParticles);
        
    }

    void updateBeacon(Material beaconColour, ParticleSystem beaconSparkle)
    {
        myMesh.material = beaconColour;
        Instantiate(changeCaptureParticles, transform.position, Quaternion.identity);
        Instantiate(beaconSparkle, transform.position, Quaternion.identity);
        mySpawner.beaconResetCapture(playerCaptured);
        myUI.updateBeaconIcon(gameObject, playerCaptured);

    }

    public bool getIsPlayerCaptured()
    {
        return playerCaptured;
    }



    public void enemyCaptureAttack()
    {
        if (captureStatus != -100)
        {
            captureStatus -= 20;
            myHealthBar.setHealthBar((100+captureStatus));
            checkStatus();
        }
    }
    public void playerCaptureAttack()
    {
        if (captureStatus != 100)
        {
            captureStatus += 20;
            myHealthBar.setHealthBar(((-1 *captureStatus)+100));
            checkStatus();
        }
    }

    private void checkStatus()
    {
        if (captureStatus == 100)
        {
            setPlayerCaptured(false);
        }
        else if (captureStatus == -100)
        {
            setEnemyCaptured(false);
        }
    }



}
