using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace RPFLib.Resources
{
    public static class xcompress
    {

        public enum XMEMCODEC_TYPE
        {
            XMEMCODEC_DEFAULT = 0,
            XMEMCODEC_LZX = 1
        }

        public struct XMEMCODEC_PARAMETERS_LZX
        {
            public int Flags;
            public int WindowSize;
            public int CompressionPartitionSize;
        }

        public const int XMEMCOMPRESS_STREAM = 0x00000001;

        [DllImport("xcompress32.dll", EntryPoint = "XMemCreateDecompressionContext")]
        public static extern int XMemCreateDecompressionContext(
            XMEMCODEC_TYPE CodecType,
            int pCodecParams,
            int Flags, ref int pContext);

        [DllImport("xcompress32.dll", EntryPoint = "XMemDestroyDecompressionContext")]
        public static extern void XMemDestroyDecompressionContext(int Context);

        [DllImport("xcompress32.dll", EntryPoint = "XMemResetDecompressionContext")]
        public static extern int XMemResetDecompressionContext(int Context);

        [DllImport("xcompress32.dll", EntryPoint = "XMemDecompress")]
        public static extern int XMemDecompress(int Context,
            byte[] pDestination, ref int pDestSize,
            byte[] pSource, int pSrcSize);

        [DllImport("xcompress32.dll", EntryPoint = "XMemDecompressStream")]
        public static extern int XMemDecompressStream(int Context,
            byte[] pDestination, ref int pDestSize,
            byte[] pSource, ref int pSrcSize);

        [DllImport("xcompress32.dll", EntryPoint = "XMemCreateCompressionContext")]
        public static extern int XMemCreateCompressionContext(
            XMEMCODEC_TYPE CodecType, int pCodecParams,
            int Flags, ref int pContext);

        [DllImport("xcompress32.dll", EntryPoint = "XMemDestroyCompressionContext")]
        public static extern void XMemDestroyCompressionContext(int Context);

        [DllImport("xcompress32.dll", EntryPoint = "XMemResetCompressionContext")]
        public static extern int XMemResetCompressionContext(int Context);

        [DllImport("xcompress32.dll", EntryPoint = "XMemCompress")]
        public static extern int XMemCompress(int Context,
            byte[] pDestination, ref int pDestSize,
            byte[] pSource, int pSrcSize);

        [DllImport("xcompress32.dll", EntryPoint = "XMemCompressStream")]
        public static extern int XMemCompressStream(int Context,
            byte[] pDestination, ref int pDestSize,
            byte[] pSource, ref int pSrcSize);

    }
}