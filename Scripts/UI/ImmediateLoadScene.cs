using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ImmediateLoadScene : MonoBehaviour {
    [SerializeField] private AssetReferenceScene scene;

    private IEnumerator Start() {
        yield return new WaitForSeconds(1f);
        Addressables.LoadSceneAsync(scene.RuntimeKey);
    }
}
