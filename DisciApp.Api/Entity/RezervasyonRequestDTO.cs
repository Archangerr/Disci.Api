namespace DisciApp.Api.Entity
{
    public class RezervasyonRequestDTO
    {
        public DateTime RezervasyonTarihi { get; set; }
        public int BaslangicSaat { get; set; }
        public int Sure { get; set; }
        public string Isim { get; set; }
    }
}
