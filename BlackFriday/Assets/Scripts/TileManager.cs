using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    // Note for the tile Prefabs: the last index is the checkout tile.
    [SerializeField]
    private GameObject[] tilePrefabs;
    // Note for the ceiling prefabs: index 0 has a light, index 1 doesn't.
    [SerializeField]
    private GameObject[] ceilingPrefabs;
    // Note for the wall prefabs: index 0 is a solid wall, index 1 is a glass wall.
    [SerializeField]
    private GameObject[] wallPrefabs;
    // Note for the stair prefabs: index 0 is the actual staircase, index 1 is the floor of the staircase without gaps, index 2 is the floor with gaps.
    [SerializeField]
    private GameObject[] stairPrefabs;
    // Array of item prefabs
    [SerializeField] 
    private GameObject[] itemPrefabs;
    // LayerMask to determine collision and location
    [SerializeField] 
    private LayerMask collisionLayer;
    private int width;
    private int length;
    private int floors;
    public float tileSize = 32;
    public float wallHeight = 40;
    public int itemAmount = 15;

    // for testing
    public string type;

    void Start()
    {
        type = PlayerPrefs.GetString("Selected Building", "Default");
        FillValues(type);
        GenerateGrid();
        SpawnItems(itemAmount);
    }

    private void FillValues(string buildingType)
    {
        if (buildingType == "Flat")
        {
            width = 9;
            length = 9;
            floors = 1;
        }
        else if (buildingType == "Regular")
        {
            width = 5;
            length = 5;
            floors = 3;
        }
        else if (buildingType == "Tower")
        {
            width = 3;
            length = 2;
            floors = 10;
        }
        else
        {
            width = 3;
            length = 3;
            floors = 5;
        }
    }

    public Vector3 GetDimensions()
    {
        return new Vector3(width, floors, length);
    }

    // Grid based level generation
    private void GenerateGrid()
    {
        // Special coordinates for entrance and checkout lane
        int midpoint = width / 2;

        // Generate offset, which is based off of width
        float xoffset = ((width * tileSize) / 2 * -1) + 16;
        float zoffset = tileSize;


        // Starts on the first floor, goes up.
        for (int y = 0; y < floors; ++y)
        {
            // x-coordinates loop
            for (int x = 0; x < width; ++x)
            {
                // z-coordinates loop, places tiles in this loop
                for (int z = 0; z < length; ++z)
                {
                    // Stair tiles will be placed at the very back left corner of the store each time.
                    // for example, if the store is 1 wide, 5 deep, and 4 stories tall, the stairs will be at the back of the store.
                    if (x == 0 && z == length - 1)
                    {
                        GameObject tile1 = stairPrefabs[0];
                        GameObject tile2;
                        if (y == 0)
                        {
                            tile2 = stairPrefabs[1];
                        }
                        else
                        {
                            tile2 = stairPrefabs[2];
                        }
                        Vector3 pos = new Vector3(x * tileSize + xoffset, y * wallHeight, z * tileSize + zoffset);
                        Instantiate(tile1, pos, Quaternion.identity);
                        Instantiate(tile2, pos, Quaternion.identity);
                    }

                    // Places the checkout tile at this specific spot
                    else if (z == 0 && x == midpoint && y == 0)
                    {
                        // The checkout tile will be the last in the tilePrefabs array, distinguished by midpoint coordinates.
                        GameObject tile = tilePrefabs[tilePrefabs.Length - 1];
                        Vector3 pos = new Vector3(x * tileSize + xoffset, y * wallHeight, z * tileSize + zoffset);
                        Instantiate(tile, pos, Quaternion.identity);
                    }

                    // Grabs a random tile prefab from prefab collection
                    // After a prefab is chosen, it instantiates at a pos in the grid
                    else
                    {
                        GameObject tile = tilePrefabs[Random.Range(0, tilePrefabs.Length - 1)];
                        Vector3 pos = new Vector3(x * tileSize + xoffset, y * wallHeight, z * tileSize + zoffset);
                        Instantiate(tile, pos, Quaternion.identity);
                    }
                }
            }
        }

        // creates ceiling for the store after
        for (int y = 0; y < floors; ++y)
        {
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < length; ++j)
                {
                    if (i == 0 && j == length - 1 && y != floors - 1)
                    {
                        continue;
                    }

                    else
                    {
                        GameObject ceiling = ceilingPrefabs[0];
                        Vector3 pos = new Vector3(i * tileSize + xoffset, (y + 1) * wallHeight, j * tileSize + zoffset);
                        Instantiate(ceiling, pos, Quaternion.identity);
                    }
                }
            }
        }


        // creates walls
        for (int y = 0; y < floors; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                for (int z = 0; z < length; ++z)
                {
                    if (z == 0)
                    {
                        // creates the door prefab
                        if (x == midpoint && y == 0)
                        {
                            GameObject wall = wallPrefabs[2];
                            Vector3 pos = new Vector3(x * tileSize + xoffset, y * wallHeight, z * tileSize + zoffset);
                            Instantiate(wall, pos, Quaternion.identity);
                        }
                        
                        // glass wall
                        else
                        {
                            GameObject wall = wallPrefabs[1];
                            Vector3 pos = new Vector3(x * tileSize + xoffset, y * wallHeight, z * tileSize + zoffset);
                            Instantiate(wall, pos, Quaternion.identity);
                        }
                    }
                    
                    if (x == 0)
                    {
                        GameObject wall = wallPrefabs[0];
                        Vector3 pos = new Vector3(x * tileSize + xoffset, y * wallHeight, z * tileSize + zoffset);
                        Instantiate(wall, pos, Quaternion.Euler(0, 90, 0));
                    }

                    if (z == length - 1)
                    {
                        GameObject wall = wallPrefabs[0];
                        Vector3 pos = new Vector3(x * tileSize + xoffset, y * wallHeight, z * tileSize + zoffset);
                        Instantiate(wall, pos, Quaternion.Euler(0, 180, 0));
                    }

                    if (x == width - 1)
                    {
                        GameObject wall = wallPrefabs[0];
                        Vector3 pos = new Vector3(x * tileSize + xoffset, y * wallHeight, z * tileSize + zoffset);
                        Instantiate(wall, pos, Quaternion.Euler(0, -90, 0));
                    }
                }
            }
        }
        
    }

    public void SpawnItems(int itemCount)
    {
        for (int i = 0; i < itemCount; i++)
        {
            // Randomize item prefab
            GameObject itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

            // Try to find a valid position
            Vector3 spawnPosition = GetRandomValidPosition();
            if (spawnPosition != Vector3.zero)
            {
                // Instantiate the item at the valid position
                Instantiate(itemPrefab, spawnPosition, Quaternion.Euler(-90,0,0));
            }
            else
            {
                Debug.Log("Could not find location for item number " + i);
            }
        }
    }

    private Vector3 GetRandomValidPosition()
    {
        float zoffset = tileSize;
        // Since this may result in infinite loops if we do something wrong,
        // using a for loop will combat 
        for (int attempts = 0; attempts < 10; attempts++)
        {
            // Random position within the grid bounds
            float x = Random.Range((width * tileSize * -1) / 2, (width * tileSize) / 2);
            float z = Random.Range(zoffset, length * tileSize);
            float y = Random.Range(0, floors * wallHeight);
            Vector3 pos = new Vector3(x, y, z);

            // Check for collision at the random position by creating a sphere.
            if (!Physics.CheckSphere(pos, 0.5f, collisionLayer))
            {
                // if its good, it will return the Vector3 of the position.
                return pos;
            }
        }
        // if there are no good positions, it will just return (0, 0, 0)
        return Vector3.zero;  
    }
}

