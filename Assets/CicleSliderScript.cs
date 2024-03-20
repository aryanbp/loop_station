using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class KnobController : MonoBehaviour
{
    // Adjust this value to control the rotation speed
    public float rotationSpeed = 1.0f;

    // Z indices for the minimum and maximum rotation angles
    public float minZIndex = 0f;
    public float maxZIndex = 359.9f;

    // Flag to track if the mouse is clicking on the knob
    private bool isMouseClicking = false;

    // Update text of LED Label
    public TextMeshProUGUI Label;

    public GameObject SettingValues;

    // Update is called once per frame
    void Update()
    {
        // If the mouse button is pressed and the cursor is hovering over the knob
        if (Input.GetMouseButtonDown(0) && IsCursorOverKnob())
        {
            isMouseClicking = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseClicking = false;
        }

        // If the mouse is clicking on the knob, rotate it
        if (isMouseClicking)
        {
            // Get the mouse movement along the x-axis
            float mouseX = Input.GetAxis("Mouse X");

            // Calculate the rotation amount based on mouse movement
            float rotationAmount = mouseX * rotationSpeed;

            // Rotate the knob around its z-axis
            float newZIndex = Mathf.Clamp(transform.eulerAngles.z + rotationAmount, minZIndex, maxZIndex);
            if (SettingValues.GetComponent<SettingsPanelScript>().knobReset)
            {
                SettingValues.GetComponent<SettingsPanelScript>().knobReset = false;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            }
            else if (SettingValues.GetComponent<SettingsPanelScript>().knobSet)
            {
                SettingValues.GetComponent<SettingsPanelScript>().knobSet = false;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(SettingValues.GetComponent<SettingsPanelScript>().z + rotationAmount, minZIndex, maxZIndex));
            }
            else
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newZIndex);
            }

            // Calculate the value based on the rotation angle
            float value = CalculateValueFromRotation(newZIndex);
            SettingValues.GetComponent<SettingsPanelScript>().FunctionSelected(value, newZIndex);
            /*Label.text = value.ToString();*/
        }
    }

    // Check if the cursor is hovering over the knob
    bool IsCursorOverKnob()
    {
        // Cast a ray from the mouse position to detect if it hits the knob collider
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        return hit.collider != null && hit.collider.gameObject == gameObject;
    }

    // Calculate the value based on the rotation angle
    int CalculateValueFromRotation(float rotationAngle)
    {
        // Map the rotation angle to a value between 1 and 100
        float mappedValue = Mathf.InverseLerp(minZIndex, maxZIndex, rotationAngle);
        float value = Mathf.Lerp(1, 100, mappedValue);

        // Round the value to the nearest integer
        int roundedValue = Mathf.RoundToInt(value);
        return roundedValue;
    }

}
