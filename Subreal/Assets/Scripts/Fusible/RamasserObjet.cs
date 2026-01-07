using UnityEngine;

public class RamasserObjet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement joueur = other.GetComponentInParent<PlayerMovement>();

        if (joueur != null)
        {
            joueur.nombreDeFusibles += 1;
            gameObject.SetActive(false);
        }
    }
}