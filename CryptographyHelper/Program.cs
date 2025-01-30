using System;
using System.Security.Cryptography;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\nCryptography Helper Menu:");
            Console.WriteLine("1. Encrypt");
            Console.WriteLine("2. Decrypt");
            Console.WriteLine("3. Compute SHA256 Hash");
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    EncryptInput();
                    break;
                case "2":
                    DecryptInput();
                    break;
                case "3":
                    HashInput();
                    break;
                case "4":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void EncryptInput()
    {
        Console.Write("Enter plaintext: ");
        string plaintext = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = GetPasswordInput(); // Mask password input

        string ciphertext = CryptographyHelper.CryptographyHelper.Encrypt(plaintext, password);
        Console.WriteLine("Ciphertext (Base64): " + ciphertext);
    }

    private static void DecryptInput()
    {
        Console.Write("Enter ciphertext (Base64): ");
        string ciphertext = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = GetPasswordInput(); // Mask password input

        try
        {
            string decryptedText = CryptographyHelper.CryptographyHelper.Decrypt(ciphertext, password);
            Console.WriteLine("Decrypted Text: " + decryptedText);
        }
        catch (Exception ex) // Catch potential decryption errors (e.g., incorrect password)
        {
            Console.WriteLine("Decryption failed: " + ex.Message);
        }
    }

    private static void HashInput()
    {
        Console.Write("Enter text to hash: ");
        string textToHash = Console.ReadLine();

        string sha256Hash = CryptographyHelper.CryptographyHelper.ComputeSha256Hash(textToHash);
        Console.WriteLine("SHA256 Hash (Base64): " + sha256Hash);
    }

    // Mask password input (basic masking - not completely secure, but better than showing it)
    private static string GetPasswordInput()
    {
        string password = "";
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true); // Don't display the key pressed

            if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
            {
                password += key.KeyChar;
                Console.Write("*"); // Display an asterisk
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b"); // Backspace and overwrite the asterisk
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine(); // Move to the next line
        return password;
    }
}