using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestryByTime : MonoBehaviour
{
    public float destroyTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
