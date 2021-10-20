using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerBrain : ScriptableObject {

    public GameManager gameManager;

    public int playerNumber;
    public GameObject playerGO;
    public FloatVariable playerScore;
    public FloatVariable maxActiveBombs;

    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode bombKey;

    public bool ready = false;

    public Color lightColor = Color.green;

    public FloatVariable movementSpeed;

    public GameObject bombPrefab;
    public FloatVariable bombPlacementDelay;
    public float nextBombTime = 0;

    public List<Bomb> activeBombs = new List<Bomb>();

    private void OnEnable() //Default unity term - when brain is going to be enabled or brought up
    {
        nextBombTime = 0;
        ready = false;
    }

    public void AddActiveBomb(Bomb bomb)
    {
        if (!activeBombs.Contains(bomb))
        {
            activeBombs.Add(bomb);
        }
    }

    public void RemoveBomb(Bomb bomb)
    {
        if(activeBombs.Contains(bomb))
        {
            activeBombs.Remove(bomb);
        }
    }

}
