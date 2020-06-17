## FbxViewer
Fbx SDK를 Wrap한 dll을 통해 Fbx 파일을 로딩합니다.
파싱된 데이터는 Model 데이터로 변환하여 OpenTK를 통해 렌더링 됩니다.


  #### 제작중
  > ###### 현재 Form 제작과 각종 라이브러리 연결 - 완료
  > ###### GLSL 연결 - 완료
  > ###### FBX 데이터 파싱 - 완료
  > ###### Model Data 구현 - 완료
  >     KeyFrame
  >     Bone
  >     Animation
  >     Mesh
  >     Subset
  >     Material
  >
  > ###### FBX -> Json - 완료
  >     Json을 거치지 않고 다이렉트로 데이터 설정을 할까 고민중
  > ###### 렌더러 제작
  >     버그 수정중
  > ###### 코드정리


  #### 외부 참조 라이브러리
  >
  >   ###### FreeImage.dll / FreeImageNet.dll
  >     이미지 파일(png, tga, jpg)을 읽어 Bitmap으로 변환시킬 때 사용합니다.
  >
  >   ###### Newtonsoft.json.dll
  >     json 파일을 읽거나 씁니다.
  >
  >   ###### OpenTK.dll / OpenTK.Compatibility.dll / OpenTK.GLControl.dll
  >     C#에서도 OpenGL을 사용할 수 있게 해줍니다.
  >     GLControl을 제공합니다.
  >
  >   ###### WeifenLuo.WinFormUI.Docking.dll
  >     창을 Docking할 수 있는 컨트롤을 제공합니다.
  >
  >   ###### CliFbx.dll
  >     자체 제작 dll입니다.
  >     FBX SDK가 C++ 전용이라 c++/cli로 wrapper(dll)를 제작하였습니다.
  >
  >     public bool Import(string filename);
  >     public void Export(string name, string filename);
  >     


  #### GLSL
  >   ###### version 430 문법을 사용합니다.
  >     Diffuse.vs / Diffuse.fs
  >
  >     Model.vs / Model.fs
  >       animated Subroutine을 적용하여 외부(project)에서 animation 여부에 루틴을 선택할 수 있습니다.
  >
  >     Outline.vs / Outline.fs
  >       모델의 외곽선을 렌더링합니다.
  >
  >     Picking.vs / Picking.fs
  >       각 GameObject의 유일키를 이용하여 픽셀단위의 피킹을 가능하게 만들 수 있습니다.
  >
  >     Shadow.vs / Shadow.fs
  >       평면 그림자

  #### VBO
  >
  > 
