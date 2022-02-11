using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && !collider.GetComponent<PlayerController>().shield) collider.GetComponent<PlayerController>().Die();
    }
}
