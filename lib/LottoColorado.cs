using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoTryDataJob.Lib
{
    public class LottoColorado : LottoBase
    {
        public LottoColorado(LottoDb lottoDbContext) : base(lottoDbContext)
        {
            string url = "https://www.coloradolottery.com/en/games/lotto/";
            Driver.Navigate().GoToUrl(url);       
        }

        private string searchDrawDate()
        {
            var cls = Driver.FindElement(By.ClassName("drawDate"));
            int year = DateTime.Now.Year;
            var arr = cls.Text.Trim().Split();
            var month = DicDateShort[arr[1]];
            var day = arr[2];
            var dat = $"{year}-{month}-{day}";
            return dat;
        }

        private List<string> searchDrawNumbers()
        {
            var numbers = Driver.FindElements(By.ClassName("drawNumber"));

            List<string> list = new();
            for (int i=0; i < 6; i++)
            {
                list.Add(numbers[i].Text);
            }
            return list;
        }

        internal override  void InsertDb()
        {
            var list = db.ColoradoLottoes.ToList();
            IList<Tuple<int, string>> dates = list.Select(x => new Tuple<int, string>(x.DrawNumber, x.DrawDate)).ToList();
            var lastDrawDate = dates.LastOrDefault().Item2;
            var currentDrawDate = searchDrawDate();

            if (DateTime.Parse(currentDrawDate) > DateTime.Parse(lastDrawDate))
            {
                var lastDrawNumber = dates.LastOrDefault().Item1;
                var numbers = searchDrawNumbers();

                var entity = new ColoradoLotto();
                entity.DrawNumber = lastDrawNumber + 1;
                entity.DrawDate = currentDrawDate;
                entity.Number1 = int.Parse(numbers[0]);
                entity.Number2 = int.Parse(numbers[1]);
                entity.Number3 = int.Parse(numbers[2]);
                entity.Number4 = int.Parse(numbers[3]);
                entity.Number5 = int.Parse(numbers[4]);
                entity.Number6 = int.Parse(numbers[5]);

                    
                // save to db
                db.ColoradoLottoes.Add(entity);
                db.SaveChanges();
            }
            
            Driver.Close();
            Driver.Quit();
        }
    }
}
