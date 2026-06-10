using System.Security.Cryptography;

if (args.Length < 3)
{
    Console.WriteLine("[Usage]: base64tool <encode|decode> <inputFilePath> <outputFilePath>");
    return;
}

string operation = args[0].ToLower();
string inputFilePath = args[1];
string outputFilePath = args[2];

if (!File.Exists(inputFilePath))
{
    Console.WriteLine($"[Error] Input file not found: {inputFilePath}");
    return;
}

try
{
    switch (operation)
    {
        case "encode":
            EncodeToBase64Streaming(inputFilePath, outputFilePath);
            Console.WriteLine($"[Success] Encoded '{inputFilePath}' -> '{outputFilePath}'");
            break;

        case "decode":
            DecodeFromBase64Streaming(inputFilePath, outputFilePath);
            Console.WriteLine($"[Success] Decoded '{inputFilePath}' -> '{outputFilePath}'");
            break;

        default:
            Console.WriteLine($"[Error] Unknown operation '{operation}'. Use 'encode' or 'decode'.");
            break;
    }
}
catch (FormatException)
{
    Console.WriteLine("[Error] The input file contains invalid Base64 characters.");
}
catch (Exception ex)
{
    Console.WriteLine($"[Error] An unexpected error occurred: {ex.Message}");
}

return;

static void EncodeToBase64Streaming(string inputPath, string outputPath)
{
    using var inputStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
    using var outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
    using var transform = new ToBase64Transform();
    using var cryptoStream = new CryptoStream(outputStream, transform, CryptoStreamMode.Write);

    inputStream.CopyTo(cryptoStream);
}

static void DecodeFromBase64Streaming(string inputPath, string outputPath)
{
    using var inputStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
    using var outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
    using var transform = new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces);
    using var cryptoStream = new CryptoStream(inputStream, transform, CryptoStreamMode.Read);

    cryptoStream.CopyTo(outputStream);
}
