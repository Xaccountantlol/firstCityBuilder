using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public string TileName;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool _isWalkable;

    public BaseUnit OccupiedUnit;
    public bool Walkable => _isWalkable && OccupiedUnit == null;

    


    public virtual void Init(int x, int y)
    {
        // Initialization code (if any)
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
        MenuManager.Instance.ShowTileInfo(this);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
        MenuManager.Instance.ShowTileInfo(null);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.Instance.GameState != GameState.HeroesTurn) return;
            if (OccupiedUnit != null && OccupiedUnit.Faction == Faction.Hero)
                UnitManager.Instance.SetSelectedHero((BaseHero)OccupiedUnit);
            else if (UnitManager.Instance.SelectedHero != null)
                UnitManager.Instance.SetSelectedHero(null);
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (GameManager.Instance.GameState != GameState.HeroesTurn) return;

            BaseHero selectedHero = UnitManager.Instance.SelectedHero;
            if (selectedHero != null)
            {
                BaseEnemy enemy = null;
                Tile treeTile = null;

                // Check if the OccupiedUnit is an enemy
                if (OccupiedUnit != null && OccupiedUnit.Faction == Faction.Enemy)
                {
                    enemy = (BaseEnemy)OccupiedUnit;
                }
                // Check if the current tile is a tree tile
                else if (this.CompareTag("TreeTile"))
                {
                    treeTile = this;
                }

                // If the tile is walkable or there is an enemy, move the hero to this tile
                if (Walkable || enemy != null)
                {
                    SetUnit(selectedHero, enemy);
                }
                // If the tile is a tree tile, move the hero and destroy the tree
                else if (treeTile != null)
                {
                    MoveHeroToTreeTile(selectedHero, treeTile);
                }
            }
        }
    }

    // Method to handle hero movement to a tree tile and destroy it
    private void MoveHeroToTreeTile(BaseHero hero, Tile treeTile)
    {
        // Move the hero to the tree tile position
        hero.transform.position = treeTile.transform.position;

        // Increase the wood count
        ResourceManager.Instance.AddWood(1);

        // Get the position of the tree tile
        Vector2 treeTilePosition = new Vector2(treeTile.transform.position.x, treeTile.transform.position.y);

        // Destroy the tree tile gameObject
        Destroy(treeTile.gameObject);

        // Instantiate a new grass tile at the tree tile's position
        Tile grassTilePrefab = GridManager.Instance.GetGrassTilePrefab();
        Tile newGrassTile = Instantiate(grassTilePrefab, treeTilePosition, Quaternion.identity).GetComponent<Tile>();

        // Initialize the new grass tile if necessary
        newGrassTile.Init((int)treeTilePosition.x, (int)treeTilePosition.y);

        // Replace the tree tile in the GridManager's tiles dictionary
        GridManager.Instance.ReplaceTileAtPosition(treeTilePosition, newGrassTile);
    }

    public void SetUnit(BaseUnit unit, BaseEnemy enemy = null)
    {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;

        StartCoroutine(MoveUnitToTile(unit, transform.position, enemy));
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }

    IEnumerator MoveUnitToTile(BaseUnit unit, Vector3 targetPosition, BaseEnemy enemy)
    {
        float timeToMove = 1.0f;
        float elapsedTime = 0;
        Vector3 startPosition = unit.transform.position;

        while (elapsedTime < timeToMove)
        {
            unit.transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        unit.transform.position = targetPosition;

        // Destroy enemy if it exists and the unit has reached the target position
        if (enemy != null)
        {
            Destroy(enemy.gameObject);
        }
    }
}