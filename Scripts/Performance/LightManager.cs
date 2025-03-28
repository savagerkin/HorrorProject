using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Transform player; // Drag your player object here in the Inspector
    [SerializeField] private float activationRadius = 10f; // Distance at which lights should activate
    private List<CandleLightController> allCandles = new List<CandleLightController>();

    private void Start()
    {
        // Cache all CandleLightController components in the scene
        allCandles.AddRange(FindObjectsOfType<CandleLightController>());
    }

    private void Update()
    {
        UpdateLights();
    }

    private void UpdateLights()
    {
        foreach (CandleLightController candle in allCandles)
        {
            float distance = Vector3.Distance(player.position, candle.transform.position);
            bool shouldEnable = distance <= activationRadius;
            candle.SetLightEnabled(shouldEnable);
        }
    }
}
