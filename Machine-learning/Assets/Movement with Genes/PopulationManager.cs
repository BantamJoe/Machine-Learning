using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace MachineLearning.Movement
{
    public class PopulationManager : MonoBehaviour
    {
        public GameObject botPrefab;
        public int populationSize = 50;
        List<GameObject> population;
        public static float elapsed = 0;
        int trialTime = 5;
        int generation = 1;

        //UI
        public Text ElapsedTime;
        public Text Generation;
        public Text Population;
      
        // Use this for initialization
        void Start()
        {
            //first random population
            population = new List<GameObject>();

            for (int i = 0; i < populationSize; i++)
            {
                //random position start of the bot
                Vector3 pos = new Vector3(this.transform.position.x + Random.Range(-2,2), transform.position.y, transform.position.z + Random.Range(-2f, 2f));
                //instantiation
                GameObject go = Instantiate(botPrefab, pos, transform.rotation);

                //in this manager, we set the Brain, so it can deal with genes.
                go.GetComponent<Brain>().Init();
                population.Add(go);
            }
        }

        // Update is called once per frame
        void Update()
        {
            Population.text = "Population: " + population;
            ElapsedTime.text = "Trial Time: " + elapsed;
            Generation.text = "Generation: " + generation;

            elapsed += Time.deltaTime;
            if (elapsed > trialTime)
            {
                BreedNewPopulation();
                elapsed = 0;
            }
        }

        void BreedNewPopulation()
        {
            //List<GameObject> newPopulation = new List<GameObject>();

            //order individuals by their dead time. For example: 3,4,1,8 in the ordered list is = 1,3,4,8
            List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<Brain>().distance).ToList();

            //clear the list (the population is in the sortedList)
            population.Clear();

            for (int i = 0; i < sortedList.Count; i++)
            {
                Debug.Log(sortedList[i].GetComponent<Brain>().distance);
               
            }

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

        //Instantiate the offsping and mix genes.
        GameObject Breed(GameObject parent1, GameObject parent2)
        {
            Vector3 pos = new Vector3(this.transform.position.x + Random.Range(-2, 2), transform.position.y, transform.position.z + Random.Range(-2f, 2f));
            GameObject offspring = Instantiate(botPrefab, pos, Quaternion.identity);
            Brain b = offspring.GetComponent<Brain>();
            b.Init();
            //combination
            if (Random.value > 0.01f)
            {
                b.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
            }
            else
            { //mutation
                b.dna.Mutate();
            }

            return offspring;
        }
    }
}