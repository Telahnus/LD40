using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Prob
{
    public float lower;
    public float upper;
    public object value;

    public Prob(float pLower, float pUpper, object pValue)
    {
        lower = pLower;
        upper = pUpper;
        value = pValue;
    }
};

public class DiscreteDistribution 
{
	public List<Prob> mySet = new List<Prob>();

	public object getRandomValue(){
		float val = Random.value;
		foreach (Prob p in mySet){
			if (val > p.lower && val <= p.upper){
				return p.value;
			}
		}
		// Random.value may return 0f, which the above loop cant use
		return mySet[0].value; 
	}

	public void add(float pLower, float pUpper, object pValue){
		mySet.Add(new Prob(pLower, pUpper, pValue));
    }

}
