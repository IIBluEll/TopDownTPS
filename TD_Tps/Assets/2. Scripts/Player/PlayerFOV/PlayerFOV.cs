using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HM_TopView.PlayerFOV
{
    public class PlayerFOV : MonoBehaviour
    {
        public float delayTime = 0.2f;
        
        public float viewRadius;    // 시야 반경
        
        [Range(0, 360)] 
        public float viewAngle;     // 시야 각도

        public LayerMask targetMask;    // 목표 레이어마스크
        public LayerMask obstacleMask;  // 장애물 레이어마스크

        // 시야에 감지된 타겟 리스트
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
        
        private void FindVisibleTargets()   // 시야 반경 내 모든 타겟 감지
        {
            ClearVisibleTargetList();
            
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                
                // 타겟이 시야 각도 내에 있는지 확인
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    float disToTarget = Vector3.Distance(transform.position, target.position);

                    // 목표물과 플레이어 사이에 장애물이 있는지 확인
                    if (!Physics.Raycast(transform.position, dirToTarget, disToTarget, obstacleMask))
                    {
                        visibleTarget.Add(target);
                        target.GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }
        }

        private void ClearVisibleTargetList()
        {
            foreach (var target in visibleTarget)
            {
                target.GetComponent<MeshRenderer>().enabled = false;
            }
            
            visibleTarget.Clear();
        }
        
        // 각도를 벡터로 변환
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

