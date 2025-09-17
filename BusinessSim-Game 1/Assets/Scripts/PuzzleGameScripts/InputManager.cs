using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace PuzzleGame
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;
        public static UnityAction<ITapable> OnTapped;
        public static UnityAction<ITapable> OnReleased;

        private float timeBetweenClicks = 0.2f;
        private float currentClick;

        private Vector2 touchPosition;
        private bool isTouching;

        private ITapable selectedShape;
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
            currentClick = -100f;
        }
        private void Update()
        {
            CheckTouchInput();

        }
        public bool TryGetIntVectorTouch(out Vector2Int touchPos)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(touch.position);
                    worldPos.z = 0; // keep it 2D
                    int x = Mathf.RoundToInt(worldPos.x);
                    int y = Mathf.RoundToInt(worldPos.y);

                    touchPos = new Vector2Int(x, y);
                    Debug.Log(touchPos);
                    return true;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(touch.position);
                    worldPos.z = 0; // keep it 2D
                    int x = Mathf.RoundToInt(worldPos.x);
                    int y = Mathf.RoundToInt(worldPos.y);
                    touchPos = new Vector2Int(x, y);

                        currentClick = Time.time;
                        //Debug.Log(touchPos);
                        return true;
                }
            }
            touchPos = new Vector2Int(-10, -10);
            return false;
        }

        private void CheckTouchInput()
        {
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                if(touch.phase == TouchPhase.Began)
                {
                    Collider2D other = Physics2D.OverlapPoint(touchPosition);
                    if(other != null)
                    {
                        if(other.TryGetComponent<ITapable>(out ITapable tapped))
                        {
                            OnTapped?.Invoke(tapped);
                            selectedShape = tapped;
                        }
                    }
                    isTouching = true;
                }
                if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isTouching = false;
                    if(selectedShape != null)
                    {
                        OnReleased?.Invoke(selectedShape);
                        selectedShape = null;
                    }
                }
            }
        }
        public bool DidTouchMove()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    return true;
                }
            }
            return false;
        }
        public Vector2 GetTouchPos() => touchPosition; 
        public bool IsTouching { get { return isTouching; } }
    }
}


