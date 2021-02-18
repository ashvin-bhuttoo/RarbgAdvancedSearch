############################################################################################
#      NSIS Installation Script created by NSIS Quick Setup Script Generator v1.09.18
#               Entirely Edited with NullSoft Scriptable Installation System                
#              by Vlasis K. Barkas aka Red Wine red_wine@freemail.gr Sep 2006               
############################################################################################

!define APP_NAME "Rarbg Advanced Search"
!define COMP_NAME "Unkn0wn"
!define WEB_SITE "https://github.com/ashvin-bhuttoo/RarbgAdvancedSearch"
!define VERSION "1.1.0.0"
!define COPYRIGHT "Copyright � 2021"
!define DESCRIPTION "RarbgAdvancedSearch"
!define INSTALLER_NAME ".\Release\RarbgAdvancedSearchSetup_nsis.exe"
!define MAIN_APP_EXE "RarbgAdvancedSearch.exe"
!define INSTALL_TYPE "SetShellVarContext all"
!define REG_ROOT "HKLM"
!define REG_APP_PATH "Software\Microsoft\Windows\CurrentVersion\App Paths\${MAIN_APP_EXE}"
!define UNINSTALL_PATH "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}"

######################################################################

VIProductVersion  "${VERSION}"
VIAddVersionKey "ProductName"  "${APP_NAME}"
VIAddVersionKey "CompanyName"  "${COMP_NAME}"
VIAddVersionKey "LegalCopyright"  "${COPYRIGHT}"
VIAddVersionKey "FileDescription"  "${DESCRIPTION}"
VIAddVersionKey "FileVersion"  "${VERSION}"

######################################################################

SetCompressor /SOLID Lzma
Name "${APP_NAME}"
Caption "${APP_NAME}"
OutFile "${INSTALLER_NAME}"
BrandingText "${APP_NAME}"
XPStyle on
InstallDirRegKey "${REG_ROOT}" "${REG_APP_PATH}" ""
InstallDir "$PROGRAMFILES32\Unkn0wn\RarbgAdvancedSearch"

######################################################################

!include "MUI.nsh"

!define MUI_ABORTWARNING
!define MUI_UNABORTWARNING

!insertmacro MUI_PAGE_WELCOME

!ifdef LICENSE_TXT
!insertmacro MUI_PAGE_LICENSE "${LICENSE_TXT}"
!endif

!ifdef REG_START_MENU
!define MUI_STARTMENUPAGE_NODISABLE
!define MUI_STARTMENUPAGE_DEFAULTFOLDER "RarbgAdvancedSearch"
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "${REG_ROOT}"
!define MUI_STARTMENUPAGE_REGISTRY_KEY "${UNINSTALL_PATH}"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "${REG_START_MENU}"
!insertmacro MUI_PAGE_STARTMENU Application $SM_Folder
!endif

!insertmacro MUI_PAGE_INSTFILES

!insertmacro MUI_UNPAGE_CONFIRM

!insertmacro MUI_UNPAGE_INSTFILES

!insertmacro MUI_UNPAGE_FINISH

!insertmacro MUI_LANGUAGE "English"

######################################################################

Section -MainProgram
    ${INSTALL_TYPE}
    SetOverwrite ifnewer
    SetOutPath "$INSTDIR"
    File "..\RarbgAdvancedSearch\bin\Release\HtmlAgilityPack.dll"
    File "..\RarbgAdvancedSearch\bin\Release\MegaApiClient.dll"
    File "..\RarbgAdvancedSearch\bin\Release\Newtonsoft.Json.dll"
    File "..\RarbgAdvancedSearch\bin\Release\Octokit.dll"
    File "..\RarbgAdvancedSearch\bin\Release\RarbgAdvancedSearch.exe"
    File "..\RarbgAdvancedSearch\bin\Release\RarbgAdvancedSearch.exe.config"
    Exec "$INSTDIR\${MAIN_APP_EXE} INSTALLER"
