using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private bool decreaseO2 = false;
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

    // Use this for initialization
    void Start()
    {
        resetSpeed = shipRadarMoveSpeed;
        shipRadarMoveSpeed = 0.5f;
        resetTimer = overrideTimer;
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
            decreaseO2 = true;
            gyroHit = true;
            triggers.Trigger("Got-ShipHit");
        }
        if (decreaseO2)
        {
            o2Levels.transform.localPosition = Vector3.MoveTowards(o2Levels.transform.localPosition, o2Target.transform.localPosition, o2DecreaseSpeed * Time.deltaTime);
        }
        if (gyroHit)
        {
            shipRadar.transform.Rotate(Vector3.forward, rotateSpeed * UnityEngine.Random.Range(0.5f, 1), Space.Self);
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
                terminalText.SetActive(true);
            }
        }
        if (toggle1 && !pod1Complete && pod1InUse)
        {
            shipRadarMoveSpeed = 5f;
            pod1Level.transform.localPosition = Vector3.MoveTowards(pod1Level.transform.localPosition, pod1Target.transform.localPosition, fuelDecreaseSpeed * Time.deltaTime);

            if (pod1Level.transform.localPosition == pod1Target.transform.localPosition)
            {
                shipRadarMoveSpeed = 0.5f;
                pod1Complete = true;
                pod1InUse = false;
            }
        }
        if (toggle2 && !pod2Complete && pod2InUse)
        {
            shipRadarMoveSpeed = 5f;
            pod2Level.transform.localPosition = Vector3.MoveTowards(pod2Level.transform.localPosition, pod2Target.transform.localPosition, fuelDecreaseSpeed * Time.deltaTime);

            if (pod2Level.transform.localPosition == pod2Target.transform.localPosition)
            {
                shipRadarMoveSpeed = 0.5f;
                pod2Complete = true;
                pod2InUse = false;
            }
        }
        if (toggle3 && !pod3Complete && pod3InUse)
        {
            shipRadarMoveSpeed = 5f;
            pod3Level.transform.localPosition = Vector3.MoveTowards(pod3Level.transform.localPosition, pod3Target.transform.localPosition, fuelDecreaseSpeed * Time.deltaTime);

            if (pod3Level.transform.localPosition == pod3Target.transform.localPosition)
            {
                shipRadarMoveSpeed = 0.5f;
                pod3Complete = true;
                pod3InUse = false;
            }
        }
        if (toggle4 && !pod4Complete && pod4InUse)
        {
            shipRadarMoveSpeed = 5f;
            pod4Level.transform.localPosition = Vector3.MoveTowards(pod4Level.transform.localPosition, pod4Target.transform.localPosition, fuelDecreaseSpeed * Time.deltaTime);

            if (pod4Level.transform.localPosition == pod4Target.transform.localPosition)
            {
                shipRadarMoveSpeed = 0.5f;
                pod4Complete = true;
                pod4InUse = false;
            }
        }
        if (toggle5 && !pod5Complete && pod5InUse)
        {
            shipRadarMoveSpeed = 5f;
            pod5Level.transform.localPosition = Vector3.MoveTowards(pod5Level.transform.localPosition, pod5Target.transform.localPosition, fuelDecreaseSpeed * Time.deltaTime);

            if (pod5Level.transform.localPosition == pod5Target.transform.localPosition)
            {
                shipRadarMoveSpeed = 0.5f;
                pod5Complete = true;
                pod5InUse = false;
            }
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


    public void AcceptedIncomingCall1()
    {
        commsButton.interactable = false;
        acceptComms.SetActive(false);
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
