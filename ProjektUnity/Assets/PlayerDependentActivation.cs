using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDependentActivation : MonoBehaviour
{
    [SerializeField] private PlayerControler playerControler;
    [SerializeField] private Transform dependentObject;
    [SerializeField] List<PlayerState> activeInStates = new List<PlayerState>();

    private void Start()
    {
        if (activeInStates.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        if(playerControler == null)
        {
            playerControler = GetComponentInParent<PlayerControler>();
        }

        if(playerControler == null)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerControler && dependentObject)
        {
            bool shouldBeActive = activeInStates.Contains(playerControler.playerState);
            dependentObject.gameObject.SetActive(shouldBeActive);
        }
    }
}
