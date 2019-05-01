using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : MonoBehaviour {

    public MeshRenderer MakeInvisible;

    // Use this for initialization
    void Start () {
        MakeInvisible.enabled = false;
    }
}
