using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PhysicsMaterialExtensionDatabase", menuName = "Data/Physics Material Extension Database", order = 17)]
public class PhysicsMaterialExtensionDatabase : ScriptableObject {
    [SerializeField]
    private List<PhysicsMaterialExtension> extensions;
    [SerializeField]
    private PhysicsMaterialExtension defaultExtension;
    private PhysicsMaterialExtension GetExtension(PhysicMaterial material) {
        foreach (var extension in extensions) {
            if (extension.AssociatedWith(material)) {
                return extension;
            }
        }

        return defaultExtension;
    }
    
    private PhysicsMaterialExtension GetExtension(Material material) {
        foreach (var extension in extensions) {
            if (extension.AssociatedWith(material)) {
                return extension;
            }
        }
        return defaultExtension;
    }

    public PhysicMaterial GetPhysicMaterial(Material material) {
        return GetExtension(material).GetPhysicMaterial();
    }

    public bool TryGetImpactInfo(PhysicMaterial material, PhysicsMaterialExtension.PhysicMaterialInfoType otherType, PhysicsMaterialExtension.PhysicsResponseType responseType, out PhysicsMaterialExtension.ImpactInfo impactInfo) {
        return GetExtension(material).TryGetImpactInfo(otherType, responseType, out impactInfo);
    }

    public float GetMassPerCubicMeter(PhysicMaterial material) {
        return GetExtension(material).GetMassPerCubicMeter();
    }

    public bool TryGetImpactInfo(ContactPoint contactPoint, PhysicsMaterialExtension.PhysicsResponseType type, out PhysicsMaterialExtension.ImpactInfo impactInfo) {
        PhysicsMaterialExtension selfMaterial = null;
        PhysicsMaterialExtension otherMaterial = null;
        foreach (var physicMaterialInfo in extensions) {
            if (physicMaterialInfo.AssociatedWith(contactPoint.thisCollider.sharedMaterial)) {
                selfMaterial = physicMaterialInfo;
                break;
            }
        }
        foreach (var physicMaterialInfo in extensions) {
            if (physicMaterialInfo.AssociatedWith(contactPoint.otherCollider.sharedMaterial)) {
                otherMaterial = physicMaterialInfo;
                break;
            }
        }
        if (selfMaterial == null || otherMaterial == null) {
            impactInfo = null;
            return false;
        }

        if (selfMaterial.TryGetImpactInfo(otherMaterial, type, out impactInfo)) {
            return true;
        }
        impactInfo = null;
        return false;
    }
}
