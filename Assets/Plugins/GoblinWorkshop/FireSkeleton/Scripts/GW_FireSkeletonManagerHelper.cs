using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class GW_FireSkeletonManagerHelper : MonoBehaviour {

    public bool run;
    public GW_FireSkeletonManager MyGW_FireSkeletonManager;
    public Material FlameMaterialSample; //Material used to filter on finding the Flame Renderers

	void Update () {

        if (run && MyGW_FireSkeletonManager && FlameMaterialSample)
        {
            run = false;

            MeshRenderer[] MeshRendererList = MyGW_FireSkeletonManager.gameObject.GetComponentsInChildren<MeshRenderer>() as MeshRenderer[];
            List<MeshRenderer> DynamicMeshRendererList = new List<MeshRenderer>();

            for (int i = 0; i < MeshRendererList.Length; i++)
            {

                if(MeshRendererList[i].sharedMaterial == FlameMaterialSample)
                {
                    DynamicMeshRendererList.Add(MeshRendererList[i]);
                }
            }

            MyGW_FireSkeletonManager.FlameMeshRendererList = DynamicMeshRendererList.ToArray();



        }

	}
}
