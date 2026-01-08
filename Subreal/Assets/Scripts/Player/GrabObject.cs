using UnityEngine;

public class ObjectGrab : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;  
    public Transform holdPosition;

    [Header("Grab Settings")]
    public float grabDistance = 3f;
    public float throwForce = 10f;
    public string grabbableTag = "Grabbable"; 
    public string fusibleTag = "FusibleBox";
    public KeyCode grabKey = KeyCode.E;
    public KeyCode throwKey = KeyCode.F;
    public LayerMask playerLayer;
    
    [Header("Hold Distance Settings")]
    public float minHoldDistance = 1f;
    public float maxHoldDistance = 5f;
    public float holdDistanceScrollSpeed = 0.5f;
    private float currentHoldDistance = 2f;

    [Header("Audio")]
    [SerializeField] private AudioSource grabAudioSource;
    [SerializeField] private AudioClip grabSound;
    [SerializeField] private AudioClip dropSound;
    [SerializeField] private AudioClip throwSound;
    
    private GameObject heldObject;
    private Rigidbody heldRigidbody;
    private bool isHolding = false;
    private int originalLayer;

    void Start()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera not assigned on ObjectGrab script!");
        }
        
        if (holdPosition == null)
        {
            Debug.LogError("Hold Position not assigned on ObjectGrab script!");
        }
        
        currentHoldDistance = Mathf.Clamp((minHoldDistance + maxHoldDistance) / 2f, minHoldDistance, maxHoldDistance);
        
        if (grabAudioSource == null)
        {
            grabAudioSource = GetComponent<AudioSource>();
            if (grabAudioSource == null)
            {
                grabAudioSource = gameObject.AddComponent<AudioSource>();
                grabAudioSource.playOnAwake = false;
                grabAudioSource.spatialBlend = 0f; 
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(grabKey))
        {
            if (!isHolding)
                GrabObject();
            else
                DropObject();
        }

        if (Input.GetKeyDown(throwKey) && isHolding)
        {
            ThrowObject();
        }
        
        if (isHolding && heldObject != null)
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                currentHoldDistance += scrollInput * holdDistanceScrollSpeed;
                currentHoldDistance = Mathf.Clamp(currentHoldDistance, minHoldDistance, maxHoldDistance);
                Debug.Log("Distance de maintien ajustée à: " + currentHoldDistance);
            }
            
            Vector3 targetPosition = playerCamera.transform.position + playerCamera.transform.forward * currentHoldDistance;
            
            heldRigidbody.linearVelocity = (targetPosition - heldObject.transform.position) * 10f;
            heldRigidbody.angularVelocity = Vector3.Lerp(heldRigidbody.angularVelocity, Vector3.zero, Time.deltaTime * 5f);
        }
    }

    void GrabObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, grabDistance))
        {  
            GameObject hitObject = hit.collider.gameObject;
            Debug.Log("Hit object: " + hitObject.name);
            
            if (hitObject.CompareTag(grabbableTag)) 
            {
                Rigidbody rb = hitObject.GetComponent<Rigidbody>();
                
                if (rb != null)
                {
                    heldObject = hitObject;
                    heldRigidbody = rb;

                    originalLayer = heldObject.layer;
                    
                    heldObject.layer = LayerMask.NameToLayer("Carried"); 
                    
                    heldRigidbody.useGravity = false;
                    heldRigidbody.linearDamping = 10;
                    heldRigidbody.angularDamping = 10;
                    heldRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                    
                    PlaySound(grabSound);
                    
                    isHolding = true;
                    Debug.Log("Grabbed object: " + heldObject.name);
                }
                else
                {
                    Debug.LogWarning("Grabbable object " + hitObject.name + " is missing a Rigidbody!");
                }
            }
            else if (hitObject.CompareTag(fusibleTag))
            {
                Debug.Log("Interacting with fusible box: " + hitObject.name);
                
                if (hit.collider.TryGetComponent(out FuseBox box))
                    {
                        box.EnterPuzzle();
                    }
                else
                {
                    Debug.LogWarning("Fusible box " + hitObject.name + " is missing the FuseBox script!");
                }
            }
            else
            {
                Debug.Log("Hit object is not tagged as grabbable.");
            }
        }
        else
        {
            Debug.Log("No object hit in range.");
        }
    }   
 void DropObject()
    {
        if (heldObject != null)
        {   
            heldObject.layer = originalLayer;
            heldRigidbody.useGravity = true;
            heldRigidbody.linearDamping = 1;
            heldRigidbody.angularDamping = 0.05f;
            
            PlaySound(dropSound);
            
            Debug.Log("Dropped object: " + heldObject.name);
            
            heldObject = null;
            heldRigidbody = null;
            isHolding = false;
        }
    }

    void ThrowObject()
    {
        if (heldObject != null)
        {
            heldObject.layer = originalLayer;
            heldRigidbody.useGravity = true;
            heldRigidbody.linearDamping = 1;
            heldRigidbody.angularDamping = 0.05f;
            
            heldRigidbody.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse);
            
            PlaySound(throwSound);
            
            heldObject = null;
            heldRigidbody = null;
            isHolding = false;
        }
    }
    
    private void PlaySound(AudioClip sound)
    {
        if (grabAudioSource != null && sound != null)
        {
            grabAudioSource.clip = sound;
            grabAudioSource.Play();
        }
    }

    public GameObject GetHeldObject()
    {
        return heldObject;
    }
}