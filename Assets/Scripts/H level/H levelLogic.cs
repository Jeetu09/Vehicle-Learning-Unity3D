using UnityEngine;

public class HlevelLogic : MonoBehaviour
{
    [HideInInspector] public int counter = 0;
    public WallCollid wallcollidercode;

    [Header("Main Objects")]
    public GameObject leveltwo;
    public GameObject Levelthree;

    [Header("Congo UI")]
    public GameObject CongoUI;
    public GameObject LevelChange;
    private CanvasGroup congoGroup;

    [Header("Car Settings")]
    public CarController newcar; // 🔥 drag your car here in Inspector

    [Header("Change Material")]
    public Renderer OneObj;
    public Renderer TwoObj;
    public Renderer ThreeObj;
    public Renderer FourObj;

    [Header("UI")]
    public GameObject OneUI;
    public GameObject TwoUI;
    public GameObject ThreeUI;
    public GameObject FourthUI;
    public GameObject LevelCompleted;

    [Header("Materials")]
    public Material defaultMat;
    public Material greenMat;

    void Start()
    {
        // Setup CongoUI
        if (CongoUI != null)
        {
            congoGroup = CongoUI.GetComponent<CanvasGroup>();
            if (congoGroup == null)
                congoGroup = CongoUI.AddComponent<CanvasGroup>();

            CongoUI.SetActive(false);
            congoGroup.alpha = 0f;
        }

        // Set all objects to default material
        OneObj.material = defaultMat;
        TwoObj.material = defaultMat;
        ThreeObj.material = defaultMat;
        FourObj.material = defaultMat;

        // UI setup
        OneUI.SetActive(true);
        TwoUI.SetActive(false);
        ThreeUI.SetActive(false);
        FourthUI.SetActive(false);
        LevelCompleted.SetActive(false);
    }

    public void ResetLevel()
    {
        counter = 0;

        // Reset materials
        OneObj.material = defaultMat;
        TwoObj.material = defaultMat;
        ThreeObj.material = defaultMat;
        FourObj.material = defaultMat;

        // Reset UI
        OneUI.SetActive(true);
        TwoUI.SetActive(false);
        ThreeUI.SetActive(false);
        FourthUI.SetActive(false);
        LevelCompleted.SetActive(false);

        // Reset CongoUI
        if (CongoUI != null)
        {
            CongoUI.SetActive(false);
            congoGroup.alpha = 0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "One" && counter == 0)
        {
            OneUI.SetActive(false);
            TwoUI.SetActive(true);
            Debug.Log("One Done");
            counter++;
            OneObj.material = greenMat;
        }
        else if (other.name == "Two" && counter == 1)
        {
            TwoUI.SetActive(false);
            ThreeUI.SetActive(true);
            Debug.Log("Two Done");
            counter++;
            TwoObj.material = greenMat;
        }
        else if (other.name == "Three" && counter == 2)
        {
            ThreeUI.SetActive(false);
            FourthUI.SetActive(true);
            Debug.Log("Three Done");
            counter++;
            ThreeObj.material = greenMat;
        }
        else if (other.name == "Four" && counter == 3)
        {
            Cursor.visible = true;  
            Cursor.lockState = CursorLockMode.None; 

            FourthUI.SetActive(false);
            LevelCompleted.SetActive(true);
            Debug.Log("Four Done");
            counter++;
            FourObj.material = greenMat;

            if (newcar != null)
            {
                newcar.FullBreak();
            }

            if (CongoUI != null)
            {
                CongoUI.SetActive(true);
                StartCoroutine(FadeInCongo());
            }
        }
    }

    public void EndUIbutton()
    {
        CongoUI.SetActive(false);
        LevelChange.SetActive(true);
    }

    public void levelthreebutton()
    {
        LevelChange.SetActive(false);
        leveltwo.SetActive(false);
        Levelthree.SetActive(true);

        // 🔥 Allow car movement again
        if (newcar != null)
        {
            newcar.NoFullBreak();
        }
    }

    System.Collections.IEnumerator FadeInCongo()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 2f; // fade duration = 2 seconds
            congoGroup.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
        congoGroup.alpha = 1f;
    }
}
