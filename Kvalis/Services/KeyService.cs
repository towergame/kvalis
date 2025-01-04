namespace Kvalis.Services;

// Handles key management
public class KeyService(Blazored.LocalStorage.ILocalStorageService localStorage, ILogger<KeyService> logger)
{
    private readonly Blazored.LocalStorage.ILocalStorageService _localStorage = localStorage;
    private readonly ILogger<KeyService> _logger = logger;
    
    public async Task<byte[]> GetPublicKey()
    {
        // Get the public key
        var publicKey = await _localStorage.GetItemAsync<byte[]>("publicKey");
        // logger.LogInformation("Public key: {publicKey}", publicKey ?? new byte[]{});
        return publicKey ?? new byte[]{};
    }
    
    public async Task<byte[]> GetPrivateKey()
    {
        // Get the private key
        return await _localStorage.GetItemAsync<byte[]>("privateKey") ?? new byte[]{};
    }

    public async Task CheckForKeypair()
    {
        // Check if a keypair exists
        if (!await _localStorage.ContainKeyAsync("publicKey") || !await _localStorage.ContainKeyAsync("privateKey"))
        {
            // Generate a new keypair
            var keypair = Sodium.PublicKeyAuth.GenerateKeyPair();

            // Save the keypair
            await _localStorage.SetItemAsync("publicKey", keypair.PublicKey);
            await _localStorage.SetItemAsync("privateKey", keypair.PrivateKey);
        }
    }
    
    public async Task<byte[]> Sign(byte[] data)
    {
        // Get the private key
        var privateKey = await _localStorage.GetItemAsync<byte[]>("privateKey");
        
        if (privateKey == null)
        {
            throw new Exception("No private key found");
        }

        // Sign the data
        return Sodium.PublicKeyAuth.Sign(data, privateKey);
    }
    
    public byte[] Validate(byte[] signedData, byte[] publicKey)
    {
        // Validate the signature
        return Sodium.PublicKeyAuth.Verify(signedData, publicKey);
    }
}