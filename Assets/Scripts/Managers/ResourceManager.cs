using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance; // Static instance

    public double woodCount = 100;
    public TextMeshProUGUI woodCountText;

    void Awake()
    {
        // Assign the static instance
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Ensures there is only one instance in the scene
        }

        UpdateWoodCountDisplay();
    }

    public void AddWood(int amount)
    {
        woodCount += amount;
        UpdateWoodCountDisplay();
    }

    public void UseWood(int amount)
    {
        woodCount -= amount;
        UpdateWoodCountDisplay();
    }

    private void UpdateWoodCountDisplay()
    {
        woodCountText.text = "Wood: " + woodCount.ToString();
    }
}
