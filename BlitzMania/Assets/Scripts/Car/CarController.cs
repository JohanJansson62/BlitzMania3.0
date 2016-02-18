using System;
using UnityEngine;


public class CarController : MonoBehaviour
{
    [SerializeField]
    private WheelCollider[] m_wheelColliders = new WheelCollider[4];
    [SerializeField]
    private GameObject[] m_wheelMeshes = new GameObject[4];
    [SerializeField]
    private WheelEffects[] m_WheelEffects = new WheelEffects[4];
    [SerializeField]
    private float m_maximumSteerAngle = 25f;
    [Range(0, 1)]
    [SerializeField]
    private float m_steerHelper = 0.644f; // 0 is raw physics , 1 the car will grip in the direction it is facing
    [Range(0, 1)]
    [SerializeField]
    private float m_tractionControl = 1f; // 0 is no traction control, 1 is full interference
    [SerializeField]
    private float m_fullTorqueOverAllWheels = 2500f;
    [SerializeField]
    private float m_reverseTorque = 500f;
    //[SerializeField]
    //private float m_maxHandbrakeTorque = 1e+08f;
    [SerializeField]
    private float m_downforce = 1500f;
    [SerializeField]
    private float m_topspeed = 300;
    [SerializeField]
    private float m_revRangeBoundary = 1f;
    [SerializeField]
    private float m_slipLimit = 0.3f;
    [SerializeField]
    private float m_brakeTorque = 20000f;

    private static int m_noOfGears = 5;
    private Vector3 m_centreOfMassOffset;
    private Quaternion[] m_wheelMeshLocalRotations;
    private Vector3 m_prevpos, m_pos;
    private float m_steerAngle;
    private int m_gearNum;
    private float m_gearFactor;
    private float m_oldRotation;
    private float m_currentTorque;
    private Rigidbody m_rigidbody;
    private const float m_reversingThreshold = 0.01f;

    public bool Skidding { get; private set; }
    public float BrakeInput { get; private set; }
    public float CurrentSteerAngle { get { return m_steerAngle; } }
    public float CurrentSpeed { get { return m_rigidbody.velocity.magnitude * 2.23693629f; } }
    public float MaxSpeed { get { return m_topspeed; } }
    public float Revs { get; private set; }
    public float AccelInput { get; private set; }

    // Use this for initialization
    private void Start()
    {
        m_wheelMeshLocalRotations = new Quaternion[4];
        for (int i = 0; i < 4; i++)
        {
            m_wheelMeshLocalRotations[i] = m_wheelMeshes[i].transform.localRotation;
        }
        m_wheelColliders[0].attachedRigidbody.centerOfMass = m_centreOfMassOffset;

        //m_maxHandbrakeTorque = float.MaxValue;

        m_rigidbody = GetComponent<Rigidbody>();
        m_currentTorque = m_fullTorqueOverAllWheels - (m_tractionControl * m_fullTorqueOverAllWheels);
    }


    private void GearChanging()
    {
        float f = Mathf.Abs(CurrentSpeed / MaxSpeed);
        float upgearlimit = (1 / (float)m_noOfGears) * (m_gearNum + 1);
        float downgearlimit = (1 / (float)m_noOfGears) * m_gearNum;

        if (m_gearNum > 0 && f < downgearlimit)
        {
            m_gearNum--;
        }

        if (f > upgearlimit && (m_gearNum < (m_noOfGears - 1)))
        {
            m_gearNum++;
        }
    }


    // simple function to add a curved bias towards 1 for a value in the 0-1 range
    private static float CurveFactor(float factor)
    {
        return 1 - (1 - factor) * (1 - factor);
    }


