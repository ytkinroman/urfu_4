using UnityEngine;

public class XORCombiner : MonoBehaviour
{
    public Perceptron perceptronAND;
    public Perceptron perceptronOR;

    private bool andTrainingCompleted = false;
    private bool orTrainingCompleted = false;

    public delegate void TrainingCompletedEventHandler ();
    public event TrainingCompletedEventHandler OnTrainingCompleted;

    private void Start () {
        // Подписка на события окончания обучения
        if (perceptronAND != null) {
            perceptronAND.OnTrainingCompleted += OnAndTrainingCompleted;
        }

        if (perceptronOR != null) {
            perceptronOR.OnTrainingCompleted += OnOrTrainingCompleted;
        }
    }

    private void OnAndTrainingCompleted () {
        andTrainingCompleted = true;
        CheckBothTrainingCompleted();
    }

    private void OnOrTrainingCompleted () {
        orTrainingCompleted = true;
        CheckBothTrainingCompleted();
    }

    private void CheckBothTrainingCompleted () {
        if (andTrainingCompleted && orTrainingCompleted) {
            if (OnTrainingCompleted != null) {
                OnTrainingCompleted.Invoke();
            }
        }
    }

    public double CalcOutput (double i1, double i2) {
        double andOutput = perceptronAND.CalcOutput(i1, i2);
        double orOutput = perceptronOR.CalcOutput(i1, i2);

        return andOutput == 1 ? 0 : orOutput;
    }
}
