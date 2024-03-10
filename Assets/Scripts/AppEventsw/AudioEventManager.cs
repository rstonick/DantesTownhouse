using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEventManager : MonoBehaviour
{

    public EventSound3D eventSound3DPrefab;

    public AudioClip minionDeathAudio;
    public AudioClip minionOuchAudio;
    public AudioClip minionSpawnAudio;
    public AudioClip[] footstepAudio;
    public AudioClip playerLandsAudio;
    public AudioClip jumpAudio;

    private UnityAction<Vector3, float> playerLandsEventListener;
    private UnityAction<Vector3> footstepEventListener;
    private UnityAction<Vector3> jumpEventListener;


    void Awake()
    {

        playerLandsEventListener = new UnityAction<Vector3, float>(playerLandsEventHandler);
        footstepEventListener = new UnityAction<Vector3>(footstepEventHandler);
        jumpEventListener = new UnityAction<Vector3>(jumpEventHandler);
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
    }

    void OnDisable()
    {
        EventManager.StopListening<PlayerLandsEvent, Vector3, float>(playerLandsEventListener);
        EventManager.StopListening<FootstepEvent, Vector3>(footstepEventListener);
        EventManager.StopListening<JumpEvent, Vector3>(jumpEventListener);
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
            Debug.Log("event fired");
            EventSound3D snd = Instantiate(eventSound3DPrefab, pos, Quaternion.identity, null);


            snd.audioSrc.clip = this.footstepAudio[Random.Range(0, this.footstepAudio.Length)];

            snd.audioSrc.minDistance = 5f;
            snd.audioSrc.maxDistance = 100f;

            snd.audioSrc.Play();
        }


    }

}
