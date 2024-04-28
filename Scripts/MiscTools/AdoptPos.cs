using UnityEngine;
using UnityEngine.Events;

public class AdoptPosition : MonoBehaviour
{
    public Transform object1; 
    public bool setx;
    public bool sety;
    public bool setz;
    public Vector3 maxpos;
    public Vector3 minpos;

    public UnityEvent<Transform> onObject1Change;

    void Update()
    {
        Vector3 objtransform = transform.position;

        if (setx)
            objtransform.x = object1.position.x;

        if (sety)
            objtransform.y = object1.position.y;

        if (setz)
            objtransform.z = object1.position.z;

        objtransform.x = Mathf.Clamp(objtransform.x, minpos.x, maxpos.x);
        objtransform.y = Mathf.Clamp(objtransform.y, minpos.y, maxpos.y);
        objtransform.z = Mathf.Clamp(objtransform.z, minpos.z, maxpos.z);

        transform.position = objtransform;
    }

    public void ChangeObject1(Transform newObject)
    {
        object1 = newObject;
    }
}