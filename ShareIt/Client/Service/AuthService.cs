using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ShareIt.Client.Helper;
using ShareIt.Shared;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Net;

namespace ShareIt.Client.Service
{
    // AuthService verwaltet die Authentifizierungsvorgänge im Client.
    // Beinhaltet sind Registrierung, Login, Logout und das Löschen von Konten.
    // Service kommuniziert mit den entsprechenden API-Endpunkten und verwaltet den Token.
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<RegisterResult> Register(RegisterModel registerModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Accounts", registerModel);

            if (!result.IsSuccessStatusCode)
            {
                var errorResponse = await result.Content.ReadAsStringAsync();
                var errorResult = JsonSerializer.Deserialize<RegisterResult>(errorResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return errorResult;
            }

            return new RegisterResult { Successful = true };
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            var loginAsJson = JsonSerializer.Serialize(loginModel);
            var response = await _httpClient.PostAsync("api/Login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
            var loginResult = JsonSerializer.Deserialize<LoginResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!response.IsSuccessStatusCode)
            {
                return loginResult!;
            }

            await _localStorage.SetItemAsync("authToken", loginResult!.Token);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email!);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

            return loginResult;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<DeleteUserResult> Delete(DeleteUserModel deleteUserModel)
        {
            var currentUser = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var currentEmail = currentUser.User?.Identity?.Name;

            if (currentEmail != deleteUserModel.Email)
            {
                // Fehlermeldung zurückgeben, wenn der Benutzer versucht, ein anderes Profil zu löschen
                return new DeleteUserResult { Successful = false, ErrorMessage = "You can only delete your own profile" };
            }

            // Überprüfen, ob die E-Mail und das Passwort übereinstimmen
            var loginResult = await Login(new LoginModel
            {
                Email = deleteUserModel.Email,
                Password = deleteUserModel.Password
            });

            if (!loginResult.Successful)
            {
                // Fehlermeldung zurückgeben, wenn Anmeldedaten ungültig sind
                return new DeleteUserResult { Successful = false, ErrorMessage = "Invalid email or password" };
            }

            // Fortfahren mit der Löschung des Benutzerprofils
            var deleteAsJson = JsonSerializer.Serialize(deleteUserModel);
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/Accounts/DeleteByEmail?email={deleteUserModel.Email}");
            request.Content = new StringContent(deleteAsJson, Encoding.UTF8, "application/json");

            using (var response = await _httpClient.SendAsync(request))
            {
                if (!response.IsSuccessStatusCode)
                {
                    // Fehlerbehandlung bei ungültigen Anmeldeinformationen
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        return new DeleteUserResult { Successful = false, ErrorMessage = errorMessage };
                    }
                    // Fehlerbehandlung bei anderen Fehlern
                    return new DeleteUserResult { Successful = false, ErrorMessage = "Error occurred" };
                }

                var deleteResult = await response.Content.ReadFromJsonAsync<DeleteUserResult>();
                return deleteResult!;
            }
        }


    }
}
