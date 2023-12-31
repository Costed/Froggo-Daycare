using System.Collections.Generic;
using UnityEngine;

public class BedArea : MonoBehaviour
{
    [SerializeField] float energyIncrease;

    List<Frog> frogs = new List<Frog>();

    void Update()
    {
        for (int i = frogs.Count - 1; i >= 0; i--)
        {
            Frog frog = frogs[i];

            frog.stats.energyStat.IncreaseStat(energyIncrease * Time.deltaTime);

            if (frog.stats.energyStat.GetStatValue() == 100f)
            {
                frog.stats.consumptionMultiplier = 1f;
                //frog.stats.energyStat.DisableConsumption = false;
                frog.DisableMovement = false;

                //frog.rb.AddForce(Maf.Direction(frog.rb.position, Vector3.up * 4f) * 5f, ForceMode.Impulse);

                frog.Sleeping = false;
                frogs.Remove(frog);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (GameManager.TryGetFrog(other, out Frog frog) && !frogs.Contains(frog))
        {
            frog.stats.consumptionMultiplier = 0f;
            //frog.stats.energyStat.DisableConsumption = true;
            frog.DisableMovement = true;
            frogs.Add(frog);
            frog.Sleeping = true;
            //frog.animator.Play(FrogAnimation.SleepStanding);
            frog.animator.PlayRandom(FrogAnimation.SleepStanding, FrogAnimation.SleepFaceDown);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (GameManager.TryGetFrog(other, out Frog frog))
        {
            frog.stats.consumptionMultiplier = 1f;
            //frog.stats.energyStat.DisableConsumption = false;
            frog.DisableMovement = false;
            frog.Sleeping = false;
            frogs.Remove(frog);
            frog.GoIdle();
        }
    }
}
