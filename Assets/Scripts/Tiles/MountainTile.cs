using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainTile : Tile
{
    // This could be a list of sprites you want to add to the tree tile.
    [SerializeField] private List<Sprite> additionalSprites;
    // [SerializeField] private Color _baseColor, _offsetColor;

    // Start is called before the first frame update
    void Start()
    {
        // Call base initialization
        base.Init((int)transform.position.x, (int)transform.position.y);

        // Add additional sprites as children
        foreach (var sprite in additionalSprites)
        {
            CreateChildSprite(sprite);
        }
    }


    /* public override void Init(int x, int y)
     {
         var isOffset = (x + y) % 2 == 1;
         _renderer.color = isOffset ? _offsetColor : _baseColor;
     }*/

    private void CreateChildSprite(Sprite sprite)
    {
        // Create a new GameObject to hold the sprite renderer
        GameObject spriteObject = new GameObject("AdditionalSprite");
        SpriteRenderer renderer = spriteObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;

        // Set the new GameObject as a child of the tree tile
        spriteObject.transform.parent = this.transform;

        // Position it on the same spot as the parent
        spriteObject.transform.localPosition = Vector3.zero;

        // You can adjust the local position, scale, and sorting order as needed
        // For example, to layer sprites correctly, you might set different sorting orders:
        // renderer.sortingOrder = ...;
    }

    // Existing code...
}

