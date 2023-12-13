using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private GridVisualizer gridVisualizer;

    // Start is called before the first frame update
    void Start()
    {
        gridVisualizer = FindObjectOfType<GridVisualizer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Here you can add code to interact with the grid
        // For example, get mouse position and convert it to grid coordinates
    }
}
