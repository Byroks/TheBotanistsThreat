using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public void ExplosionEnd(){
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
