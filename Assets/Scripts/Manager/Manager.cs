using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Manager : MonoBehaviour
{
    // Prefab lié à Unity
    public Environment prefabEnvironment;

    public float bestTime;
    private float lastBestTime;
    public float[] bestAttributes;
    private Environment currentEnvironment;
    private float currentTime;
    private float[] currentAttributes;
    private Dictionary<float[], float> neighbours = new Dictionary<float[],float>();
    
    void Start()
    {
        //Un peu déçu mais après test je peux en jouer qu'un seul à la fois
        //On initialise le premier environnement

        bestAttributes = new float[] {
            1f,
            1f,
            10,
            10,
            10,
            10};
        bestTime = 0;
        lastBestTime = 0;
        this.GenerateNeighborHood();
        float[] nextAttribute = GetNextNeighbour();
        this.SetEnvironment(nextAttribute);
    }

    //Génération des voisins
    //J'écris tout à la main
    void GenerateNeighborHood()
    {
        neighbours.Clear();
        float[] attributes = new float[] {
            Mathf.Max(bestAttributes[0],1),
            Mathf.Max(bestAttributes[1],1),
            Mathf.Max(bestAttributes[2],1),
            Mathf.Max(bestAttributes[3],1),
            Mathf.Max(bestAttributes[4],1),
            Mathf.Max(bestAttributes[5],1)};
        neighbours.Add(attributes, 0);
        attributes = new float[] {
            Mathf.Max(bestAttributes[0] - 0.3f, 1),
            Mathf.Max(bestAttributes[1],1),
            Mathf.Max(bestAttributes[2],1),
            Mathf.Max(bestAttributes[3],1),
            Mathf.Max(bestAttributes[4],1),
            Mathf.Max(bestAttributes[5],1)};
        neighbours.Add(attributes, 0);
        attributes = new float[] {
            Mathf.Max(bestAttributes[0] + 0.3f, 1),
            Mathf.Max(bestAttributes[1],1),
            Mathf.Max(bestAttributes[2],1),
            Mathf.Max(bestAttributes[3],1),
            Mathf.Max(bestAttributes[4],1),
            Mathf.Max(bestAttributes[5],1)};
        neighbours.Add(attributes, 0);
        attributes = new float[] {
            Mathf.Max(bestAttributes[0],1),
            Mathf.Max(bestAttributes[1] - 0.3f, 1),
            Mathf.Max(bestAttributes[2],1),
            Mathf.Max(bestAttributes[3],1),
            Mathf.Max(bestAttributes[4],1),
            Mathf.Max(bestAttributes[5],1)};
        neighbours.Add(attributes, 0);
        attributes = new float[] {
            Mathf.Max(bestAttributes[0],1),
            Mathf.Max(bestAttributes[1] + 0.3f, 1),
            Mathf.Max(bestAttributes[2],1),
            Mathf.Max(bestAttributes[3],1),
            Mathf.Max(bestAttributes[4],1),
            Mathf.Max(bestAttributes[5],1)};
        neighbours.Add(attributes, 0);
        attributes = new float[] {
            Mathf.Max(bestAttributes[0],1),
            Mathf.Max(bestAttributes[1],1),
            Mathf.Max(bestAttributes[2] - 30, 1),
            Mathf.Max(bestAttributes[3],1),
            Mathf.Max(bestAttributes[4],1),
            Mathf.Max(bestAttributes[5],1)};
        neighbours.Add(attributes, 0);
        attributes = new float[] {
            Mathf.Max(bestAttributes[0],1),
            Mathf.Max(bestAttributes[1],1),
            Mathf.Max(bestAttributes[2] + 30, 1),
            Mathf.Max(bestAttributes[3],1),
            Mathf.Max(bestAttributes[4],1),
            Mathf.Max(bestAttributes[5],1)};
        neighbours.Add(attributes, 0);
        attributes = new float[] {
            Mathf.Max(bestAttributes[0],1),
            Mathf.Max(bestAttributes[1],1),
            Mathf.Max(bestAttributes[2],1),
            Mathf.Max(bestAttributes[3] - 50,1),
            Mathf.Max(bestAttributes[4],1),
            Mathf.Max(bestAttributes[5],1)};
        neighbours.Add(attributes, 0);
        attributes = new float[] {
            Mathf.Max(bestAttributes[0],1),
            Mathf.Max(bestAttributes[1],1),
            Mathf.Max(bestAttributes[2],1),
            Mathf.Max(bestAttributes[3] + 50,1),
            Mathf.Max(bestAttributes[4],1),
            Mathf.Max(bestAttributes[5],1)};
        neighbours.Add(attributes, 0);
        attributes = new float[] {
            Mathf.Max(bestAttributes[0],1),
            Mathf.Max(bestAttributes[1],1),
            Mathf.Max(bestAttributes[2],1),
            Mathf.Max(bestAttributes[3],1),
            Mathf.Max(bestAttributes[4] - 50,1),
            Mathf.Max(bestAttributes[5],1)};
        neighbours.Add(attributes, 0);
        attributes = new float[] {
            Mathf.Max(bestAttributes[0],1),
            Mathf.Max(bestAttributes[1],1),
            Mathf.Max(bestAttributes[2],1),
            Mathf.Max(bestAttributes[3],1),
            Mathf.Max(bestAttributes[4] + 50,1),
            Mathf.Max(bestAttributes[5],1)};
        neighbours.Add(attributes, 0);
        attributes = new float[] {
            Mathf.Max(bestAttributes[0],1),
            Mathf.Max(bestAttributes[1],1),
            Mathf.Max(bestAttributes[2],1),
            Mathf.Max(bestAttributes[3],1),
            Mathf.Max(bestAttributes[4],1),
            Mathf.Max(bestAttributes[5] - 100,1)};
        neighbours.Add(attributes, 0);
        attributes = new float[] {
            Mathf.Max(bestAttributes[0],1),
            Mathf.Max(bestAttributes[1],1),
            Mathf.Max(bestAttributes[2],1),
            Mathf.Max(bestAttributes[3],1),
            Mathf.Max(bestAttributes[4],1),
            Mathf.Max(bestAttributes[5] + 100,1)};
        neighbours.Add(attributes, 0);
    }

    // Retourne le prochain voisin à tester
    // Retourne un tableau de taille 0 sinon
    public float[] GetNextNeighbour()
    {
        foreach(float[] res in neighbours.Keys)
        {
            if (neighbours[res] == 0)
            {
                return res;
            }
        }
        return new float[0];
    }

    void SetEnvironment(float[] attributes)
    {
        currentAttributes = attributes;
        currentEnvironment = Instantiate(prefabEnvironment);
        currentEnvironment.moveSpeedBunny = currentAttributes[0];
        currentEnvironment.moveSpeedFox = currentAttributes[1];
        currentEnvironment.thirstResistanceBunny = currentAttributes[2];
        currentEnvironment.thirstResistanceFox = currentAttributes[3];
        currentEnvironment.hungerResistanceBunny = currentAttributes[4];
        currentEnvironment.hungerResistanceFox = currentAttributes[5];
        currentTime = Time.time;
        currentEnvironment.StartByManager();
    }

    public void ChooseBestNeighbour()
    {
        foreach(float[] attributes in neighbours.Keys)
        {
            if(neighbours[attributes] > bestTime)
            {
                bestTime = neighbours[attributes];
                bestAttributes = attributes;
            }
        }
        if(bestTime == lastBestTime)
        {
            Debug.Log(bestTime);
            Debug.Break();
        }
        lastBestTime = bestTime;
    }

    void Update()
    {
        if(currentEnvironment.bunny == 0 || currentEnvironment.fox == 0)
        {
            currentTime = Time.time - currentTime;
            neighbours[currentAttributes] = currentTime;
            Destroy(currentEnvironment.gameObject);
            float[] nextAttributes = GetNextNeighbour();
            if(nextAttributes.Length == 0)
            {
                Debug.Log("Choix et génération du nouveau!");
                ChooseBestNeighbour();
                GenerateNeighborHood();
            }
            nextAttributes = GetNextNeighbour();
            this.SetEnvironment(nextAttributes);
        }
    }
}
