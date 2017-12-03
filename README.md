# unity-cntk

Version history
[3-12-2017] Updated text and demo for Unity 2017.3 beta 11 with cntk 2.3 <br />
[14-5-2017] Initial commit for Unity 2017 beta 5 with cntk 2.0rc 2 <br />

Unity 2017.3 (.net 4.6 support!) with CNTK 2.3 demo (now featuring both training and test API's)

See thread https://github.com/Microsoft/CNTK/issues/960 for some history 

Tested With/Requires
- Unity 2017, download https://unity3d.com/get-unity/download
- CNTK GPU 2.3 https://cntk.ai/dlwg-2.3.html (or see https://github.com/Microsoft/CNTK/releases for latest version)

# How to run this demo project
1) Download unity and cntk libs from links above
2) Copy the following DLL's to C:\Program Files\[Your unity version]\Editor    (eg. C:\Program Files\2017.3.0b11\Editor )

(see https://github.com/Microsoft/CNTK/wiki/CNTK-Library-Evaluation-on-Windows#using-the-cntk-library-managed-api 
and http://stackoverflow.com/questions/36527985/dllnotfoundexception-in-while-building-desktop-unity-application-using-artoolkit)

  - Cntk.Core-<VERSION>.dll (eg. Cntk.Core-2.3.dll)
  - Cntk.Math-<VERSION>.dll
  - libiomp5md.dll
  - mkl_cntk_p.dll
  - Cntk.Core.Managed-<VERSION>.dll
  - Cntk.Core.CSBinding-<VERSION>.dll
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



# Create New Unity-CNTK Project
1) Create new project and in File > Build settings > Player settings set scripting runtime to 4.6 (see image below), restart is required.
2) Add the Cntk.Core.Managed-<VERSION>.dll to your Asset folder
![unity_net46setting](https://user-images.githubusercontent.com/6376127/33528346-0ba4aeb2-d85f-11e7-81fa-9ed0d8113612.png)

