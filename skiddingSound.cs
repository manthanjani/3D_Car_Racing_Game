using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skiddingSound : MonoBehaviour
{
    public float currentFriction;
    public float markWidth = 0.2f;
    public GameObject skidSound;
    public ParticleSystem skidSmoke;
    public float smokeDepth = 0.4f;
    
    private float skitAt = 0.2f;
    private float soundEmission = 15f;
    private float soundWait;
    private int skidding;
    private Vector3[] lastPos = new Vector3[2];
    public Material skidMaterial;
    private void Start()
    {
        skidSmoke.transform.position = transform.position;
        Vector3 temp = skidSmoke.transform.position;
        temp.y -= smokeDepth;
        skidSmoke.transform.position = temp;
    }

        private void FixedUpdate()
    {
        WheelHit hit;
        transform.GetComponent<WheelCollider>().GetGroundHit(out hit);
        currentFriction = Mathf.Abs(hit.sidewaysSlip);
        if (skitAt <= currentFriction && soundWait <= 0)
        {
            Instantiate(skidSound, hit.point, Quaternion.identity);
            soundWait = 1;
        }
        soundWait -= Time.deltaTime * soundEmission;
        if (skitAt <= currentFriction)
        {
           
            ParticleSystem.EmissionModule isSkidding = skidSmoke.emission;
            isSkidding.enabled = true;
            skidMesh();
        }
        else
        {
           
            ParticleSystem.EmissionModule isSkidding = skidSmoke.emission;
            isSkidding.enabled = false;

            skidding = 0;
        }
    }

    private void skidMesh()
    {
        WheelHit hit;
        transform.GetComponent<WheelCollider>().GetGroundHit(out hit);
        GameObject mark = new GameObject("Mark");
        MeshFilter filter = mark.AddComponent<MeshFilter>();
        mark.AddComponent<MeshRenderer>();
        Mesh markMesh = new Mesh();
        Vector3[] vertices = new Vector3[4];
        int[] triangles = { 0, 1, 2, 0, 2, 3, 0, 3, 2, 0, 2, 1 };
        markMesh.RecalculateNormals();
        if (skidding == 0)
        {
            vertices[0] = hit.point + Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * new Vector3(markWidth, 0.01f, 0f);
            vertices[1] = hit.point + Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * new Vector3(-markWidth, 0.01f, 0f);
            vertices[2] = hit.point + Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * new Vector3(-markWidth, 0.01f, 0f);
            vertices[3] = hit.point + Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * new Vector3(markWidth, 0.01f, 0f);
            lastPos[0] = vertices[2];
            lastPos[1] = vertices[3];
            skidding = 1;
        }
        else
        {
            vertices[1] = lastPos[0];
            vertices[0] = lastPos[1];
            vertices[2] = hit.point + Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * new Vector3(-markWidth, 0.01f, 0f);
            vertices[3] = hit.point + Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * new Vector3(markWidth, 0.01f, 0f);
            lastPos[0] = vertices[2];
            lastPos[1] = vertices[3];
        }
        markMesh.vertices = vertices;
        markMesh.triangles = triangles;

        Vector2[] uvm = new Vector2[4];

        uvm[0] = new Vector2(1, 0);
        uvm[1] = new Vector2(0, 0);
        uvm[2] = new Vector2(0, 1);
        uvm[3] = new Vector2(1, 1);

        markMesh.uv = uvm;
        filter.mesh = markMesh;
        mark.GetComponent<Renderer>().material = skidMaterial;
       
        mark.AddComponent<destroySkidSound>();
    }
}