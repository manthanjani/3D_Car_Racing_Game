using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroySkidSound : MonoBehaviour
{
	
	private float destroyAfter =2f;

    private void Start()
    {
        Destroy(gameObject, destroyAfter);
    }
}
