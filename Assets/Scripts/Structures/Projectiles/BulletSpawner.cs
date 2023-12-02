using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{

    // La idea es que esto sea el transform de cada torreta peeero por ahora se pone a mano
    [SerializeField] private Transform _transform;
    // El objetin de la bala
    [SerializeField] private BaseBullet _bulletGameObject;
    //
    [SerializeField] private int _maxBulletsInScene;
    [SerializeField] private float _shootDelay;

    private ObjectPool _bulletPool;
    private int _spawnedBullets = 0;
    private int _activeBullets = 0;

    // Para que se dispare por el cañon
    private float _offset = 0.5f;

    private void Awake()
    {
        _bulletPool = new ObjectPool(_bulletGameObject, _maxBulletsInScene, true, _transform);
    }

    public void SpawnBullet()
    {
        var bullet = _bulletPool.Get() as BaseBullet;

        if (!bullet) return;

        var turret = bullet.gameObject.GetComponentInParent<Transform>().transform;
        var bulletPosition = turret.position + Vector3.forward * _offset;
        bullet.transform.SetPositionAndRotation(bulletPosition, Quaternion.identity);
        
        bullet.Spawner = this;

        _spawnedBullets++;
        _activeBullets++;

        Invoke(nameof(SpawnBullet), _shootDelay);
    }

    public void DespawnBullet(BaseBullet bullet)
    {
        _bulletPool.Return(bullet);
        _activeBullets--;
    }
}
