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
    [HideInInspector]
    public bool gyroHit = false;
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
    private bool step1, step2, step3, step4, step5 = false;
    private bool clip75, clip50, clip25, clip0 = false;
    private bool clipHurryUp = false;
    public AudioSource sfx;
    public AudioClip[] sfxClips;
    [HideInInspector]
    public bool dockingShip = false;
    public GameObject iSSImage;
    public float shipFullSpeed = 2.5f;
    private bool clipEngine = false;
    public float scaleMultiplier = 1.5f;
    public AudioSource engine;
    private bool finalVideo = false;
    private bool finalDie = false;
    // Use this for initialization

    private void Awake()
    {
        shipRadarMoveSpeed = 0f;
    }

    void Start()
    {
        sfx.clip = sfxClips[10];
        sfx.loop = true;
        sfx.Play();
        resetSpeed = shipRadarMoveSpeed;
        shipRadarMoveSpeed = 0f;
        resetTimer = overrideTimer;
        iSSImage.transform.localScale *= 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (startComm1)
        {
            shipRadarMoveSpeed = shipFullSpeed;
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
                StartCoroutine(Die());
            }
        }
        if (shipRadarMoveSpeed > 1.0f)
        {
            if (!clipEngine)
            {
                //sfx.clip = sfxClips[16];
                //sfx.loop = true;
                //sfx.Play();
                engine.Play();
                clipEngine = true;
            }
            iSSImage.transform.localScale += new Vector3(0.000075f * scaleMultiplier, 0.0001f * scaleMultiplier, 0.0001f * scaleMultiplier);
        }
        else {
            engine.Stop();
            clipEngine = false;
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
            shipRadarMoveSpeed = shipFullSpeed;
            pod1Level.transform.localPosition = Vector3.MoveTowards(pod1Level.transform.localPosition, pod1Target.transform.localPosition, fuelDecreaseSpeed * Time.deltaTime);
           
            if (pod1Level.transform.localPosition == pod1Target.transform.localPosition)
            {
                shipRadarMoveSpeed = 0.5f;
                pod1Complete = true;
                pod1InUse = false;
            }
        }
        if (toggle2 && !pod2Complete && pod2InUse && overrideAccess)
        {
            shipRadarMoveSpeed = shipFullSpeed;
            pod2Level.transform.localPosition = Vector3.MoveTowards(pod2Level.transform.localPosition, pod2Target.transform.localPosition, fuelDecreaseSpeed * Time.deltaTime);
            
            if (pod2Level.transform.localPosition == pod2Target.transform.localPosition)
            {
                shipRadarMoveSpeed = 0.5f;
                pod2Complete = true;
                pod2InUse = false;
            }
        }
        if (toggle3 && !pod3Complete && pod3InUse && overrideAccess)
        {
            shipRadarMoveSpeed = shipFullSpeed;
            pod3Level.transform.localPosition = Vector3.MoveTowards(pod3Level.transform.localPosition, pod3Target.transform.localPosition, fuelDecreaseSpeed * Time.deltaTime);
           
            if (pod3Level.transform.localPosition == pod3Target.transform.localPosition)
            {
                shipRadarMoveSpeed = 0.5f;
                pod3Complete = true;
                pod3InUse = false;
            }
        }
        if (toggle4 && !pod4Complete && pod4InUse && overrideAccess)
        {
            shipRadarMoveSpeed = shipFullSpeed;
            pod4Level.transform.localPosition = Vector3.MoveTowards(pod4Level.transform.localPosition, pod4Target.transform.localPosition, fuelDecreaseSpeed * Time.deltaTime);
            
            if (pod4Level.transform.localPosition == pod4Target.transform.localPosition)
            {
                shipRadarMoveSpeed = 0.5f;
                pod4Complete = true;
                pod4InUse = false;
            }
        }
        if (toggle5 && !pod5Complete && pod5InUse && overrideAccess)
        {
            shipRadarMoveSpeed = shipFullSpeed;
            pod5Level.transform.localPosition = Vector3.MoveTowards(pod5Level.transform.localPosition, pod5Target.transform.localPosition, fuelDecreaseSpeed * Time.deltaTime);
            
            if (pod5Level.transform.localPosition == pod5Target.transform.localPosition)
            {
                shipRadarMoveSpeed = 0.5f;
                pod5Complete = true;
                pod5InUse = false;
            }
        }

        if (dockingShip && !gyroHit && !finalVideo)
        {
            StartCoroutine(triggers.WaitToTrigger("Got-ShipDocked", 2.0f));
            finalVideo = true;

        }
        else if (dockingShip && gyroHit && !finalDie)
        {
            StartCoroutine(Die());
            finalDie = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!toggle1)
            {
                triggers.Trigger("Got-Toggle1");
            }
            else
            {
                triggers.Trigger("Lost-Toggle1");
            }
            Debug.Log("Toggle1: " + toggle1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!toggle2)
            {
                triggers.Trigger("Got-Toggle2");
            }
            else
            {
                triggers.Trigger("Lost-Toggle2");
            }
            Debug.Log("Toggle2: " + toggle2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (!toggle3)
            {
                triggers.Trigger("Got-Toggle3");
            }
            else
            {
                triggers.Trigger("Lost-Toggle3");
            }
            Debug.Log("Toggle3: " + toggle3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (!toggle4)
            {
                triggers.Trigger("Got-Toggle4");
            }
            else
            {
                triggers.Trigger("Lost-Toggle4");
            }
            Debug.Log("Toggle4: " + toggle4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (!toggle5)
            {
                triggers.Trigger("Got-Toggle5");
            }
            else
            {
                triggers.Trigger("Lost-Toggle5");
            }
            Debug.Log("Toggle5: " + toggle5);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!alClip1)
            {
                triggers.Trigger("Got-AlClip1");
            }
            else
            {
                triggers.Trigger("Lost-AlClip1");
            }
            Debug.Log("AlClip1: " + alClip1);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!alClip2)
            {
                triggers.Trigger("Got-AlClip2");
            }
            else
            {
                triggers.Trigger("Lost-AlClip2");
            }
            Debug.Log("AlClip2: " + alClip2);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!alClip3)
            {
                triggers.Trigger("Got-AlClip3");
            }
            else
            {
                triggers.Trigger("Lost-AlClip3");
            }
            Debug.Log("AlClip3: " + alClip3);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!alClip4)
            {
                triggers.Trigger("Got-AlClip4");
            }
            else
            {
                triggers.Trigger("Lost-AlClip4");
            }
            Debug.Log("AlClip4: " + alClip4);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!knife1)
            {
                triggers.Trigger("Got-CoverToggle1");
            }
            else
            {
                triggers.Trigger("Lost-CoverToggle1");
            }
            Debug.Log("CoverToggle1: " + knife1);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (!knife2)
            {
                triggers.Trigger("Got-CoverToggle2");
            }
            else
            {
                triggers.Trigger("Lost-CoverToggle2");
            }
            Debug.Log("CoverToggle2: " + knife2);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (!knife3)
            {
                triggers.Trigger("Got-CoverToggle3");
            }
            else
            {
                triggers.Trigger("Lost-CoverToggle3");
            }
            Debug.Log("CoverToggle3: " + knife3);
        }
    }

    public IEnumerator Gyroscope()
    {
        yield return new WaitForSeconds(0.1f);
        if (overrideAccess)
        {
            if (alClip3 && !alClip1 && !alClip2 && !alClip4 && !knife1 && !knife2 && !knife3 && !step1)
            {
                rotateSpeed = 1f;
                step1 = true;
                sfx.clip = sfxClips[14];
                sfx.Play();
            }
            else if (alClip3 && !alClip1 && !alClip2 && alClip4 && !knife1 && !knife2 && !knife3 && step1 && !step2)
            {
                rotateSpeed = 0.6f;
                step2 = true;
                sfx.clip = sfxClips[14];
                sfx.Play();
            }
            else if (alClip3 && alClip1 && !alClip2 && alClip4 && !knife1 && !knife2 && !knife3 && step1 && step2 && !step3)
            {
                rotateSpeed = 0.3f;
                step3 = true;
                sfx.clip = sfxClips[14];
                triggers.Trigger("On-Panel3");
                sfx.Play();
            }
            else if (alClip3 && alClip1 && !alClip2 && alClip4 && (knife1 || knife2 || knife3) && step1 && step2 && step3 && !step4)
            {
                rotateSpeed = 0.2f;
                if ((knife1 && knife2) || (knife1 && knife3) || (knife2 && knife3))
                    step4 = true;
                sfx.clip = sfxClips[14];
                sfx.Play();
            }
            else if (alClip3 && alClip1 && !alClip2 && alClip4 && knife1 && knife2 && knife3 && step1 && step2 && step3 && step4 && !step5)
            {
                rotateSpeed = 0f;
                shipRadar.transform.localEulerAngles = new Vector3(0, 0, 0);
                iSSImage.transform.localEulerAngles = new Vector3(0, 0, 0);
                step5 = true;
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
                step5 = false;
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

    public IEnumerator Die()
    {
        yield return new WaitForSeconds(3.0f);
        if (gyroHit || decreaseO2)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(3);
        }
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
