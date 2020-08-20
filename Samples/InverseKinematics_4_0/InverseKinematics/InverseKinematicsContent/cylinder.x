xof 0303txt 0032
template Frame {
 <3d82ab46-62da-11cf-ab39-0020af71e433>
 [...]
}

template Matrix4x4 {
 <f6f23f45-7686-11cf-8f52-0040333594a3>
 array FLOAT matrix[16];
}

template FrameTransformMatrix {
 <f6f23f41-7686-11cf-8f52-0040333594a3>
 Matrix4x4 frameMatrix;
}

template Vector {
 <3d82ab5e-62da-11cf-ab39-0020af71e433>
 FLOAT x;
 FLOAT y;
 FLOAT z;
}

template MeshFace {
 <3d82ab5f-62da-11cf-ab39-0020af71e433>
 DWORD nFaceVertexIndices;
 array DWORD faceVertexIndices[nFaceVertexIndices];
}

template Mesh {
 <3d82ab44-62da-11cf-ab39-0020af71e433>
 DWORD nVertices;
 array Vector vertices[nVertices];
 DWORD nFaces;
 array MeshFace faces[nFaces];
 [...]
}

template MeshNormals {
 <f6f23f43-7686-11cf-8f52-0040333594a3>
 DWORD nNormals;
 array Vector normals[nNormals];
 DWORD nFaceNormals;
 array MeshFace faceNormals[nFaceNormals];
}

template ColorRGBA {
 <35ff44e0-6c7c-11cf-8f52-0040333594a3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
 FLOAT alpha;
}

template ColorRGB {
 <d3e16e81-7835-11cf-8f52-0040333594a3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
}

template Material {
 <3d82ab4d-62da-11cf-ab39-0020af71e433>
 ColorRGBA faceColor;
 FLOAT power;
 ColorRGB specularColor;
 ColorRGB emissiveColor;
 [...]
}

template MeshMaterialList {
 <f6f23f42-7686-11cf-8f52-0040333594a3>
 DWORD nMaterials;
 DWORD nFaceIndexes;
 array DWORD faceIndexes[nFaceIndexes];
 [Material <3d82ab4d-62da-11cf-ab39-0020af71e433>]
}


