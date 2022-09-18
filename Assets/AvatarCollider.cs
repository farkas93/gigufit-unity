using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarCollider : MonoBehaviour
{
    public RigControl controls;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "StandingInShaft" ||
            other.tag == "CrouchingInShaft" ||
            other.tag == "LyingInShaft")
            controls.SetState( other.gameObject);
    }
}
