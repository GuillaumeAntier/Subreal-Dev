using UnityEngine;

public class ClickableFuse : MonoBehaviour
{
    private Renderer objectRenderer;
    private Color originalColor;
    private bool isActive = false;

    public Color activeColor = Color.red;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }

    public void Interact(PlayerMovement playerScript)
    {
        if (objectRenderer == null) return;

        isActive = !isActive;

        if (isActive)
        {
            objectRenderer.material.color = activeColor;
            playerScript.nombreDeFusibles--; 
        }
        else
        {
            objectRenderer.material.color = originalColor;
            playerScript.nombreDeFusibles++;
        }

        Debug.Log("Fusibles actifs : " + playerScript.nombreDeFusibles);
    }
}