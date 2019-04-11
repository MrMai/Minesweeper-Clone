using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Minefield world;
    private SpriteRenderer spriteRenderer;
    private int id = 0;
    private bool revealed = false;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void init(Minefield kworld, int kid)
    {
        Start();
        setWorld(kworld);
        setId(kid);
        spriteRenderer.sprite = kworld.sprites[10];
        // Debug.Log(spriteRenderer); // unrevealed yet
    }

    public void setWorld(Minefield gameworld)
    {
        world = gameworld;
    }

    public void setId(int kid)
    {
        id = kid;
    }

    public bool isMine()
    {
        return id == 9; // 9 is a mine
    }

    public void reveal()
    {
        revealed = true;
        spriteRenderer.sprite = world.sprites[id];
    }

    public int getId()
    {
        return id;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
