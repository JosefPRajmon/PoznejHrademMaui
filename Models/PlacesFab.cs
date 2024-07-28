namespace PoznejHrademMaui.Models
{
    public class PlacesFab
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string Interest { get; set; }
        public string OpenTime { get; set; }
        public string Information { get; set; }
        public bool Trase { get; set; }
        public List<string> Photo { get; set; }
        public List<int> Quiz { get; set; }
        public Location Position { get; set; }
        public PlacesFab(Location possotion, string title, string descript, string interest, string openT,
            string information, List<string> PhotoLink, List<int> ints = null, bool trase = false)
        {
            Title = title;
            Description = descript;
            Interest = interest;
            OpenTime = openT;
            Information = information;
            Photo = PhotoLink;
            if (ints == null)
            {
                ints = new List<int> { };
            }
            Quiz = ints;
            Trase = trase;
            Position = possotion;
        }
        //generate constructor PlacesFab where photo is only one string
        public PlacesFab(Location possotion, string title, string descript, string interest, string openT,
            string information, string PhotoLink, List<int> ints = null, bool trase = false)
        {
            Title = title;
            Description = descript;
            Interest = interest;
            OpenTime = openT;
            Information = information;
            Photo = new List<string> { PhotoLink };
            if (ints == null)
            {
                ints = new List<int> { };
            }
            Quiz = ints;
            Trase = trase;
            Position = possotion;
        }
    };
}
