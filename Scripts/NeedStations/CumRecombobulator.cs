using UnityEngine;

public class CumRecombobulator : NeedStation {
    [SerializeField] private Transform recombobulateLocation;
    
    public override bool ShouldInteract(CharacterBase from) {
        foreach (var condom in Condom.GetCondoms()) {
            float distance = Vector3.Distance(condom.transform.position, transform.position);
            if (distance < 10f) return base.ShouldInteract(from);
        }
        return false;
    }

    protected override void Activate() {
        base.Activate();
        foreach (var condom in Condom.GetCondoms()) {
            float distance = Vector3.Distance(condom.transform.position, transform.position);
            if (!(distance < 10f)) continue;
            var churnable = condom.GetChurnable();
            if (churnable == null) {
                Destroy(condom.gameObject);
                break;
            }
            churnable.transform.position = recombobulateLocation.position;
            churnable.transform.rotation = Quaternion.identity;
            if (churnable is CharacterBase churnableCharacter) {
                if (churnableCharacter.voreContainer is Balls balls) {
                    var ballsBody = balls.GetStorageTransform();
                    ballsBody.position = recombobulateLocation.position + Vector3.up;
                    ballsBody.rotation = Quaternion.identity;
                }
                if (churnableCharacter.IsPlayer())
                {
                    RestorePlayerCamera(churnableCharacter);
                }
            }

            churnable.transform.gameObject.SetActive(true);
            Destroy(condom.gameObject);
            beingUsedBy.StartCoroutine(DialogueLibrary.GetDialogue(DialogueLibrary.DialogueGroupType.Recombobulate).Begin(new DialogueCharacter[] {
                DialogueCharacterSpecificCharacter.Get(churnable.transform.GetComponent<CharacterBase>()),
                DialogueCharacterSpecificCharacter.Get(beingUsedBy)
            }));
            break;
        }
    }
    private void RestorePlayerCamera(CharacterBase churnableCharacter)
    {
        OrbitCameraPivotBasic headPivot = churnableCharacter.transform.Find("HeadPivot")?.GetComponent<OrbitCameraPivotBasic>();
        OrbitCameraPivotBasic crouchPivot = churnableCharacter.transform.Find("CrouchPivot")?.GetComponent<OrbitCameraPivotBasic>();
        OrbitCameraPivotBasic buttPivot = churnableCharacter.GetDisplayAnimator().GetBoneTransform(HumanBodyBones.Hips).Find("ButtPivot")?.GetComponent<OrbitCameraPivotBasic>();

        OrbitCameraCharacterHitmanConfiguration hitmanConfig = new OrbitCameraCharacterHitmanConfiguration();
        hitmanConfig.SetPivots(churnableCharacter, headPivot, crouchPivot, buttPivot);
        OrbitCamera.AddConfiguration(hitmanConfig);
    }
}
