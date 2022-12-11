using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControlScript : MonoBehaviour
{
   


    private float horizotalInput;
    private float verticalInput;
    private float steeringAngle;
    private float breakingInput;
    private float mySidewayFriction;
    private float myForwardFriction;
    private float slipSidewayFriction;
    private float slipforwardfriction;
  

    AudioSource sound;



    public WheelCollider frontDriverW, frontPassengerW;
    public WheelCollider backDriverW, backPassengerW;
    public Transform frontDriverT, frontPassengerT;
    public Transform backDriverT, backPassengerT;
    public Vector3 centerOfMass;
    public float currentSpeed;
    public float maxSpeed = 250f;
    public float maxSteerAngle = 45f;
    public float motorforce = 200f;
    public float breaks = 500000f;
    public float decelarationSpeed = 100f;
    Rigidbody rigidbody;
    
  //  public bool braked = false;
   
    public int[] gearRatio;
    public int i;
    
    public void GetInput()
    {
        horizotalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        breakingInput = Input.GetAxis("Jump");
       
        if(Input.GetButton("Vertical")==false)
        {
            frontDriverW.brakeTorque = decelarationSpeed*Time.deltaTime;
            frontPassengerW.brakeTorque = decelarationSpeed*Time.deltaTime;
        }
        else
        {
            backDriverW.brakeTorque = 0;
            backPassengerW.brakeTorque = 0;
        }

    
    }
    private void Steer()
    {
        steeringAngle = maxSteerAngle * horizotalInput;
        frontDriverW.steerAngle = steeringAngle;
        frontPassengerW.steerAngle = steeringAngle;

    }
    private void Accelerate()
    {
        currentSpeed = 2 * Mathf.PI * frontDriverW.radius * frontPassengerW.rpm * 60 / 1000;
        if (currentSpeed < maxSpeed )
        {
            frontDriverW.motorTorque = verticalInput * motorforce;
            frontPassengerW.motorTorque = verticalInput * motorforce;

        }

    }
    private void Breaks()
    {

        if (Input.GetButton("Jump"))
        {
            if (rigidbody.velocity.magnitude > 1)
            {
                setSlip(slipforwardfriction, slipSidewayFriction);
            }
            else
            {
                setSlip(1, 1);
            }
            /* WheelFrictionCurve wheelFrictionCurve;                 //wheel friction curve is used by the wheel colliders to determine the friction of the wheel

            WheelFrictionCurve wheelFrictionCurve1;

            wheelFrictionCurve = backPassengerW.sidewaysFriction;

            wheelFrictionCurve.stiffness = .6f;
            backPassengerW.sidewaysFriction = wheelFrictionCurve;

            wheelFrictionCurve1 = backDriverW.sidewaysFriction;
            wheelFrictionCurve1.stiffness = .6f;
            backDriverW.sidewaysFriction = wheelFrictionCurve1;*/
           
        }
        else
                {
            setSlip(myForwardFriction, mySidewayFriction);
        }
        frontDriverW.brakeTorque = breakingInput*breaks;
        frontPassengerW.brakeTorque = breakingInput*breaks;
        backDriverW.brakeTorque = breakingInput * breaks;
        backPassengerW.brakeTorque = breakingInput * breaks;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontDriverW, frontDriverT);
        UpdateWheelPose(frontPassengerW, frontPassengerT);
        UpdateWheelPose(backDriverW, backDriverT);
        UpdateWheelPose(backPassengerW, backPassengerT);
    }
    private void UpdateWheelPose(WheelCollider wheelCollider, Transform transform)
    {
        Vector3 _pos = transform.position;
        Quaternion _quat = transform.rotation;

        wheelCollider.GetWorldPose(out _pos, out _quat);

        transform.position = _pos;
        transform.rotation = _quat;
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = centerOfMass;
       sound = GetComponent<AudioSource>();

        setValues();
    }
    private void setValues()
    {
        myForwardFriction = backPassengerW.forwardFriction.stiffness;
        mySidewayFriction = backPassengerW.sidewaysFriction.stiffness;
        slipforwardfriction = 0.3f;
        slipSidewayFriction = 0.1f;
    }
    

    private void EngineSound()
    {
       for( i =0; i<gearRatio.Length; i++)
        {
            if (gearRatio[i] > currentSpeed)
            { break; }
        }
        
        float gearMinValue = 0f;
        float gearMaxValue = 0f;
        if(i==0)
            {
                gearMinValue = 0;
            gearMaxValue = gearRatio[i];
                
            }
        else
        {
            gearMinValue = gearRatio[i-1];
            gearMaxValue = gearRatio[i];
        }

        float enginePitch = ((currentSpeed - gearMinValue) / (gearMaxValue - gearMinValue))+1;




        sound.pitch = enginePitch;
    }


    private void setSlip(float currentForwardfriction , float currentSidewayFriction)
    {
        WheelFrictionCurve wheelRR = backPassengerW.forwardFriction;
        WheelFrictionCurve wheelRL = backDriverW.forwardFriction;
        
        wheelRR.stiffness = currentForwardfriction;
        wheelRL.stiffness = currentForwardfriction;
        backPassengerW.forwardFriction = wheelRR;
        backDriverW.forwardFriction = wheelRL;

        WheelFrictionCurve wheelR = backPassengerW.sidewaysFriction;
        WheelFrictionCurve wheelL = backDriverW.sidewaysFriction;

        wheelL.stiffness = currentSidewayFriction;
        wheelR.stiffness = currentSidewayFriction;
        backPassengerW.sidewaysFriction = wheelR;
        backDriverW.sidewaysFriction = wheelL;

    }

 


    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        Breaks();
        UpdateWheelPoses();
        EngineSound();
        
    }


}


