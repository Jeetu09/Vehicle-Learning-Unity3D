using UnityEngine;

public class WallCollid : MonoBehaviour
{
    public Transform Car;
    public CarController newcar;

    private Vector3 initialPos;
    private Quaternion initialRot;

    int count = 0;

    public GameObject blackScreen; 
    private CanvasGroup blackGroup;

    void Start()
    {
        initialPos = Car.localPosition;
        initialRot = Car.localRotation;

        blackGroup = blackScreen.GetComponent<CanvasGroup>();
        blackScreen.SetActive(false);
    }

    void OnCollisionEnter(Collision other)
    {
        if ((other.collider.name == "Wall" || other.collider.name == "HWalls") && count == 0)
        {
            Debug.Log("Level Reset");
            newcar.FullBreak();

            blackScreen.SetActive(true);
            StartCoroutine(FadeIn());

            Invoke("Reset", 6f);
            count = 1;
        }
    }

    System.Collections.IEnumerator FadeIn()
    {
        blackGroup.alpha = 0;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 2f; // fade in 2 sec
            blackGroup.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
        blackGroup.alpha = 1;
    }

    public void Reset()
    {
        Car.localPosition = initialPos;
        Car.localRotation = initialRot;
        newcar.NoFullBreak();
        count = 0;

        blackScreen.SetActive(false);

        // Reset level logic properly
        CircuitDetect levelLogic = FindObjectOfType<CircuitDetect>(); // ✅ correct script
        if (levelLogic != null)
        {
            levelLogic.ResetLevel();
        }
    }


}