Frame Scene_Root {
 

 Frame cylinder {
  

  FrameTransformMatrix {
   1.000000,0.000000,0.000000,0.000000,0.000000,1.000000,0.000000,0.000000,0.000000,0.000000,1.000000,0.000000,0.000000,0.000000,0.000000,1.000000;;
  }

  Mesh cylinder_obj {
   97;
   0.000000;-2.000000;0.000000;,
   0.000000;2.000000;0.000000;,
   -0.500000;-2.000000;0.000000;,
   -1.000000;-2.000000;0.000000;,
   -1.000000;0.000000;0.000000;,
   -1.000000;2.000000;0.000000;,
   -0.500000;2.000000;0.000000;,
   -0.472909;-2.000000;0.162350;,
   -0.945817;-2.000000;0.324699;,
   -0.945817;0.000000;0.324699;,
   -0.945817;2.000000;0.324699;,
   -0.472909;2.000000;0.162350;,
   -0.394570;-2.000000;0.307106;,
   -0.789141;-2.000000;0.614213;,
   -0.789141;0.000000;0.614213;,
   -0.789141;2.000000;0.614213;,
   -0.394570;2.000000;0.307106;,
   -0.273474;-2.000000;0.418583;,
   -0.546948;-2.000000;0.837166;,
   -0.546948;0.000000;0.837166;,
   -0.546948;2.000000;0.837166;,
   -0.273474;2.000000;0.418583;,
   -0.122743;-2.000000;0.484700;,
   -0.245485;-2.000000;0.969400;,
   -0.245485;0.000000;0.969400;,
   -0.245485;2.000000;0.969400;,
   -0.122743;2.000000;0.484700;,
   0.041290;-2.000000;0.498292;,
   0.082579;-2.000000;0.996584;,
   0.082579;0.000000;0.996584;,
   0.082579;2.000000;0.996584;,
   0.041290;2.000000;0.498292;,
   0.200848;-2.000000;0.457887;,
   0.401695;-2.000000;0.915773;,
   0.401695;0.000000;0.915773;,
   0.401695;2.000000;0.915773;,
   0.200848;2.000000;0.457887;,
   0.338641;-2.000000;0.367862;,
   0.677282;-2.000000;0.735724;,
   0.677282;0.000000;0.735724;,
   0.677282;2.000000;0.735724;,
   0.338641;2.000000;0.367862;,
   0.439737;-2.000000;0.237974;,
   0.879474;-2.000000;0.475947;,
   0.879474;0.000000;0.475947;,
   0.879474;2.000000;0.475947;,
   0.439737;2.000000;0.237974;,
   0.493181;-2.000000;0.082297;,
   0.986361;-2.000000;0.164595;,
   0.986361;0.000000;0.164595;,
   0.986361;2.000000;0.164595;,
   0.493181;2.000000;0.082297;,
   0.493181;-2.000000;-0.082297;,
   0.986361;-2.000000;-0.164595;,
   0.986361;0.000000;-0.164595;,
   0.986361;2.000000;-0.164595;,
   0.493181;2.000000;-0.082297;,
   0.439737;-2.000000;-0.237974;,
   0.879474;-2.000000;-0.475947;,
   0.879474;0.000000;-0.475947;,
   0.879474;2.000000;-0.475947;,
   0.439737;2.000000;-0.237974;,
   0.338641;-2.000000;-0.367862;,
   0.677282;-2.000000;-0.735724;,
   0.677282;0.000000;-0.735724;,
   0.677282;2.000000;-0.735724;,
   0.338641;2.000000;-0.367862;,
   0.200848;-2.000000;-0.457887;,
   0.401695;-2.000000;-0.915773;,
   0.401695;0.000000;-0.915773;,
   0.401695;2.000000;-0.915773;,
   0.200848;2.000000;-0.457887;,
   0.041290;-2.000000;-0.498292;,
   0.082579;-2.000000;-0.996584;,
   0.082579;0.000000;-0.996584;,
   0.082579;2.000000;-0.996584;,
   0.041290;2.000000;-0.498292;,
   -0.122743;-2.000000;-0.484700;,
   -0.245485;-2.000000;-0.969400;,
   -0.245485;0.000000;-0.969400;,
   -0.245485;2.000000;-0.969400;,
   -0.122743;2.000000;-0.484700;,
   -0.273474;-2.000000;-0.418583;,
   -0.546948;-2.000000;-0.837166;,
   -0.546948;0.000000;-0.837166;,
   -0.546948;2.000000;-0.837166;,
   -0.273474;2.000000;-0.418583;,
   -0.394570;-2.000000;-0.307106;,
   -0.789141;-2.000000;-0.614213;,
   -0.789141;0.000000;-0.614213;,
   -0.789141;2.000000;-0.614213;,
   -0.394570;2.000000;-0.307106;,
   -0.472909;-2.000000;-0.162350;,
   -0.945817;-2.000000;-0.324699;,
   -0.945817;0.000000;-0.324699;,
   -0.945817;2.000000;-0.324699;,
   -0.472909;2.000000;-0.162350;;
   114;
   3;7,2,0;,
   4;2,7,8,3;,
   4;3,8,9,4;,
   4;4,9,10,5;,
   4;5,10,11,6;,
   3;6,11,1;,
   3;12,7,0;,
   4;7,12,13,8;,
   4;8,13,14,9;,
   4;9,14,15,10;,
   4;10,15,16,11;,
   3;11,16,1;,
   3;17,12,0;,
   4;12,17,18,13;,
   4;13,18,19,14;,
   4;14,19,20,15;,
   4;15,20,21,16;,
   3;16,21,1;,
   3;22,17,0;,
   4;17,22,23,18;,
   4;18,23,24,19;,
   4;19,24,25,20;,
   4;20,25,26,21;,
   3;21,26,1;,
   3;27,22,0;,
   4;22,27,28,23;,
   4;23,28,29,24;,
   4;24,29,30,25;,
   4;25,30,31,26;,
   3;26,31,1;,
   3;32,27,0;,
   4;27,32,33,28;,
   4;28,33,34,29;,
   4;29,34,35,30;,
   4;30,35,36,31;,
   3;31,36,1;,
   3;37,32,0;,
   4;32,37,38,33;,
   4;33,38,39,34;,
   4;34,39,40,35;,
   4;35,40,41,36;,
   3;36,41,1;,
   3;42,37,0;,
   4;37,42,43,38;,
   4;38,43,44,39;,
   4;39,44,45,40;,
   4;40,45,46,41;,
   3;41,46,1;,
   3;47,42,0;,
   4;42,47,48,43;,
   4;43,48,49,44;,
   4;44,49,50,45;,
   4;45,50,51,46;,
   3;46,51,1;,
   3;52,47,0;,
   4;47,52,53,48;,
   4;48,53,54,49;,
   4;49,54,55,50;,
   4;50,55,56,51;,
   3;51,56,1;,
   3;57,52,0;,
   4;52,57,58,53;,
   4;53,58,59,54;,
   4;54,59,60,55;,
   4;55,60,61,56;,
   3;56,61,1;,
   3;62,57,0;,
   4;57,62,63,58;,
   4;58,63,64,59;,
   4;59,64,65,60;,
   4;60,65,66,61;,
   3;61,66,1;,
   3;67,62,0;,
   4;62,67,68,63;,
   4;63,68,69,64;,
   4;64,69,70,65;,
   4;65,70,71,66;,
   3;66,71,1;,
   3;72,67,0;,
   4;67,72,73,68;,
   4;68,73,74,69;,
   4;69,74,75,70;,
   4;70,75,76,71;,
   3;71,76,1;,
   3;77,72,0;,
   4;72,77,78,73;,
   4;73,78,79,74;,
   4;74,79,80,75;,
   4;75,80,81,76;,
   3;76,81,1;,
   3;82,77,0;,
   4;77,82,83,78;,
   4;78,83,84,79;,
   4;79,84,85,80;,
   4;80,85,86,81;,
   3;81,86,1;,
   3;87,82,0;,
   4;82,87,88,83;,
   4;83,88,89,84;,
   4;84,89,90,85;,
   4;85,90,91,86;,
   3;86,91,1;,
   3;92,87,0;,
   4;87,92,93,88;,
   4;88,93,94,89;,
   4;89,94,95,90;,
   4;90,95,96,91;,
   3;91,96,1;,
   3;2,92,0;,
   4;92,2,3,93;,
   4;93,3,4,94;,
   4;94,4,5,95;,
   4;95,5,6,96;,
   3;96,6,1;;

   MeshNormals {
    418;
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    -1.000000;0.000000;0.000000;,
    -0.945817;0.000000;0.324699;,
    -0.945817;0.000000;0.324699;,
    -1.000000;0.000000;0.000000;,
    -1.000000;0.000000;0.000000;,
    -0.945817;0.000000;0.324699;,
    -0.945817;0.000000;0.324699;,
    -1.000000;0.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    -0.945817;0.000000;0.324699;,
    -0.789141;0.000000;0.614213;,
    -0.789141;0.000000;0.614213;,
    -0.945817;0.000000;0.324699;,
    -0.945817;0.000000;0.324699;,
    -0.789141;0.000000;0.614213;,
    -0.789141;0.000000;0.614213;,
    -0.945817;0.000000;0.324699;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    -0.789141;0.000000;0.614213;,
    -0.546948;0.000000;0.837166;,
    -0.546948;0.000000;0.837166;,
    -0.789141;0.000000;0.614213;,
    -0.789141;0.000000;0.614213;,
    -0.546948;0.000000;0.837166;,
    -0.546948;0.000000;0.837166;,
    -0.789141;0.000000;0.614213;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    -0.546948;0.000000;0.837166;,
    -0.245485;0.000000;0.969400;,
    -0.245485;0.000000;0.969400;,
    -0.546948;0.000000;0.837166;,
    -0.546948;0.000000;0.837166;,
    -0.245485;0.000000;0.969400;,
    -0.245485;0.000000;0.969400;,
    -0.546948;0.000000;0.837166;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    -0.245485;0.000000;0.969400;,
    0.082579;0.000000;0.996584;,
    0.082579;0.000000;0.996584;,
    -0.245485;0.000000;0.969400;,
    -0.245485;0.000000;0.969400;,
    0.082579;0.000000;0.996584;,
    0.082579;0.000000;0.996584;,
    -0.245485;0.000000;0.969400;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.082579;0.000000;0.996584;,
    0.401695;0.000000;0.915773;,
    0.401695;0.000000;0.915773;,
    0.082579;0.000000;0.996584;,
    0.082579;0.000000;0.996584;,
    0.401695;0.000000;0.915773;,
    0.401695;0.000000;0.915773;,
    0.082579;0.000000;0.996584;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.401695;0.000000;0.915773;,
    0.677282;0.000000;0.735724;,
    0.677282;0.000000;0.735724;,
    0.401695;0.000000;0.915773;,
    0.401695;0.000000;0.915773;,
    0.677282;0.000000;0.735724;,
    0.677282;0.000000;0.735724;,
    0.401695;0.000000;0.915773;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.677282;0.000000;0.735724;,
    0.879474;0.000000;0.475947;,
    0.879474;0.000000;0.475947;,
    0.677282;0.000000;0.735724;,
    0.677282;0.000000;0.735724;,
    0.879474;0.000000;0.475947;,
    0.879474;0.000000;0.475947;,
    0.677282;0.000000;0.735724;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.879474;0.000000;0.475947;,
    0.986361;0.000000;0.164595;,
    0.986361;0.000000;0.164595;,
    0.879474;0.000000;0.475947;,
    0.879474;0.000000;0.475947;,
    0.986361;0.000000;0.164595;,
    0.986361;0.000000;0.164595;,
    0.879474;0.000000;0.475947;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.986361;0.000000;0.164595;,
    0.986361;0.000000;-0.164595;,
    0.986361;0.000000;-0.164595;,
    0.986361;0.000000;0.164595;,
    0.986361;0.000000;0.164595;,
    0.986361;0.000000;-0.164595;,
    0.986361;0.000000;-0.164595;,
    0.986361;0.000000;0.164595;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.986361;0.000000;-0.164595;,
    0.879474;0.000000;-0.475947;,
    0.879474;0.000000;-0.475947;,
    0.986361;0.000000;-0.164595;,
    0.986361;0.000000;-0.164595;,
    0.879474;0.000000;-0.475947;,
    0.879474;0.000000;-0.475947;,
    0.986361;0.000000;-0.164595;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.879474;0.000000;-0.475947;,
    0.677282;0.000000;-0.735724;,
    0.677282;0.000000;-0.735724;,
    0.879474;0.000000;-0.475947;,
    0.879474;0.000000;-0.475947;,
    0.677282;0.000000;-0.735724;,
    0.677282;0.000000;-0.735724;,
    0.879474;0.000000;-0.475947;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.677282;0.000000;-0.735724;,
    0.401695;0.000000;-0.915773;,
    0.401695;0.000000;-0.915773;,
    0.677282;0.000000;-0.735724;,
    0.677282;0.000000;-0.735724;,
    0.401695;0.000000;-0.915773;,
    0.401695;0.000000;-0.915773;,
    0.677282;0.000000;-0.735724;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.401695;0.000000;-0.915773;,
    0.082579;0.000000;-0.996584;,
    0.082579;0.000000;-0.996584;,
    0.401695;0.000000;-0.915773;,
    0.401695;0.000000;-0.915773;,
    0.082579;0.000000;-0.996584;,
    0.082579;0.000000;-0.996584;,
    0.401695;0.000000;-0.915773;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.082579;0.000000;-0.996584;,
    -0.245485;0.000000;-0.969400;,
    -0.245485;0.000000;-0.969400;,
    0.082579;0.000000;-0.996584;,
    0.082579;0.000000;-0.996584;,
    -0.245485;0.000000;-0.969400;,
    -0.245485;0.000000;-0.969400;,
    0.082579;0.000000;-0.996584;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    -0.245485;0.000000;-0.969400;,
    -0.546948;0.000000;-0.837166;,
    -0.546948;0.000000;-0.837166;,
    -0.245485;0.000000;-0.969400;,
    -0.245485;0.000000;-0.969400;,
    -0.546948;0.000000;-0.837166;,
    -0.546948;0.000000;-0.837166;,
    -0.245485;0.000000;-0.969400;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    -0.546948;0.000000;-0.837166;,
    -0.789141;0.000000;-0.614213;,
    -0.789141;0.000000;-0.614213;,
    -0.546948;0.000000;-0.837166;,
    -0.546948;0.000000;-0.837166;,
    -0.789141;0.000000;-0.614213;,
    -0.789141;0.000000;-0.614213;,
    -0.546948;0.000000;-0.837166;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    -0.789141;0.000000;-0.614213;,
    -0.945817;0.000000;-0.324699;,
    -0.945817;0.000000;-0.324699;,
    -0.789141;0.000000;-0.614213;,
    -0.789141;0.000000;-0.614213;,
    -0.945817;0.000000;-0.324699;,
    -0.945817;0.000000;-0.324699;,
    -0.789141;0.000000;-0.614213;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;0.000000;,
    -0.945817;0.000000;-0.324699;,
    -1.000000;0.000000;0.000000;,
    -1.000000;0.000000;0.000000;,
    -0.945817;0.000000;-0.324699;,
    -0.945817;0.000000;-0.324699;,
    -1.000000;0.000000;0.000000;,
    -1.000000;0.000000;0.000000;,
    -0.945817;0.000000;-0.324699;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;,
    0.000000;1.000000;0.000000;;
    114;
    3;0,1,2;,
    4;3,4,5,6;,
    4;7,8,9,10;,
    4;11,12,13,14;,
    4;15,16,17,18;,
    3;19,20,21;,
    3;22,23,24;,
    4;25,26,27,28;,
    4;29,30,31,32;,
    4;33,34,35,36;,
    4;37,38,39,40;,
    3;41,42,43;,
    3;44,45,46;,
    4;47,48,49,50;,
    4;51,52,53,54;,
    4;55,56,57,58;,
    4;59,60,61,62;,
    3;63,64,65;,
    3;66,67,68;,
    4;69,70,71,72;,
    4;73,74,75,76;,
    4;77,78,79,80;,
    4;81,82,83,84;,
    3;85,86,87;,
    3;88,89,90;,
    4;91,92,93,94;,
    4;95,96,97,98;,
    4;99,100,101,102;,
    4;103,104,105,106;,
    3;107,108,109;,
    3;110,111,112;,
    4;113,114,115,116;,
    4;117,118,119,120;,
    4;121,122,123,124;,
    4;125,126,127,128;,
    3;129,130,131;,
    3;132,133,134;,
    4;135,136,137,138;,
    4;139,140,141,142;,
    4;143,144,145,146;,
    4;147,148,149,150;,
    3;151,152,153;,
    3;154,155,156;,
    4;157,158,159,160;,
    4;161,162,163,164;,
    4;165,166,167,168;,
    4;169,170,171,172;,
    3;173,174,175;,
    3;176,177,178;,
    4;179,180,181,182;,
    4;183,184,185,186;,
    4;187,188,189,190;,
    4;191,192,193,194;,
    3;195,196,197;,
    3;198,199,200;,
    4;201,202,203,204;,
    4;205,206,207,208;,
    4;209,210,211,212;,
    4;213,214,215,216;,
    3;217,218,219;,
    3;220,221,222;,
    4;223,224,225,226;,
    4;227,228,229,230;,
    4;231,232,233,234;,
    4;235,236,237,238;,
    3;239,240,241;,
    3;242,243,244;,
    4;245,246,247,248;,
    4;249,250,251,252;,
    4;253,254,255,256;,
    4;257,258,259,260;,
    3;261,262,263;,
    3;264,265,266;,
    4;267,268,269,270;,
    4;271,272,273,274;,
    4;275,276,277,278;,
    4;279,280,281,282;,
    3;283,284,285;,
    3;286,287,288;,
    4;289,290,291,292;,
    4;293,294,295,296;,
    4;297,298,299,300;,
    4;301,302,303,304;,
    3;305,306,307;,
    3;308,309,310;,
    4;311,312,313,314;,
    4;315,316,317,318;,
    4;319,320,321,322;,
    4;323,324,325,326;,
    3;327,328,329;,
    3;330,331,332;,
    4;333,334,335,336;,
    4;337,338,339,340;,
    4;341,342,343,344;,
    4;345,346,347,348;,
    3;349,350,351;,
    3;352,353,354;,
    4;355,356,357,358;,
    4;359,360,361,362;,
    4;363,364,365,366;,
    4;367,368,369,370;,
    3;371,372,373;,
    3;374,375,376;,
    4;377,378,379,380;,
    4;381,382,383,384;,
    4;385,386,387,388;,
    4;389,390,391,392;,
    3;393,394,395;,
    3;396,397,398;,
    4;399,400,401,402;,
    4;403,404,405,406;,
    4;407,408,409,410;,
    4;411,412,413,414;,
    3;415,416,417;;
   }

   MeshMaterialList {
    1;
    114;
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0;

    Material DefaultLib_Scene_Material {
     0.700000;0.700000;0.700000;1.000000;;
     50.000000;
     1.000000;1.000000;1.000000;;
     0.000000;0.000000;0.000000;;
    }
   }
  }
 }
}