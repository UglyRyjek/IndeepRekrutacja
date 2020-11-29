using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// helper roboczy
public class Helper_PositionSaver : MonoBehaviour
{

    public List<PositionRotation> data = new List<PositionRotation>();

    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            data.Add(new PositionRotation(transform));
        }
    }
}


[System.Serializable]
public class PositionRotation
{
    public PositionRotation (Transform t)
    {
        position = t.position;
        rotation = t.eulerAngles;
    }

    public Vector3 position;
    public Vector3 rotation;
}