    // unclamped version of Lerp, to allow value to exceed the from-to range
    private static float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }


    private void CalculateGearFactor()
    {
        float f = (1 / (float)m_noOfGears);
        // gear factor is a normalised representation of the current speed within the current gear's range of speeds.
        // We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
        var targetGearFactor = Mathf.InverseLerp(f * m_gearNum, f * (m_gearNum + 1), Mathf.Abs(CurrentSpeed / MaxSpeed));
        m_gearFactor = Mathf.Lerp(m_gearFactor, targetGearFactor, Time.deltaTime * 5f);
    }


    private void CalculateRevs()
    {
        // calculate engine revs (for display / sound)
        // (this is done in retrospect - revs are not used in force/power calculations)
        CalculateGearFactor();
        var gearNumFactor = m_gearNum / (float)m_noOfGears;
        var revsRangeMin = ULerp(0f, m_revRangeBoundary, CurveFactor(gearNumFactor));
        var revsRangeMax = ULerp(m_revRangeBoundary, 1f, gearNumFactor);
        Revs = ULerp(revsRangeMin, revsRangeMax, m_gearFactor);
    }


    public void Move(float steering, float accel, float footbrake, float handbrake)
    {
        for (int i = 0; i < 4; i++)
        {
            Quaternion quat;
            Vector3 position;
            m_wheelColliders[i].GetWorldPose(out position, out quat);
            m_wheelMeshes[i].transform.position = position;
            m_wheelMeshes[i].transform.rotation = quat;
        }

        //clamp input values
        steering = Mathf.Clamp(steering, -1, 1);
        AccelInput = accel = Mathf.Clamp(accel, 0, 1);
        BrakeInput = footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);
        //handbrake = Mathf.Clamp(handbrake, 0, 1);

        //Set the steer on the front wheels.
        //Assuming that wheels 0 and 1 are the front wheels.
        m_steerAngle = steering * m_maximumSteerAngle;
        m_wheelColliders[0].steerAngle = m_steerAngle;
        m_wheelColliders[1].steerAngle = m_steerAngle;

        SteerHelper();
        ApplyDrive(accel, footbrake);
        CapSpeed();

        //Set the handbrake.
        //Assuming that wheels 2 and 3 are the rear wheels.
        //if (handbrake > 0f)
        //{
        //    var hbTorque = handbrake * m_maxHandbrakeTorque;
        //    m_wheelColliders[2].brakeTorque = hbTorque;
        //    m_wheelColliders[3].brakeTorque = hbTorque;
        //}


        CalculateRevs();
        GearChanging();

        AddDownForce();
        CheckForWheelSpin();
        TractionControl();
    }


    private void CapSpeed()
    {
        float speed = m_rigidbody.velocity.magnitude;

        speed *= 2.23693629f;
        if (speed > m_topspeed)
            m_rigidbody.velocity = (m_topspeed / 2.23693629f) * m_rigidbody.velocity.normalized;
    }

    private void ApplyDrive(float accel, float footbrake)
    {

        float thrustTorque;
        thrustTorque = accel * (m_currentTorque / 4f);
        for (int i = 0; i < 4; i++)
        {
            m_wheelColliders[i].motorTorque = thrustTorque;
        }


        for (int i = 0; i < 4; i++)
        {
            if (CurrentSpeed > 5 && Vector3.Angle(transform.forward, m_rigidbody.velocity) < 50f)
            {
                m_wheelColliders[i].brakeTorque = m_brakeTorque * footbrake;
            }
            else if (footbrake > 0)
            {
                m_wheelColliders[i].brakeTorque = 0f;
                m_wheelColliders[i].motorTorque = -m_reverseTorque * footbrake;
            }
        }
    }


    private void SteerHelper()
    {
        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelhit;
            m_wheelColliders[i].GetGroundHit(out wheelhit);
            if (wheelhit.normal == Vector3.zero)
                return; // wheels arent on the ground so dont realign the rigidbody velocity
        }

        // this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
        if (Mathf.Abs(m_oldRotation - transform.eulerAngles.y) < 10f)
        {
            var turnadjust = (transform.eulerAngles.y - m_oldRotation) * m_steerHelper;
            Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
            m_rigidbody.velocity = velRotation * m_rigidbody.velocity;
        }
        m_oldRotation = transform.eulerAngles.y;
    }


    // this is used to add more grip in relation to speed
    private void AddDownForce()
    {
        m_wheelColliders[0].attachedRigidbody.AddForce(-transform.up * m_downforce *
                                                     m_wheelColliders[0].attachedRigidbody.velocity.magnitude);
    }


    //checks if the wheels are spinning and is so does three things
    // 1) emits particles
    // 2) plays tiure skidding sounds
    // 3) leaves skidmarks on the ground
    // these effects are controlled through the WheelEffects class
    private void CheckForWheelSpin()
    {
        // loop through all wheels
        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelHit;
            m_wheelColliders[i].GetGroundHit(out wheelHit);

            // is the tire slipping above the given threshhold
            if (Mathf.Abs(wheelHit.forwardSlip) >= m_slipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= m_slipLimit)
            {
                //m_WheelEffects[i].EmitTyreSmoke();

                // avoiding all four tires screeching at the same time
                // if they do it can lead to some strange audio artefacts
                if (!AnySkidSoundPlaying())
                {
                    m_WheelEffects[i].PlayAudio();
                }
                continue;
            }

            //if it wasnt slipping stop all the audio
            if (m_WheelEffects[i].PlayingAudio)
            {
                m_WheelEffects[i].StopAudio();
            }
            // end the trail generation
            m_WheelEffects[i].EndSkidTrail();
        }
    }

    // crude traction control that reduces the power to wheel if the car is wheel spinning too much
    private void TractionControl()
    {
        WheelHit wheelHit;
        for (int i = 0; i < 4; i++)
        {
            m_wheelColliders[i].GetGroundHit(out wheelHit);

            AdjustTorque(wheelHit.forwardSlip);
        }
    }


    private void AdjustTorque(float forwardSlip)
    {
        if (forwardSlip >= m_slipLimit && m_currentTorque >= 0)
        {
            m_currentTorque -= 10 * m_tractionControl;
        }
        else
        {
            m_currentTorque += 10 * m_tractionControl;
            if (m_currentTorque > m_fullTorqueOverAllWheels)
            {
                m_currentTorque = m_fullTorqueOverAllWheels;
            }
        }
    }


    private bool AnySkidSoundPlaying()
    {
        for (int i = 0; i < 4; i++)
        {
            if (m_WheelEffects[i].PlayingAudio)
            {
                return true;
            }
        }
        return false;
    }
}

