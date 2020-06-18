using Networking;
using UnityEngine;

using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class NetworkSpawner : MonoBehaviour
{
    public List<NetworkUnit> units = new List<NetworkUnit>();
    private static List<NetworkUnit> spawnerUnits = new List<NetworkUnit>();

    public static void Spawn(NetworkUnit unit)
    {
        if (!unit.isReal)
        {
            return;
        }

        spawnerUnits.Add(unit);
        ClientManager.Communicator.Send(NetworkCommands.SpawnCommandID, $"{unit.GetType()}:{unit.ID}");
    }

    private static void InitSync(NetworkUnit unit)
    {
        foreach (var property in unit.GetType().GetProperties())
        {
            if (property.GetCustomAttribute<NetworkSync>() == null)
                continue;

            ClientManager.Communicator.RegisterRecieveCallback(Hasher.GetHash($"{unit.ID}_{property.Name}"), value => property.SetValue(unit, value));
        }

        foreach (var field in unit.GetType().GetFields())
        {
            if (field.GetCustomAttribute<NetworkSync>() == null)
                continue;

            ClientManager.Communicator.RegisterRecieveCallback(Hasher.GetHash($"{unit.ID}_{field.Name}"), value => field.SetValue(unit, value));
        }
    }

    private static void SyncData(NetworkUnit unit)
    {
        if (!unit.isReal)
        {
            return;
        }

        foreach (var property in unit.GetType().GetProperties())
        {
            if (property.GetCustomAttribute<NetworkSync>() == null)
                continue;

            ClientManager.Communicator.Send(Hasher.GetHash($"{unit.ID}_{property.Name}"), property.GetValue(unit));
        }

        foreach (var field in unit.GetType().GetFields())
        {
            if (field.GetCustomAttribute<NetworkSync>() == null)
                continue;

            ClientManager.Communicator.Send(Hasher.GetHash($"{unit.ID}_{field.Name}"), field.GetValue(unit));
        }
    }

    private void Start()
    {
        ClientManager.Communicator.RegisterRecieveCallback<string>(NetworkCommands.SpawnCommandID, data =>
        {
            string[] separated_data = data.Split(':');

            if (separated_data.Length != 2)
            {
                return;
            }

            if (int.TryParse(separated_data[1], out int ID))
            {
                var unit = units.Find(x => x.GetType().ToString() == separated_data[0]);

                if (unit != null)
                {
                    var spawnedUnit = Instantiate(unit);
                    spawnedUnit.SetID(ID);
                    InitSync(spawnedUnit);

                    Debug.Log($"Spawn {unit}");
                }
            }
        });

        StartCoroutine(SendThread());
    }

    private IEnumerator SendThread()
    {
        while(true)
        {
            foreach (var unit in spawnerUnits)
            {
                SyncData(unit);
            }

            yield return new WaitForSeconds(0.1F);
        }
       
    }
}
