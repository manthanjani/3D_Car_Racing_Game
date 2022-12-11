using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class youWIn : MonoBehaviour
{
    public GameObject youWinPanel;
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            youWinPanel.SetActive(true);
            
            Time.timeScale = 0f;

        }
    }

}
