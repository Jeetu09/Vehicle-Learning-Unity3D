
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonLook : MonoBehaviour
{
    [System.Serializable]
    public class FocusTarget
    {
        public Transform targetObject;
        [TextArea] public string messageToShow;
        public GameObject uiElement;
        public Animator targetAnimator;
        public string openTriggerName = "Open";
        public string closeTriggerName = "Close";
        [HideInInspector] public bool isOpen = false;
    }

    [Header("Player Vehicle System")]
    public GameObject Player;
    public GameObject Seat;
    public GameObject Vehicle;

    [Header("Seat Focus Settings")]
    public Transform seatFocusObject;
    public Vector3 seatTargetPosition;
    public Rigidbody playerRigidbody;

    [Header("Relocation After Seat")]
    public Transform relocationTarget;
    public GameObject EtoOutUI;

    [Header("Player and Camera")]
    public Transform character;
    public MonoBehaviour playerMovementScript;

    [Header("UI Elements")]
    public GameObject clickImage;

    [Header("Look Settings")]
    public float sensitivity = 2f;
    public float smoothing = 1.5f;
    public float focusRange = 5f;

    [Header("Focus Targets")]
    public FocusTarget[] focusTargets;

    private Vector2 velocity;
    private Vector2 frameVelocity;
    private FocusTarget currentFocus;

    private bool isMovementLocked = false;
    private bool hasSatOnSeat = false;

    void Start()
    {
        EtoOutUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (clickImage != null)
            clickImage.SetActive(false);

        foreach (FocusTarget target in focusTargets)
        {
            if (target.uiElement != null)
                target.uiElement.SetActive(false);
        }
    }

    void Update()
    {
        if (!isMovementLocked)
        {
            HandleMouseLook();
        }

        CheckFocusObject();
        CheckSeatClick();
        CheckRelocationInput();

        if (Input.GetMouseButtonDown(0) && currentFocus != null)
        {
            TriggerTargetInteraction(currentFocus);
        }
    }

    void HandleMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }

    void CheckFocusObject()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        currentFocus = null;

        bool lookingAtSeat = false;

        if (Physics.Raycast(ray, out hit, focusRange))
        {
            // Check if looking at seat
            if (hit.transform == seatFocusObject)
            {
                lookingAtSeat = true;

                if (!hasSatOnSeat && clickImage != null)
                    clickImage.SetActive(true);

                return; // Skip checking other targets
            }

            // Check other focus targets
            foreach (FocusTarget target in focusTargets)
            {
                if (hit.transform == target.targetObject)
                {
                    currentFocus = target;
                    break;
                }
            }
        }

        // Show click UI for valid focus targets (not seat)
        if (clickImage != null)
        {
            bool showClick = currentFocus != null &&
                             (currentFocus.uiElement == null || !currentFocus.uiElement.activeSelf);
            clickImage.SetActive(showClick);
        }
    }

    void CheckSeatClick()
    {
        if (hasSatOnSeat) return;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, focusRange) && hit.transform == seatFocusObject)
        {
            if (Input.GetMouseButtonDown(0))
            {
                EtoOutUI.SetActive(true);
                // Move player to seat position
                Player.transform.position = seatTargetPosition;

                // Disable Rigidbody physics
                if (playerRigidbody != null)
                    playerRigidbody.isKinematic = true;

                // Parent to vehicle
                Player.transform.SetParent(Vehicle.transform);

                // Disable movement
                if (playerMovementScript != null)
                    playerMovementScript.enabled = false;

                // Hide UI
                if (clickImage != null)
                    clickImage.SetActive(false);

                hasSatOnSeat = true;
            }
        }
    }

    void CheckRelocationInput()
    {
        if (hasSatOnSeat && Input.GetKeyDown(KeyCode.E) && relocationTarget != null)
        {
            // Move to relocation point
            Player.transform.position = relocationTarget.position;
            EtoOutUI.SetActive(false);

            // Re-enable movement
            if (playerMovementScript != null)
                playerMovementScript.enabled = true;

            // Unparent
            Player.transform.SetParent(null);

            // Re-enable mouse look
            UnlockControls();

            // Enable Rigidbody
            Rigidbody rb = Player.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = false;

            // Hide UI again
            if (clickImage != null)
                clickImage.SetActive(false);

            // Allow seat interaction again
            hasSatOnSeat = false;

            Debug.Log("Relocated with E key. Click UI reset.");
        }
    }

    void TriggerTargetInteraction(FocusTarget target)
    {
        Debug.Log(target.messageToShow);

        if (target.targetAnimator != null)
        {
            if (target.isOpen)
                target.targetAnimator.SetTrigger(target.closeTriggerName);
            else
                target.targetAnimator.SetTrigger(target.openTriggerName);

            target.isOpen = !target.isOpen;
        }

        if (target.uiElement != null && !target.uiElement.activeSelf)
        {
            target.uiElement.SetActive(true);
            LockControls();

            if (clickImage != null)
                clickImage.SetActive(false);
        }
    }

    public void LockControls()
    {
        isMovementLocked = true;

        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnlockControls()
    {
        isMovementLocked = false;

        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void CloseUIAndResume(GameObject uiPanel)
    {
        if (uiPanel != null)
            uiPanel.SetActive(false);

        UnlockControls();
    }
}