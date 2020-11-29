using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputByMouse : InputBase
{
    private float dragTimer = 0f;
    private Vector2 deltaMouse;
    [SerializeField] private float dragThresshold = 0.5f;
    [SerializeField] private float rotationSpeed = 50f;


    public override void UpdateMe(Transform playerPivot, Action onClickAction)
    {
        Vector2 mousePoz = Input.mousePosition;
        Vector2 movement = mousePoz - deltaMouse;

        bool isMouseOverUI = EventSystem.current.IsPointerOverGameObject();
        if(isMouseOverUI)
        {
            deltaMouse = mousePoz;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            if (dragTimer >= dragThresshold)
            {
                playerPivot.Rotate(new Vector3(0f, movement.x * rotationSpeed * Time.deltaTime, 0f));
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (dragTimer < dragThresshold)
            {
                onClickAction?.Invoke();
            }
        }

        if (Input.GetMouseButton(0))
        {
            dragTimer += Time.deltaTime;
        }
        else
        {
            dragTimer = 0f;
        }

        deltaMouse = mousePoz;


    }
}