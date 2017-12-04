# unity-cntk

Version history  <br />
[3-12-2017] Updated text and demo for Unity 2017.3 beta 11 with cntk 2.3 (for both CPU and GPU). Included a training demo in Unity using the new CNTK training API's. WARNING: i'm still in the process of validating the .net training implementation compared to python, so until then: validate often<br />
[14-5-2017] Initial commit for Unity 2017 beta 5 with cntk 2.0rc2 (for both CPU and GPU). Included a trained CNTK model file (done with python API's) and evaluation demo in Unity <br />

Unity 2017.3 (.net 4.6 support!) with CNTK 2.3 demo (now featuring both training and test API's)

See thread https://github.com/Microsoft/CNTK/issues/960 for some history 

Tested With/Requires
- Unity 2017, download https://unity3d.com/get-unity/download
- CNTK GPU 2.3 https://cntk.ai/dlwg-2.3.html (or see https://github.com/Microsoft/CNTK/releases for latest version)

# How to run this demo project
1) Download unity and cntk libs from links above
2) Copy the following DLL's to C:\Program Files\[Your unity version]\Editor    (eg. C:\Program Files\2017.3.0b11\Editor )

[Note. These assembly files are required for the demo project to run. For your project you might need other deserializers, Unity will alert when and which assemblies cannot be found. Copy the required dll's to the editor folder and the alert will disappear shortly.] 

(see https://github.com/Microsoft/CNTK/wiki/CNTK-Library-Evaluation-on-Windows#using-the-cntk-library-managed-api 
and http://stackoverflow.com/questions/36527985/dllnotfoundexception-in-while-building-desktop-unity-application-using-artoolkit)

  - Cntk.Core-VERSION.dll (eg. Cntk.Core-2.3.dll)
  - Cntk.Math-VERSION.dll
  - Cntk.Core.Managed-VERSION.dll
  - Cntk.Core.CSBinding-VERSION.dll
  - Cntk.Composite-VERSION.dll
  - Cntk.PerformanceProfiler-VERSION.dll
  - Cntk.Deserializers.TextFormat-VERSION.dll  
  - libiomp5md.dll
  - mkl_cntk_p.dll
  - cublas64_80.dll
  - cudart64_80.dll
  - cudnn64_5.dll
  - curand64_80.dll
  - cusparse64_80.dll
  - nvml.dll
  
3) Clone project. 

[Note. This demo project has been build for CNTK 2.3, and has the Cntk.Core.Managed-2.3.dll in the Assets\CNTK folder. If you wish to use another CNTK version please update this dll to that version accordingly. Restart Unity, as Unity does not unload the assembly.]

4) Run test.unity or train.unity scene to boot up Unity
5) Hit play to see results and checkout the .cs script to see how the API's work

For test result should look like
![test](https://cloud.githubusercontent.com/assets/6376127/26030649/fb312fd4-3858-11e7-8e1d-947ac4d7e965.png)

For training result should look like

![unity_2017-12-04_16-59-13](https://user-images.githubusercontent.com/6376127/33562094-9d60f030-d914-11e7-914a-f26728f0a0fe.png)

# Create New Unity-CNTK Project
1) Create new project and in File > Build settings > Player settings set scripting runtime to 4.6 (see image below), restart is required.

![unity_net46setting](https://user-images.githubusercontent.com/6376127/33528346-0ba4aeb2-d85f-11e7-81fa-9ed0d8113612.png)

2) Add the Cntk.Core.Managed-<VERSION>.dll to your Asset folder

[Note. For the managed dll to work, the native dll's need to be present for unity (see step 2 from "how to run this demo project")].
