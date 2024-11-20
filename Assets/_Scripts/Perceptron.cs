using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TrainingSet
{
    public double[] input;
    public double output;
}


public class Perceptron : MonoBehaviour
{
    public int epochCount;

    public TrainingSet[] trainingSet;
    double[] weights = { 0, 0 };
    double bias = 0;
    double totalError = 0;

    // Событие по окончанию обучения.
    public delegate void TrainingCompletedEventHandler ();
    public event TrainingCompletedEventHandler OnTrainingCompleted;


    private double DotProductBias (double[] v1, double[] v2)
    {
        if (v1 == null || v2 == null)
            return -1;

        if (v1.Length != v2.Length)
            return -1;

        double d = 0;
        for (int x = 0; x < v1.Length; x++) {
            d += v1[x] * v2[x];
        }

        d += bias;

        return d;
    }

    private double CalcOutput (int i)
    {
        double dp = DotProductBias(weights, trainingSet[i].input);
        if (dp > 0) return (1);
        return (0);
    }

    private void InitialiseWeights ()
    {
        for (int i = 0; i < weights.Length; i++) {
            weights[i] = Random.Range(-1.0f, 1.0f);
        }
        bias = Random.Range(-1.0f, 1.0f);
    }

    private void UpdateWeights (int j)
    {
        double error = trainingSet[j].output - CalcOutput(j);
        totalError += Mathf.Abs((float)error);
        for (int i = 0; i < weights.Length; i++) {
            weights[i] = weights[i] + error * trainingSet[j].input[i];
        }
        bias += error;
    }

    public double CalcOutput (double i1, double i2)
    {
        double[] inp = new double[] { i1, i2 };
        double dp = DotProductBias(weights, inp);
        if (dp > 0) return (1);
        return (0);
    }

    private void Train (int epochs)
    {
        InitialiseWeights();

        for (int e = 0; e < epochs; e++) {
            totalError = 0;
            for (int t = 0; t < trainingSet.Length; t++) {
                UpdateWeights(t);
                Debug.Log("W1: " + (weights[0]) + " W2: " + (weights[1]) + " B: " + bias);
            }
            Debug.Log("TOTAL ERROR: " + totalError);
        }

        Debug.Log("Обучение завершено");

        // Вызов события по окончанию обучения.
        if (OnTrainingCompleted != null) {
            OnTrainingCompleted.Invoke();
        }
    }

    private void Start ()
    {
        Debug.Log("Старт обучения...");
        Train(epochCount);

        Debug.Log("Тестировани...");

        Debug.Log("Test 1, values 0 0 --> " + CalcOutput(0, 0));
        Debug.Log("Test 2, values 0 1 --> " + CalcOutput(0, 1));
        Debug.Log("Test 3, values 1 0 --> " + CalcOutput(1, 0));
        Debug.Log("Test 4, values 1 1 --> " + CalcOutput(1, 1));

        Debug.Log("Тестировани завершено");
    }
}