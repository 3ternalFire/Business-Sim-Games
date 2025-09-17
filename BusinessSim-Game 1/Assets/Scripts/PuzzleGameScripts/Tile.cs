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
        [SerializeField] private GameObject fxPrefab;

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

        public void SetTileType(TileType type, bool isScore = false)
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
                    _spriteRenderer.sprite = emptySprite;
                    if (isScore) 
                    {
                        Instantiate(fxPrefab, transform.position, Quaternion.identity);
                    }
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


