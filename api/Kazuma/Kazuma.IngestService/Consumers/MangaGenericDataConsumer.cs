﻿using Confluent.Kafka;
using Kazuma.Common.Kafka;
using Kazuma.Common.Models.Supabase;
using Kazuma.IngestService.DTO;
using Newtonsoft.Json;

namespace Kazuma.IngestService.Consumers
{
    public class MangaGenericDataConsumer : TopicConsumer<Null, ICollection<MangaInfoGenericRequest>>
    {
        private readonly Supabase.Client _supasbaseClient;
        public MangaGenericDataConsumer(ILogger<MangaGenericDataConsumer> logger, Supabase.Client supasbaseClient) : base(logger, "Manga-Generic")
        {
            _supasbaseClient = supasbaseClient;
        }
        protected override async Task OnMessageAsync(ConsumeResult<Null, ICollection<MangaInfoGenericRequest>> consumeResult, CancellationToken stoppingToken)
        {
            var message = consumeResult.Message.Value;
            try
            {
                HashSet<string> duplicate = new();
                List<MangaInfoGeneric> mangaInfoGenerics = new();

                DateTime now = DateTime.Now;
                Console.WriteLine("Processing at: " + now);
                foreach (var item in message)
                    if (duplicate.Add(item.Id))
                    {
                        mangaInfoGenerics.Add(new MangaInfoGeneric()
                        {
                            Id = item.Id,
                            Title = item.Title,
                            CreatedAt = now,
                            UpdatedAt = now
                        });
                    }

                _ = await _supasbaseClient.From<MangaInfoGeneric>().Upsert(mangaInfoGenerics);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error When Trying to Upsert: {JsonConvert.SerializeObject(message)}");
            }
        }
    }
}
