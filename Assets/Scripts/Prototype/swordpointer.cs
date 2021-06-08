using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordpointer : MonoBehaviour
{
    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //print(Input.mousePosition);

        transform.LookAt(cube.transform, Vector3.up);

        Vector3 pos = transform.position;
        pos.y = pos.z;
        pos.z = 0;
        transform.position = pos;

        //transform.rotation = Quaternion.Euler(Vector3.RotateTowards(transform.eulerAngles, Input.mousePosition, 1, 1));
    }
}
