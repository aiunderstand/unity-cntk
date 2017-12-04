using UnityEngine;
using CNTK;
using System;
using System.Linq;
using System.Collections.Generic;
using CNTK.CSTrainingExamples;
using System.Diagnostics;

public class train : MonoBehaviour {

    Function model;
    Trainer trainer;
    DeviceDescriptor device;
    string trainPath;
    string testPath;
    string modelPath;
    static int inputDim = 18;
    static int numOutputClasses = 9;
    MinibatchSource trainingData;
    MinibatchSource testData;
    IList<StreamConfiguration> streamConfigurations;
    Variable input;
    Variable label;
    Stopwatch sw = new Stopwatch();
    public bool useGPU = false;
    public int fullSweeps = 10;

    void Start () {
        Init();
        Train();
        SaveModel();
        Test();    
	}

    private void Init()
    {
        if (useGPU)
            device = DeviceDescriptor.GPUDevice(0);
        else
            device = DeviceDescriptor.CPUDevice;

        trainPath = System.IO.Path.Combine(Environment.CurrentDirectory, @"Assets\CNTK\Data\train.txt");
        testPath = System.IO.Path.Combine(Environment.CurrentDirectory, @"Assets\CNTK\Data\test.txt");
        modelPath = System.IO.Path.Combine(Environment.CurrentDirectory, @"Assets\CNTK\Models\mymodel.model");

        streamConfigurations = new StreamConfiguration[]
              { new StreamConfiguration("features", inputDim), new StreamConfiguration("labels", numOutputClasses) };

        trainingData = ReadData(trainPath, true);
        testData = ReadData(testPath, false);
    }
    
    private MinibatchSource ReadData(string filePath, bool isTraining)
    {
        return MinibatchSource.TextFormatMinibatchSource(filePath, streamConfigurations, isTraining ? MinibatchSource.InfinitelyRepeat : MinibatchSource.FullDataSweep, isTraining);
    }

    private Function CreateModel(Variable input)
    {
        Function h1 = TestHelper.Dense(input, inputDim,device, Activation.LeakyReLU);
        Function h2 = TestHelper.Dense(h1,numOutputClasses,device,Activation.Sigmoid);

        return h2;
    }

    public void Train()
    {
        sw.Start();
           
        //Instantiate the input and the label variables
        input = Variable.InputVariable(new int[] { inputDim }, DataType.Float);
        label = Variable.InputVariable(new int[] { numOutputClasses }, DataType.Float);
        var featureStreamInfo = trainingData.StreamInfo("features");
        var labelStreamInfo = trainingData.StreamInfo("labels");

        //Create the model function
        model = CreateModel(input);

        var loss = CNTKLib.BinaryCrossEntropy(model, label);
        var evalError = CNTKLib.ClassificationError(model, label);

        //Training config
        uint epoch_size = 0;
        uint minibatch_size = 64;
        int num_sweeps_to_train_with = fullSweeps;
        int num_samples_per_sweep = 950;
        int num_minibatches_to_train = (num_samples_per_sweep * num_sweeps_to_train_with);
        int updatePerMinibatches = 950;

        //Instantiate the learner object to drive the model training
        TrainingParameterScheduleDouble lr_per_sample = new TrainingParameterScheduleDouble(0.00003f, epoch_size);
        TrainingParameterScheduleDouble momentum_as_time_constant = new TrainingParameterScheduleDouble(1000f, minibatch_size);
        // IList<Learner> learner = new List<Learner>() { CNTKLib.AdamLearner(Helper.AsParameterVector(model.Parameters()),lr_per_sample, momentum_as_time_constant)}; //Bug? Model outputs only NaNs
        IList<Learner> learner = new List<Learner>() { Learner.SGDLearner(model.Parameters(), lr_per_sample) };

        //Instantiate the trainer
        trainer = Trainer.CreateTrainer(model, loss, evalError, learner);

        //Start training
        double aggregate_metric = 0;
        for (int minibatchCount = 0; minibatchCount < num_minibatches_to_train; minibatchCount++)
        {
            var minibatchData = trainingData.GetNextMinibatch(minibatch_size, device);
            var arguments = new Dictionary<Variable, MinibatchData>
                {
                    { input, minibatchData[featureStreamInfo] },
                    { label, minibatchData[labelStreamInfo] }
                };
            
            trainer.TrainMinibatch(arguments, device);
            TestHelper.PrintTrainingProgress(trainer, minibatchCount, updatePerMinibatches);
            var samples = trainer.PreviousMinibatchSampleCount();
            aggregate_metric += trainer.PreviousMinibatchEvaluationAverage() * samples;
        }

        sw.Stop();

        var train_error = aggregate_metric / (trainer.TotalNumberOfSamplesSeen());
        UnityEngine.Debug.Log(String.Format("Average training error: {0:P2} , in {1} hour {2} minutes and {3} seconds using {4}", train_error,sw.Elapsed.Hours, sw.Elapsed.Minutes, sw.Elapsed.Seconds, device.Type.ToString()));
    }

    private void SaveModel()
    {
        model.Save(modelPath);
    }

    public void Test()
    {
        TestHelper.ValidateModelWithMinibatchSource(modelPath, testData, "features", "labels", "", device, 817);
    }
}
