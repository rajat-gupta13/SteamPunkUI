using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DigitalButtonManager : MonoBehaviour
{


    //public GameObject oscInput;
    //public Dropdown settingsDropdown;
    public Triggers triggers;
    [HideInInspector]
    public bool startComm1 = false;
    [HideInInspector]
    public bool initialCallPicked = false;
    public GameObject acceptComms;
    public Button commsButton;
    public GameObject shipRadar;
    public float shipRadarMoveSpeed = 2.0f;
    [HideInInspector]
    public bool startAsteroids = false;
    public GameObject asteroids;
    public float asteroidMoveSpeed = 2.0f;
    [HideInInspector]
    public bool asteroidsHit = false;
    public GameObject damageIndicator;
    private bool asteroidFirst = false;
    [HideInInspector]
    public bool decreaseO2 = false;
    public GameObject o2Levels, o2Target;
    public float o2DecreaseSpeed = 5.6f;
    
    public GameObject scanAnimation;
    private Vector3 animStart = new Vector3(-2.79f, 42, 0);
    private Vector3 animEnd = new Vector3(-2.79f, -65, 0);
    private bool movingDown = false;
    private bool scanning = false;
    public float scanAnimationSpeed = 75f;
    private bool gyroHit = false;
    private float rotateSpeed = 1.5f;
    [HideInInspector]
    public bool overrideAccess = false;
    public float overrideTimer = 3f;
    private float resetTimer;
    private float resetSpeed;
    public GameObject terminalText;
    [HideInInspector]
    public bool toggle1, toggle2, toggle3, toggle4, toggle5 = false;
    [HideInInspector]
    public bool pod1Complete, pod2Complete, pod3Complete, pod4Complete, pod5Complete = false;
    public GameObject pod1Level, pod1Target;
    public GameObject pod2Level, pod2Target;
    public GameObject pod3Level, pod3Target;
    public GameObject pod4Level, pod4Target;
    public GameObject pod5Level, pod5Target;
    [HideInInspector]
    public bool pod1InUse, pod2InUse, pod3InUse, pod4InUse, pod5InUse = false;
    public float fuelDecreaseSpeed = 17.7f;
    [HideInInspector]
    public bool alClip1, alClip2, alClip3, alClip4 = false;
    [HideInInspector]
    public bool knife1, knife2, knife3 = false;
    private bool step1, step2, step3, step4 = false;
    private bool clip75, clip50, clip25, clip0 = false;
    private bool clipHurryUp = false;
    public AudioSource sfx;
    public AudioClip[] sfxClips;
    [HideInInspector]
    public bool dockingShip = false;
    public GameObject iSSImage;
    public float multiplyFactor = 1.05f;
    // Use this for initialization
    void Start()
    {
        sfx.clip = sfxClips[10];
        sfx.loop = true;
        sfx.Play();
        resetSpeed = shipRadarMoveSpeed;
        shipRadarMoveSpeed = 0.5f;
        resetTimer = overrideTimer;
        iSSImage.transform.localScale *= 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (startComm1)
        {
            shipRadarMoveSpeed = 3f;
            StartCoroutine(AcceptIncomingComms());
            startComm1 = false;
        }
        shipRadar.transform.localPosition += new Vector3(0, shipRadarMoveSpeed * Time.deltaTime, 0);
        if (startAsteroids)
        {
            asteroids.transform.localPosition = Vector3.MoveTowards(asteroids.transform.localPosition, shipRadar.transform.localPosition, asteroidMoveSpeed * Time.deltaTime);
        }
        if (asteroidsHit && !asteroidFirst)
        {
            asteroidFirst = true;
            asteroids.SetActive(false);
            shipRadarMoveSpeed = 0.5f;
            InvokeRepeating("DamagageIndicatorStatus", 0.5f, 0.5f);
            //decreaseO2 = true;

            gyroHit = true;
            triggers.Trigger("Got-ShipHit");
        }
        if (decreaseO2)
        {
            o2Levels.transform.localPosition = Vector3.MoveTowards(o2Levels.transform.localPosition, o2Target.transform.localPosition, o2DecreaseSpeed * Time.deltaTime);
            if (o2Levels.transform.localPosition.y <= -140 && o2Levels.transform.localPosition.y >= -145 && !clip75)
            {
                clip75 = true;
                sfx.clip = sfxClips[6];
                sfx.Play();
            }
            else if (o2Levels.transform.localPosition.y <= -280 && o2Levels.transform.localPosition.y >= -285 && !clip50)
            {
                clip50 = true;
                sfx.clip = sfxClips[7];
                sfx.Play();
            }
            else if (o2Levels.transform.localPosition.y <= -420 && o2Levels.transform.localPosition.y >= -425 && !clip25)
            {
                clip25 = true;
                sfx.clip = sfxClips[8];
                sfx.Play();
            }
            else if (o2Levels.transform.localPosition == o2Target.transform.localPosition && !clip0)
            {
                clip0 = true;
                sfx.clip = sfxClips[9];
                sfx.Play();
                Die();
            }
        }
        if (shipRadarMoveSpeed > 1.0f)
        {
            iSSImage.transform.localScale += new Vector3(0.0000375f, 0.00005f, 0.00005f);
        }
        
        if (gyroHit)
        {
            iSSImage.transform.Rotate(Vector3.forward, rotateSpeed * UnityEngine.Random.Range(0.5f, 1), Space.Self);
            shipRadar.transform.Rotate(Vector3.forward, rotateSpeed * UnityEngine.Random.Range(0.5f, 1), Space.Self);
            if (o2Levels.transform.localPosition.y <= -350 && !clipHurryUp)
            {
                sfx.clip = sfxClips[13];
                sfx.Play();
                clipHurryUp = true;
            }
        }
        if (scanning && overrideAccess)
        {
            overrideTimer -= Time.deltaTime;
            if (scanAnimation.transform.localPosition == animStart)
            {
                movingDown = true;

            }
            else if (scanAnimation.transform.localPosition == animEnd)
            {
                movingDown = false;
            }
            if (movingDown)
            {
                scanAnimation.transform.localPosition = Vector3.MoveTowards(scanAnimation.transform.localPosition, animEnd, scanAnimationSpeed * Time.deltaTime);
            }
            else
            {
                scanAnimation.transform.localPosition = Vector3.MoveTowards(scanAnimation.transform.localPosition, animStart, scanAnimationSpeed * Time.deltaTime);
            }
            if (overrideTimer <= 0.0f)
            {
                overrideTimer = resetTimer;
                sfx.clip = sfxClips[12];
                sfx.Play();
                terminalText.SetActive(true);
            }
        }
        if (toggle1 && !pod1Complete && pod1InUse && overrideAccess)
        {
            shipRadarMoveSpeed = 5f;
            pod1Level.transform.localPosition = Vector3.MoveTowards(pod1Level.transform.localPosition, pod1Target.transform.localPosition, fuelDecreaseSpeed * Time.deltaTime);
            sfx.clip = sfxClips[0];
            sfx.Play();
            if (pod1Level.transform.localPosition == pod1Target.transform.localPosition)
            {
                shipRadarMoveSpeed = 0.5f;
                pod1Complete = true;
                pod1InUse = false;
            }
        }
        if (toggle2 && !pod2Complete && pod2InUse && overrideAccess)
        {
            shipRadarMoveSpeed = 5f;
            pod2Level.transform.localPosition = Vector3.MoveTowards(pod2Level.transform.localPosition, pod2Target.transform.localPosition, fuelDecreaseSpeed * Time.deltaTime);
            sfx.clip = sfxClips[1];
            sfx.Play();
            if (pod2Level.transform.localPosition == pod2Target.transform.localPosition)
            {
                shipRadarMoveSpeed = 0.5f;
                pod2Complete = true;
                pod2InUse = false;
            }
        }
        if (toggle3 && !pod3Complete && pod3InUse && overrideAccess)
        {
            shipRadarMoveSpeed = 5f;
            pod3Level.transform.localPosition = Vector3.MoveTowards(pod3Level.transform.localPosition, pod3Target.transform.localPosition, fuelDecreaseSpeed * Time.deltaTime);
            sfx.clip = sfxClips[2];
            sfx.Play();
            if (pod3Level.transform.localPosition == pod3Target.transform.localPosition)
            {
                shipRadarMoveSpeed = 0.5f;
                pod3Complete = true;
                pod3InUse = false;
            }
        }
        if (toggle4 && !pod4Complete && pod4InUse && overrideAccess)
        {
            shipRadarMoveSpeed = 5f;
            pod4Level.transform.localPosition = Vector3.MoveTowards(pod4Level.transform.localPosition, pod4Target.transform.localPosition, fuelDecreaseSpeed * Time.deltaTime);
            sfx.clip = sfxClips[3];
            sfx.Play();
            if (pod4Level.transform.localPosition == pod4Target.transform.localPosition)
            {
                shipRadarMoveSpeed = 0.5f;
                pod4Complete = true;
                pod4InUse = false;
            }
        }
        if (toggle5 && !pod5Complete && pod5InUse && overrideAccess)
        {
            shipRadarMoveSpeed = 5f;
            pod5Level.transform.localPosition = Vector3.MoveTowards(pod5Level.transform.localPosition, pod5Target.transform.localPosition, fuelDecreaseSpeed * Time.deltaTime);
            sfx.clip = sfxClips[4];
            sfx.Play();
            if (pod5Level.transform.localPosition == pod5Target.transform.localPosition)
            {
                shipRadarMoveSpeed = 0.5f;
                pod5Complete = true;
                pod5InUse = false;
            }
        }

        if (dockingShip && !gyroHit)
        {

        }
        else if (dockingShip && gyroHit)
        {
            Die();
        }

    }

    public void Gyroscope()
    {
        if (overrideAccess)
        {
            if (alClip3 && !alClip1 && !alClip2 && !alClip4 && knife1 && !knife2 && !knife3 && !step1)
            {
                rotateSpeed = 1f;
                step1 = true;
            }
            else if (alClip3 && alClip1 && !alClip2 && !alClip4 && knife1 && !knife2 && !knife3 && step1 && !step2)
            {
                rotateSpeed = 0.6f;
                step2 = true;
            }
            else if (alClip3 && alClip1 && !alClip2 && alClip4 && knife1 && !knife2 && !knife3 && step1 && step2 && !step3)
            {
                rotateSpeed = 0.3f;
                step3 = true;
            }
            else if (alClip3 && alClip1 && !alClip2 && alClip4 && knife1 && knife2 && !knife3 && step1 && step2 && step3 && !step4)
            {
                rotateSpeed = 0f;
                shipRadar.transform.localEulerAngles = new Vector3(0, 0, 0);
                iSSImage.transform.localEulerAngles = new Vector3(0, 0, 0);
                step4 = true;
                gyroHit = false;
                sfx.clip = sfxClips[5];
                sfx.Play();
                Debug.Log("Gyroscope Fixed Successfully");
            }
            else
            {
                step1 = false;
                step2 = false;
                step3 = false;
                step4 = false;
                Debug.Log("Gyroscope Fix Failed");
                sfx.clip = sfxClips[11];
                sfx.Play();
            }
        }
    }

    private void DamagageIndicatorStatus()
    {
        if (damageIndicator.activeInHierarchy)
        {
            damageIndicator.SetActive(false);
        }
        else
        {
            damageIndicator.SetActive(true);
        }
    }

    public void ScanningOverrideStart()
    {
        scanAnimation.SetActive(true);
        scanning = true;
    }

    public void ScanningOverrideEnd()
    {
        overrideTimer = resetTimer;
        scanning = false;
        scanAnimation.SetActive(false);
    }

    private IEnumerator AcceptIncomingComms()
    {
        while (!initialCallPicked)
        {
            if (acceptComms.activeInHierarchy)
            {
                acceptComms.SetActive(false);
            }
            else
            {
                acceptComms.SetActive(true);
            }
            yield return new WaitForSeconds(0.5f);
        }
        triggers.Trigger("Got-incomingCall");
    }

    private void Die()
    {
        SceneManager.LoadScene(2);
    }

    public void AcceptedIncomingCall1()
    {
        commsButton.interactable = false;
        acceptComms.SetActive(false);
        sfx.clip = null;
        sfx.loop = false;
        initialCallPicked = true;
        //StartCoroutine(StartPlayerMovement());
    }

    IEnumerator StartPlayerMovement()
    {
        yield return null;
        float time = (float)triggers.videoFiles[0].length;
        while (time > 0)
        {
            time -= Time.deltaTime;
            Debug.Log(time);
            shipRadar.transform.localPosition += new Vector3(0, shipRadarMoveSpeed * Time.deltaTime, 0);
        }
    }


}
