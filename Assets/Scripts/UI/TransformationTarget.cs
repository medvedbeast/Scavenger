using UnityEngine;
using System.Collections;

public class TransformationTarget
{
    public GameObject subject;
    public object targetPosition;
    public object targetScale;
    public object targetRotation;
    public float speed;
    public bool endless;

    public event System.Action OnTransformationEnd;

    public TransformationTarget(GameObject subject, object targetPosition, object targetScale, object targetRotation, float speed, bool endless)
    {
        this.subject = subject;
        this.targetPosition = targetPosition;
        this.targetScale = targetScale;
        this.targetRotation = targetRotation;
        this.speed = speed;
        this.endless = endless;
    }

    public void TransfomationEnded()
    {
        if (OnTransformationEnd != null)
        {
            OnTransformationEnd();
            Dispose();
        }
    }

    void Dispose()
    {
        OnTransformationEnd = null;
    }
}
