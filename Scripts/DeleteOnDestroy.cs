using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOnDestroy : MonoBehaviour {
    [SerializeField] private GameObject target;
    void OnDestroy() {
        Destroy(target);
    }
}
