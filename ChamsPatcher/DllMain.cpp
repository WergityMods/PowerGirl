#include <cstdint>
#include <Windows.h>

uintptr_t GetCheatBase()
{
    char path[MAX_PATH];
    GetCurrentDirectory(MAX_PATH, path);
    strcat_s(path, "\\opengl32.dll");

    return uintptr_t(GetModuleHandleA(path));
}

void Crack()
{
    uintptr_t cheatBase = GetCheatBase();
    if (!cheatBase)
    {
        MessageBoxA(
            0,
            "opengl32.dll not found, inject later",
            "Error",
            MB_OK
        );

        return;
    }

    uint16_t JMP = 0x03EB;
    uintptr_t authCheck = cheatBase + 0xA0913;

    DWORD oldProtect = 0;
    VirtualProtect(LPVOID(authCheck), sizeof(JMP), PAGE_EXECUTE_READWRITE, &oldProtect);
    *(uint16_t*)authCheck = JMP;
    VirtualProtect(LPVOID(authCheck), sizeof(JMP), PAGE_EXECUTE_READ, &oldProtect);
}

BOOL WINAPI DllMain(HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved)
{
    if (fdwReason == DLL_PROCESS_ATTACH)
    {
        CreateThread(0, 0, LPTHREAD_START_ROUTINE(Crack), 0, 0, 0);
    }

    return TRUE;
}