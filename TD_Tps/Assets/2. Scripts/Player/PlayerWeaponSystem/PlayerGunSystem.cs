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

        public Transform mouseFollower;
        public GameObject bullet;
        public GameObject bulletCasing;
        
        //총기 스탯
        public Vector3 bulletSpread = new Vector3(0.1f, 0.1f, 0.1f);
        public float shootForce;
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

            BulletMethod();
            
            // 오디오

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

        public void BulletMethod()
        {
            Vector3 directionWithSpread = CalculateDirectionWithSpread();
            
            GameObject currentBullet = ObjectPool.Spawn(bullet, bulletStartPos.position, Quaternion.identity);
            currentBullet.transform.forward = directionWithSpread.normalized;
            
            currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
            
            // 탄피 생성 코드
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

