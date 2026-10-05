using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    [Header("References")]
    public Laser laserPrefab;
    public Transform spawnPoint;    // far end of the corridor, center, on the floor surface
    public Transform despawnPoint;  // start of the corridor

    [Header("Settings")]
    public float corridorWidth = 4f;
    public float spawnInterval = 2f;
    public float laserSpeed = 6f;
    public float laserThickness = 0.3f; // depth along the direction of travel

    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnLaser();
        }
    }

    void SpawnLaser()
    {
        Vector3 toStart = despawnPoint.position - spawnPoint.position;
        toStart.y = 0f;

        Vector3 right = spawnPoint.right;
        Vector3 size;
        Vector3 pos = spawnPoint.position;

        switch (Random.Range(0, 3))
        {
            case 0: // low bar across the whole corridor: jump over it
                size = new Vector3(corridorWidth, 0.4f, laserThickness);
                pos += Vector3.up * (size.y / 2f);
                break;

            case 1: // block on the left: sidestep right
                size = new Vector3(corridorWidth * 0.6f, 2f, laserThickness);
                pos += Vector3.up * (size.y / 2f) - right * (corridorWidth * 0.2f);
                break;

            default: // block on the right: sidestep left
                size = new Vector3(corridorWidth * 0.6f, 2f, laserThickness);
                pos += Vector3.up * (size.y / 2f) + right * (corridorWidth * 0.2f);
                break;
        }

        Laser laser = Instantiate(laserPrefab, pos, Quaternion.LookRotation(toStart));
        laser.transform.localScale = size;
        laser.speed = laserSpeed;
        laser.Init(toStart, toStart.magnitude);
    }
}