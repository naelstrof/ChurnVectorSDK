using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class OfflinkDetector : MonoBehaviour {
    private static OfflinkDetector instance;
    private List<NavMeshLink> links;
    private const float deviation = 0.2f;

    private void Awake() {
        if (instance == null) {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else {
            Destroy(gameObject);
        }
    }
    
    private static Vector3 GetGlobalStartPoint(NavMeshLink link) {
        // Necessary because link positions are calculated with discarded scale because fuck you. No documentation states this either.
        var linkTransform = link.transform;
        Vector3 localPositionRotated = linkTransform.rotation * link.startPoint;
        return linkTransform.position+localPositionRotated;
    }
    
    private static Vector3 GetGlobalEndPoint(NavMeshLink link) {
        // Necessary because link positions are calculated with discarded scale because fuck you. No documentation states this either.
        var linkTransform = link.transform;
        Vector3 localPositionRotated = linkTransform.rotation * link.endPoint;
        return linkTransform.position+localPositionRotated;
    }

    private void Start() {
        links = new List<NavMeshLink>(FindObjectsOfType<NavMeshLink>(true));
        var triangulation = NavMesh.CalculateTriangulation();
        // Check for errors in the map
        foreach (var linkA in links) {
            //if (Vector3.Distance(linkA.transform.lossyScale, Vector3.one) > 0.1f) {
                //Debug.LogError("Link " + linkA.gameObject + " has a non-neutral scale, which makes detection difficult for the AI");
            //}
            foreach (var linkB in links) {
                if (linkA == linkB) {
                    continue;
                }
                if (linkA.TryGetComponent(out NavMeshLinkToInteractable _throwaway) ||
                    linkB.TryGetComponent(out NavMeshLinkToInteractable _throwAway2)) {
                    Vector3 globalPosA = GetGlobalStartPoint(linkA);
                    Vector3 globalPosB = GetGlobalStartPoint(linkB);
                    float dist = Vector3.Distance(globalPosA, globalPosB);
                    if (dist < deviation) {
                        Debug.LogError(
                            "Link " + linkA + " is extremely similar to " + linkB + " and will cause confusion for AI.",
                            linkA);
                    }
                }
            }
            for (int i = 0; i < triangulation.indices.Length; i += 3) {
                var v1 = triangulation.vertices[triangulation.indices[i]];
                var v2 = triangulation.vertices[triangulation.indices[i + 1]];
                var v3 = triangulation.vertices[triangulation.indices[i + 2]];
                var pos = GetGlobalStartPoint(linkA);
                if (Vector3.Distance(v1, pos) < deviation || Vector3.Distance(v2, pos) < deviation ||
                    Vector3.Distance(v3, pos) < deviation) {
                    Debug.LogError(
                        "Link " + linkA +
                        " is extremely similar to a corner on the navmesh and will cause confusion for AI.", linkA);
                }
            }
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        links = new List<NavMeshLink>(FindObjectsOfType<NavMeshLink>(true));
    }

    public static NavMeshLink GetLinkFromPosition(Vector3 position) {
        foreach (var link in instance.links) {
            if (!link.isActiveAndEnabled) {
                continue;
            }

            Vector3 globalPos = GetGlobalStartPoint(link);
            float dist = Vector3.Distance(globalPos, position);
            if (dist < deviation) {
                return link;
            }

            if (!link.bidirectional) {
                continue;
            }
            
            globalPos = GetGlobalEndPoint(link);
            dist = Vector3.Distance(globalPos, position);
            if (dist < deviation) {
                return link;
            }
        }
        return null;
    }
}
