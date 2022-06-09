using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantScale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // scale
        transform.localScale = new Vector3(2.0f, 2.0f, 0);
    }
}