SectionEnd

######################################################################

Section -Icons_Reg
    SetOutPath "$INSTDIR"
    WriteUninstaller "$INSTDIR\uninstall.exe"

    !ifdef REG_START_MENU
    !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    CreateDirectory "$SMPROGRAMS\$SM_Folder"
    CreateShortCut "$SMPROGRAMS\$SM_Folder\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
    CreateShortCut "$DESKTOP\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
    !ifdef WEB_SITE
    WriteIniStr "$INSTDIR\${APP_NAME} website.url" "InternetShortcut" "URL" "${WEB_SITE}"
    CreateShortCut "$SMPROGRAMS\$SM_Folder\${APP_NAME} Website.lnk" "$INSTDIR\${APP_NAME} website.url"
    !endif
    !insertmacro MUI_STARTMENU_WRITE_END
    !endif

    !ifndef REG_START_MENU
    CreateDirectory "$SMPROGRAMS\RarbgAdvancedSearch"
    CreateShortCut "$SMPROGRAMS\RarbgAdvancedSearch\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
    CreateShortCut "$DESKTOP\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
    !ifdef WEB_SITE
    WriteIniStr "$INSTDIR\${APP_NAME} website.url" "InternetShortcut" "URL" "${WEB_SITE}"
    CreateShortCut "$SMPROGRAMS\RarbgAdvancedSearch\${APP_NAME} Website.lnk" "$INSTDIR\${APP_NAME} website.url"
    !endif
    !endif

    WriteRegStr ${REG_ROOT} "${REG_APP_PATH}" "" "$INSTDIR\${MAIN_APP_EXE}"
    WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayName" "${APP_NAME}"
    WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "UninstallString" "$INSTDIR\uninstall.exe"
    WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayIcon" "$INSTDIR\${MAIN_APP_EXE}"
    WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayVersion" "${VERSION}"
    WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "Publisher" "${COMP_NAME}"

    !ifdef WEB_SITE
    WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "URLInfoAbout" "${WEB_SITE}"
    !endif
SectionEnd

######################################################################

Section Uninstall
    ${INSTALL_TYPE}
    Delete "$INSTDIR\HtmlAgilityPack.dll"
    Delete "$INSTDIR\MegaApiClient.dll"
    Delete "$INSTDIR\Newtonsoft.Json.dll"
    Delete "$INSTDIR\Octokit.dll"
    Delete "$INSTDIR\RarbgAdvancedSearch.exe"
    Delete "$INSTDIR\RarbgAdvancedSearch.exe.config"
    Delete "$INSTDIR\uninstall.exe"
    !ifdef WEB_SITE
    Delete "$INSTDIR\${APP_NAME} website.url"
    !endif

    RmDir "$INSTDIR"

    !ifdef REG_START_MENU
    !insertmacro MUI_STARTMENU_GETFOLDER "Application" $SM_Folder
    Delete "$SMPROGRAMS\$SM_Folder\${APP_NAME}.lnk"
    !ifdef WEB_SITE
    Delete "$SMPROGRAMS\$SM_Folder\${APP_NAME} Website.lnk"
    !endif
    Delete "$DESKTOP\${APP_NAME}.lnk"

    RmDir "$SMPROGRAMS\$SM_Folder"
    !endif

    !ifndef REG_START_MENU
    Delete "$SMPROGRAMS\RarbgAdvancedSearch\${APP_NAME}.lnk"
    !ifdef WEB_SITE
    Delete "$SMPROGRAMS\RarbgAdvancedSearch\${APP_NAME} Website.lnk"
    !endif
    Delete "$DESKTOP\${APP_NAME}.lnk"

    RmDir "$SMPROGRAMS\RarbgAdvancedSearch"
    !endif

    DeleteRegKey ${REG_ROOT} "${REG_APP_PATH}"
    DeleteRegKey ${REG_ROOT} "${UNINSTALL_PATH}"
SectionEnd

######################################################################
