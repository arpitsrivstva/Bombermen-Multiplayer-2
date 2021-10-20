using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public new Rigidbody rigidbody;
    public PlayerBrain playerBrain;

    //TODO Add Animator
    public Light playerLight;
    public StateManager stateManager;

    Quaternion newRotation;
    Vector3 newVelocity;

    // Start is called before the first frame update
    void Start()
    {
        playerBrain.playerGO = gameObject;
        playerLight.color = playerBrain.lightColor;
    }

    // Update is called once per frame
    void Update() //Player Inputs are always stored here.
    {
        if(Input.GetKey(playerBrain.upKey))
        {
            newVelocity = new Vector3(0, 0, playerBrain.movementSpeed.value);
            newRotation = Quaternion.Euler(0,0,0);
        }

        if(Input.GetKey(playerBrain.downKey))
        {
            newVelocity = new Vector3(0, 0, -playerBrain.movementSpeed.value);
            newRotation = Quaternion.Euler(0,180,0);
        }

        if(Input.GetKey(playerBrain.rightKey))
        {
            newVelocity = new Vector3(playerBrain.movementSpeed.value, 0, 0);
            newRotation = Quaternion.Euler(0,90,0);
        }

        if(Input.GetKey(playerBrain.leftKey))
        {
            newVelocity = new Vector3(-playerBrain.movementSpeed.value, 0, 0);
            newRotation = Quaternion.Euler(0,270,0);
        }

        

        if(Input.GetKey(playerBrain.bombKey))
        {
            if(stateManager.currentGameState == GameState.GameActive)
            {
            DropBomb();
            }
            else if(stateManager.currentGameState == GameState.PreGame)
            {
            playerBrain.ready = true;
            playerLight.gameObject.SetActive(true);
            }
        }

       
    }

    private void FixedUpdate() //Player Movements are always stored here coz of FPS
    {
        if(newVelocity != Vector3.zero && stateManager.currentGameState == GameState.GameActive)
        {
            rigidbody.velocity = newVelocity;
            transform.rotation = newRotation;
        }
        newVelocity = Vector3.zero;
    }

    void DropBomb()
    {
        Debug.Log("Dropping bomb");

        if(Time.time > playerBrain.nextBombTime && playerBrain.activeBombs.Count < playerBrain.maxActiveBombs.value)
        {
            GameObject.Instantiate(playerBrain.bombPrefab, new Vector3(Utils.RoundToInt(transform.position.x), Utils.RoundToInt(transform.position.y), Utils.RoundToInt(transform.position.z)), Quaternion.identity);
            playerBrain.nextBombTime = Time.time + playerBrain.bombPlacementDelay.value;
        }
    }
}
