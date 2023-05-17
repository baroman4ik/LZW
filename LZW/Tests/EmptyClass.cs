using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public static class LZWCompressionPerformanceTest
{
    public static void Run(int iterations, int dataSize)
    {
        var data = new byte[dataSize];
        var rnd = new Random();
        rnd.NextBytes(data);

        var stopwatch = new Stopwatch();

        // Замер производительности метода сжатия
        stopwatch.Start();
        for (int i = 0; i < iterations; i++)
        {
            var compressedData = LZWCompression.Compress(data);
        }
        stopwatch.Stop();
        var compressionTime = stopwatch.Elapsed.TotalMilliseconds / iterations;

        // Замер производительности метода распаковки
        var compressedBytes = LZWCompression.Compress(data);
        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < iterations; i++)
        {
            var decompressedData = LZWCompression.Decompress(compressedBytes);
        }
        stopwatch.Stop();
        var decompressionTime = stopwatch.Elapsed.TotalMilliseconds / iterations;

        Console.WriteLine($"Data size: {dataSize} bytes, iterations: {iterations}");
        Console.WriteLine($"Compression time: {compressionTime:F3} ms");
        Console.WriteLine($"Decompression time: {decompressionTime:F3} ms");
    }
}
