using SQLite;
namespace PoznejHrademMaui.Components
{

    public class ScannedCode
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string QRCodeText { get; set; }
        public string Enigma { get; set; } = "";
        public bool IsScanned { get; set; } = false;
        public DateTime SavedDate { get; set; } = DateTime.Now;
        /*public ScannedCode(bool scaned)
        {
            IsScanned = scaned;
            SavedDate = DateTime.Now;
        }*/
    }

}
