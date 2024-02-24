using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

[System.Serializable]
public class ObjectivePlaceCondom : Objective {
    private class CondomDetector : MonoBehaviour {
        public delegate void CondomEnteredAction(Condom condom);
        public event CondomEnteredAction condomEntered;

        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent(out Condom condom)) {
                condomEntered?.Invoke(condom);
            }
        }
    }

    [SerializeField]
    private CharacterLoader targetLoader;
    private CharacterBase target;
    [SerializeField]
    private Collider[] colliders;

    private ObjectiveStatus status;
    private List<CondomDetector> detectors;

    public override ObjectiveStatus GetStatus() => status;
    public AsyncOperationHandle handle;

    public override void OnRegister() {
        detectors = new();
        foreach (var c in colliders) {
            var d = c.gameObject.AddComponent<CondomDetector>();
            d.condomEntered += OnCondomEntered;
            detectors.Add(d);
        }
        handle = targetLoader.GetCharacterAsync();
    }

    private void OnCondomEntered(Condom condom) {
        if (handle.Status != AsyncOperationStatus.Succeeded) return;
        
        if (condom.GetChurnable() is not CharacterBase characterBase || characterBase != ((GameObject)handle.Result).GetComponent<CharacterBase>()) return;
        status = ObjectiveStatus.Completed;
        Complete();
    }

    public override void OnUnregister() {
        detectors.ForEach(Object.Destroy);        
    }
}
