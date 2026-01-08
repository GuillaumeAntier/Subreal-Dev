using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;
    public KeyCode interactKey = KeyCode.E;

    [Header("UI References")]
    public Image crosshair;
    public TextMeshProUGUI interactionText;

    private Camera playerCamera;
    private GameObject currentInteractable;
    [HideInInspector] public bool isPaused = false;

    void Start()
    {
        playerCamera = Camera.main;
        
        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }

    void Update()
    {   
        if (isPaused)
        {
            if (interactionText != null) interactionText.gameObject.SetActive(false);
            if (crosshair != null) crosshair.gameObject.SetActive(false); 
            return; 
        }

        CheckForInteractable();
        
        if (currentInteractable != null && Input.GetKeyDown(interactKey))
        {
            InteractWithObject(currentInteractable);
        }
    }

    void CheckForInteractable()
    {
        RaycastHit hit;
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            currentInteractable = hit.collider.gameObject;
            
            if (crosshair != null)
                crosshair.color = Color.green;
                
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(true);
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Grabbable"))
                    interactionText.text = "Press E to grab";
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Fusible"))
                    interactionText.text = "Press E to pick up";
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("FusibleBox"))
                    interactionText.text = "Press E to open";
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Button"))
                    interactionText.text = "Press E to press";
            }
        }
        else
        {
            currentInteractable = null;
            
            if (crosshair != null)
                crosshair.color = Color.white;
                
            if (interactionText != null)
                interactionText.gameObject.SetActive(false);
        }
    }
    
    public void SetPaused(bool state)
    {
        isPaused = state;
        if (!state && crosshair != null) crosshair.gameObject.SetActive(true);
    }
    
    void InteractWithObject(GameObject interactable)
    {
        Debug.Log($"Interacting with {interactable.name}");
    }
}