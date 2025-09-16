using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    public enum TileType
    {
        Empty,
        Wooden,
        Jewel,
        Selected
    }

    public class Tile : MonoBehaviour
    {
        public TileType tileType { get; private set;}
        public Vector2Int gridPosition { get; private set; }

        [SerializeField] private Color _baseColor, _offsetColor, _selectedColor;

        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private GameObject selectedPrefab;

        [SerializeField] private Sprite woodenSprite;
        [SerializeField] private Sprite emptySprite;
        [SerializeField] private Sprite jewelSprite;
        public void SetTile(Vector2Int gridPos, TileType type = TileType.Empty, bool isOffset = false)
        {
            selectedPrefab.SetActive(false);
            if (_spriteRenderer != null) _spriteRenderer.color = isOffset ? _offsetColor : _baseColor;
            gridPosition = gridPos;
            SetTileType(type);
        }

        public void SetTileType(TileType type)
        {
            tileType = type;
            switch (tileType) 
            {
                case TileType.Wooden:
                    _spriteRenderer.sprite = woodenSprite;
                    break;
                case TileType.Jewel:
                    break;
                case TileType.Selected:
                    break;
                case TileType.Empty:
                    break;
            }

        }

        public void SelectTile()
        {
            _spriteRenderer.color += _selectedColor;
        }

        public void SetSelected(bool isActive)
        {
            selectedPrefab.SetActive(isActive);
        }
    }
}


