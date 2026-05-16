using System;
using System.IO;
using System.Security.Cryptography;

namespace TodoList
{
    public static class CryptoConfig
    {
        private static readonly byte[] KeyBytes = 
        {
            0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF,
            0xFE, 0xDC, 0xBA, 0x98, 0x76, 0x54, 0x32, 0x10,
            0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77,
            0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF
        };
        private static readonly byte[] IvBytes = 
        {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
            0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F
        };

        public static byte[] GetKey() => (byte[])KeyBytes.Clone();
        public static byte[] GetIV() => (byte[])IvBytes.Clone();

        public static CryptoStream CreateCryptoStream(Stream underlyingStream, bool encrypt)
        {
            using var aes = Aes.Create();
            aes.Key = GetKey();
            aes.IV = GetIV();
            var transform = encrypt ? aes.CreateEncryptor() : aes.CreateDecryptor();
            var mode = encrypt ? CryptoStreamMode.Write : CryptoStreamMode.Read;
            return new CryptoStream(underlyingStream, transform, mode);
        }

        public static CryptoStream CreateCryptoStream(Stream underlyingStream, CryptoStreamMode mode)
        {
            using var aes = Aes.Create();
            aes.Key = GetKey();
            aes.IV = GetIV();
            var transform = mode == CryptoStreamMode.Write ? aes.CreateEncryptor() : aes.CreateDecryptor();
            return new CryptoStream(underlyingStream, transform, mode);
        }
    }
}