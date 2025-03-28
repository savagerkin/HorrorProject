using UnityEngine;

public class CandleLightController : MonoBehaviour
{
    public Light pointLight; // Assign the Point Light in the Inspector

    private void Awake()
    {
        // Automatically find the light if not assigned
        if (pointLight == null)
        {
            pointLight = GetComponentInChildren<Light>();
        }
    }

    public void SetLightEnabled(bool state)
    {
        if (pointLight != null)
        {
            pointLight.enabled = state;
        }
    }
}