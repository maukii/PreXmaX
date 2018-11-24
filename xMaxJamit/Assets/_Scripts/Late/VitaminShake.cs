using UnityEngine;

public class VitaminShake : MonoBehaviour
{

    public Transform camTransform;

    public float shakeDuration;
    public float shakeAmount;
    [SerializeField] float decreaseFactor = 1.0f;

    Vector3 originalPos;
    float originalShakeDuration;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, originalPos + Random.insideUnitSphere * shakeAmount, Time.deltaTime * 3);
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            camTransform.localPosition = originalPos;
            shakeDuration = 0;
        }
    }
    
}