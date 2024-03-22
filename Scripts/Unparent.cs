using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unparent : MonoBehaviour {
    void Start() {
        transform.SetParent(null);
    }
}
