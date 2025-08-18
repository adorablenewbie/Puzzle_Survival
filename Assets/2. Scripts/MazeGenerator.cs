using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Header("Maze Settings")]
    public int width = 10;   // 셀 개수 (가로)
    public int height = 10;  // 셀 개수 (세로)

    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject floorPrefab;

    [Header("Start & Exit Settings")]
    public Vector3 exitPosition; // 인스펙터에서 지정하는 종료 지점

    private int[,] maze; // 0 = 길, 1 = 벽
    private int cellSize = 5; // 길과 벽 크기 고정 = 5
    private List<GameObject> walls = new List<GameObject>();

    void Start()
    {
        maze = GenerateEllerMaze(width, height);
        BuildMaze();
        ClearStartArea();
        ClearExitArea();
    }

    int[,] GenerateEllerMaze(int w, int h)
    {
        int[,] grid = new int[h * 2 + 1, w * 2 + 1];

        // 전체를 벽으로 초기화
        for (int y = 0; y < h * 2 + 1; y++)
            for (int x = 0; x < w * 2 + 1; x++)
                grid[y, x] = 1;

        List<HashSet<int>> sets = new List<HashSet<int>>();
        int setId = 0;
        int[] cellSet = new int[w];

        for (int y = 0; y < h; y++)
        {
            // 1. 세트 초기화
            for (int x = 0; x < w; x++)
            {
                if (cellSet[x] == 0)
                {
                    setId++;
                    cellSet[x] = setId;
                    sets.Add(new HashSet<int> { setId });
                }
            }

            // 2. 오른쪽 연결
            for (int x = 0; x < w - 1; x++)
            {
                if (Random.value > 0.5f && cellSet[x] != cellSet[x + 1])
                {
                    int oldSet = cellSet[x + 1];
                    int newSet = cellSet[x];
                    for (int i = 0; i < w; i++)
                        if (cellSet[i] == oldSet) cellSet[i] = newSet;

                    grid[y * 2 + 1, (x + 1) * 2] = 0;
                }
            }

            // 3. 아래 연결
            Dictionary<int, List<int>> setMembers = new Dictionary<int, List<int>>();
            for (int x = 0; x < w; x++)
            {
                if (!setMembers.ContainsKey(cellSet[x]))
                    setMembers[cellSet[x]] = new List<int>();
                setMembers[cellSet[x]].Add(x);
            }

            int[] nextRow = new int[w];
            foreach (var kv in setMembers)
            {
                bool placed = false;
                foreach (int x in kv.Value)
                {
                    if (Random.value > 0.5f || !placed)
                    {
                        nextRow[x] = cellSet[x];
                        grid[y * 2 + 2, x * 2 + 1] = 0;
                        placed = true;
                    }
                }
            }

            cellSet = nextRow;
        }

        // 4. 마지막 행 처리
        for (int x = 0; x < w - 1; x++)
        {
            if (cellSet[x] != cellSet[x + 1])
            {
                int oldSet = cellSet[x + 1];
                int newSet = cellSet[x];
                for (int i = 0; i < w; i++)
                    if (cellSet[i] == oldSet) cellSet[i] = newSet;

                grid[h * 2 - 1, (x + 1) * 2] = 0;
            }
        }

        // 길 표시
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                grid[y * 2 + 1, x * 2 + 1] = 0;

        return grid;
    }

    void BuildMaze()
    {
        int rows = maze.GetLength(0);
        int cols = maze.GetLength(1);

        // 바닥
        Vector3 floorPos = new Vector3(
            (cols * cellSize) / 2f - cellSize / 2f,
            -0.5f,
            (rows * cellSize) / 2f - cellSize / 2f
        );
        GameObject floor = Instantiate(floorPrefab, floorPos, Quaternion.identity, transform);
        floor.transform.localScale = new Vector3(cols * cellSize, 1, rows * cellSize);

        // 벽
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                // grid 값이 1이거나, 테두리일 경우 벽 생성
                if (maze[y, x] == 1 || x == 0 || x == cols - 1 || y == 0 || y == rows - 1)
                {
                    Vector3 pos = new Vector3(x * cellSize, 0, y * cellSize);
                    GameObject wall = Instantiate(wallPrefab, pos, Quaternion.identity, transform);
                    wall.transform.localScale = new Vector3(cellSize, wall.transform.localScale.y, cellSize);
                    walls.Add(wall);
                }
            }
        }
    }

    void ClearStartArea()
    {
        Vector3 startPos = transform.position;
        ClearWallsInArea(startPos, 10);
    }

    void ClearExitArea()
    {
        ClearWallsInArea(exitPosition, 10);
    }

    void ClearWallsInArea(Vector3 center, float size)
    {
        for (int i = walls.Count - 1; i >= 0; i--)
        {
            GameObject wall = walls[i];
            if (wall == null) continue;

            Vector3 pos = wall.transform.position;
            if (Mathf.Abs(pos.x - center.x) <= size / 2f &&
                Mathf.Abs(pos.z - center.z) <= size / 2f)
            {
                Destroy(wall);
                walls.RemoveAt(i);
            }
        }
    }
}
