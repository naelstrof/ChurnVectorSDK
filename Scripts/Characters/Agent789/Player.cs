using UnityEngine;

public partial class CharacterBase {
    private static CharacterBase playerInstance;
    public static CharacterBase GetPlayer() => playerInstance;

    public delegate void PlayerSpawnedAction(CharacterBase player);

    public static event PlayerSpawnedAction playerEnabled;
    public static event PlayerSpawnedAction playerDisabled;
    
    private GameObject interactVisualization;

    private TicketLock.Ticket cutsceneLock;

    private OrbitCameraBasicConfiguration playerPredConfig;


    public delegate void SeenAction(KnowledgeDatabase.Knowledge knowledge, CharacterBase by);
    public event SeenAction seen;

    private void OnEnablePlayer() {
        Cutscene.cutsceneStarted += OnCutsceneStarted;
        Cutscene.cutsceneEnded += OnCutsceneEnded;
        endCockVoreAsPrey += OnCockVoreAsPrey;
        playerEnabled?.Invoke(this);
        interactVisualization = Instantiate(GameManager.GetLibrary().interactVisualizationPrefab);
    }

    private void OnCockVoreAsPrey(CharacterBase other) {
        if (IsPlayer()) {
            GameManager.PlayerGotCockVored();
            var configuration = new OrbitCameraBasicConfiguration();
            var pivot = other.voreContainer.GetStorageTransform().gameObject.AddComponent<OrbitCameraPivotBasic>();
            pivot.SetInfo(Vector2.one * 0.5f, 4f, 65f);
            configuration.SetPivot(pivot);
            OrbitCamera.AddConfiguration(configuration);
            playerPredConfig = configuration;

        }
        this.gameObject.transform.position = this.gameObject.transform.position.With(y: this.gameObject.transform.position.y - 100);
        CharacterDetector.RemoveTrackingGameObjectFromAll(gameObject);
    }

    public void RemovePredConfig() {
        if(playerPredConfig != null) {
            OrbitCamera.RemoveConfiguration(playerPredConfig);
            playerPredConfig = null;
        }        
    }

    private void OnDisablePlayer() {
        Cutscene.cutsceneStarted -= OnCutsceneStarted;
        Cutscene.cutsceneEnded -= OnCutsceneEnded;
        endCockVoreAsPrey -= OnCockVoreAsPrey;
        playerDisabled?.Invoke(this);
        Destroy(interactVisualization);
    }

    private void OnCutsceneStarted() {
        if (activeInteractable != null) {
            StopInteractionWith(activeInteractable);
        }
        cutsceneLock ??= ticketLock.AddLock(this);
    }

    private void OnCutsceneEnded() {
        ticketLock.RemoveLock(ref cutsceneLock);
    }

    private void UpdatePlayer() {
        if (GetTaseCount() == 0 && !ticketLock.GetLocked(TicketLock.LockFlags.IgnoreUsables)) {
            interactTarget = QueryBestInteractable();
            if (activeInteractable != null) {
                if (interactVisualization.activeInHierarchy) {
                    interactVisualization.SetActive(false);
                }
            } else {
                if (interactTarget != null) {
                    if (!interactVisualization.activeInHierarchy) {
                        interactVisualization.SetActive(true);
                    }

                    var bounds = interactTarget.GetInteractBounds();
                    interactVisualization.transform.position = bounds.center + Vector3.up * (bounds.extents.y * 0.5f);
                } else {
                    if (interactVisualization.activeInHierarchy) {
                        interactVisualization.SetActive(false);
                    }
                }
            }
        } else {
            if (interactVisualization.activeInHierarchy) {
                interactVisualization.SetActive(false);
            }
        }
    }
    
}
