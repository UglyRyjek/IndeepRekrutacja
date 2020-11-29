using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccentPainter : MonoBehaviour
{
    [SerializeField] private Renderer rendere;
    [SerializeField] private Material matOff;
    [SerializeField] private Material matOn;
    private IObjective objective;

    private void Reset()
    {
        rendere = GetComponent<Renderer>();
    }

    private void Start()
    {
        objective = GetComponentInParent<IObjective>();

        if(matOff == null)
        {
            matOff = DataBase.DB.accentMaterial_Off;
        }
        if(matOn == null)
        {
            matOn = DataBase.DB.accentMaterial_On;
        }

    }

    private void Update()
    {
            if (rendere != null && objective != null)
            {
                rendere.material = objective.IsCompleted() ? matOn : matOff;
            }
    }
}
