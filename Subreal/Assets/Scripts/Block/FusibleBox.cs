using UnityEngine;

public class FuseBox : MonoBehaviour
{
    [Header("Cameras")]
    public Camera playerCamera;
    public Camera puzzleCamera;

    [Header("Player Settings")]
    public PlayerMovement playerMovement; 
    public PlayerInteraction playerInteraction;

    private bool isFocused = false;

    void Update()
    {
        if (isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitPuzzle();
            }

            if (Input.GetMouseButtonDown(0))
            {
                TryClickButton();
            }
        }
    }

    private void TryClickButton()
    {
        Ray ray = puzzleCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            ClickableFuse fuse = hit.collider.GetComponent<ClickableFuse>();
            
            if (fuse != null)
            {
                fuse.Interact(playerMovement);
            }
        }
    }

    public void EnterPuzzle()
    {
        isFocused = true;

        playerCamera.enabled = false;
        puzzleCamera.enabled = true;

        playerMovement.enabled = false;
        
        var rb = playerMovement.GetComponent<Rigidbody>();
        if(rb != null) rb.linearVelocity = Vector3.zero;

        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;                

        playerInteraction.SetPaused(true);
    }

    public void ExitPuzzle()
    {
        isFocused = false;

        playerCamera.enabled = true;
        puzzleCamera.enabled = false;

        playerMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;                 

        playerInteraction.SetPaused(false);
    }
}