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
    public float beamThickness = 0.15f;   
    public float verticalBeamWidth = 0.15f;
    public float ceilingHeight = 4f;  

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

        // "Right" from the player's point of view as they walk toward the far end
        Vector3 playerForward = -toStart.normalized;
        Vector3 right = Vector3.Cross(Vector3.up, playerForward);

        if (Random.value < 0.5f)
        {
            // one horizontal beam across the whole corridor: jump over it
            SpawnBeam(spawnPoint.position + Vector3.up * 0.3f,
                      new Vector3(corridorWidth, beamThickness, beamThickness), toStart);
        }
        else
        {
            // several standing beams at random spots: weave between them
            float height = ceilingHeight;
            int count = 4; // how many beams per spawn (could also be a public field)
            float maxOffset = (corridorWidth - verticalBeamWidth) / 2f;

            for (int i = 0; i < count; i++)
            {
                float x = Random.Range(-maxOffset, maxOffset);
                SpawnBeam(spawnPoint.position + right * x + Vector3.up * (height / 2f),
                          new Vector3(verticalBeamWidth, height, beamThickness), toStart);
            }
        }
    }
  
    void SpawnBeam(Vector3 pos, Vector3 size, Vector3 toStart)
    {
        Laser laser = Instantiate(laserPrefab, pos, Quaternion.LookRotation(toStart));
        laser.transform.localScale = size;
        laser.speed = laserSpeed;
        laser.Init(toStart, toStart.magnitude);
    }

}