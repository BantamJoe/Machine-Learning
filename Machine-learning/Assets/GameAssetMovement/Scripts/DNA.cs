using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MachineLearning.SmartMovement
{
    public class DNA
    {
        List<int> genes = new List<int>();
        int dnaLength = 0;
        int maxValue = 0;

        public DNA(int l, int v)
        {
            dnaLength = l;
            maxValue = v;
            SetRandom();
        }

        public void SetRandom()
        {
            genes.Clear();
            for (int i = 0; i < dnaLength; i++)
            {
                genes.Add(Random.Range(0, maxValue));
            }
        }

        public void SetInt(int pos, int value)
        {
            genes[pos] = value;
        }

        /// <summary>
        /// Combine all the genes of the offspring, first half of d1 parent, second half of d2 parent.
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        public void Combine(DNA d1, DNA d2)
        {
            for (int i = 0; i < dnaLength; i++)
            {
                if (i < dnaLength / 2.0f)
                {
                    int c = d1.genes[i];
                    genes[i] = c;
                }
                else
                {
                    int c = d2.genes[i];
                    genes[i] = c;
                }
            }
        }

        //random gene get random value
        public void Mutate()
        {
            genes[Random.Range(0, dnaLength)] = Random.Range(0, maxValue);
        }

        public int GetGene(int pos)
        {
            return genes[pos];
        }

    }
}