using System.Net.Http;
using System.Text.Json;
using LogisticsAPI.Gateways.Interfaces;
using LogisticsAPI.Models.External.ViaCep;
using LogisticsAPI.Models.Inputs;
using LogisticsAPI.Models.ValueObjects;

namespace LogisticsAPI.Gateways
{
    public class ViaCepGateway : IAddressValidationGateway
    {
        private readonly HttpClient _httpClient;

        public ViaCepGateway(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Address?> ValidateAndNormalizeAsync(AddressInput input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (!input.Country.Equals("BR", StringComparison.OrdinalIgnoreCase))
                return null;

            var response = await _httpClient.GetAsync(
                $"https://viacep.com.br/ws/{input.PostalCode}/json/");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var viaCep = JsonSerializer.Deserialize<ViaCepResponse>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (viaCep == null || viaCep.Erro)
                return null;

            return new Address(
                string.IsNullOrWhiteSpace(viaCep.Logradouro) ? input.Street ?? string.Empty : viaCep.Logradouro,
                string.IsNullOrWhiteSpace(viaCep.Localidade) ? input.City ?? string.Empty : viaCep.Localidade,
                string.IsNullOrWhiteSpace(viaCep.Uf) ? input.State ?? string.Empty : viaCep.Uf,
                string.IsNullOrWhiteSpace(viaCep.Cep) ? input.PostalCode : viaCep.Cep,
                "BR"
            );
        }
    }
}