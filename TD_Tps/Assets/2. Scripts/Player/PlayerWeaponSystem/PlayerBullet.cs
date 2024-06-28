using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : BulletSystem
{
    protected override void BulletHit()
    {
        base.BulletHit();
    }

    protected override void BulletHitObstacle()
    {
        base.BulletHitObstacle();

        if (!isDestroying)
        {
            StartCoroutine(DestroyBullets(0));
        }
    }

    protected override void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            BulletHit();
        }
        else if (other.collider.CompareTag("Obstacle"))
        {
            BulletHitObstacle();
        }
    }
}
