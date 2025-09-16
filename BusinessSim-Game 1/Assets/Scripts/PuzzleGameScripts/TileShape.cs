using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleGame;
using Unity.VisualScripting;


[RequireComponent(typeof(Collider2D))]
public class TileShape : MonoBehaviour, ITapable
{
    [SerializeField] private Vector2Int[] shapeOffset = new Vector2Int[4];
    [SerializeField] private GameObject visuals;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float selectedScale = 1.2f;
    private bool isHeld;
    private Vector2 touchOffset;

   // private bool something;
    private Vector2 localScale;
    private Vector2 startingPosition;
    private float elapsedTime;

    private List<Tile> tempList = new List<Tile>();
    private void OnEnable()
    {
        PuzzleGame.InputManager.OnTapped += OnTapped;
        PuzzleGame.InputManager.OnReleased += OnReleased;
    }
    private void OnDisable()
    {
        PuzzleGame.InputManager.OnTapped -= OnTapped;
        PuzzleGame.InputManager.OnReleased -= OnReleased;
    }
    private void Awake()
    {
        if(visuals != null) localScale = visuals.transform.localScale;
        SetStartSpawn();
    }

    public void SetStartSpawn()
    {
        startingPosition = transform.position;
    }
    private void Update()
    {
        if (isHeld)
        {
            MoveToPosition();
            if (PuzzleGame.InputManager.instance.DidTouchMove())
            {
                CheckGridPosition();
            }
        }
        if (!isHeld && visuals.transform.localScale.magnitude > localScale.magnitude)
        { 
            elapsedTime += Time.deltaTime;
            visuals.transform.localScale = new Vector2
                (Mathf.Lerp(visuals.transform.localScale.x, localScale.x, elapsedTime / duration), 
                Mathf.Lerp(visuals.transform.localScale.y, localScale.y, elapsedTime / duration));

            float posX = Mathf.Lerp(transform.position.x, startingPosition.x, elapsedTime / duration);
            float posY = Mathf.Lerp(transform.position.y, startingPosition.y, elapsedTime / duration);

            transform.position = new Vector2(posX, posY);

        }
    }
    public Vector2Int[] GetShapeOffset()
    {
        return shapeOffset;
    }

    public void OnTapped(ITapable tapped)
    {
        if((ITapable)this == tapped)
        {
            isHeld = true;
            touchOffset = PuzzleGame.InputManager.instance.GetTouchPos() - (Vector2)transform.position;
            localScale = visuals.transform.localScale;
            if(visuals != null) visuals.transform.localScale *= selectedScale;
        }
    }

    public void OnReleased(ITapable released)
    {
        if((ITapable)this == released)
        {
            isHeld = false;
            touchOffset = Vector2.zero;
            elapsedTime = 0;
            if (CheckGridPosition())
            {
                foreach(Tile tile in tempList)
                {
                    tile.SetSelected(false);
                    tile.SetTileType(TileType.Wooden);
                }
                tempList.Clear();
                Destroy(gameObject);
            }
        }
    }
    private bool CheckGridPosition()
    {
        if(isHeld && tempList.Count > 0)
        {
            foreach (Tile tileToCheck in tempList)
            {
                tileToCheck.SetSelected(false);
            }
            tempList.Clear();
        }

        Vector2 positionToCheck = (Vector2)transform.position + Grid.instance.GetTouchOffset();
        int x = Mathf.RoundToInt(positionToCheck.x);
        int y = Mathf.RoundToInt(positionToCheck.y);

        foreach (Vector2Int tilePos in shapeOffset)
        {
            Vector2Int targetPos = tilePos + new Vector2Int(x, y);
            Debug.Log(targetPos);
            Tile tileToCheck = Grid.instance.GetTileByGridPos(targetPos);
            if (tileToCheck == null)
            {
                foreach (Tile tile in tempList)
                {
                    tile.SetSelected(false);
                }
                tempList.Clear();
                return false;
            }
            tempList.Add(tileToCheck);
            tileToCheck.SetSelected(true);
        }
        foreach (Tile tileToCheck in tempList)
        {
            if (tileToCheck.tileType != TileType.Empty)
            {
                return false;
            }
        }
        return true;
    }
    private void MoveToPosition()
    {
        transform.position = PuzzleGame.InputManager.instance.GetTouchPos() + touchOffset;
    }
}
