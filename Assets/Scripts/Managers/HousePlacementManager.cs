using UnityEngine;
using System;


public class HousePlacementManager : MonoBehaviour
{
    public GameObject housePrefab; // Assign in Inspector
    private GameObject currentHouseInstance;
    private bool isHouseSelected = false;
    private SpriteRenderer houseRenderer; // Reference to the SpriteRenderer of the house prefab
    private Color originalColor; // Store the original color of the house
    private Color invalidColor = Color.red;
    private bool isRotated = false;

    [SerializeField] private int width = 3;
    [SerializeField] private int height = 2;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (GameManager.Instance.GameState != GameState.BuildingPlacement)
            {
                GameManager.Instance.ChangeState(GameState.BuildingPlacement);
                isHouseSelected = true;
                CreateHouseInstance();
            }
            else
            {
                GameManager.Instance.ChangeState(GameState.HeroesTurn);

                isHouseSelected = false;
            }
        }

        if (isHouseSelected && houseRenderer != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int gridPosition = SnapPositionToGrid(mousePosition);
            Vector3 worldPosition = GetCellCenter(gridPosition);
            // Note: depending on your prefab setup, this worldPosition may still not give you the visual offset you'd like
            // maybe you don't want to use GetCellCenter and would rather just stick with the snapped corner of a tile, gridPosition


            bool canBePlaced = IsPlacementValid(gridPosition, width, height);

            currentHouseInstance.transform.position = worldPosition;
            currentHouseInstance.SetActive(true);

            houseRenderer.color = canBePlaced ? originalColor : invalidColor;

            if (canBePlaced && Input.GetMouseButtonDown(0))
            {

                isHouseSelected = false;
                GameManager.Instance.ChangeState(GameState.HeroesTurn); // Change state back after placement
                                                                        // currentHouseInstance = null; // Uncomment this if you want to destroy the reference
                                                                        // Destroy(currentHouseInstance);
                currentHouseInstance = null;
            }
            if (Input.GetKeyDown(KeyCode.R) && currentHouseInstance != null)
            {
                isRotated = false; // Toggle rotation state

                currentHouseInstance.transform.Rotate(0, 0, 90);
            }
        }
    }

    private void CreateHouseInstance()
    {
        if (currentHouseInstance != null)
        {
            Destroy(currentHouseInstance);
        }
        currentHouseInstance = Instantiate(housePrefab);
        houseRenderer = currentHouseInstance.GetComponentInChildren<SpriteRenderer>();
        originalColor = houseRenderer.color;
        houseRenderer.color = invalidColor;
        // Make sure to set the rotation to default
        currentHouseInstance.transform.rotation = Quaternion.identity;
    }




    private bool IsPlacementValid(Vector2Int gridPosition, int originalWidth, int originalHeight)
    {
        // Check the current rotation of the building
        bool isRotated = Mathf.RoundToInt(currentHouseInstance.transform.eulerAngles.z / 90f) % 2 != 0;



        // Swap width and height based on rotation
        int width = isRotated ? originalHeight : originalWidth;
        int height = isRotated ? originalWidth : originalHeight;

        // Adjust the starting grid position if necessary
        // Depending on where your pivot point is, you might need to adjust this logic
        Vector2Int adjustedGridPosition = gridPosition; // Adjust this based on your pivot and rotation logic

        int xMultiplier = 1;
        int yMultiplier = 1;

        int currentRotation = (int)currentHouseInstance.transform.eulerAngles.z;
        if(currentRotation == 90 || currentRotation == 180){
            xMultiplier = -1;

        }

        if(currentRotation == 180 || currentRotation ==270){
             yMultiplier = -1;
        }

        // Check each tile in the area covered by the building
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int checkPosition = adjustedGridPosition + new Vector2Int(x*xMultiplier, y*yMultiplier);
                Tile tile = GridManager.Instance.GetTileAtPosition(checkPosition);

                if (tile == null || !(tile is GrassTile) || tile.OccupiedUnit != null)
                {
                    return false; // Invalid position for placement
                }
            }
        }
        return true; // Valid position for placement
    }


    private Vector2Int SnapPositionToGrid(Vector3 worldPosition) => Vector2Int.FloorToInt((Vector2)worldPosition);

    private Vector3 GetCellCenter(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x, gridPos.y, 0f); // Explicitly create a new Vector3
    }





}