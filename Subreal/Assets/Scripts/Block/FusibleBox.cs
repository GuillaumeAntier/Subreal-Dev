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
        if (isFocused && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPuzzle();
        }
    }

    public void EnterPuzzle()
    {
        isFocused = true;

        playerCamera.enabled = false;
        puzzleCamera.enabled = true;

        playerMovement.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerMovement.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
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