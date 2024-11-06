#include <Windows.h>

#include "minhook/MinHook.h"

BOOL(*oNtSetInformationProcess)
(
    HANDLE                    hProc,
    PROCESS_INFORMATION_CLASS processInformationClass,
    LPVOID                    processInformation,
    DWORD                     processInformationLength
    );

BOOL hkNtSetInformationProcess
(
    HANDLE                    hProc,
    PROCESS_INFORMATION_CLASS processInformationClass,
    LPVOID                    processInformation,
    DWORD                     processInformationLength
)
{
    if (processInformationClass == 0x1D)
    {
        return FALSE;
    }

    return oNtSetInformationProcess(
        hProc,
        processInformationClass,
        processInformation,
        processInformationLength
    );
}

BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
    if (fdwReason != DLL_PROCESS_ATTACH)
    {
        return false;
    }

    bool init = MH_Initialize() == MH_OK;

    bool hooked = MH_CreateHookApi(
        L"ntdll.dll",
        "NtSetInformationProcess",
        hkNtSetInformationProcess,
        (void**)&oNtSetInformationProcess
    ) == MH_OK;

    return init && hooked && (MH_EnableHook(MH_ALL_HOOKS) == MH_OK);
}