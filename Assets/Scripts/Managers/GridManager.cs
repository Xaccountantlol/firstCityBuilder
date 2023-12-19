using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public Camera gridCamera;
    public static GridManager Instance;
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _grassTile, _mountainTile, _treeTile;

    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;

    public void ReplaceTileAtPosition(Vector2 position, Tile newTile)
    {
        if (_tiles.ContainsKey(position))
        {
            _tiles[position] = newTile;
        }
    }

    public Tile GetGrassTilePrefab()
    {
        return _grassTile;
    }

    void Awake()
    {
        Instance = this;
    }

    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tile randomTile;
                int randomType = Random.Range(0, 10); // Range from 0 to 9

                if (randomType == 0) // 1 out of 10 for 10%
                {
                    randomTile = _mountainTile;
                }
                else if (randomType >= 1 && randomType <= 4) // 4 out of 10 for 40%
                {
                    randomTile = _treeTile; // This is your new forest tile
                }
                else // The remaining 50% chance
                {
                    randomTile = _grassTile;
                }

                var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.Init(x, y);
                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);

        GameManager.Instance.ChangeState(GameState.SpawnHeroes);
    }

    public Tile GetHeroSpawnTile()
    {
        return _tiles.Where(t => t.Key.x < _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    public Tile GetEnemySpawnTile()
    {
        return _tiles.Where(t => t.Key.x > _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
  /*  private void OnGUI()
    {
        foreach (KeyValuePair<Vector2, Tile> entry in _tiles)
        {
            Tile tile = entry.Value;
            Vector3 point = gridCamera.WorldToScreenPoint(tile.transform.position);

            // Invert the y coordinate to work with GUI's coordinate system
            point.y = Screen.height - point.y;

            // Draw the label on the screen
            GUI.Label(new Rect(point.x - 25, point.y - 10, 50, 20), $"{entry.Key.x},{entry.Key.y}");
        }
    }*/
}
