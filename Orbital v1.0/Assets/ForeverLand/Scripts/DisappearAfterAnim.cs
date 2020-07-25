using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearAfterAnim : MonoBehaviour
{
   public void Disappear()
    {
        Destroy(this.gameObject);
    }
}
