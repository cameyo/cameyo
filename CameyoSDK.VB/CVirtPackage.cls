VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CVirtPackage"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

' www.cameyo.com

' CVirtPackage.cls

Private phPkg As Long
'
Private Const MAX_STRING_LENGTH As Long = 64 * 1024&
'
Private Declare Function PackageOpen Lib "PackagerDll.dll" (ByVal PackageExeFile As Long, Reserved As Long, ByRef phPkg As Long) As Long
Private Declare Function PackageCreate Lib "PackagerDll.dll" (ByVal AppID As Long, ByVal AppVirtDll As Long, ByVal LoaderExe As Long, ByRef phPkg As Long) As Long
Private Declare Sub PackageClose Lib "PackagerDll.dll" (ByVal phPkg As Long)
Private Declare Function PackageSave Lib "PackagerDll.dll" (ByVal phPkg As Long, ByVal OutFileName As Long) As Long
Private Declare Function PackageGetProperty Lib "PackagerDll.dll" (ByVal phPkg As Long, ByVal Name As Long, ByVal Value As Long, ByVal ValueLen As Long) As Long
Private Declare Function PackageSetProperty Lib "PackagerDll.dll" (ByVal phPkg As Long, ByVal Name As Long, ByVal Value As Long) As Long
Private Declare Function PackageSetIconFile Lib "PackagerDll.dll" (ByVal phPkg As Long, ByVal FileName As Long) As Long
'Private Declare Function PackageBuildStreaming Lib "PackagerDll.dll" (ByVal phPkg As Long, ByVal StreamingServerPath As Long, ByVal OutputDir As Long) As Long
Private Declare Function VirtFsEnum Lib "PackagerDll.dll" (ByVal phPkg As Long, ByVal Callback As Long, ByVal Data As Long) As Long
Private Declare Function VirtFsAdd Lib "PackagerDll.dll" (ByVal phPkg As Long, ByVal SrcFileName As Long, ByVal DestFileName As Long, ByVal bVariablizeName As Long) As Long
Private Declare Function VirtFsAddEmptyDir Lib "PackagerDll.dll" (ByVal phPkg As Long, ByVal DirName As Long, ByVal DestFileName As Long, ByVal bVariablizeName As Long) As Long
Private Declare Function VirtFsExtract Lib "PackagerDll.dll" (ByVal phPkg As Long, ByVal FileName As Long, ByVal TargetDir As Long) As Long
Private Declare Function VirtFsDelete Lib "PackagerDll.dll" (ByVal phPkg As Long, ByVal FileName As Long) As Long
Private Declare Function VirtFsGetFileFlags Lib "PackagerDll.dll" (ByVal phPkg As Long, ByVal FileName As Long, ByRef FileFlags As Long) As Long
Private Declare Function VirtFsSetFileFlags Lib "PackagerDll.dll" (ByVal phPkg As Long, ByVal FileName As Long, ByVal FileFlags As Long) As Long
Private Declare Function VirtRegGetWorkKey Lib "PackagerDll.dll" (ByVal phPkg As Long, ByVal WorkKey As Long, ByVal WorkKeyLen As Long) As Long
Private Declare Sub VirtRegSaveWorkKey Lib "PackagerDll.dll" (ByVal phPkg As Long)
Private Declare Function SandboxGetRegistryFlags Lib "PackagerDll.dll" (ByVal phPkg As Long, ByVal Path As Long, ByVal bVariablizeName As Long, ByRef SandboxFlags As Long) As Long
Private Declare Function SandboxGetFileFlags Lib "PackagerDll.dll" (ByVal phPkg As Long, ByVal Path As Long, ByVal bVariablizeName As Long, ByRef SandboxFlags As Long) As Long
Private Declare Function SandboxSetRegistryFlags Lib "PackagerDll.dll" (ByVal phPkg As Long, ByVal Path As Long, ByVal bVariablizeName As Long, ByVal SandboxFlags As Long) As Long
Private Declare Function SandboxSetFileFlags Lib "PackagerDll.dll" (ByVal phPkg As Long, ByVal Path As Long, ByVal bVariablizeName As Long, ByVal SandboxFlags As Long) As Long
'

Private Sub Class_Initialize()
On Error Resume Next
phPkg = 0
End Sub

Private Sub Class_Terminate()
On Error Resume Next
Call ClosePackage
End Sub

Public Function OpenPackage(ByVal PackageExeFile As String) As Boolean
OpenPackage = (APIRET_SUCCESS = PackageOpen(StrPtr(PackageExeFile), 0, phPkg))
End Function

