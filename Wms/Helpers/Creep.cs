using System.Security.Cryptography;
using System.Text;

namespace Wms.Helpers;

public static class Creep
{
    private static byte[] key;// = { }; //Encryption Key
    private static byte[] iv; // = { 10, 20, 30, 40, 50, 60, 70, 80 };

    static Creep()
    {
        // VS2rskixD/o3fcCuF2ZTyvmzJ4ti0ajl;0DI70CaEmm4=
        //key = Convert.FromBase64String("VS2rskixD/o3fcCuF2ZTyvmzJ4ti0ajl");
        //iv = Convert.FromBase64String("0DI70CaEmm4=");
    }

    public static void Initialize(string base64KeyIvSsv)
    {
        string[] base64KeyIv = base64KeyIvSsv.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        if (base64KeyIv.Length == 2)
        {
            key = Convert.FromBase64String(base64KeyIv[0]);
            iv = Convert.FromBase64String(base64KeyIv[1]);
        }
    }

    public static string Encrypt(string plainText)
    {
        MemoryStream ms;

        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        using (ms = new MemoryStream())
        using (var tdes = TripleDES.Create())
        using (var cs = new CryptoStream(ms, tdes.CreateEncryptor(key, iv), CryptoStreamMode.Write))
        {
            cs.Write(plainTextBytes, 0, plainTextBytes.Length);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(string base64CipherBytes)
    {
        MemoryStream ms;

        byte[] cipherBytes = Convert.FromBase64String(base64CipherBytes);

        using (ms = new MemoryStream())
        using (var tdes = TripleDES.Create())
        using (var cs = new CryptoStream(ms, tdes.CreateDecryptor(key, iv), CryptoStreamMode.Read))
        {
            //cs.Write(plainTextBytes, 0, plainTextBytes.Length);
            cs.Read(cipherBytes, 0, cipherBytes.Length);
        }

        return Encoding.UTF8.GetString(ms.ToArray());
        //return Convert.ToBase64String(ms.ToArray());
    }
}
