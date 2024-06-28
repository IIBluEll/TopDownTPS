using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSystem : MonoBehaviour
{
    // Assign Field
    [Header("Assign Field")]
    public Rigidbody rb;
    public LayerMask enemy;
    
    [Space(5f), Header("총알 물리")] 
    public float bounciness;
    public bool userGravity;

    public float maxLifeTime;
    public float setMaxLifeTime;
    
    protected int collisions;
    protected bool isDestroying = false;
    private PhysicMaterial physics_mat;

    protected virtual void OnEnable()
    {
        maxLifeTime = setMaxLifeTime;
        
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = userGravity;
        
        physics_mat = new PhysicMaterial()
        {
            bounciness = bounciness,
            frictionCombine = PhysicMaterialCombine.Minimum,
            bounceCombine = PhysicMaterialCombine.Maximum
        };
        
        GetComponent<CapsuleCollider>().material = physics_mat;
    }
    
    protected void Update()
    {
        if (maxLifeTime <= 0 && !isDestroying)
        {
            StartCoroutine(DestroyBullets(1));
        }

        maxLifeTime -= Time.deltaTime;
    }
    
    protected IEnumerator DestroyBullets(float time)
    {
        isDestroying = true;
      
        yield return new WaitForSeconds(time);
        ObjectPool.Unspawn(this.gameObject);
    }
    
    protected virtual void BulletHit()
    {
        
    }
    
    protected virtual void BulletHitObstacle()
    {
        
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        
    }
}