Public Function CreatePackage(ByVal AppID As String, ByVal AppVirtDll As String, ByVal LoaderExe As String) As Boolean
''phPkg = 0
CreatePackage = (APIRET_SUCCESS = PackageCreate(StrPtr(AppID), StrPtr(AppVirtDll), StrPtr(LoaderExe), phPkg))
End Function

Public Sub ClosePackage()
If phPkg <> 0 Then
Call PackageClose(phPkg)
phPkg = 0
End If
End Sub

Public Function SavePackage(ByVal OutFileName As String) As Boolean
SavePackage = (APIRET_SUCCESS = PackageSave(phPkg, StrPtr(OutFileName)))
End Function

Public Function GetProperty(ByVal Name As String) As String
Dim cnt As Long
Dim ret As Long
Dim buf As String
buf = String(MAX_STRING_LENGTH, Chr(0))
ret = PackageGetProperty(phPkg, StrPtr(Name), StrPtr(buf), MAX_STRING_LENGTH)
If ret = APIRET_SUCCESS Then
cnt = InStr(1, buf, Chr(0))
If cnt = 0 Then
GetProperty = vbNullString
Else
GetProperty = Left(buf, cnt - 1)
End If
End If
End Function

Public Function SetProperty(ByVal Name As String, ByVal Value As String) As Boolean
SetProperty = (APIRET_SUCCESS = PackageSetProperty(phPkg, StrPtr(Name), StrPtr(Value)))
End Function

Public Function SetIcon(ByVal FileName As String) As Boolean
SetIcon = (APIRET_SUCCESS = PackageSetIconFile(phPkg, StrPtr(FileName)))
End Function

'Public Function BuildStreaming(ByVal StreamingServerPath As String, ByVal OutputDir As String) As Boolean
'BuildStreaming = (APIRET_SUCCESS = PackageBuildStreaming(phPkg, StrPtr(StreamingServerPath), StrPtr(OutputDir)))
'End Function

Public Function Enumerate(ByVal AddressOfCallback As Long, ByVal Data As Long) As Boolean
    Enumerate = (APIRET_SUCCESS = VirtFsEnum(phPkg, AddressOfCallback, Data))
End Function

Public Function AddFile(ByVal SrcFileName As String, ByVal DestFileName As String, Optional ByVal bVariablizeName As Boolean = False) As Boolean
    AddFile = (APIRET_SUCCESS = VirtFsAdd(phPkg, StrPtr(SrcFileName), StrPtr(DestFileName), bVariablizeName))
End Function

Public Function AddEmptyDir(ByVal DirName As String, ByVal DestFileName As String, Optional ByVal bVariablizeName As Boolean = False) As Boolean
    AddEmptyDir = (APIRET_SUCCESS = VirtFsAddEmptyDir(phPkg, StrPtr(DirName), StrPtr(DestFileName), bVariablizeName))
End Function

Public Function ExtractFile(ByVal FileName As String, ByVal TargetDir As String) As Boolean
    ExtractFile = (APIRET_SUCCESS = VirtFsExtract(phPkg, StrPtr(FileName), StrPtr(TargetDir)))
End Function

Public Function DeleteFile(ByVal FileName As String) As Boolean
    DeleteFile = (APIRET_SUCCESS = VirtFsDelete(phPkg, StrPtr(FileName)))
End Function

Public Function GetFileFlags(ByVal FileName As String) As Long
    Dim FileFlags As Long
    Call VirtFsGetFileFlags(phPkg, StrPtr(FileName), FileFlags)
    GetFileFlags = FileFlags
End Function

Public Function SetFileFlags(ByVal FileName As String, ByVal FileFlags As Long) As Boolean
    SetFileFlags = (APIRET_SUCCESS = VirtFsSetFileFlags(phPkg, StrPtr(FileName), FileFlags))
End Function

Public Function GetRegWorkKey() As String
    Dim cnt As Long
    Dim ret As Long
    Dim buf As String
buf = String(MAX_STRING_LENGTH, Chr(0))
    ret = VirtRegGetWorkKey(phPkg, StrPtr(buf), MAX_STRING_LENGTH)
    If ret = APIRET_SUCCESS Then
        cnt = InStr(1, buf, Chr(0))
        If cnt = 0 Then
            GetRegWorkKey = vbNullString
        Else
            GetRegWorkKey = Left(buf, cnt - 1)
        End If
    End If
