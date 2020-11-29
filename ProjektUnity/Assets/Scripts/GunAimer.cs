using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAimer : MonoBehaviour
{
    [SerializeField] private PlayerControler playerControler;

    private void Start()
    {
        if(playerControler == null)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {

    }
}
