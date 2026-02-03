using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ParkingDetect : MonoBehaviour
{
    bool detect1Triggered = false;
    bool detect2Triggered = false;
    float parkedTimer = 0f;       // timer to count continuous detection
    float requiredTime = 3f;      // 3 seconds

    public MeshRenderer Sign1;
    public MeshRenderer Sign2;

    [Header("UI Settings")]
    public Image parkingCompleteImage;  // assign UI image in Inspector

    void Start()
    {
        // Initially both signs are red
        Sign1.material.color = Color.red;
        Sign2.material.color = Color.red;

        // Hide parking complete image initially
        if (parkingCompleteImage)
        {
            parkingCompleteImage.gameObject.SetActive(false);
            Color c = parkingCompleteImage.color;
            c.a = 0f;
            parkingCompleteImage.color = c;
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // If both detections are active, start counting
        if (detect1Triggered && detect2Triggered)
        {
            // Change to yellow when both detected
            Sign1.material.color = Color.yellow;
            Sign2.material.color = Color.yellow;

            parkedTimer += Time.deltaTime;

            // After 3 continuous seconds
            if (parkedTimer >= requiredTime)
            {
                Sign1.material.color = Color.green;
                Sign2.material.color = Color.green;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                // Start fade-in image when green light appears
                if (parkingCompleteImage && !parkingCompleteImage.gameObject.activeSelf)
                    StartCoroutine(FadeInImage(parkingCompleteImage));
            }
        }
        else
        {
            // Reset timer and color if detection is lost
            parkedTimer = 0f;
            Sign1.material.color = Color.red;
            Sign2.material.color = Color.red;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Detect1")
            detect1Triggered = true;

        if (other.name == "Detect2")
            detect2Triggered = true;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.name == "Detect1")
            detect1Triggered = false;

        if (other.name == "Detect2")
            detect2Triggered = false;
    }

    IEnumerator FadeInImage(Image img)
    {
        img.gameObject.SetActive(true);
        Color c = img.color;
        float duration = 2f; // fade-in duration
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / duration); // fade from 0 to full opacity
            img.color = c;
            yield return null;
        }
    }
}
