using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.K)) {
            Debug.Log("Den day roi");
            // gameObject.GetComponent<CharacterController>().Move();
            transform.Translate(new Vector3(0.1f, 0, 0));
        }
    }
}
