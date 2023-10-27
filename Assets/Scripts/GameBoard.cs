using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{

    [SerializeField] private Tilemap currentStatus;
    [SerializeField] private Tilemap nextState;
    [SerializeField] private Tile aliveTile;
    [SerializeField] private Tile deadTile;
    [SerializeField] private float updateInterval = 0.05f;
    [SerializeField] private Pattern pattern;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
