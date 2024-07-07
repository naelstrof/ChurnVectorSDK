using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

[System.Serializable]
public class OrbitCameraCharacterLoaderConfiguration : OrbitCameraConfiguration {
    [SerializeField] private CharacterLoader targetLoader;
    private Civilian target;
    private OrbitCameraPivotBasic pivot;
    
    public override OrbitCameraData GetData(Camera cam) {
        if (target != null) return pivot.GetData(cam);
        var handle = targetLoader.GetCharacterAsync();
        if (handle.IsDone) {
            OnCharacterSpawn(handle);
        } else {
            handle.Completed += OnCharacterSpawn;
        }
        return pivot.GetData(cam);
    }

    private void OnCharacterSpawn(AsyncOperationHandle<Civilian> handle) {
        target = handle.Result;
        GameObject pivotObj = new GameObject("Character Pivot", typeof(OrbitCameraPivotBasic));
        pivotObj.transform.SetParent(target.GetDisplayAnimator().GetBoneTransform(HumanBodyBones.Head));
        pivotObj.transform.localPosition = Vector3.zero;
        pivot = pivotObj.GetComponent<OrbitCameraPivotBasic>();
        pivot.SetInfo(Vector2.one*0.5f, 2f, 65f);
    }
}
