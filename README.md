# CryptographyHelper

This .NET library provides a set of cryptographic helper functions for common tasks such as encrypting and decrypting data with AES (using PBKDF2 for key derivation), generating and verifying password hashes, and computing SHA256 hashes. The library prioritizes ease of use and secure defaults.

## Installation

You can install the CryptographyHelper library via NuGet.  ( *Replace with actual NuGet instructions when you publish the package.* )

Or, if you prefer to include the source code directly in your project:

1. Clone the repository: `git clone https://github.com/YourUsername/CryptographyHelper.git`
2. Add the `CryptographyHelper.cs` file to your project.

## Usage

### Encryption and Decryption

```csharp
string plaintext = "This is the text I want to encrypt.";
string password = "MyStrongPassword"; // In real applications, handle passwords securely!

string ciphertext = CryptographyHelper.Encrypt(plaintext, password);
Console.WriteLine("Ciphertext: " + ciphertext);

string decryptedText = CryptographyHelper.Decrypt(ciphertext, password);
Console.WriteLine("Decrypted Text: " + decryptedText);

string password = "AnotherStrongPassword";

string hashedPassword = CryptographyHelper.ComputeSha256Hash(password); // For storing passwords securely
Console.WriteLine("Hashed Password: " + hashedPassword);

//  In a real application, you would store the hashedPassword and then compare 
//  the hash of the entered password with the stored hash.  Don't store passwords in plaintext!

string textToHash = "Some text to hash.";

string sha256Hash = CryptographyHelper.ComputeSha256Hash(textToHash);
Console.WriteLine("SHA256 Hash: " + sha256Hash);

string salt = CryptographyHelper.GenerateSalt();
Console.WriteLine("Generated Salt: " + salt);

```

## Security Considerations:
Password Storage: Never store passwords in plaintext. Always hash passwords using a strong, one-way hashing algorithm like SHA256 (as demonstrated in the example).

Consider using a dedicated password hashing library like BCrypt or Argon2 for even stronger security. The example uses SHA256 for demonstration, but a more robust KDF is recommended for production.

## Key Derivation: 
The Encrypt function uses PBKDF2 to derive the encryption key from the provided password. PBKDF2 is a strong key derivation function, but it's essential to use a sufficient number of iterations (the library uses 10000 by default, which is a good starting point).

### Salt: 
A unique salt is used for each encryption operation. This is crucial for security.
The salt is combined with the ciphertext and stored.

### IV:
A unique Initialization Vector (IV) is used for each encryption operation. This also significantly increases the security of the encryption. The IV is combined with the ciphertext and stored.

### Secure Password Handling: 
The example code for password input is a basic demonstration of masking. It is not fully secure. In a real-world application, you should use platform-specific secure password input methods if available.

Protecting passwords is a complex topic, and this library provides the building blocks, but you are responsible for the secure handling and storage of passwords in your application.
