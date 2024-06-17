using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HM_TopView.PlayerGunSystem
{
    public class PlayerGunSystem : MonoBehaviour
    {
        public Transform bulletStartPos;

        public TrailRenderer trail;
        
        //총기 스탯
        public Vector3 bulletSpread = new Vector3(0.1f, 0.1f, 0.1f);
        public float bulletRange;
        public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;    // 발사 간격, 탄퍼짐, 사정거리, 재장전 시간, 연속 발사 간의 시간
        public int magazineSize, bulletsPerTap;                                    // 탄창 크기, 탭당 발사하는 총알 수
        public bool allowButtonHold;                                               // 연속 발사 허용 여부
        private int bulletsLeft, bulletShot;                                       // 남은 총알 수, 발사된 총알 수
    
        // 상태 변수
        private bool shooting, readyToShoot, reloading;                            // 발사 중, 발사 준비 완료, 재장전 중
    
        // 키 입력
        public KeyCode reloadKey = KeyCode.R;
        public KeyCode shootingKey = KeyCode.Mouse0;
        public KeyCode trueAimKey = KeyCode.Mouse1;
    
        // 게임 시작 or 총기 바뀔 떄 업데이트 해주는 함수 필요
        public void UpdateGunInfo()
        {
        
        }

        private void OnEnable()
        {
            bulletsLeft = magazineSize;
            readyToShoot = true;
        }

        private void Update()
        {
            PlayerInput();
            
            Debug.DrawRay(this.transform.position, transform.forward, Color.green);
            Debug.DrawRay(bulletStartPos.position, bulletStartPos.forward, Color.red);
        }

        private void PlayerInput()
        {
            shooting = allowButtonHold ? Input.GetKey(shootingKey) : Input.GetKeyDown(shootingKey);
        
            if (Input.GetKeyDown(reloadKey) && bulletsLeft < magazineSize && !reloading)
            {
                //StartCoroutine(Reload());
            }

            if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            RaycastHit hit;
            
            bulletShot = 0;
            readyToShoot = false;

            Vector3 direction = CalculateDirectionWithSpread();
            
            if (Physics.Raycast(bulletStartPos.position, direction, out hit, float.MaxValue))
            {
                Debug.Log(hit.transform.name);

                TrailRenderer trailR = Instantiate(trail, bulletStartPos.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trailR, hit));
            }
            
            // 오디오
            // 탄피 생성
            bulletsLeft--;
            bulletShot++;
            
            if (bulletShot < bulletsPerTap && bulletsLeft > 0)
            {
                StartCoroutine(ShootWithDelay(timeBetweenShooting));
            }
            else
            {
                StartCoroutine(ResetShot(timeBetweenShooting));
            }
        }
        
        private IEnumerator ShootWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Shoot();
        }

        // 발사 상태 복구
        private IEnumerator ResetShot(float delay)
        {
            yield return new WaitForSeconds(delay);
            readyToShoot = true; 
        }

        private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
        {
            float time = 0;
            
            Vector3 startPosition = bulletStartPos.position;

            while (time < 1)
            {
                trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
                time += Time.deltaTime / trail.time;

                yield return null;
            }

            trail.transform.position = hit.point;
            // 명중 파티클
            
            Destroy(trail.gameObject, trail.time);
        }

        private Vector3 CalculateDirectionWithSpread()
        {
            Vector3 direction = bulletStartPos.forward;

            direction += new Vector3(
                Random.Range(-bulletSpread.x, bulletSpread.x), 0, Random.Range(-bulletSpread.z, bulletSpread.z));

            direction.Normalize();

            return direction;
        }
    }
}

