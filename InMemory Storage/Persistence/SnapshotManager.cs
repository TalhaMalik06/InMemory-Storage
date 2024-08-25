using InMemory_Storage.Models;
using InMemory_Storage.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InMemory_Storage.Persistence
{
    public class SnapshotManager : ISnapshotManager
    {
        public SnapshotManager(IOptions<PersitenceSettings> persitenceSettings, IListRepository listRepository, IKeyValueRepository keyValueRepository, ILogger<SnapshotManager> logger)
        {
            if (persitenceSettings is null)
            {
                throw new ArgumentNullException(nameof(persitenceSettings));
            }

            FilePath = persitenceSettings.Value.Path ?? throw new ArgumentNullException(nameof(persitenceSettings));
            ListRepository = listRepository ?? throw new ArgumentNullException(nameof(listRepository));
            KeyValueRepository = keyValueRepository ?? throw new ArgumentNullException(nameof(keyValueRepository));
            Timer = new Timer(SnapshotCallback, null, TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(persitenceSettings.Value.Interval));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private readonly string FilePath;
        private readonly IListRepository ListRepository;
        private readonly IKeyValueRepository KeyValueRepository;
        private readonly Timer Timer;
        private readonly ILogger<SnapshotManager> Logger;


        public async Task CreateSnapshot()
        {
            try
            {
                var listData = ListRepository.GetAllData();
                var keyValueData = KeyValueRepository.GetAllData();

                var snapshot = new SnapshotData
                {
                    Lists = listData,
                    KeyValuePairs = keyValueData
                };

                var json = JsonSerializer.Serialize(snapshot, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });
                await File.WriteAllTextAsync(FilePath, json);
                Logger.LogInformation("Snapshot created successfully.");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error creating snapshot: {ex.Message}");
            }
        }

        public async Task RestoreFromSnapshot()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    var json = await File.ReadAllTextAsync(FilePath);
                    var snapshot = JsonSerializer.Deserialize<SnapshotData>(json);

                    if(snapshot != null)
                    {
                        if(snapshot.Lists != null) ListRepository.RestoreData(snapshot.Lists);

                        if(snapshot.KeyValuePairs != null) KeyValueRepository.RestoreData(snapshot.KeyValuePairs);
                    }
                    
                    Logger.LogInformation("Snapshot restored successfully.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error restoring from snapshot: {ex.Message}");
            }
        }

        public async void SnapshotCallback(object? state)
        {
            await CreateSnapshot();
        }
    }
}