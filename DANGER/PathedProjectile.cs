﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//objektet som får denna har möjligheten att få en destination och röra sig mot denna destination med en specifierad hastighet
public class PathedProjectile : MonoBehaviour, ITakeDamage {

    private Transform _destination;
    private float _speed;
    public GameObject DestroyEffect;
    public int PointsToGivePlayer;

    public GameObject Prefabs;

    public void Initialize(Transform destination, float speed)
    {
       
         _destination = destination;
         _speed = speed;
        

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile")
        {
            Instantiate(DestroyEffect, transform.position, transform.rotation);
            Destroy(gameObject);
            // skapar först texten vi vill ska visas, nästa syle namn (kontrollera "presentationen" för hur texten ska visas.s slutligen specifierar vi textpositioneraren (gå upp under 1.5 sek och 50 pixlar/sek)
            GameManager.Instance.AddPoints(450);
            FloatingText.Show(string.Format("+{0}", 30), "PointStarText", new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));

            //Spawn COin
            Instantiate(Prefabs, transform.position, Quaternion.identity);

        }

        if (other.tag == "Platforms")
        {
            Instantiate(DestroyEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }



    }

    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _destination.position, Time.deltaTime * _speed);

    

        var distanceSquared = (_destination.transform.position - transform.position).sqrMagnitude;
      

        if (distanceSquared > .01f * .01f)
            return;
        
        if (DestroyEffect != null) // kan välja om vi vill ha en spawn effect eller ej!
            Instantiate(DestroyEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        if (DestroyEffect != null)
            Instantiate(DestroyEffect, transform.position, transform.rotation);

        Destroy(gameObject);

        var projectile = instigator.GetComponent<Projectile>();
        if(projectile!=null && projectile.Owner.GetComponent<Player>() != null && PointsToGivePlayer != 0)
        {
            GameManager.Instance.AddPoints(PointsToGivePlayer);
            FloatingText.Show(string.Format("+{0}", PointsToGivePlayer), "PointStarText", new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));
        }
    }
}
