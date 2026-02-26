using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public GameObject groundPrefab;
    public GameObject obstaclePrefab;
    public GameObject starPrefab;
    public GameObject boomPrefab;

    public float groundLength = .5f;
    private float nextSpawnY = 0f;
   
    private float[] lanes = {-3.5f,-3.2f,-3f,-2.8f,-2.5f,-2.3f,-2f,-1.8f,-1.5f,-1.2f,-0.8f,
        3.5f, 3.2f, 3f, 2.8f, 2.5f, 2.3f, 2f, 1.8f, 1.5f, 1.2f, 0.8f };

    private float[] starLanes = { -2f, -1.8f, -1.5f, -1, -0.8f, -0.5f, 0f, -2f, -1.8f, -1.5f, -1, -0.8f, -0.5f };
    public int groundPoolSize = 3;
    public int obstaclePoolSize = 10;
    public int starPoolSize = 5;
    public int boomPoolSize = 3;


    private Queue<GameObject> groundPool = new Queue<GameObject>();
    private Queue<GameObject> obstaclePool = new Queue<GameObject>();
    private Queue<GameObject> starPool = new Queue<GameObject>();
    private Queue<GameObject> boomPool = new Queue<GameObject>();

    void Start()
    {
        CreatePool(groundPrefab, groundPool, groundPoolSize);
        CreatePool(obstaclePrefab, obstaclePool, obstaclePoolSize);
        CreatePool(starPrefab, starPool, starPoolSize);
        CreatePool(boomPrefab, boomPool, boomPoolSize);


        for (int i = 0; i < groundPoolSize; i++)
        {
            SpawnGround();
            SpawnPattern();
        }
    }

    void Update()
    {
        if (!GameManager.IsGameStart)
        {
            return;
        }

        if (PlayerController2d.Instance.transform.position.y > nextSpawnY - (groundLength * 2))
        {
            SpawnGround();
            SpawnPattern();
        }
    }

    void CreatePool(GameObject prefab, Queue<GameObject> pool, int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    void SpawnGround()
    {
        GameObject ground = groundPool.Dequeue();
        ground.transform.position = new Vector3(0, nextSpawnY, 0);
        ground.SetActive(true);
        groundPool.Enqueue(ground);

        nextSpawnY += groundLength;
    }
    public bool isSpawningFirst = false;
    int randomValue;
    void SpawnPattern()
    {
        if (!isSpawningFirst)
        {
            isSpawningFirst = true;
            randomValue = Random.Range(0, 50);
        }
        float spawnY = nextSpawnY - groundLength / 2f;

        randomValue = Random.Range(0, 100);

        if (randomValue < 50)                 // 0–49  (50%)
        {
            SpawnDoubleObstacle(spawnY);
        }
        else if (randomValue < 70)            // 50–69 (20%)
        {
            SpawnSingleObstacle(spawnY);
        }
        else if (randomValue < 88)            // 70–87 (18%)
        {
            SpawnStarLine(spawnY);
        }
        else                                  // 88–99 (12%)
        {
            SpawnBoomObstacle(spawnY);
        }
    }
    

    void SpawnSingleObstacle(float yPos)
    {
        GameObject obstacle = obstaclePool.Dequeue();

        int lane = Random.Range(0, lanes.Length);
        obstacle.transform.position = new Vector3(lanes[lane], yPos, 0);
        obstacle.SetActive(true);

        obstaclePool.Enqueue(obstacle);
    }

    void SpawnDoubleObstacle(float yPos)
    {
        SpawnSingleObstacle(yPos);
        SpawnSingleObstacle(yPos + 3f);
    }

    void SpawnStarLine(float yPos)
    {
        int starCount = Random.Range(0, 5);
        for (int i = 0; i <= starCount; i++)
        {
            GameObject star = starPool.Dequeue();

            star.transform.position = new Vector3(
                starLanes[Random.Range(0, starLanes.Length)],
                yPos + i * 1.5f,
                0);

            star.SetActive(true);
            starPool.Enqueue(star);
        }
    }

    void SpawnBoomObstacle(float yPos)
    {
        GameObject boom = boomPool.Dequeue();

        int lane = Random.Range(0, starLanes.Length);
        boom.transform.position = new Vector3(starLanes[lane], yPos, 0);
        boom.SetActive(true);

        boomPool.Enqueue(boom);
    }
}