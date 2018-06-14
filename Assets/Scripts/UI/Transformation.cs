using UnityEngine;
using System.Collections;

public class Transformation : MonoBehaviour
{

    public static System.Collections.Generic.List<TransformationTarget> list = new System.Collections.Generic.List<TransformationTarget>();
    public float tolerance = 0.5f;


    public static void Set(GameObject subject, object targetPosition, object targetScale, object targetRotation, float speed, bool endless, System.Action callback)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].subject == subject)
            {
                list.RemoveAt(i);
            }
        }
        TransformationTarget tt = new TransformationTarget(subject, targetPosition, targetScale, targetRotation, speed, endless);
        tt.OnTransformationEnd += callback;
        list.Add(tt);
    }

    void FixedUpdate ()
    {
	    for (int i = 0; i < list.Count; i++)
        {
            bool end = true;
            TransformationTarget t = list[i];
            if (t.targetPosition != null)
            {
                t.subject.transform.position = Vector3.Lerp(t.subject.transform.position, (Vector3)t.targetPosition, Time.deltaTime * t.speed);
                if (Vector3.Distance(t.subject.transform.position, (Vector3)t.targetPosition) > tolerance)
                {
                    end = false;
                }
            }
            if (t.targetScale != null)
            {
                t.subject.transform.localScale = Vector3.Lerp(t.subject.transform.localScale, (Vector3)t.targetScale, Time.deltaTime * t.speed);
                if (Vector3.Distance(t.subject.transform.localScale, (Vector3)t.targetScale) > tolerance)
                {
                    end = false;
                }
            }
            if (t.targetScale != null)
            {
                float x = t.subject.transform.rotation.x;
                float y = t.subject.transform.rotation.y;
                float z = t.subject.transform.rotation.z;
                t.subject.transform.rotation = Quaternion.Lerp(t.subject.transform.rotation, (Quaternion)t.targetRotation, Time.deltaTime * t.speed);
                if (x - ((Quaternion)t.targetRotation).x > tolerance || y - ((Quaternion)t.targetRotation).y > tolerance || z - ((Quaternion)t.targetRotation).z > tolerance)
                {
                    end = false;
                }
            }
            if (end)
            {
                if (!t.endless)
                {
                    t.TransfomationEnded();
                    list.RemoveAt(i);
                }
                Debug.Log("Transformation ended!");
            }
        }
	}
}