End Function

Public Sub SaveRegWorkKey()
    Call VirtRegSaveWorkKey(phPkg)
End Sub

Public Function SetRegistrySandbox(ByVal Path As String, ByVal SandboxFlags As Long, Optional ByVal bVariablizeName As Boolean = False) As Boolean
    SetRegistrySandbox = (APIRET_SUCCESS = SandboxSetRegistryFlags(phPkg, StrPtr(Path), bVariablizeName, SandboxFlags))
End Function

Public Function GetRegistrySandbox(ByVal Path As String, Optional ByVal bVariablizeName As Boolean = False) As Long
    Dim SandboxFlags As Long
    Call SandboxGetRegistryFlags(phPkg, StrPtr(Path), bVariablizeName, SandboxFlags)
    GetRegistrySandbox = SandboxFlags
End Function

Public Function GetFileSandbox(ByVal Path As String, Optional ByVal bVariablizeName As Boolean = False) As Long
    Dim SandboxFlags As Long
    Call SandboxGetFileFlags(phPkg, StrPtr(Path), bVariablizeName, SandboxFlags)
    GetFileSandbox = SandboxFlags
End Function

Public Function SetFileSandbox(ByVal Path As String, ByVal SandboxFlags As Long, Optional ByVal bVariablizeName As Boolean = False) As Boolean
    SetFileSandbox = (APIRET_SUCCESS = SandboxSetFileFlags(phPkg, StrPtr(Path), bVariablizeName, SandboxFlags))
End Function

Public Function GetIsolationMode() As Long
    Dim ret As Long

    ' Isolation. Note: it is allowed to have no checkbox selected at all.
    If (GetFileSandbox("") = SANDBOXFLAGS_COPY_ON_WRITE And _
    GetRegistrySandbox("") = SANDBOXFLAGS_COPY_ON_WRITE And _
    GetFileSandbox("%Personal%") = SANDBOXFLAGS_PASSTHROUGH And _
    GetFileSandbox("%Desktop%") = SANDBOXFLAGS_PASSTHROUGH And _
    GetFileSandbox("UNC") = SANDBOXFLAGS_PASSTHROUGH) Then

        ret = ISOLATIONMODE_DATA

    ElseIf (GetFileSandbox("") = SANDBOXFLAGS_COPY_ON_WRITE And _
    GetRegistrySandbox("") = SANDBOXFLAGS_COPY_ON_WRITE) Then

        ret = ISOLATIONMODE_ISOLATED

    ElseIf (GetFileSandbox("") = SANDBOXFLAGS_PASSTHROUGH And _
    GetRegistrySandbox("") = SANDBOXFLAGS_PASSTHROUGH) Then

        ret = ISOLATIONMODE_FULL_ACCESS

    Else

        ret = ISOLATIONMODE_CUSTOM

    End If

    GetIsolationMode = ret
End Function

Public Sub SetIsolationMode(IsolationMode As Long)
    Dim SandboxMode As Long

    SandboxMode = 0

    If (IsolationMode = ISOLATIONMODE_ISOLATED Or IsolationMode = ISOLATIONMODE_DATA) Then
        SandboxMode = SANDBOXFLAGS_COPY_ON_WRITE
    ElseIf (IsolationMode = ISOLATIONMODE_FULL_ACCESS) Then
        SandboxMode = SANDBOXFLAGS_PASSTHROUGH
    End If

    If (SandboxMode <> 0) Then
        Call SetFileSandbox("", SandboxMode)
        Call SetRegistrySandbox("", SandboxMode)
    End If

    ' Do / undo special folders newly / previously set by Data Isolation mode
    If (IsolationMode = ISOLATIONMODE_DATA) Then
        Call SetProperty("DataMode", "TRUE")
        Call SetFileSandbox("%Personal%", SANDBOXFLAGS_PASSTHROUGH)
        Call SetFileSandbox("%Desktop%", SANDBOXFLAGS_PASSTHROUGH)
        Call SetFileSandbox("UNC", SANDBOXFLAGS_PASSTHROUGH)
    Else
        If (GetProperty("DataMode") = "TRUE") Then ' Need to undo special dirs changed by Data Isolation mode (as opposed to set by user)
            Call SetProperty("DataMode", "FALSE")
            Call SetFileSandbox("%Personal%", SandboxMode)
            Call SetFileSandbox("%Desktop%", SandboxMode)
            Call SetFileSandbox("UNC", SandboxMode)
        End If
    End If

End Sub



