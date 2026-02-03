using UnityEngine;
using UnityEngine.UI;   // required for Image
using System.Collections;

public class Signal : MonoBehaviour
{
    [System.Serializable]
    public class SignalSet
    {
        public GameObject SignalWall;
        public GameObject SignalColour;
        public int SignalTimer = 5;
    }

    [Header("Signals")]
    public SignalSet[] signals;

    [Header("CheckPoints")]
    public GameObject[] checkpoints;

    [Header("Checkpoint Animation Settings")]
    public float moveHeight = 0.5f;
    public float rotationSpeed = 30f;
    public float moveSpeed = 2f;

    private Vector3[] initialPositions;

    [Header("UI")]
    public Image GuidImage;   // assign in Inspector

    void Start()
    {
        // Hide the GuidImage at the start
        if (GuidImage)
            GuidImage.gameObject.SetActive(false);

        foreach (var sig in signals)
        {
            if (sig.SignalWall)
                sig.SignalWall.SetActive(true);
            if (sig.SignalColour)
                sig.SignalColour.GetComponent<Renderer>().material.color = Color.red;
        }

        initialPositions = new Vector3[checkpoints.Length];
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (checkpoints[i])
                initialPositions[i] = checkpoints[i].transform.position;
        }
    }

    void Update()
    {
        AnimateCheckpoints();
    }

    void OnTriggerEnter(Collider other)
    {
        // Signal detection
        for (int i = 0; i < signals.Length; i++)
        {
            if (other.CompareTag("Signal Check"))
            {
                Debug.Log("Triggered Signal " + i);
                StartCoroutine(SignalRoutine(signals[i], i));
            }
        }

        // Checkpoint detection
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (other.gameObject == checkpoints[i])
            {
                Debug.Log("Player entered checkpoint: " + checkpoints[i].name);
                checkpoints[i].SetActive(false);

                // Turn on GuidImage when checkpoint 17 is reached (index 16)
                if (i == 16 && GuidImage)
                {
                    GuidImage.gameObject.SetActive(true);
                    Debug.Log("Checkpoint 17 reached — GuidImage enabled!");
                }
            }
        }
    }

    void AnimateCheckpoints()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (checkpoints[i] && checkpoints[i].activeSelf)
            {
                Vector3 startPos = initialPositions[i];
                float newY = startPos.y + Mathf.Sin(Time.time * moveSpeed) * moveHeight;
                checkpoints[i].transform.position = new Vector3(startPos.x, newY, startPos.z);
                checkpoints[i].transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
            }
        }
    }

    IEnumerator SignalRoutine(SignalSet sig, int index)
    {
        Renderer rend = sig.SignalColour.GetComponent<Renderer>();

        // RED
        rend.material.color = Color.red;
        sig.SignalWall.SetActive(true);
        Debug.Log("Signal " + index + " is RED | Wall Enabled");
        yield return new WaitForSeconds(sig.SignalTimer / 3f);

        // YELLOW
        rend.material.color = Color.yellow;
        Debug.Log("Signal " + index + " changed to YELLOW");
        yield return new WaitForSeconds(sig.SignalTimer / 3f);

        // GREEN
        rend.material.color = Color.green;
        sig.SignalWall.SetActive(false);
        Debug.Log("Signal " + index + " changed to GREEN | Wall Disabled");
        yield return new WaitForSeconds(sig.SignalTimer / 3f);
    }
}
