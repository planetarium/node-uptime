using System.Text.Json;

namespace NodeUptime.Client;

public class PlanetRegistryClient
{
    private const string PLANET_REGISTRY = "https://planets.nine-chronicles.com/planets/";

    private readonly HttpClient _httpClient;

    public IDictionary<string, string[]> Nodes { get; private set; }

    public PlanetRegistryClient()
    {
        _httpClient = new HttpClient();
        Nodes = GetNodeUrlsAsync().Result;
    }

    private async Task<IDictionary<string, string[]>> GetNodeUrlsAsync()
    {
        var response = await _httpClient.GetAsync(PLANET_REGISTRY);
        var content = await response.Content.ReadAsStringAsync();
        var planets = JsonSerializer.Deserialize<PlanetRegistry[]>(content);

        return planets?.ToDictionary(
            planet => planet.Name,
            planet => planet.RpcEndpoints["headless.gql"]
        ) ?? new Dictionary<string, string[]>();
    }
}
