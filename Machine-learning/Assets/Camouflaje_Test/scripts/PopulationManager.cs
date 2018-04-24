using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
namespace MachineLearning.Camouflage
{
    public class PopulationManager : MonoBehaviour
    {

        public GameObject personPrefab;
        public int populationSize = 10;
        List<GameObject> population;
        public static float elapsed = 0;

        //UI
        public Text TrialTime;
        public Text Generation;

        int trialTime = 10;
        int generation = 1;

        // Use this for initialization
        void Start()
        {

            //first random population
            population = new List<GameObject>();

            for (int i = 0; i < populationSize; i++)
            {
                //random position
                Vector3 pos = new Vector3(Random.Range(-9, 9), Random.Range(-4.5f, 4.5f), 0);
                GameObject go = Instantiate(personPrefab, pos, Quaternion.identity);
                DNA dna = go.GetComponent<DNA>();
                dna.r = Random.Range(0.0f, 1.0f);
                dna.g = Random.Range(0.0f, 1.0f);
                dna.b = Random.Range(0.0f, 1.0f);
                dna.size = Random.Range(0.1f, 0.5f);
                //if we set size = 1 in the start of DNA as default, can happen that default is called after this line, and reset localScale to 1.
                dna.transform.localScale = Vector3.one * dna.size;
                population.Add(go);
            }

        }

        // Update is called once per frame
        void Update()
        {
            TrialTime.text = "Trial Time: " + trialTime;
            Generation.text = "Generation: " + generation;

            //each 10 seconds a new generation show up
            elapsed += Time.deltaTime;
            if (elapsed > trialTime)
            {
                BreedNewPopulation();
                elapsed = 0;
            }
        }

        //Delete the old population and creates a new one.
        void BreedNewPopulation()
        {
            //List<GameObject> newPopulation = new List<GameObject>();

            //order individuals by their dead time. For example: 3,4,1,8 in the ordered list is = 1,3,4,8
            List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<DNA>().timeToDie).ToList();

            //clear the list (the population is in the sortedList)
            population.Clear();

            //breed upper half of sorted List. Following the previous example: 4 elements/2 =2 -1 = 1
            //if we have 10 elements, the for goes from index 4 to 8, both included (first 0, last 9)
            for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
            {
                //breed current element with element + 1 and viceversa. 2 new offsping for each parent swap.
                population.Add(Breed(sortedList[i], sortedList[i + 1]));
                population.Add(Breed(sortedList[i + 1], sortedList[i]));
            }
            //we end up with 4 new individuals: 3+4, 4+3,4+8 and 8+4


            //destroy all parents and previous population
            for (int i = 0; i < sortedList.Count; i++)
            {
                Destroy(sortedList[i]);
            }
            generation++;
        }

        //Instantiate the offsping and choose random parent colors.
        GameObject Breed(GameObject parent1, GameObject parent2)
        {
            Vector3 pos = new Vector3(Random.Range(-9, 9), Random.Range(-4.5f, 4.5f), 0);
            GameObject offspring = Instantiate(personPrefab, pos, Quaternion.identity);
            DNA dna1 = parent1.GetComponent<DNA>();
            DNA dna2 = parent2.GetComponent<DNA>();
            //This is the core operation in DNA algorithm
            DNA offspringDNA = offspring.GetComponent<DNA>();
            //we open the posibility to mutations
            if (Random.value > 0.1f)
            {
                offspringDNA.r = Random.value < 0.5f ? dna1.r : dna2.r;
                offspringDNA.g = Random.value < 0.5f ? dna1.g : dna2.g;
                offspringDNA.b = Random.value < 0.5f ? dna1.b : dna2.b;

            }
            else
            { //mutation
                offspringDNA.r = Random.value;
                offspringDNA.g = Random.value;
                offspringDNA.b = Random.value;
            }

            if (Random.value > 0.05f)
            {
                offspringDNA.size = Random.value > 0.5f ? dna1.size : dna2.size;
            }
            else
            {
                offspringDNA.size = Random.Range(0.1f, 0.5f);
            }
            offspring.transform.localScale = Vector3.one * offspringDNA.size;

            return offspring;
        }

    }
}