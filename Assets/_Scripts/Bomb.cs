using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    public ParticleSystem mainParticle;
    public PlayerBrain ownerBrain;

    public GameManager gameManager;
    public StateManager stateManager;
    
    public FloatVariable bombTimerLength;
    public float explosionTick;
    bool exploded = true;

    public AudioSource audioSource;
    public AudioClip beepingSound;
    public AudioClip explosionSound;

    private void OnEnable()
    {
        //ToDO particles and systems
        gameManager = stateManager.gameManager;

        explosionTick = Time.time + bombTimerLength.value;

        var particle = mainParticle.main; //ref to main particle system i.e. bomb itself
        var particles = mainParticle.subEmitters; //ref to the explosion effects

        particle.startLifetime = bombTimerLength.value;


        for(int i = 0; i < particles.subEmittersCount; i++)
        {
            var emitter = particles.GetSubEmitterSystem(i).main;
            emitter.startDelay = bombTimerLength.value;
        }  

        mainParticle.Play();

        exploded = false;

        if (ownerBrain) //////
        { 
            ownerBrain.AddActiveBomb(this);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!exploded && Time.time > explosionTick) //Time.time in Unity is the time at the beginning of the frame
        {
            exploded = true;
            HandleExplosion();
        }
    }

    void HandleExplosion()
    {

        Debug.Log("Exploding.");

        audioSource.PlayOneShot(explosionSound);

        RaycastHit hit;

        for(int i = 0; i < 4; i++)
        {
            transform.rotation = Quaternion.Euler(0, 90 * i, 0);
            Debug.DrawRay(transform.position, transform.forward * 2f, Color.green, 60f);

            if(Physics.Raycast(transform.position, transform.forward, out hit, 2f))
            {
                Debug.Log("I hit " + hit.transform.name);

                if(hit.transform.tag == "Player")
                {
                    gameManager.OnPlayerKilled(hit.transform.GetComponent<Player>().playerBrain);
                }
                else if(hit.transform.tag == "BreakableBlock")
                {
                    Destroy(hit.transform.gameObject);
                }
                else if(hit.transform.tag == "Enemy")
                {
                    Destroy(hit.transform.gameObject);
                }

            }
        }

        Destroy(gameObject, 0.6f); //at 0.6 seconds
    }

    private void OnDestroy() 
    {
        if (ownerBrain)
        {
            ownerBrain.RemoveBomb(this);
        }
    }
}
