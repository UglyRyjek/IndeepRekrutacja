using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMarker : MonoBehaviour
{
    private void Awake()
    {
        if(type == MarkerType.CAMERA)
        {
            Camera c = GetComponent<Camera>();
            if(c != null)
            {
                c.enabled = false;
            }
        }
    }

    public MarkerType type;
}

[System.Serializable]
public enum MarkerType
{
    NONE = 0,
    PLAYER_START = 1,
    CAMERA = 2,
}
