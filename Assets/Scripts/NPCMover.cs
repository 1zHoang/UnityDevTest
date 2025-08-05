using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public bool autoMove = false;

    private GridManager gridManager;
    private PathfindingAgent agent;
    private Coroutine moveCoroutine;
    private Vector2Int currentNPCPosition;

    void Start()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        agent = FindAnyObjectByType<PathfindingAgent>();

        if (gridManager != null)
        {
            currentNPCPosition = gridManager.GetNPCPosition();
            UpdateNPCVisualPosition();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartMovement();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StopMovement();
            ResetPosition();
        }
    }

    public void StartMovement()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        if (agent != null)
        {
            agent.FindAndShowPath();
            var path = agent.GetCurrentPath();

            if (path != null && path.Count > 0)
            {
                moveCoroutine = StartCoroutine(MoveAlongPath(path));
            }
        }
    }

    public void StopMovement()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

    public void ResetPosition()
    {
        if (gridManager != null)
        {
            gridManager.ResetPathVisualization();
            currentNPCPosition = gridManager.GetNPCPosition();
            UpdateNPCVisualPosition();

            gridManager.UpdateNPCPosition(currentNPCPosition);
        }
    }

    private void UpdateNPCVisualPosition()
    {
        if (gridManager != null)
        {
            float cellSize = gridManager.GetCellSize();
            transform.position = new Vector3(currentNPCPosition.x * cellSize,
                                           currentNPCPosition.y * cellSize,
                                           -1f);
        }
    }

    private IEnumerator MoveAlongPath(List<Vector2Int> path)
    {
        float cellSize = gridManager.GetCellSize();

        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int waypoint = path[i];
            Vector3 targetPosition = new Vector3(waypoint.x * cellSize, waypoint.y * cellSize, -1f);

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPosition;

            Vector2Int previousPosition = currentNPCPosition;
            currentNPCPosition = waypoint;

            if (i > 0) 
            {
                gridManager.ClearNPCFromPosition(previousPosition);
            }

            if (i < path.Count - 1) 
            {
                gridManager.UpdateNPCPosition(currentNPCPosition);
            }

            yield return new WaitForSeconds(0.2f);
        }

        ShowText.Instance.ShowTextTu("Completed !!!");
        ShowText.Instance.HideTextTu(1f);

        if (path.Count > 1)
        {
            gridManager.ClearNPCFromPosition(path[path.Count - 2]);
        }
    }

    void OnGUI()
    {
        Rect safeArea = Screen.safeArea;

        GUIStyle bigStyle = new GUIStyle(GUI.skin.label);
        bigStyle.fontSize = 40;
        GUIStyle bigButton = new GUIStyle(GUI.skin.button);
        bigButton.fontSize = 45;

        GUILayout.BeginArea(new Rect(
            safeArea.x + safeArea.width - 500,
            safeArea.y + 80,
            480,
            400));
        GUILayout.BeginVertical("box");

        GUILayout.Label("NPC Controls:", bigStyle);

        if (GUILayout.Button("Start Move (Space)", bigButton))
        {
            StartMovement();
        }

        if (GUILayout.Button("Stop Movement", bigButton))
        {
            StopMovement();
        }

        if (GUILayout.Button("Reset Position (R)", bigButton))
        {
            StopMovement();
            ResetPosition();
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    public Vector2Int GetCurrentPosition()
    {
        return currentNPCPosition;
    }
}