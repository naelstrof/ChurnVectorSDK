using UnityEngine;

public class LoadDisplayHandler : MonoBehaviour {
    /*[SerializeField]
    private LoadDisplay loadDisplayPrefab;
    [SerializeField]
    private DickCum dickCum;

    private List<LoadDisplay> displays;

    private void OnEnable() {
        displays = new List<LoadDisplay>();
        for (int i = 0; i < dickCum.GetMaxLoads(); i++) {
            displays.Add(GameObject.Instantiate(loadDisplayPrefab.gameObject, transform).GetComponent<LoadDisplay>());
        }

        dickCum.onLoadsChanged += OnLoadsChanged;
    }

    private void OnLoadsChanged(int newCount) {
        for (int i = 0; i < displays.Count; i++) {
            displays[i].SetDisplay(i < newCount);
        }
    }

    private void OnDisable() {
        dickCum.onLoadsChanged -= OnLoadsChanged;
        foreach (var obj in displays) {
            Destroy(obj.gameObject);
        }
    }*/
}
