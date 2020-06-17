## FbxViewer
Fbx 모델 파일을 읽어와 창에 렌더링하는 프로젝트입니다.


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

