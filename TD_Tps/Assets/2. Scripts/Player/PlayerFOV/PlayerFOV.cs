using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HM_TopView.PlayerFOV
{
    public class PlayerFOV : MonoBehaviour
    {
        public float delayTime = 0.2f;
        
        public float viewRadius;
        
        [Range(0, 360)] 
        public float viewAngle;

        public LayerMask targetMask;
        public LayerMask obstacleMask;

        public List<Transform> visibleTarget = new List<Transform>();


        private void Start()
        {
            StartCoroutine(FindTargetCo(delayTime));
        }

        IEnumerator FindTargetCo(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                
                FindVisibleTargets();
            }
        }
        
        private void FindVisibleTargets()
        {
            visibleTarget.Clear();
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    float disToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, dirToTarget, disToTarget, obstacleMask))
                    {
                        visibleTarget.Add(target);
                    }
                }
            }
            
        }
        
        public Vector3 DirFromAngle(float angleInDegree, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegree += transform.eulerAngles.y;
            }

            return new Vector3(Mathf.Sin(angleInDegree * Mathf.Deg2Rad), 0, Mathf.Cos((angleInDegree * Mathf.Deg2Rad)));
        }
    }
}

