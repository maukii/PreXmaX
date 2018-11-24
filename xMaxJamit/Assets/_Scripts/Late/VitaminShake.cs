using UnityEngine;
using System.Collections;

public class VitaminShake : MonoBehaviour
{

    public Transform camTransform;

    [SerializeField] float shakeDuration = 0f;
    [SerializeField] float shakeAmount = 0.7f;
    [SerializeField] float decreaseFactor = 1.0f;

    public bool shaketrue = false;

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
        originalShakeDuration = shakeDuration; 
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
        shakeDuration = originalShakeDuration;
        camTransform.localPosition = originalPos;
        }
    }
    
}