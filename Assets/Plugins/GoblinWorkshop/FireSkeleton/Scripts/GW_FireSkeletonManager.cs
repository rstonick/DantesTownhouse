using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GW_FireSkeletonManager : MonoBehaviour {

    public float sampleFrequency = 0.1f; //How often the position should be sampled
    public int numberOfSamples = 5; //How many samples will be kept for calculating motion
    public float movementIntensity = 1.0f; // The intensity of the motion on the flames, it´s worth adjusting this value depending on the range of animations being used.
    public float MaxMovement = 1.0f; //Maximum velocity that will be sent to the shader, the magnitude of the motion vector is normalized and multiplied by this amount in case it´s a higher value.
    public Vector3 neutralDirection = Vector3.up; //Neutral direction of the flames, usually up, but can include random values or emulate wind.

    public MeshRenderer[] FlameMeshRendererList; //A list of all Mesh Rendererds of flames, this includes all LODs. The calculation only happens on the visible parts.
    private MaterialPropertyBlock _propBlock;

    private Vector3[,] positionList;
    private int posIndex;
    private float sampleTimer = 0.0f;

    void Start () {
        _propBlock = new MaterialPropertyBlock();

        //One extra sample value since we avoid sampling at index 0 for average
        positionList = new Vector3[FlameMeshRendererList.Length, numberOfSamples];

        sampleTimer = Random.Range(0.0f, sampleFrequency);
    }
	
	void Update () {
        if (numberOfSamples > 0) {
            for (int i = 0; i < FlameMeshRendererList.Length; i++)
            {
                if (FlameMeshRendererList[i])
                {
                    if (FlameMeshRendererList[i].isVisible)
                    {
                        Vector3 deltaPos = Vector3.zero;
                        
                        //Calculating average delta and passing to shader
                        for (int posIndex = 0; posIndex < numberOfSamples; posIndex++)
                        {
                            deltaPos = deltaPos + (positionList[i, posIndex]);
                        }
                       
                        deltaPos = deltaPos / (numberOfSamples);
                        deltaPos = deltaPos - FlameMeshRendererList[i].transform.position;

                        deltaPos = deltaPos * movementIntensity + neutralDirection;

                        float magnitude = deltaPos.magnitude;

                        if (deltaPos.magnitude > MaxMovement)
                        {
                            deltaPos.Normalize();
                            deltaPos = deltaPos * MaxMovement;
                        }
                        
                        FlameMeshRendererList[i].GetPropertyBlock(_propBlock);
                        _propBlock.SetFloat("_UseMotionProp", 1.0f);
                        _propBlock.SetVector("_MoveVectorProp", new Vector3(deltaPos.x, deltaPos.y, deltaPos.z));
                        _propBlock.SetVector("_BaseVectorProp", new Vector3(neutralDirection.x, neutralDirection.y, neutralDirection.z));
                        FlameMeshRendererList[i].SetPropertyBlock(_propBlock);
                    }
                    
                }
            }
        }
	}

    void LateUpdate()
    {
        sampleTimer = sampleTimer + Time.deltaTime;
        if (sampleTimer >= sampleFrequency)
        {
            sampleTimer = sampleTimer - sampleFrequency;

            posIndex = posIndex + 1;
            if (posIndex >= numberOfSamples)
            {
                //One extra sample value since we avoid sampling at index 0 for average
                posIndex = 0;
            }

            for (int i = 0; i < FlameMeshRendererList.Length; i++)
            {
                if (FlameMeshRendererList[i])
                {
                    if (FlameMeshRendererList[i].isVisible)
                    {
                        positionList[i, posIndex] = FlameMeshRendererList[i].transform.position;
                    }
                }
            }

        }
    }
}

