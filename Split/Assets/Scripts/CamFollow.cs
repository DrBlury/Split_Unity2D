using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour {

    public GameObject target;
	
	// Update is called once per frame
	void Update () {
        if (target != null) {
            transform.position = target.transform.position + new Vector3(0, 0, -1);
        }
	}
}
