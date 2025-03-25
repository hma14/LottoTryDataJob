using LottoTryDataJob;
using LottoTryDataJob.Lib;
using OpenQA.Selenium.Chrome;

namespace LottoTryDataJob
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    internal class SeleniumJob
    {
        private readonly LottoDb _context;
        private readonly ILogger<SeleniumJob> _logger;

        public SeleniumJob(LottoDb context, ILogger<SeleniumJob> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task RunSeleniumScraper()
        {
            _logger.LogInformation("Selenium job started at {time}", DateTime.Now);

            try
            {
                using (var driver = new ChromeDriver())
                {

                    LottoBase obj = new LottoMAX(_context);
                    obj.InsertDb();

                    obj = new LottoFloridaFantasy5(_context);
                    obj.InsertDb();

                    obj = new LottoFloridaLotto(_context);
                    obj.InsertDb();

                    obj = new LottoBC49(_context);
                    obj.InsertDb();

                    obj = new Lottery649(_context);
                    obj.InsertDb();

                    obj = new LottoColorado(_context);
                    obj.InsertDb();

                    obj = new LottoGermanLotto(_context);
                    obj.InsertDb();

                    //obj = new LottoConnecticutLotto(_context);
                    //obj.InsertDb();         

                    obj = new LottoEuroMillions(_context);
                    obj.InsertDb();

                    obj = new LottoEuroJackpot(_context);
                    obj.InsertDb();

                    obj.CloseDriver();

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Selenium scraping.");
            }

            _logger.LogInformation("Selenium job completed at {time}", DateTime.Now);
            await Task.CompletedTask;
        }
    }
}
