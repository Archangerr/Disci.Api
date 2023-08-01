using DisciApp.Api.DataBaseContext;
using DisciApp.Api.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System.Globalization;

namespace DisciApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RezervasyonController : ControllerBase
    {
        private readonly RezervasyonDbContext _context;

        public RezervasyonController(RezervasyonDbContext context)
        {
            _context = context;
        }

        //[HttpPost]
        //public async Task SaveImage(string imageBase64)
        //{
        //    var myEntity = new Img
        //    {
        //        ImageBase64 = imageBase64
        //        // set other properties
        //    };

        //  //  _context.
        //    await _context.SaveChangesAsync();
        //}

        [HttpPost]
        public async Task<IActionResult> RezervasyonYap(RezervasyonRequestDTO request)
        {

            //DateTime date;
            //bool success = DateTime.TryParseExact(request.RezervasyonTarihi, "yyyyMMdd",
            //                                     System.Globalization.CultureInfo.InvariantCulture,
            //                                     System.Globalization.DateTimeStyles.None,
            //                                      out date);

            //if (success)
            //{
                
            //}
            //else
            //{
            //   return BadRequest("Tarih formati hatali");
            //}

            if (request.RezervasyonTarihi.Hour != 0 || request.RezervasyonTarihi.Minute != 0 || request.RezervasyonTarihi.Second != 0)
            {
                //Exception handling
                throw new ApplicationException("Tarih sadece gun bilgisi iceriyor olmali");
               
            }

            var bitisSaat = request.BaslangicSaat + request.Sure;

            if (request.BaslangicSaat < 9 || bitisSaat > 17)
            {
                //Exception handling
                throw new ApplicationException("Mesai saati disinda");
            }

            if (request.RezervasyonTarihi.DayOfWeek == DayOfWeek.Saturday || request.RezervasyonTarihi.DayOfWeek == DayOfWeek.Sunday)
            {
                //Exception handling
                throw new ApplicationException("Mesai saati disinda");
                
            }

            var overlappingReservation = await _context.Rezervasyonlar
                .Where(r => r.Tarih.Date == request.RezervasyonTarihi.Date &&
                            (r.BaslangicSaat < bitisSaat && r.BaslangicSaat + r.Sure > request.BaslangicSaat))
                .FirstOrDefaultAsync();

            if (overlappingReservation != null)
            {
                return BadRequest("Tarih rezerve");
            }

            var rezervasyon = new Rezervasyon
            {
                Tarih = request.RezervasyonTarihi.AddHours(request.BaslangicSaat),
                Isim = request.Isim,
                BaslangicSaat = request.BaslangicSaat,
                Sure = request.Sure
            };

            _context.Rezervasyonlar.Add(rezervasyon);
            await _context.SaveChangesAsync();

            return Ok("Rezervasyon basarili");
        }


        [HttpGet]
        [Route("Rezervasyon Kontrol")]
        // public async Task<IActionResult> RezervasyonKontrol([FromBody]RezervasyonKontrolRequestDTO request)
        public async Task<IActionResult> RezervasyonKontrol(string BaslangicTarihiString, string BitisTarihiString)
        {
            DateTime BaslangicTarihi;
            DateTime BitisTarihi;
            var success1 = DateTime.TryParseExact(BaslangicTarihiString,
                           "yyyy-MM-dd",
                           System.Globalization.CultureInfo.InvariantCulture,
                           System.Globalization.DateTimeStyles.None,
                           out BaslangicTarihi);    
            var success2 = DateTime.TryParseExact(BitisTarihiString,
                           "yyyy-MM-dd",
                           System.Globalization.CultureInfo.InvariantCulture,
                           System.Globalization.DateTimeStyles.None,
                           out BitisTarihi);

            if (success1 && success2)
            {

            }
            else
            {   //Exception handling
                throw new ApplicationException("Tarih formati hatali");
            }
            if (BaslangicTarihi.Hour != 0 ||BaslangicTarihi.Minute != 0 || BaslangicTarihi.Second != 0 ||
                BitisTarihi.Hour != 0 || BitisTarihi.Minute != 0 || BitisTarihi.Second != 0)
            {
                throw new ApplicationException("Tarih sadece gun bilgisi iceriyor olmali");
            }

            var rezervasyonlar = await _context.Rezervasyonlar
                .Where(r => r.Tarih.Date >= BaslangicTarihi.Date && r.Tarih.Date <= BitisTarihi.Date)
                .Select(r => new RezervasyonRequestDTO
                 {
                    RezervasyonTarihi = r.Tarih,
                    BaslangicSaat = r.BaslangicSaat,
                    Sure = r.Sure,
                    Isim = r.Isim
                }).ToListAsync();

            return Ok(rezervasyonlar);
        }


    }





    
}
