using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "New PhysicsMaterialExtension", menuName = "Data/Physics Material Extension", order = 26)]
public class PhysicsMaterialExtension : ScriptableObject {
    [FormerlySerializedAs("associatedMaterials")] [SerializeField] private List<PhysicMaterial> associatedPhysicMaterials;
    [SerializeField] private List<Material> associatedMaterials;
    [SerializeField] private float massKgPerCubicMeter = 73f;
    [SerializeField] private List<ImpactInfo> impactInfos;
    [SerializeField] private PhysicMaterialInfoType materialType;
    
    [System.Flags]
    public enum PhysicMaterialInfoType {
        Soft = 1,
        Hard = 2,
    }
    [System.Flags]
    public enum PhysicsResponseType {
        Impact = 1,
        Footstep = 2,
        Scrape = 4,
        FluidSplash = 8,
    }
    [System.Serializable]
    public class ImpactInfo {
        public PhysicMaterialInfoType collisionWith;
        public PhysicsResponseType responseType;
        public AudioPack soundEffect;
        public List<VisualEffectAsset> visualEffects;
    }

    public bool AssociatedWith(PhysicMaterial material) {
        return associatedPhysicMaterials.Contains(material);
    }

    public bool AssociatedWith(Material material) {
        if (!material.name.Contains("(Instance)")) return associatedMaterials.Contains(material);
        foreach (var mat in associatedMaterials) {
            if (material.name == mat.name + " (Instance)") {
                return true;
            }
        }
        return false;
    }

    public float GetMassPerCubicMeter() => massKgPerCubicMeter;

    public PhysicMaterial GetPhysicMaterial() {
        return associatedPhysicMaterials[0];
    }
    public bool TryGetImpactInfo(PhysicMaterialInfoType collisionWith, PhysicsResponseType responseType, out ImpactInfo outputInfo) {
        foreach (var impactInfo in impactInfos) {
            if ((impactInfo.collisionWith & collisionWith) == 0 || (impactInfo.responseType & responseType) == 0) {
                continue;
            }
            outputInfo = impactInfo;
            return true;
        }
        outputInfo = null;
        return false;
    }
    
    public bool TryGetImpactInfo(PhysicsMaterialExtension collisionWith, PhysicsResponseType responseType, out ImpactInfo outputInfo) {
        return TryGetImpactInfo(collisionWith.materialType, responseType, out outputInfo);
    }
    
}
