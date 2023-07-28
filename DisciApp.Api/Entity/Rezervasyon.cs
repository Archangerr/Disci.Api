namespace DisciApp.Api.Entity
{
    public class Rezervasyon
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public string Isim { get; set; }
        public int BaslangicSaat { get; set; }
        public int Sure { get; set; }
    }

}
