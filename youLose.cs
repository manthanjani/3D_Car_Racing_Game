using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class youLose : MonoBehaviour
{
    public GameObject youLosePanel;
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AI")
        {
            youLosePanel.SetActive(true);
            
            Time.timeScale = 0f;

        }
    }

}
