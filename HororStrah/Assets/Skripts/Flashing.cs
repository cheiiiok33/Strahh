using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight;

    void Start()
    {
        if (flashlight != null)
        {
            flashlight.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (flashlight != null)
            {
                flashlight.enabled = !flashlight.enabled;
            }
        }
    }
}