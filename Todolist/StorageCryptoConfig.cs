using System;

static class StorageCryptoConfig
{
    // Fixed AES-256 key and IV stored centrally for all storage operations.
    public static readonly byte[] Key = Convert.FromHexString("00112233445566778899AABBCCDDEEFF102132435465768798A9BACBDCEDFE0F");
    public static readonly byte[] Iv = Convert.FromHexString("0F1E2D3C4B5A69788796A5B4C3D2E1F0");
}
