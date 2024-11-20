using UnityEngine;
using System.Collections;

public class PerceptronController : MonoBehaviour
{
    public GameObject cube1;
    public GameObject cube2;

    private const float speed = 1.7f;
    private const float stopDistance = 1.3f;
    private const float delayStartAction = 0.2f;

    public Material materialForZero;
    public Material materialForOne;

    public double input1 = 1;
    public double input2 = 0;

    public Perceptron perceptron;

    private Renderer rendererCube1;
    private Renderer rendererCube2;

    private bool isMoving = false;

    public enum LogicOperation
    {
        AND,
        OR,
        XOR,
        NAND
    }

    public LogicOperation logicOperation;

    private void Awake ()
    {
        rendererCube1 = cube1.GetComponent<Renderer>();
        rendererCube2 = cube2.GetComponent<Renderer>();

        UpdateCubeMaterials();

        if (perceptron != null) {
            perceptron.OnTrainingCompleted += OnTrainingCompleted;
        }
    }

    private void Update ()
    {
        if (isMoving) {
            MoveCubesTowardsEachOther();
        }
    }


    private void OnTrainingCompleted ()
    {
        StartCoroutine(DelayedAction());
    }

    private IEnumerator DelayedAction ()
    {
        yield return new WaitForSeconds(delayStartAction);
        isMoving = true;
    }

    private void MoveCubesTowardsEachOther ()
    {
        Vector3 direction = (cube2.transform.position - cube1.transform.position).normalized;
        float distance = Vector3.Distance(cube1.transform.position, cube2.transform.position);

        if (distance > stopDistance) {
            cube1.transform.position += direction * speed * Time.deltaTime;
            cube2.transform.position -= direction * speed * Time.deltaTime;
        }
        else {
            isMoving = false;
            Debug.Log("Кубы соприкоснулись и остановились");
            CalculateAndLogOutput();
        }
    }

    private void CalculateAndLogOutput ()
    {
        if (perceptron != null) {
            double output = perceptron.CalcOutput(input1, input2);
            Debug.Log("Полученное значение из перцептрона: " + output);
            UpdateCubeMaterialsBasedOnOutput(output);
        }
    }

    private void UpdateCubeMaterials ()
    {
        if ((int)input1 == 1) {
            rendererCube1.material = materialForOne;
        }
        else {
            rendererCube1.material = materialForZero;
        }

        if ((int)input2 == 1) {
            rendererCube2.material = materialForOne;
        }
        else {
            rendererCube2.material = materialForZero;
        }
    }

    private void UpdateCubeMaterialsBasedOnOutput (double output)
    {
        int outputInt = (int)output;
        int input1Int = (int)input1;
        int input2Int = (int)input2;

        switch (logicOperation) {
            case LogicOperation.AND:
                ApplyAndLogic(outputInt, input1Int, input2Int);
                break;
            case LogicOperation.OR:
                ApplyOrLogic(outputInt, input1Int, input2Int);
                break;
            case LogicOperation.XOR:
                ApplyXorLogic(outputInt, input1Int, input2Int);
                break;
            case LogicOperation.NAND:
                ApplyNandLogic(outputInt, input1Int, input2Int);
                break;
        }
    }

    private void ApplyAndLogic (int output, int input1, int input2)
    {
        if (output == 1) {
            rendererCube1.material = materialForOne;
            rendererCube2.material = materialForOne;
        }
        else {
            rendererCube1.material = materialForZero;
            rendererCube2.material = materialForZero;
        }

        Debug.Log("Кубы изменили свой цвет");
    }

    private void ApplyOrLogic (int output, int input1, int input2)
    {
        if (output == 0) {
            rendererCube1.material = materialForZero;
            rendererCube2.material = materialForZero;
        }
        else {
            rendererCube1.material = materialForOne;
            rendererCube2.material = materialForOne;
        }
        Debug.Log("Кубы изменили свой цвет");
    }

    private void ApplyXorLogic (int output, int input1, int input2)
    {
        int xorResult = (input1 != input2) ? 1 : 0;

        if (output == xorResult) {
            if (xorResult == 0) {
                rendererCube1.material = materialForZero;
                rendererCube2.material = materialForZero;
            }
            else {
                rendererCube1.material = materialForOne;
                rendererCube2.material = materialForOne;
            }
            Debug.Log("Кубы изменили свой цвет");
        }
    }

    private void ApplyNandLogic (int output, int input1, int input2)
    {
        int nandResult = (input1 == 1 && input2 == 1) ? 0 : 1;

        if (output == nandResult) {
            if (nandResult == 0) {
                rendererCube1.material = materialForZero;
                rendererCube2.material = materialForZero;
            }
            else {
                rendererCube1.material = materialForOne;
                rendererCube2.material = materialForOne;
            }
        }
        Debug.Log("Кубы изменили свой цвет");
    }
}
