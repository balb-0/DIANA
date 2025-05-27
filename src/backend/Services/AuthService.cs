using backend.Models;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using System.Text.Json;
namespace backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly FirebaseAuth _firebaseAuth;
        private readonly ILogger<AuthService> _logger;
        private readonly string _firebaseApiKey;
        private readonly HttpClient _httpClient = new();

        public AuthService(IConfiguration configuration, ILogger<AuthService> logger)
        {

            _logger = logger;
            _firebaseApiKey = configuration["Firebase:ApiKey"]
                ?? throw new InvalidOperationException("Firebase:ApiKey não está configurado.");

            // Inicializar Firebase Admin SDK
            if (FirebaseApp.DefaultInstance == null)
            {
                try
                {
                    var serviceAccountPath = configuration["Firebase:ServiceAccountKeyPath"];

                    if (string.IsNullOrEmpty(serviceAccountPath))
                    {
                        throw new InvalidOperationException("Caminho do Service Account Key não configurado. Configure Firebase:ServiceAccountKeyPath no appsettings.json");
                    }

                    if (!File.Exists(serviceAccountPath))
                    {
                        throw new FileNotFoundException($"Arquivo Service Account Key não encontrado: {serviceAccountPath}");
                    }

                    var credential = GoogleCredential.FromFile(serviceAccountPath);

                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = credential,
                        ProjectId = configuration["Firebase:ProjectId"] ?? "diana-446a1"
                    });

                    _logger.LogInformation("Firebase Admin SDK inicializado com sucesso usando Service Account Key: {Path}", serviceAccountPath);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao inicializar Firebase Admin SDK");
                    throw;
                }
            }

            _firebaseAuth = FirebaseAuth.DefaultInstance;
        }

        public async Task<AuthResponse> RegisterUserAsync(RegisterRequest request)
        {
            try
            {
                var userRecord = await _firebaseAuth.CreateUserAsync(new UserRecordArgs()
                {
                    Email = request.Email,
                    Password = request.Password,
                    DisplayName = request.DisplayName,
                    EmailVerified = false
                });

                return new AuthResponse
                {
                    Success = true,
                    Message = "Usuário registrado com sucesso",
                    User = new UserInfo
                    {
                        Uid = userRecord.Uid,
                        Email = userRecord.Email,
                        DisplayName = userRecord.DisplayName,
                        EmailVerified = userRecord.EmailVerified
                    }
                };
            }
            catch (FirebaseAuthException ex)
            {
                _logger.LogError(ex, "Erro ao registrar usuário: {ErrorCode}", ex.ErrorCode);
                return new AuthResponse
                {
                    Success = false,
                    Message = GetFirebaseErrorMessage(ex.ErrorCode)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao registrar usuário");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Erro interno do servidor"
                };
            }
        }

        private string GetFirebaseErrorMessage(ErrorCode errorCode)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResponse> VerifyTokenAsync(string idToken)
        {
            try
            {
                // Verificar o ID Token do Firebase
                var decodedToken = await _firebaseAuth.VerifyIdTokenAsync(idToken);

                // Buscar informações completas do usuário
                var userRecord = await _firebaseAuth.GetUserAsync(decodedToken.Uid);

                return new AuthResponse
                {
                    Success = true,
                    Message = "Token válido",
                    User = new UserInfo
                    {
                        Uid = userRecord.Uid,
                        Email = userRecord.Email,
                        DisplayName = userRecord.DisplayName,
                        EmailVerified = userRecord.EmailVerified
                    }
                };
            }
            catch (FirebaseAuthException ex)
            {
                _logger.LogError(ex, "Erro ao verificar token Firebase: {ErrorCode}", ex.ErrorCode);
                return new AuthResponse
                {
                    Success = false,
                    Message = "Token inválido ou expirado"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao verificar token");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Erro interno do servidor"
                };
            }
        }

        private string GetFirebaseErrorMessage(string errorCode)
        {
            return errorCode switch
            {
                "EMAIL_ALREADY_EXISTS" => "Este email já está sendo usado por outra conta",
                "INVALID_EMAIL" => "Email inválido",
                "WEAK_PASSWORD" => "A senha deve ter pelo menos 6 caracteres",
                "USER_NOT_FOUND" => "Usuário não encontrado",
                "WRONG_PASSWORD" => "Senha incorreta",
                "INVALID_ID_TOKEN" => "Token inválido",
                "EXPIRED_ID_TOKEN" => "Token expirado",
                _ => "Erro de autenticação"
            };
        }

        public async Task<AuthResponse> LoginUserAsync(LoginRequest request)
        {
            try
            {
                var payload = new
                {
                    email = request.Email,
                    password = request.Password,
                    returnSecureToken = true
                };

                var response = await _httpClient.PostAsJsonAsync(
                    $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_firebaseApiKey}",
                    payload);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var errorObj = JsonDocument.Parse(errorContent);
                    var errorCode = errorObj.RootElement
                                            .GetProperty("error")
                                            .GetProperty("message")
                                            .GetString();

                    _logger.LogWarning("Falha no login: {ErrorCode}", errorCode);
                    return new AuthResponse
                    {
                        Success = false,
                        Message = GetFirebaseErrorMessage(errorCode)
                    };
                }

                var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                var idToken = result.GetProperty("idToken").GetString();
                var uid = result.GetProperty("localId").GetString();

                var userRecord = await _firebaseAuth.GetUserAsync(uid);

                return new AuthResponse
                {
                    Success = true,
                    Message = "Login bem-sucedido",
                    IdToken = idToken,
                    User = new UserInfo
                    {
                        Uid = userRecord.Uid,
                        Email = userRecord.Email,
                        DisplayName = userRecord.DisplayName,
                        EmailVerified = userRecord.EmailVerified
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao fazer login do usuário");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Erro interno do servidor"
                };
            }
        }

    }
}