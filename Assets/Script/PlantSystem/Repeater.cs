using UnityEngine;
using System.Collections;

public class Repeater : PeaShooter
{
    [Header("Repeater Settings")]
    [SerializeField] private float delayBetweenShots = 0.25f; // Delay antar peluru pertama dan kedua

    protected override void Shoot()
    {
        StartCoroutine(ShootTwice());
    }

    private IEnumerator ShootTwice()
    {
        // Tembakan pertama — pakai logic Shoot() milik PeaShooter
        base.Shoot();

        // Tunggu sebentar
        yield return new WaitForSeconds(delayBetweenShots);

        // Tembakan kedua
        base.Shoot();
    }
}