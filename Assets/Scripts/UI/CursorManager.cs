using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    private Image crossHair;
    [SerializeField] private Sprite mouseUpCursor;
    [SerializeField] private Sprite mouseDownCursor;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined; // Ensures the cursor stays within the game window
        crossHair = GetComponent<Image>();
        crossHair.color = Color.green;
    }
    private void Update()
    {
        //If game paused
        if(GameManager.instance.IsGamePaused())
        {
            crossHair.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            return;
        }
        else //if not paused
        {
            crossHair.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        Vector2 cursorPos = Input.mousePosition;
        transform.position = cursorPos;

        if(Input.GetMouseButtonDown(0))
        {
            crossHair.sprite = mouseDownCursor;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            crossHair.sprite = mouseUpCursor;
        }
    }
}
