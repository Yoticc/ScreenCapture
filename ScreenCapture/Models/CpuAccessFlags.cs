﻿namespace ScreenCapture.DirectXModels;
public enum CpuAccessFlags
{
    None = 0x00000,
    Read = 0x20000,
    Write = 0x10000,
    ReadWrite = Read | Write
}