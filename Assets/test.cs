using UnityEngine;
using CNTK;
using System;
using System.Linq;
using System.Collections.Generic;

public class test : MonoBehaviour {

    Function model;

    // Use this for initialization
    void Start () {

        //load model
        string modelPath = System.IO.Path.Combine(Environment.CurrentDirectory, @"Assets\CNTK\model.dnn");
        model = Function.LoadModel(modelPath, DeviceDescriptor.GPUDevice(0));

        Evaluate();
	}

    public void Evaluate()
    {
        string inputOneHotString = "1 0 0 1 0 0 0 1 0 1 0 0 1 0 0 0 1 0";

        // Get input variable
        var inputVar = model.Arguments.Single();

        var seqData = new List<float>();
        string[] inputWords = inputOneHotString.Split(' ');

        foreach (var str in inputWords)
            seqData.Add(float.Parse(str));
        
        // Create input value using OneHot vector data.
        var inputValue = Value.CreateBatch(inputVar.Shape, seqData, DeviceDescriptor.GPUDevice(0));

        // Build input data map.
        var inputDataMap = new Dictionary<Variable, Value>();
        inputDataMap.Add(inputVar, inputValue);

        // Prepare output
        Variable outputVar = model.Output;

        // Create ouput data map. Using null as Value to indicate using system allocated memory.
        var outputDataMap = new Dictionary<Variable, Value>();
        outputDataMap.Add(outputVar, null);

        // Evaluate the model.
        model.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.GPUDevice(0));

        // Get output result
       
        Value outputVal = outputDataMap[outputVar];        
        var outputData = outputVal.GetDenseData<float>(outputVar);

        // output the result
        if (outputData.Count != 1)
            throw new ApplicationException("Only one sequence of slots is expected as output.");
        else
        {
            Debug.Log(String.Format("input: {0} output: {1} {2} {3} {4} {5} {6} {7} {8} {9}", inputOneHotString, outputData[0][0],
                                                                                                                 outputData[0][1],
                                                                                                                 outputData[0][2],
                                                                                                                 outputData[0][3],
                                                                                                                 outputData[0][4],
                                                                                                                 outputData[0][5],
                                                                                                                 outputData[0][6],
                                                                                                                 outputData[0][7],
                                                                                                                 outputData[0][8]));                
        }
    }
		
}
