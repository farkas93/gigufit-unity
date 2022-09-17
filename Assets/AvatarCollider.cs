using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarCollider : MonoBehaviour
{
    RigControl controls;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "StandingInShaft")
        {

        }
    }
}
