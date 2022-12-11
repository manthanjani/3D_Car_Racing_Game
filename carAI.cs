using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carAI : MonoBehaviour
{
    public Transform path;
    public float carTorque = 120f;
    public float maxSteerAngle = 45;
    public WheelCollider wheelFl;
    public Transform WheelFLt;
    public WheelCollider wheelFr;
    public Transform WheelFRt;
    public WheelCollider wheelBl;
    public Transform WheelBLt;
    public WheelCollider wheelBr;
    public Transform WheelBRt;
    public float CurrentSpeed;
    public float maxSpeed = 250f;
    public bool isBraking = false;
    public Texture2D textureNormal;
    public Texture2D textureBraking;
    public Vector3 centerOfMass;
    public Renderer carRenderer;
    public float maxBrakeTorque = 100000f;
    public int[] gearRatio;
    public int i;
    

    private List<Transform> nodes;
    private int currentNode = 0;   //the variable that keeps track of our current node
    AudioSource sound;

    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
        sound = GetComponent<AudioSource>();
        Transform[] pathTransform = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();
        for (int i = 0; i < pathTransform.Length ; i++)
        {
            if (pathTransform[i] != path.transform)
            {
                nodes.Add(pathTransform[i]);
            }
        }
    }

    //physics and calcutaions involved
    private void FixedUpdate()
    {
        ApplySteer();
        Drive();
        CheckWayPointDistance();
        UpdateWheel();
        braking();
        //EngineSound();
    }

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);

        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheelFl.steerAngle = newSteer;
        wheelFr.steerAngle = newSteer;
    }
    private void Drive()
    {
        CurrentSpeed = 2 * Mathf.PI * wheelFl.radius * wheelFl.rpm * 60 / 1000;
        if (CurrentSpeed < maxSpeed && !isBraking)
        {
            wheelFl.motorTorque = carTorque;
            wheelFr.motorTorque = carTorque;
        }
        else
        {
            wheelFl.motorTorque = 0;
            wheelFr.motorTorque = 0;
        }
    }

    private void CheckWayPointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) <0.9f)
        {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else 
            {
                currentNode = currentNode + 1;
            }

        }


    }

    private void UpdateWheel()
    {
        Updatewheels(wheelFl, WheelFLt);
        Updatewheels(wheelFr, WheelFRt);
        Updatewheels(wheelBl, WheelBLt);
        Updatewheels(wheelBr, WheelBRt);

    }

    private void Updatewheels(WheelCollider wheelCollider, Transform transform)
    {
        Vector3 _position = transform.position;
        Quaternion _quat = transform.rotation;
        wheelCollider.GetWorldPose(out _position, out _quat);
        transform.position = _position;
        transform.rotation = _quat;
    }

    private void braking()
    {
        if(isBraking)
        {
            carRenderer.material.mainTexture = textureBraking;
            wheelBl.brakeTorque = maxBrakeTorque;
            wheelBr.brakeTorque = maxBrakeTorque;
        }
        else
        {
            carRenderer.material.mainTexture = textureNormal;
            wheelBl.brakeTorque = 0; 
            wheelBr.brakeTorque = 0;
        }
    }
   /* private void EngineSound()
    {
        for (i = 0; i < gearRatio.Length; i++)
        {
            if (gearRatio[i] > CurrentSpeed)
            { break; }
        }

        float gearMinValue;
        float gearMaxValue=0;
        if (i == 0)
        {
            gearMinValue = 0;
            gearMaxValue = gearRatio[i];

        }
        else
        {
            gearMinValue = gearRatio[i - 1];
            gearMaxValue = gearRatio[i];
        }

        float enginePitch = ((CurrentSpeed - gearMinValue) / (gearMaxValue - gearMinValue)) + 1;




        sound.pitch = enginePitch;
    }*/

}
