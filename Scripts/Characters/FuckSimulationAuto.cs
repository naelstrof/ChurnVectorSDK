@ -0,0 + 1,146 @@
using DPG;
using UnityEngine;

public class FuckSimulationAuto : FuckSimulation
{
    private Transform hips;
    private AutoFuckController controller;
    private Penetrator penetrator;

    public Vector3 hipPosition => hips.position;


    public FuckSimulationAuto(CharacterBase user, Transform hipTransform, Penetrable targetHole, Penetrator targetPenetrator, Animator penetratorAnimator) : base(null, hipTransform, targetHole, targetPenetrator, penetratorAnimator)
    {
        penetrator = targetPenetrator;
        hips = hipTransform;
        controller = new AutoFuckController(user);
    }

    protected override Vector3 GetMousePosition()
    {
        float thrust = controller.GetThrustValue();
        Vector3 localHipPosition = penetrator.transform.InverseTransformPoint(hips.position);
        Vector3 localOutput = new Vector3(localHipPosition.x, localHipPosition.y, thrust);
        Vector3 trueWorldOutput = penetrator.transform.TransformPoint(localOutput);

        return trueWorldOutput;
    }

    protected override void SubStep(float dt, double time)
    {
        controller.Step(dt);
        base.SubStep(dt, time);
    }

    private class AutoFuckController
    {
        private float timeUsed = 0f;
        private float thrustProgress = 0f;
        private bool pullingOut = false;
        private bool nut = false;

        CumStorage storage;
        DickCum stimulationData;

        float deltaTime;
        private float outerLimit = -0.05f;
        private float insertLimit = 1f;

        public AutoFuckController(CharacterBase user)
        {
            if (user.TryGetComponent(out DickCum dc))
            {
                stimulationData = dc;
                stimulationData.cummed += Nut;
            }

            thrustProgress = outerLimit;
            storage = user.voreContainer.GetStorage();
        }

        public void Step(float dt)
        {
            deltaTime = dt;
            timeUsed += deltaTime;

            thrustProgress += ThrustDirection() * ThrustSpeedScalar() * deltaTime;
        }

        public float GetThrustValue()
        {
            return Mathf.Clamp(thrustProgress, outerLimit, insertLimit);
        }

        private float ThrustDirection()
        {
            if ((pullingOut && thrustProgress >= insertLimit) || (!pullingOut && thrustProgress <= outerLimit))
                pullingOut = !pullingOut;

            return ((pullingOut) ? 1 : -1);

        }

        private void Nut()
        {
            if (storage.GetDoneChurning())
            {
                nut = true;
            }
        }

        private float ThrustSpeedScalar()
        {
            float baseSpeed;

            if (nut)
            {
                if (!pullingOut)
                    baseSpeed = (thrustProgress <= insertLimit * 0.9f) ? 2f : 0.04f;
                else
                    baseSpeed = 3f;
            }

            else
            {
                if (timeUsed < 2.5f)
                {
                    baseSpeed = (pullingOut) ? 0.5f : 0.25f;
                }

                else if (timeUsed < 10f)
                {
                    baseSpeed = (pullingOut) ? 1f : 0.75f;
                }
                else if (timeUsed < 15f)
                {
                    baseSpeed = (pullingOut) ? 1.25f : 1f;
                }
                else if (timeUsed < 20f)
                {
                    baseSpeed = (pullingOut) ? 1.5f : 1.25f;
                }

                else
                    baseSpeed = (pullingOut) ? 2f : 1.75f;
            }

            return baseSpeed;
        }

        public void Disable()
        {
            if (stimulationData != null)
            {
                stimulationData.cummed -= Nut;
            }
        }
    }
}