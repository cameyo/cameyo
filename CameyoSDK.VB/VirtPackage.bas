Option Explicit

' www.cameyo.com

' VirtPackage.bas

'' Sandbox flags
Public Const SANDBOXFLAGS_PASSTHROUGH As Long = 1
Public Const SANDBOXFLAGS_COPY_ON_WRITE As Long = 2
Public Const SANDBOXFLAGS_FULL_ISOLATION As Long = 3

'' API return codes
Public Const APIRET_SUCCESS As Long = 0
Public Const APIRET_FAILURE As Long = 1
Public Const APIRET_VIRTFILES_DB_ERROR As Long = 2
Public Const APIRET_VIRTFILES_ZIP_ERROR As Long = 3
Public Const APIRET_NOT_FOUND As Long = 5
Public Const APIRET_INVALID_PARAMETER As Long = 6
Public Const APIRET_FILE_CREATE_ERROR As Long = 7
Public Const APIRET_PE_RESOURCE_ERROR As Long = 8
Public Const APIRET_MEMORY_ERROR As Long = 9
Public Const APIRET_COMMIT_ERROR As Long = 10
Public Const APIRET_VIRTREG_DEPLOY_ERROR As Long = 11
Public Const APIRET_OUTPUT_ERROR As Long = 12
Public Const APIRET_INSUFFICIENT_BUFFER As Long = 13
Public Const APIRET_LOADLIBRARY_ERROR As Long = 14
Public Const APIRET_VIRTFILES_INI_ERROR As Long = 15
Public Const APIRET_APP_NOT_DEPLOYED As Long = 16
Public Const APIRET_INSUFFICIENT_PRIVILEGES As Long = 17
Public Const APIRET_32_64_BIT_MISMATCH As Long = 18
Public Const APIRET_DOTNET_REQUIRED As Long = 19
Public Const APIRET_CANCELLED As Long = 20
Public Const APIRET_INJECTION_FAILED As Long = 21
Public Const APIRET_OLD_VERSION As Long = 22

'' VirtFilesDb flags
Public Const VIRT_FILE_FLAGS_ISFILE As Long = &H1 '' File or directory?
Public Const VIRT_FILE_FLAGS_DELETED As Long = &H2 '' Deleted by virtual app (NOT_FOUND)
Public Const VIRT_FILE_FLAGS_DEPLOY_UPON_PRELOAD As Long = &H8 '' Set upon first file opening
'' 0x10 is reserved
'
'' UI isolation constants:
Public Const ISOLATIONMODE_CUSTOM As Long = 0
Public Const ISOLATIONMODE_ISOLATED As Long = 1
Public Const ISOLATIONMODE_FULL_ACCESS As Long = 2
Public Const ISOLATIONMODE_DATA As Long = 3