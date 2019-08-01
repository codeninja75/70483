namespace DotNet.E70483.ImplementDataAccess
{
    public class QueryWithLINQ
    {
        public static void LinqStuff()
        {

        }
    }
    public class Artist
    {
        public string Name { get; set; }
    }
    public class MusicTrack
    {
        public Artist Artist { get; set; }
        public string Title { get; set; }
        public int Length { get; set; }
    }
}
