Attribute VB_Name = "TestCallBack"
Option Explicit

' TestCallback.bas

Public Type SYSTEMTIME
wYear As Integer
wMonth As Integer
wDayOfWeek As Integer
wDay As Integer
wHour As Integer
wMinute As Integer
wSecond As Integer
wMilliseconds As Integer
End Type

Public Declare Function FileTimeToSystemTime Lib "kernel32" (lpFileTime As Currency, lpSystemTime As SYSTEMTIME) As Long

Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByVal pDest As Long, ByVal pSrc As Long, ByVal ByteLen As Long)
'

Public Sub Main()

Dim exe As String

Dim obj As CVirtPackage

Set obj = New CVirtPackage

exe = App.Path & "\Cameyo-1.7.597.exe"

If obj.OpenPackage(exe) Then

obj.Enumerate AddressOf VirtFsEnumCallback, &H12345678

obj.ClosePackage

End If

End Sub

Public Sub ShowMsg(Text As String)
On Error Resume Next
Debug.Print Text

End Sub

Public Function VirtFsEnumCallback(ByVal Data As Long, ByVal FileName As Long, ByVal FileFlags As Long, ByVal CreationTime As Currency, ByVal LastAccessTime As Currency, ByVal LastWriteTime As Currency, ByVal ChangeTime As Currency, ByVal EndOfFile As Currency, ByVal FileAttributes As Long) As Long
On Error Resume Next

Dim message As String

ShowMsg "" & Hex(Data)
ShowMsg "FileName " & GetStringFromPtr(FileName)
If FileFlags And VIRT_FILE_FLAGS_ISFILE Then
ShowMsg "FileFlags " & FileFlags
ShowMsg GetSerialFileTime(CreationTime) & " CreationTime"
ShowMsg GetSerialFileTime(LastAccessTime) & " LastAccessTime"
ShowMsg GetSerialFileTime(LastWriteTime) & " LastWriteTime"
ShowMsg GetSerialFileTime(ChangeTime) & " ChangeTime"
ShowMsg "EndOfFile " & Format(EndOfFile * 10000, "#,###,###,###,##0") & " Bytes" 'will fail for large file sizes.
ShowMsg "FileAttributes " & FileAttributes
End If
ShowMsg ""

Dim CRLF As String
CRLF = Chr(13) & Chr(10)
message = "" & Hex(Data)
message = message & CRLF & "FileName " & GetStringFromPtr(FileName)
If FileFlags And VIRT_FILE_FLAGS_ISFILE Then
message = message & CRLF & "FileFlags " & FileFlags
message = message & CRLF & GetSerialFileTime(CreationTime) & " CreationTime"
message = message & CRLF & GetSerialFileTime(LastAccessTime) & " LastAccessTime"
message = message & CRLF & GetSerialFileTime(LastWriteTime) & " LastWriteTime"
message = message & CRLF & GetSerialFileTime(ChangeTime) & " ChangeTime"
message = message & CRLF & "EndOfFile " & Format(EndOfFile * 10000, "#,###,###,###,##0") & " Bytes"  'will fail for large file sizes.
message = message & CRLF & "FileAttributes " & FileAttributes
End If
message = message & ""

'MsgBox message

VirtFsEnumCallback = -1
End Function

Private Function GetSerialFileTime(FileTime As Currency) As String
Dim ret As Long
Dim tok As String
Dim sys As SYSTEMTIME
tok = vbNullString
If FileTime <> 0 Then
ret = FileTimeToSystemTime(FileTime, sys)
If ret <> 0 Then
tok = tok & Format(sys.wYear, "0000") & "-" & Format(sys.wMonth, "00") & "-" & Format(sys.wDay, "00")
tok = tok & " "
tok = tok & Format(sys.wHour, "00") & ":" & Format(sys.wMinute, "00") & ":" & Format(sys.wSecond, "00")
End If
End If
GetSerialFileTime = tok
End Function

Private Function GetStringFromPtr(Address As Long) As String
On Error Resume Next

Dim cnt As Long
Dim tok As String

cnt = 512

tok = String(512, Chr(0))

Call CopyMemory(StrPtr(tok), Address, cnt)

cnt = InStr(1, tok, Chr(0), vbBinaryCompare)

If cnt = 0 Then
tok = vbNullString
Else
tok = Left(tok, cnt - 1)
End If

GetStringFromPtr = tok
End Function
