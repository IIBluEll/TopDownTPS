using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_MouseFollower : MonoBehaviour
{
   private void Update()
   {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit, Mathf.Infinity))
      {
         Vector3 newPos = hit.point;
         newPos.y = 1.5f;

         transform.position = newPos;
      }
   }
}
