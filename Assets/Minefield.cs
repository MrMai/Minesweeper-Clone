using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minefield : MonoBehaviour
{
    public Camera camera;
    public int mapW = 32;
    public int mapH = 16;
    public float mineFrequency = 0.5f;
    public int maxMines = 100;
    public Sprite[] sprites;
    public Tile tile;
    private Tile[,] mineMap;
    private bool udedlol = false;

    // Start is called before the first frame update
    void Start()
    {
        mineMap = new Tile[mapW, mapH];
        Debug.Log("sprites " + sprites.Length);
        randomMaxGenerate();
        calculateMines();
        /*
        for (int kx = 0; kx < mapW; kx++)
        {
            for (int ky = 0; ky < mapH; ky++)
            {
                mineMap[kx, ky].reveal();
            }
        }
        */
    }
    Tile createNewTile(int id, Vector3 loc, Quaternion rot)
    {
        Tile newTile = Instantiate(tile, loc, rot);
        newTile.init(this, id);
        return newTile;
    }
    public bool clickZone(int x, int y) // returns true if it is a mine and then u lose lol
    {
        Tile clicked = mineMap[x, y];
        if (clicked.isMine())
        {
            return true;
        }
        else
        {
            mineMap[x, y].reveal();
            List<Vector2> emptyAround = checkEmptyAround(x, y, new List<Vector2>(), 0);
            for (int i = 0; i<emptyAround.Count; i++)
            {
                mineMap[(int)emptyAround[i].x, (int)emptyAround[i].y].reveal();
                revealAround((int)emptyAround[i].x, (int)emptyAround[i].y);
            }
            return false;
        }
    }
    private Vector2[] boxAround =
        {
            new Vector2(1,0),
            new Vector2(1,1),
            new Vector2(0,1),
            new Vector2(-1,1),
            new Vector2(-1,0),
            new Vector2(-1,-1),
            new Vector2(0,-1),
            new Vector2(1,-1)
        };
    int checkMinesAround(int x, int y)
    {
        int numMines = 0;
        for (int i = 0; i < 8; i++)
        {
            int checkX = x + (int)boxAround[i].x;
            int checkY = y + (int)boxAround[i].y;
            if (checkX >= 0 && checkX < mapW && checkY >= 0 && checkY < mapH)
            {
                if (mineMap[checkX, checkY].isMine())
                {
                    numMines++;
                }
            }
        }
        return numMines;
    }
    List<Vector2> checkEmptyAround(int x, int y, List<Vector2> empties, int depth)
    {
        depth++;
        if (depth > 10)
        {
            return empties;
        }
        for (int i=0; i<8; i++)
        {
            int checkX = x + (int)boxAround[i].x;
            int checkY = y + (int)boxAround[i].y;
            if (checkX >= 0 && checkX < mapW && checkY >= 0 && checkY < mapH)
            {
                if (mineMap[checkX, checkY].getId() == 0)
                {
                    bool allowed = true;
                    for (int v=0;v<empties.Count;v++)
                    {
                        if(empties[v].Equals(new Vector2(checkX, checkY))) {
                            allowed = false;
                        }
                    }
                    if (allowed)
                    {
                        //Debug.Log("Found 0 space at " + checkX + " " + checkY);
                        empties.Add(new Vector2(checkX, checkY));
                        checkEmptyAround(checkX, checkY, empties, depth);
                    }
                }
            }
        }
        return empties;
    }
    void revealAround(int x, int y)
    {
        if (mineMap[x, y].getId() == 0)
        {
            for (int i = 0; i < 8; i++)
            {
                int checkX = x + (int)boxAround[i].x;
                int checkY = y + (int)boxAround[i].y;
                if (checkX >= 0 && checkX < mapW && checkY >= 0 && checkY < mapH)
                {
                    mineMap[checkX, checkY].reveal();
                }
            }
        }
    }
    void calculateMines()
    {
        for (int kx = 0; kx < mapW; kx++)
        {
            for (int ky = 0; ky < mapH; ky++)
            {
                if(!mineMap[kx,ky].isMine())
                {
                    mineMap[kx,ky].setId(checkMinesAround(kx, ky));
                }
            }
        }
    }
    void randomMaxGenerate()
    {
        for (int kx = 0; kx < mapW; kx++)
        {
            for (int ky = 0; ky < mapH; ky++)
            {
                Tile newTile = createNewTile(0, transform.position + new Vector3(kx, 0, ky), Quaternion.AngleAxis(90, Vector3.right));
                mineMap[kx, ky] = newTile;
            }
        }
        for (int i = 0; i < maxMines; i++)
        {
            int randX = (int)(Random.value * mapW);
            int randY = (int)(Random.value * mapH);
            if (mineMap[randX, randY].getId() == 0)
            {
                mineMap[randX, randY].setId(9);
            }
            else
            {
                i--;
            }
        }
    }
    void perlinGenerate()
    {
        for (int kx = 0; kx < mapW; kx++)
        {
            for (int ky = 0; ky < mapH; ky++)
            {
                if (Mathf.PerlinNoise((float)kx / mapW, (float)ky / mapH) > mineFrequency)
                {
                    Tile newTile = createNewTile(9, transform.position + new Vector3(kx, 0, ky), Quaternion.AngleAxis(90, Vector3.right));
                    mineMap[kx, ky] = newTile;
                }
                else
                {
                    Tile newTile = createNewTile(0, transform.position + new Vector3(kx, 0, ky), Quaternion.AngleAxis(90, Vector3.right)); // new tile is unrevealed tile
                    mineMap[kx, ky] = newTile;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !udedlol)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if (objectHit.gameObject.name.IndexOf("Tile") >= 0)
                {
                    Debug.Log(objectHit.position.x + " " + objectHit.position.z);
                    udedlol = clickZone((int)objectHit.position.x, (int)objectHit.position.z);
                    if(udedlol)
                    {
                        mineMap[(int)objectHit.position.x, (int)objectHit.position.z].reveal();
                    }
                }
                // Do something with the object that was hit by the raycast.
            }
        }
    }
}
