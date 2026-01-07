using UnityEngine;

public class AnimationFusible : MonoBehaviour
{
    [Header("Réglages Rotation")]
    public float vitesseRotation = 50f;
    public Vector3 axeDeRotation = new Vector3(0, 1, 0); 

    [Header("Réglages Flottement")]
    public float hauteurFlottement = 0.5f;
    public float vitesseFlottement = 1f;

    private Vector3 positionDepart;

    void Start()
    {
        positionDepart = transform.position;
    }

    void Update()
    {
        transform.Rotate(axeDeRotation * vitesseRotation * Time.deltaTime);

        float y = Mathf.Sin(Time.time * vitesseFlottement) * hauteurFlottement;
        transform.position = positionDepart + Vector3.up * y;
    }
}