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

        float deltaTime;
        CumStorage storage;
        DickCum stimulationData;

        //Inner & Outer thrust distances, could be calculated for character size?
        private float outerLimit = -0.05f;
        private float insertLimit = 1f;

        public AutoFuckController(CharacterBase user)
        {
            if (user.TryGetComponent(out DickCum dc))
            {
                stimulationData = dc;
                stimulationData.cummed += Nut;
                stimulationData.stimulated += SlowStimulation;
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

        //Decrease stimulation to artificially extend the time AI spend using stands.
        private void SlowStimulation(float newStim)
        {
            if (newStim < 0.9f && newStim > 0.25f)
                stimulationData.ReduceStimulation(0.35f * Time.deltaTime);
        }

        private void Nut()
        {
            if (storage.GetDoneChurning())
            {
                nut = true;
            }
        }

        //Adjusts the speed of the motion as time goes on. Perhaps move this out to be adjustable later?
        private float ThrustSpeedScalar()
        {
            if (nut)
            {
                if (pullingOut)
                    return 3f;

                return (thrustProgress <= insertLimit * 0.9f) ? 2f : 0.04f;
            }

            if (timeUsed < 2.5f)
                return pullingOut ? 0.5f : 0.25f;

            if (timeUsed < 10f)
                return pullingOut ? 1f : 0.75f;
            
            if (timeUsed < 15f)
                return pullingOut ? 1.25f : 1f;

            if (timeUsed < 20f)
                return pullingOut ? 1.5f : 1.25f;

            return pullingOut ? 2f : 1.75f;
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