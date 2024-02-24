using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SceneLoader : MonoBehaviour {
    [SerializeField]
    private List<GameObject> hideWhileLoading;
    [SerializeField]
    private List<GameObject> showWhileLoading;

    private List<bool> activeMemory;
    private static SceneLoader instance;
    private static bool loadingLevel;
    private static List<MonoBehaviour> loadingStill = new List<MonoBehaviour>();

    public static void AddLoadingRequirement(MonoBehaviour self) {
        if (!loadingStill.Contains(self)) {
            loadingStill.Add(self);
        }
    }
    
    public static void RemoveLoadingRequirement(MonoBehaviour self) {
        if (loadingStill.Contains(self)) {
            loadingStill.Remove(self);
        }
    }
    private void Awake() {
        if (instance == null) {
            activeMemory = new List<bool>();
            instance = this;
        } else {
            Destroy(this);
        }
    }

    public static bool IsLoading() {
        loadingStill.RemoveAll(MatchNullLoaders);
        return loadingLevel || loadingStill.Count > 0;
    }

    private static bool MatchNullLoaders(MonoBehaviour x) {
        return x == null;
    }

    public static Coroutine LoadScene( AssetReferenceScene scene ) {
        // Allows to quickly display a loading graphic
        if (loadingLevel) {
            return null;
        }
        return instance.StartCoroutine(instance.LoadLevelRoutine(scene));
    }

    private IEnumerator LoadLevelRoutine(AssetReferenceScene scene) {
        yield return new WaitUntil(() => !loadingLevel);
        loadingLevel = true;
        activeMemory.Clear();
        foreach (var obj in hideWhileLoading) {
            activeMemory.Add(obj.activeSelf);
            obj.SetActive(false);
        }
        foreach (var obj in showWhileLoading) {
            obj.SetActive(true);
        }
        
        yield return new WaitForSecondsRealtime(0.1f);

        try {
            var handle = Addressables.LoadSceneAsync(scene);
            yield return handle;
            loadingLevel = false;
            yield return new WaitForSecondsRealtime(1f);
            yield return new WaitUntil(() => !IsLoading());
        } finally {
            loadingLevel = false;
            for (int i=0;i<hideWhileLoading.Count;i++) {
                hideWhileLoading[i].SetActive(activeMemory[i]);
            }
            foreach (var obj in showWhileLoading) {
                obj.SetActive(false);
            }
        }
    }
}
