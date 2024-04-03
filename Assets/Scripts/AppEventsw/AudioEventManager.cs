using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEventManager : MonoBehaviour
{

    public EventSound3D eventSound3DPrefab;

    public AudioClip[] footstepAudio;
    public AudioClip playerLandsAudio;
    public AudioClip jumpAudio;
    public AudioClip trapAudio;

    private UnityAction<Vector3, float> playerLandsEventListener;
    private UnityAction<Vector3> footstepEventListener;
    private UnityAction<Vector3> jumpEventListener;
    private UnityAction<Vector3> trapEventListener;


    void Awake()
    {

        playerLandsEventListener = new UnityAction<Vector3, float>(playerLandsEventHandler);
        footstepEventListener = new UnityAction<Vector3>(footstepEventHandler);
        jumpEventListener = new UnityAction<Vector3>(jumpEventHandler);
        trapEventListener = new UnityAction<Vector3>(trapEventHandler);
    }


    // Use this for initialization
    void Start()
    {



    }


    void OnEnable()
    {
        EventManager.StartListening<PlayerLandsEvent, Vector3, float>(playerLandsEventListener);
        EventManager.StartListening<FootstepEvent, Vector3>(footstepEventListener);
        EventManager.StartListening<JumpEvent, Vector3>(jumpEventListener);
        EventManager.StartListening<TrapEvent, Vector3>(trapEventListener);
    }

    void OnDisable()
    {
        EventManager.StopListening<PlayerLandsEvent, Vector3, float>(playerLandsEventListener);
        EventManager.StopListening<FootstepEvent, Vector3>(footstepEventListener);
        EventManager.StopListening<JumpEvent, Vector3>(jumpEventListener);
        EventManager.StopListening<TrapEvent, Vector3>(trapEventListener);
    }


	
 

    void playerLandsEventHandler(Vector3 worldPos, float collisionMagnitude)
    {
        //AudioSource.PlayClipAtPoint(this.explosionAudio, worldPos, 1f);

        if (eventSound3DPrefab)
        {
            if (collisionMagnitude > 300f)
            {

                EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

                snd.audioSrc.clip = this.playerLandsAudio;

                snd.audioSrc.minDistance = 5f;
                snd.audioSrc.maxDistance = 100f;

                snd.audioSrc.Play();
            }


        }
    }

   
    void jumpEventHandler(Vector3 worldPos)
    {
        //AudioSource.PlayClipAtPoint(this.explosionAudio, worldPos, 1f);

        if (eventSound3DPrefab)
        {

            EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd.audioSrc.clip = this.jumpAudio;

            snd.audioSrc.minDistance = 5f;
            snd.audioSrc.maxDistance = 100f;

            snd.audioSrc.Play();
        }
    }

    void footstepEventHandler(Vector3 pos)
    {

        if (eventSound3DPrefab)
        {

            EventSound3D snd = Instantiate(eventSound3DPrefab, pos, Quaternion.identity, null);


            snd.audioSrc.clip = this.footstepAudio[Random.Range(0, this.footstepAudio.Length)];

            snd.audioSrc.minDistance = 5f;
            snd.audioSrc.maxDistance = 100f;

            snd.audioSrc.Play();
        }


    }

    void trapEventHandler(Vector3 pos)
    {

        if (eventSound3DPrefab)
        {

            EventSound3D snd = Instantiate(eventSound3DPrefab, pos, Quaternion.identity, null);


            snd.audioSrc.clip = this.trapAudio;

            snd.audioSrc.minDistance = 5f;
            snd.audioSrc.maxDistance = 100f;

            snd.audioSrc.Play();
        }


    }

}
