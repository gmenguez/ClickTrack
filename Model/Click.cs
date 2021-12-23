using System;
using LiteDB;

namespace ClickTrack.Model
{
    public record Click
    {
        [BsonId]
        public int ClickId { get; set; }

        public string Url { get; set; }

        public int Clicks { get; set; }

        public DateTime Created { get; set; }
    }
}
