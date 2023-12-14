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
                if (OccupiedUnit != null && OccupiedUnit.Faction == Faction.Enemy)
                {
                    enemy = (BaseEnemy)OccupiedUnit;
                }

                if (Walkable || enemy != null)
                {
                    SetUnit(selectedHero, enemy);
                }
            }
        }
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